using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation
{
    public class FacilityValidator : IValidator
    {
        public FacilityValidator()
        {
            TerminationMode = TerminationMode.ExecuteCompletely;
        }

        public Facility Validate(Facility requirement, Facility submitted)
        {
            var retFacility = new Facility();

            // a facility validation passes is carried out through the validation of
            // assetTypes
            // spaces
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
            return retFacility;
        }

        public TerminationMode TerminationMode { get; set; }
        public bool HasFailures { get; set; }
    }
}
