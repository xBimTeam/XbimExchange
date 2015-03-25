using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.CobieLite.Validation.Extensions;
using Xbim.CobieLite.Validation.RequirementDetails;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLite.Validation
{
    class AssetTypeValidator : IValidator
    {
        private AssetType _requirementType;

        public AssetTypeValidator(AssetType requirementType)
        {
            _requirementType = requirementType;
        }

        private List<RequirmentDetail> _requirementDetails;

        internal List<RequirmentDetail> RequirementDetails
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
            _requirementDetails = new List<RequirmentDetail>();
            foreach (var attrib in _requirementType.Attributes.Where(x => x.Categories.Any(c=>c.Classification=="DPoW" && c.Code=="required" )))
            {
                _requirementDetails.Add(new RequirmentDetail(attrib));
            }
        }

        public TerminationMode TerminationMode { get; set; }

        public bool HasFailures { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidateType">If null provides a missing match report</param>
        /// <returns></returns>
        internal AssetType Validate(AssetType candidateType)
        {
            // initialise returning assetType
            var retType = new AssetType
            {
                Categories = _requirementType.Categories, 
                Name = _requirementType.Name
            };

            if (!RequirementDetails.Any())
            {
                // todo: there's no requirement for the specified class
                return retType;
            }

            // if candidate is null then consider there's no match
            if (candidateType == null)
            {
                // todo: add information about missing candidates
                return retType;
            }
            return retType;
        }

        internal IEnumerable<AssetType> GetCandidates(Facility submitted)
        {
            var reqClass = _requirementType.Categories.FirstOrDefault();
            return submitted.AssetTypes.GetClassificationSubset(reqClass);
        }
    }
}
