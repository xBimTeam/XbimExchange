using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using log4net.Util;
using Xbim.CobieLiteUK.Validation.Extensions;
using Xbim.CobieLiteUK.Validation.RequirementDetails;
using Xbim.COBieLiteUK;
using Attribute = Xbim.COBieLiteUK.Attribute;

namespace Xbim.CobieLiteUK.Validation
{


    public class AssetTypeValidator : IValidator
    {
        private readonly AssetType _requirementType;

        public AssetTypeValidator(AssetType requirementType)
        {
            _requirementType = requirementType;
        }

        private List<RequirementDetail> _requirementDetails;

        public List<RequirementDetail> RequirementDetails
        {
            get
            {
                if (_requirementDetails == null)
                    RefreshRequirementDetails();
                return _requirementDetails;
            }
        }

        private void RefreshRequirementDetails()
        {
            _requirementDetails = new List<RequirementDetail>();
            if (_requirementType.Attributes != null)
            {
                foreach (
                    var attrib in
                        _requirementType.Attributes.Where(
                            x =>
                                x.Categories != null &&
                                x.Categories.Any(c => c.Classification == "DPoW" && c.Code == "required")))
                {
                    _requirementDetails.Add(new RequirementDetail(attrib));
                }
            }
        }

        public TerminationMode TerminationMode { get; set; }

        public bool HasFailures { get; private set; }

        public bool HasRequirements 
        {
            get { return RequirementDetails.Any(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidateType">If null provides a missing match report</param>
        /// <returns></returns>
        internal AssetType Validate(AssetType candidateType)
        {
            var iSubmitted = 0;
            var iPassed = 0;


            // initialisation
            var retType = new AssetType
            {
                Categories = new List<Category>(_requirementType.Categories.Clone()) // classification comes from the requirement
            };

            // improve returning assetType
            if (candidateType == null) // the following properties depend on the nullity of candidate
            {
                retType.Name = _requirementType.Name;
                retType.ExternalId = _requirementType.ExternalId;
                iSubmitted = 0;
            }
            else
            {
                retType.Name = candidateType.Name;
                retType.ExternalId = candidateType.ExternalId;
                if (candidateType.Assets != null)
                {
                    iSubmitted = candidateType.Assets.Count;
                }
            }

            var returnWithoutFurtherTests = false;
            if (!RequirementDetails.Any())
            {
                retType.Description = "No requirement for the specific classification.\r\n";
                retType.Categories.Add(FacilityValidator.PassedCat);
                iPassed = iSubmitted;
                returnWithoutFurtherTests = true;
            }

            // if candidate is null then consider there's no match
            if (candidateType == null)
            {
                retType.Categories.Add(FacilityValidator.FailedCat);
                retType.Description = "No candidates in submission match the required classification.\r\n";
                returnWithoutFurtherTests = true;    
            }
            
            if (returnWithoutFurtherTests)
            {
                retType.SetSubmittedAssetsCount(iSubmitted);
                retType.SetValidAssetsCount(iPassed);
                return retType;
            }

            if (retType.Attributes == null)
                retType.Attributes = new List<Attribute>();

            // produce type level description
            var outstandingRequirements = MissingFrom(candidateType).ToList();
            var outstandingRequirementsCount = outstandingRequirements.Count();
            retType.Description = string.Format("{0} of {1} requirement addressed at type level.", RequirementDetails.Count - outstandingRequirementsCount, RequirementDetails.Count);
            
            
            foreach (var provided in ProvidedRequirementValues(candidateType.Attributes))
            {
                // todo: need to clone the original attribute here.
                var a = provided.Requirement.Attribute;
                a.Value = provided.ProvidedValue.Value;
                retType.Attributes.Add(a);
            }
            foreach (var missing in outstandingRequirements)
            {
                retType.Attributes.Add(missing.Attribute);
            }
            
            var anyAssetFails = false;
            retType.Assets = new List<Asset>();
            // perform tests at asset level
            if (candidateType.Assets != null)
            {
                foreach (var modelAsset in candidateType.Assets)
                {
                    var reportAsset = new Asset
                    {
                        Attributes = new List<Attribute>(),
                        Name = modelAsset.Name,
                        AssetIdentifier = modelAsset.AssetIdentifier,
                        Categories = new List<Category>(),
                        ExternalId = modelAsset.ExternalId
                    };
                    // reportAsset.Description = modelAsset.Description;
                    if (modelAsset.Categories != null)
                        reportAsset.Categories.AddRange(modelAsset.Categories);

                    // at this stage we are only validating for the existence of attributes.
                    //
                    var matching =
                        outstandingRequirements.Select(x => x.Name)
                            .Intersect(modelAsset.Attributes.Select(at => at.Name));
                    var matchingCount = 0;
                    // add passes to the report.
                    foreach (var matched in matching)
                    {
                        var attV = modelAsset.Attributes.FirstOrDefault(a => a.Name == matched);
                        var attributeV = attV == null ? new StringAttributeValue() { Value = "Unexpected error." } : attV.Value;
                        matchingCount++;

                        var att = new Attribute()
                        {
                            Name = matched,
                            Categories = new List<Category>() {FacilityValidator.PassedCat},
                            Value = attributeV
                        };
                        reportAsset.Attributes.Add(att);
                    }

                    var sb = new StringBuilder();
                    sb.AppendFormat("{0} of {1} requirements matched at asset level.\r\n\r\n", matchingCount,
                        outstandingRequirementsCount);

                    var pass = (outstandingRequirementsCount == matchingCount);
                    if (!pass)
                    {
                        anyAssetFails = true;
                        sb.AppendLine("Attributes are missing.");
                        foreach (var req in outstandingRequirements)
                        {
                            if (matching.Contains(req.Name))
                                continue;
                            // sb.AppendFormat("{0}\r\n{1}\r\n\r\n", req.Name, req.Description);

                            var att = new Attribute()
                            {
                                Name = req.Name,
                                Description = req.Description,
                                Categories = new List<Category>() {FacilityValidator.FailedCat}
                            };
                            reportAsset.Attributes.Add(att);
                        }
                        reportAsset.Categories.Add(FacilityValidator.FailedCat);
                    }
                    else
                    {
                        iPassed++;
                        reportAsset.Categories.Add(FacilityValidator.PassedCat);
                    }
                    reportAsset.Description = sb.ToString();
                    retType.Assets.Add(reportAsset);
                }
            }
            retType.Categories.Add(
                anyAssetFails ? FacilityValidator.FailedCat : FacilityValidator.PassedCat
                );

            retType.SetSubmittedAssetsCount(iSubmitted);
            retType.SetValidAssetsCount(iPassed);
            return retType;
        }

        private IEnumerable<RequirementProvision<Attribute>> ProvidedRequirementValues(List<Attribute> attributes)
        {
            if (attributes == null)
                yield break;
            foreach (var reqDetail in providedRequirementsList(attributes))
            {
                // fill in the value from the property
                var found = attributes.FirstOrDefault(a => a.Name == reqDetail.Name);
                if (found == null)
                    continue;

                yield return new RequirementProvision<Attribute>(reqDetail, found);
            }
        }

        private IEnumerable<RequirementDetail> providedRequirementsList(List<Attribute> attributes)
        {
            if (attributes == null)
                return Enumerable.Empty<RequirementDetail>();

            var req = new HashSet<string>(RequirementDetails.Select(x => x.Name));
            var got = new HashSet<string>(attributes.Select(x => x.Name));

            got.IntersectWith(req);

            return RequirementDetails.Where(creq => got.Contains(creq.Name));
        }

        private IEnumerable<RequirementDetail> MissingFrom(AssetType typeToTest)
        {
            if (typeToTest.Attributes == null)
                return RequirementDetails;

            var req = new HashSet<string>(RequirementDetails.Select(x => x.Name));
            var got = new HashSet<string>(typeToTest.Attributes.Select(x => x.Name));

            req.RemoveWhere(got.Contains);
            return req.Select(left => RequirementDetails.FirstOrDefault(x => x.Name == left));
        }

        /// <summary>
        /// Identifies all the assetTypes in the submitted facility that match requirement classifications
        /// </summary>
        /// <param name="submitted"></param>
        /// <returns></returns>
        internal IEnumerable<AssetTypeCategoryMatch> GetCandidates(Facility submitted)
        {
            if (_requirementType.Categories == null)
                yield break;
            
            var ret = new Dictionary<AssetType, List<Category>>();
            foreach (var reqClass in _requirementType.Categories)
            {
                var thisClassMatch = submitted.AssetTypes.GetClassificationMatches(reqClass);
                foreach (var matchedAsset in thisClassMatch)
                {
                    if (!ret.ContainsKey(matchedAsset.MatchedAssetType))
                    {
                        ret.Add(matchedAsset.MatchedAssetType, matchedAsset.MatchingCategories);
                    }
                    else
                    {
                        ret[matchedAsset.MatchedAssetType].AddRange(matchedAsset.MatchingCategories);
                    }
                }
            }

            foreach (var item in ret)
            {
                yield return new AssetTypeCategoryMatch(item.Key) { MatchingCategories = item.Value } ;
            }
        }

        internal AssetType Validate(AssetTypeCategoryMatch candidate)
        {
            var validated = Validate(candidate.MatchedAssetType);
            validated.SetMatchingCategories(candidate.MatchingCategories);
            return validated;
        }
    }
}
