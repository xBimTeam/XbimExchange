using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.CobieLiteUK.Validation.Extensions;
using Xbim.CobieLiteUK.Validation.RequirementDetails;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation
{
    internal class CobieObjectValidator<T, TSub> : IValidator
        where T : CobieObject, new()
        where TSub : CobieObject, new()
    {
        private readonly T _requirementObject;

        public CobieObjectValidator(T requirementObject)
        {
            HasFailures = false;
            _requirementObject = requirementObject;
        }

        private List<RequirementDetail> _requirementDetails;

        internal List<RequirementDetail> RequirementDetails
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
            if (_requirementObject.Attributes == null) 
                return Enumerable.Empty<Attribute>();
            return _requirementObject.Attributes.Where(x => x.IsClassifiedAsRequirement());
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

        /// <param name="candidateParent">If null provides a missing match report</param>
        /// <param name="targetFacility">The target facility is required to ensure that the facility tree is properly referenced</param>
        /// <returns></returns>
        internal T Validate(T candidateParent, Facility targetFacility)
        {
            var iSubmitted = 0;
            var iPassed = 0;
            List<TSub> candidateChildren = null;
            var returnChildren = new List<TSub>();

            // initialisation
            var retType = targetFacility.Create<T>();
            retType.Categories = new List<Category>();
            retType.SetRequirementExternalSystem(_requirementObject.ExternalSystem);
            retType.SetRequirementExternalId(_requirementObject.ExternalId);
            retType.SetRequirementName(_requirementObject.Name);
            retType.SetRequirementCategories(_requirementObject.Categories);

            // improve returning Parent
            if (candidateParent == null) // the following properties depend on the nullity of candidate
            {
                retType.Name = _requirementObject.Name;
            }
            else
            {
                retType.Name = candidateParent.Name;
                retType.ExternalId = candidateParent.ExternalId;
                retType.ExternalSystem = candidateParent.ExternalSystem;
                retType.Categories = candidateParent.Categories.Clone().ToList();
                candidateChildren = candidateParent.GetChildObjects<TSub>();
                if (candidateChildren != null)
                {
                    iSubmitted = candidateChildren.Count;
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
            if (candidateParent == null)
            {
                // it's only a failure if the requirement was itself required (not only it had atteributes required)
                if (_requirementObject.Categories != null
                    && _requirementObject.Categories.Any(c => c.Classification == "DPoW" && c.Code == "required")
                    )
                    retType.Categories.Add(FacilityValidator.FailedCat);
                else
                {
                    retType.Categories.Add(FacilityValidator.PassedCat);
                }
                retType.Description = "No candidates in submission match the required classification.\r\n";
                retType.Attributes.AddRange(RequirementAttributes());
                returnWithoutFurtherTests = true;    
            }
            if (returnWithoutFurtherTests)
            {
                retType.SetSubmittedChildrenCount(iSubmitted);
                retType.SetValidChildrenCount(iPassed);
                return retType;
            }

            // ==================== begin testing at parent level

            CachedPropertiesAndAttributesValidator<T> parentCachedValidator;
            try
            {
                parentCachedValidator = new CachedPropertiesAndAttributesValidator<T>(candidateParent);
            }
            catch (ValidationException ex)
            {
                retType.Categories.Add(FacilityValidator.FailedCat);
                retType.Description += ex.Message;
                return retType;
            }

            if (retType.Attributes == null)
                retType.Attributes = new List<Attribute>();

            // produce parent level description
            var outstandingRequirementsCount = 0;
            
            

            foreach (var req in RequirementDetails)
            {
                object satValue;
                var pass = parentCachedValidator.CanSatisfy(req, out satValue);
                var a = targetFacility.Clone(req.Attribute);

                // todo: determine the correct theoretical behaviour; it should probably be null, but needs changes in the reporting mechanism.
                // ReSharper disable once ConvertIfStatementToNullCoalescingExpression
                if (satValue != null)
                    a.Value = AttributeValue.CreateFromObject(satValue);
                else
                    a.Value = AttributeValue.CreateFromObject(""); 

                //// it was previously:
                //if (satValue is AttributeValue)
                //    a.Value = satValue as AttributeValue;
                //else
                //    a.Value = null;

                if (pass)
                {
                    a.Categories = new List<Category> { FacilityValidator.PassedCat };
                }
                else
                {
                    a.Categories = new List<Category> { FacilityValidator.FailedCat };
                    outstandingRequirementsCount++;
                }
                
                retType.Attributes.Add(a);    

            }

            retType.Description = string.Format("{0} of {1} requirement addressed.", RequirementDetails.Count - outstandingRequirementsCount, RequirementDetails.Count);
            var anyChildFails = false;

            // ==================== begin testing at child level
            // 
            if (candidateChildren != null)
            {
                foreach (var modelChild in candidateChildren)
                {
                    var iChildRequirementsMatched = 0;
                    var reportChild = targetFacility.Create<TSub>();
                    reportChild.Name = modelChild.Name;
                    reportChild.ExternalId = modelChild.ExternalId;
                    reportChild.Categories = new List<Category>();
                    reportChild.Attributes = new List<Attribute>();
                
                    // child classification can be copied from model
                    //
                    if (modelChild.Categories != null)
                        reportChild.Categories.AddRange(modelChild.Categories);

                    var childCachedValidator = new CachedPropertiesAndAttributesValidator<TSub>(modelChild);
                    foreach (var req in RequirementDetails)
                    {
                        object satValue;
                        if (childCachedValidator.CanSatisfy(req, out satValue))
                        {
                            // passes locally
                            if (!parentCachedValidator.AlreadySatisfies(req))
                            {
                                iChildRequirementsMatched++;
                            }
                            var a = targetFacility.Clone(req.Attribute);
                            a.Value = AttributeValue.CreateFromObject(satValue);
                            a.Categories = new List<Category> {FacilityValidator.PassedCat};
                            reportChild.Attributes.Add(a);
                        }
                        else if (!parentCachedValidator.AlreadySatisfies(req)) // fails locally, and is not passed at higher level, then add to explicit report fail
                        {
                            var a = targetFacility.Clone(req.Attribute);

                            // todo: determine the correct theoretical behaviour; it should probably be null, but needs changes in the reporting mechanism.

                            // ReSharper disable once ConvertIfStatementToNullCoalescingExpression
                            if (satValue != null)
                                a.Value = AttributeValue.CreateFromObject(satValue);
                            else
                                a.Value = AttributeValue.CreateFromObject(""); 
                                
                            
                            a.Categories = new List<Category> { FacilityValidator.FailedCat };
                            reportChild.Attributes.Add(a);
                        }
                    }

                    var sb = new StringBuilder();
                    sb.AppendFormat("{0} of {1} outstanding requirements addressed.", iChildRequirementsMatched, outstandingRequirementsCount);

                    var pass = (outstandingRequirementsCount == iChildRequirementsMatched);
                    if (!pass)
                    {
                        anyChildFails = true;
                        reportChild.Categories.Add(FacilityValidator.FailedCat);
                    }
                    else
                    {
                        iPassed++;
                        reportChild.Categories.Add(FacilityValidator.PassedCat);
                    }
                    reportChild.Description = sb.ToString();
                    returnChildren.Add(reportChild);
                }
            }
            retType.SetChildObjects(returnChildren);
            retType.Categories.Add(
                anyChildFails ? FacilityValidator.FailedCat : FacilityValidator.PassedCat
                );

            retType.SetSubmittedChildrenCount(iSubmitted);
            retType.SetValidChildrenCount(iPassed);
            return retType;
        }
        
        /// <summary>
        /// Identifies the subset of submitted items that match requirement classifications
        /// </summary>
        /// <param name="submitted"></param>
        /// <returns></returns>
        internal IEnumerable<CobieObjectCategoryMatch> GetCandidates<TReq>(List<TReq> submitted) where TReq : CobieObject
        {
            if (_requirementObject.Categories == null)
                yield break;
 
            var ret = new Dictionary<TReq, List<Category>>();
            foreach (var reqClass in _requirementObject.Categories)
            {
                var thisClassMatch = reqClass.GetClassificationMatches(submitted);
                foreach (var matchedItem in thisClassMatch)
                {
                    var matchedObjectAsTReq = matchedItem.MatchedObject as TReq;
                    if (matchedObjectAsTReq == null)
                        continue;
                    if (!ret.ContainsKey(matchedObjectAsTReq))
                    {
                        ret.Add(matchedObjectAsTReq, matchedItem.MatchingCategories);
                    }
                    else
                    {
                        ret[matchedObjectAsTReq].AddRange(matchedItem.MatchingCategories);
                    }
                }
            }
            foreach (var item in ret)
            {
                yield return new CobieObjectCategoryMatch(item.Key) { MatchingCategories = item.Value };
            }
        }

        internal T Validate(CobieObjectCategoryMatch candidateMatch, Facility targetFacility)
        {
            var candidateT = candidateMatch.MatchedObject as T;
            if (candidateT == null)
                return null;
            var validated = Validate(candidateT, targetFacility);
            validated.SetMatchingCategories(candidateMatch.MatchingCategories);
            return validated;
        }
    }
}
