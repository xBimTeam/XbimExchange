using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using log4net.Util;
using NPOI.SS.Formula.Functions;
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

        private IEnumerable<Attribute> RequirementAttributes()
        {
            if (_requirementType.Attributes == null) 
                return Enumerable.Empty<Attribute>();
            return _requirementType.Attributes.Where(x =>
                x.Categories != null &&
                x.Categories.Any(c => c.Classification == "DPoW" && c.Code == "required"));
        }


        private void RefreshRequirementDetails()
        {
            _requirementDetails = new List<RequirementDetail>();
            foreach (var requirementAttribute in RequirementAttributes())
            {
                _requirementDetails.Add(new RequirementDetail(requirementAttribute));
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
                Categories = new List<Category>() 
            };
            retType.SetRequirementExternalSystem(_requirementType.ExternalSystem);
            retType.SetRequirementExternalId(_requirementType.ExternalId);
            retType.SetRequirementName(_requirementType.Name);
            retType.SetRequirementCategories(_requirementType.Categories);

            // improve returning assetType
            if (candidateType == null) // the following properties depend on the nullity of candidate
            {
                retType.Name = _requirementType.Name;
            }
            else
            {
                retType.Name = candidateType.Name;
                retType.ExternalId = candidateType.ExternalId;
                retType.ExternalSystem = candidateType.ExternalSystem;
                retType.Categories = candidateType.Categories.Clone().ToList();
                if (candidateType.Assets != null)
                {
                    iSubmitted = candidateType.Assets.Count;
                }
            }

            var returnWithoutFurtherTests = false;
            if (!RequirementDetails.Any())
            {
                retType.Description = "Classification has no requirements.\r\n";
                retType.Categories.Add(FacilityValidator.PassedCat);
                iPassed = iSubmitted;
                returnWithoutFurtherTests = true;
            }

            // if candidate is null then consider there's no match
            if (candidateType == null)
            {
                retType.Categories.Add(FacilityValidator.FailedCat);
                retType.Description = "No candidates in submission match the required classification.\r\n";
                retType.Attributes.AddRange(RequirementAttributes());
                returnWithoutFurtherTests = true;    
            }
            
            if (returnWithoutFurtherTests)
            {
                retType.SetSubmittedAssetsCount(iSubmitted);
                retType.SetValidAssetsCount(iPassed);
                return retType;
            }

            // ==================== begin Assets testing

            if (retType.Attributes == null)
                retType.Attributes = new List<Attribute>();

            // produce type level description
            var outstandingRequirementsCount = 0;
            
            var cachedValidator = new CachedPropertiesAndAttributesValidator<AssetType>(candidateType);
            foreach (var req in RequirementDetails)
            {
                object satValue;
                var pass = cachedValidator.CanSatisfy(req, out satValue);
                var a = req.Attribute.Clone();
                if (satValue is AttributeValue)
                    a.Value = satValue as AttributeValue;
                else
                    a.Value = null;
                if (pass)
                {
                    a.Categories = new List<Category>() { FacilityValidator.PassedCat };
                }
                else
                {
                    a.Categories = new List<Category>() { FacilityValidator.FailedCat };
                    outstandingRequirementsCount++;
                }
                
                retType.Attributes.Add(a);    

            }

            retType.Description = string.Format("{0} of {1} requirement addressed at type level.", RequirementDetails.Count - outstandingRequirementsCount, RequirementDetails.Count);
            
            var anyAssetFails = false;
            retType.Assets = new List<Asset>();
            // perform tests at asset level
            if (candidateType.Assets != null)
            {
                foreach (var modelAsset in candidateType.Assets)
                {
                    int iAssetRequirementsMatched = 0;
                    var reportAsset = new Asset
                    {
                        Name = modelAsset.Name,
                        // AssetIdentifier = modelAsset.AssetIdentifier,
                        ExternalId = modelAsset.ExternalId,
                        Categories = new List<Category>(),
                        Attributes = new List<Attribute>()
                    };

                    // asset classification can be copied from model
                    //
                    if (modelAsset.Categories != null)
                        reportAsset.Categories.AddRange(modelAsset.Categories);

                    var assetCachedValidator = new CachedPropertiesAndAttributesValidator<Asset>(modelAsset);
                    foreach (var req in RequirementDetails)
                    {
                        object satValue;
                        if (assetCachedValidator.CanSatisfy(req, out satValue))
                        {
                            // passes locally
                            if (!cachedValidator.AlreadySatisfies(req))
                            {
                                iAssetRequirementsMatched++;
                            }
                            var a = req.Attribute.Clone();
                            if (satValue is AttributeValue)
                                a.Value = satValue as AttributeValue;
                            else
                                a.Value = null;
                            a.Categories = new List<Category>() {FacilityValidator.PassedCat};
                            reportAsset.Attributes.Add(a);
                        }
                        else if (!cachedValidator.AlreadySatisfies(req)) // fails locally, and is not passed at higher level, then add to explicit report fail
                        {
                            var a = req.Attribute.Clone();
                            if (satValue is AttributeValue)
                                a.Value = satValue as AttributeValue;
                            else
                                a.Value = null;
                            a.Categories = new List<Category>() { FacilityValidator.FailedCat };
                            reportAsset.Attributes.Add(a);
                        }
                    }


                    var sb = new StringBuilder();
                    sb.AppendFormat("{0} of {1} outstanding requirements addressed at asset level.", iAssetRequirementsMatched, outstandingRequirementsCount);

                    var pass = (outstandingRequirementsCount == iAssetRequirementsMatched);
                    if (!pass)
                    {
                        anyAssetFails = true;
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

        private IEnumerable<RequirementDetail> RequirementsNotSatisfiedFrom(AssetType typeToTest)
        {
            if (typeToTest.Attributes == null)
            {
                foreach (var req in RequirementDetails)
                {
                    yield return req;
                }
                yield break;
            }

            // prepare a dictionary of attributes for speed
            var dicAtt = typeToTest.Attributes.ToDictionary(att => att.Name, att => att);
            foreach (var requirement in RequirementDetails)
            {
                if (!dicAtt.ContainsKey(requirement.Name))
                {
                    yield return requirement;
                    continue;
                }
                if (!requirement.IsSatisfiedBy(dicAtt[requirement.Name]))
                {
                    yield return requirement;
                }
            }
        }

        /// <summary>
        /// Identifies all the assetTypes in the submitted facility that match requirement classifications
        /// </summary>
        /// <param name="submitted"></param>
        /// <returns></returns>
        internal IEnumerable<AssetTypeCategoryMatch<AssetType>> GetCandidates(List<AssetType> submitted)
        {
            if (_requirementType.Categories == null)
                yield break;
            
            var ret = new Dictionary<AssetType, List<Category>>();
            foreach (var reqClass in _requirementType.Categories)
            {
                var thisClassMatch = reqClass.GetClassificationMatches(submitted);
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
                yield return new AssetTypeCategoryMatch<AssetType>(item.Key) { MatchingCategories = item.Value } ;
            }
        }

        internal AssetType Validate(AssetTypeCategoryMatch<AssetType> candidate)
        {
            var validated = Validate(candidate.MatchedAssetType);
            validated.SetMatchingCategories(candidate.MatchingCategories);
            return validated;
        }
    }
}
