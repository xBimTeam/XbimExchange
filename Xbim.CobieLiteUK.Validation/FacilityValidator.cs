using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation
{
    public class FacilityValidator : IValidator
    {
        public FacilityValidator()
        {
            TerminationMode = TerminationMode.ExecuteCompletely;
        }

        internal static readonly Category FailedCat = new Category()
        {
            Classification = "DPoW",
            Code = "Failed"
        };

        internal static readonly Category PassedCat = new Category()
        {
            Classification = "DPoW",
            Code = "Passed"
        };

        public Facility Validate(Facility requirement, Facility submitted)
        {
            var retFacility = new Facility();
            retFacility.Categories = new List<Category>();
            StringBuilder sb = new StringBuilder();

            // a facility validation passes is carried out through the validation of
            // a) local values
            // b) Project
            // b) assetTypes (WIP)
            // d) spaces (planned)

            // a)
            bool FacilityPasses = true;

            if (RequiredStringMatch(requirement.AreaUnitsCustom, submitted.AreaUnitsCustom, "Area units", retFacility.AreaUnitsCustom, sb))
                FacilityPasses = false;
            if (RequiredStringMatch(requirement.LinearUnitsCustom, submitted.LinearUnitsCustom, "Linear units", retFacility.LinearUnitsCustom, sb))
                FacilityPasses = false;
            if (RequiredStringMatch(requirement.VolumeUnitsCustom, submitted.VolumeUnitsCustom, "Volume units", retFacility.VolumeUnitsCustom, sb))
                FacilityPasses = false;
            if (RequiredStringMatch(requirement.CurrencyUnitCustom, submitted.CurrencyUnitCustom, "Currency units", retFacility.CurrencyUnitCustom, sb))
                FacilityPasses = false;
            
            // to be added project level validation here.
            

            // b)
            foreach (var assetType in requirement.AssetTypes)
            {
                var v = new AssetTypeValidator(assetType) { TerminationMode = TerminationMode };
                if (! v.HasRequirements )
                    continue;
                var candidates = v.GetCandidates(submitted);
                // ReSharper disable once PossibleMultipleEnumeration
                if (candidates.Any())
                {
                    foreach (var candidate in candidates)
                    {
                        if (retFacility.AssetTypes == null)
                            retFacility.AssetTypes  = new List<AssetType>();
                        retFacility.AssetTypes.Add(v.Validate(candidate));
                    }
                }
                else
                {
                    if (retFacility.AssetTypes == null)
                        retFacility.AssetTypes = new List<AssetType>();
                    retFacility.AssetTypes.Add(v.Validate(null));
                }
            }
            retFacility.Description = sb.ToString();
            retFacility.Categories.Add(FacilityPasses ? PassedCat : FailedCat);

            return retFacility;
        }

        private static bool RequiredStringMatch(string requiredString, string providedString, string propertyName, String targetString, StringBuilder sb)
        {
            // todo: the calling mechanism needs to be changed.
            if (requiredString == providedString)
            {
                targetString = requiredString;
                return true;
            }
            targetString = string.Format("{0} (should be '{1}')", providedString, requiredString);
            sb.AppendFormat("{0} failure: {1}\r\n", propertyName, targetString);
            return false;
        }

        public TerminationMode TerminationMode { get; set; }
        public bool HasFailures { get; set; }
    }
}
