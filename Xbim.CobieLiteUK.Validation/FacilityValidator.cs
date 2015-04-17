using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;
using Xbim.CobieLiteUK.Validation.Extensions;

namespace Xbim.CobieLiteUK.Validation
{
    internal enum StatusOptions
    {
        NotEvaluated,
        Passed,
        Failed,
        Invalid
    }

    /// <summary>
    /// Used to produce a COBieLiteUK Facility with validation information of a submitted facility against a requirement facility. 
    /// </summary>
    public class FacilityValidator : IValidator
    {
        internal const string TemporaryFloorName = @"DPoWTemporaryZoneValidationSpaceHoldingFloor";

        /// <summary>
        /// Empty default constructor.
        /// </summary>
        public FacilityValidator()
        {
            TerminationMode = TerminationMode.ExecuteCompletely;
            HasFailures = false;
        }

        internal static readonly Category FailedCat = new Category
        {
            Classification = "DPoW",
            Code = "Failed"
        };

        internal static readonly Category PassedCat = new Category
        {
            Classification = "DPoW",
            Code = "Passed"
        };

        /// <summary>
        /// Performs the validation
        /// </summary>
        /// <param name="requirement">a requirement facility</param>
        /// <param name="submitted">the submission model to be validated</param>
        /// <returns></returns>
        public Facility Validate(Facility requirement, Facility submitted)
        {
            var retFacility = new Facility {Categories = new List<Category>()};
            var sb = new StringBuilder();

            // facility validation is carried out through the validation of:
            // a) local values
            // b) Project
            // c) AssetTypes/Assets
            // d) Zones/Spaces 
            // e) Documents

            // a)
            // area units
            if (requirement.AreaUnitsCustom != submitted.AreaUnitsCustom)
            {
                retFacility.AreaUnitsCustom = string.Format("{0} (should be '{1}')", submitted.AreaUnitsCustom, requirement.AreaUnitsCustom);
                sb.AppendFormat("{0} failure: {1}\r\n", "Area units", retFacility.AreaUnitsCustom);
                HasFailures = true;
            }
            else
            {
                retFacility.AreaUnitsCustom = submitted.AreaUnitsCustom;
            }

            // linear units
            if (requirement.LinearUnitsCustom != submitted.LinearUnitsCustom)
            {
                retFacility.LinearUnitsCustom = string.Format("{0} (should be '{1}')", submitted.LinearUnitsCustom, requirement.LinearUnitsCustom);
                sb.AppendFormat("{0} failure: {1}\r\n", "Linear units", retFacility.LinearUnitsCustom);
                HasFailures = true;
            }
            else
            {
                retFacility.LinearUnitsCustom = submitted.LinearUnitsCustom;
            }

            // Volume units
            if (requirement.VolumeUnitsCustom != submitted.VolumeUnitsCustom)
            {
                retFacility.VolumeUnitsCustom = string.Format("{0} (should be '{1}')", submitted.VolumeUnitsCustom, requirement.VolumeUnitsCustom);
                sb.AppendFormat("{0} failure: {1}\r\n", "Volume units", retFacility.VolumeUnitsCustom);
                HasFailures = true;
            }
            else
            {
                retFacility.VolumeUnitsCustom = submitted.VolumeUnitsCustom;
            }

            // Currency units
            if (requirement.CurrencyUnitCustom != submitted.CurrencyUnitCustom)
            {
                retFacility.CurrencyUnitCustom = string.Format("{0} (should be '{1}')", submitted.CurrencyUnitCustom, requirement.CurrencyUnitCustom);
                sb.AppendFormat("{0} failure: {1}\r\n", "Currency units", retFacility.CurrencyUnitCustom);
                HasFailures = true;
            }
            else
            {
                retFacility.CurrencyUnitCustom = submitted.CurrencyUnitCustom;
            }

            if (requirement.Project != null)
            {
                // to be added project level validation here.
                var pv = new ProjectValidator(requirement.Project);
                retFacility.Project = pv.Validate(submitted.Project);

                if (pv.HasFailures)
                {
                    sb.AppendFormat("Validation of Project information fails, see project information for detail.\r\n");
                    HasFailures = true;
                }
            }
            // c) AssetTypes/Assets
            HasFailures |= ValidateAssetTypes(requirement, submitted, retFacility);
            // d) Zones/Spaces
            HasFailures |= ValidateZones(requirement, submitted, retFacility);

            // e) Documents
            HasFailures |= ValidateDocuments(requirement, submitted, retFacility);

            retFacility.Description = sb.ToString();
            retFacility.Categories.Add(HasFailures ? FailedCat : PassedCat);

            return retFacility;
        }

        private bool ValidateDocuments(Facility requirement, Facility submitted, Facility retFacility)
        {
            if (requirement.Documents == null)
                return true;
            var dv = new DocumentsValidator(requirement.Documents, retFacility) {TerminationMode = TerminationMode};
            if (retFacility.Documents == null)
                retFacility.Documents = new List<Document>();
            var bAnyDocs = false;
            foreach (var doc in dv.ValidatedDocs(submitted.Documents))
            {
                bAnyDocs = true;
                retFacility.Documents.Add(doc);
            }
            if (!bAnyDocs)
                retFacility.Documents = null; // if empty then remove the list for cleanness
            return dv.HasFailures;
        }

        
        
        /// <returns>true if it has failures</returns>
        private bool ValidateAssetTypes(Facility requirement, Facility submitted, Facility retFacility)
        {
            if (requirement.AssetTypes == null) 
                return true;
            var ret = false;
            foreach (var assetTypeRequirement in requirement.AssetTypes)
            {
                var v = new CobieObjectValidator<AssetType, Asset>(assetTypeRequirement)
                {
                    TerminationMode = TerminationMode
                };
                if (! v.HasRequirements)
                    continue;
                var candidates = v.GetCandidates(submitted.AssetTypes).ToList();
                
                if (candidates.Any())
                {
                    foreach (var candidate in candidates)
                    {
                        if (retFacility.AssetTypes == null)
                            retFacility.AssetTypes = new List<AssetType>();
                        retFacility.AssetTypes.Add(v.Validate(candidate, retFacility));
                    }
                }
                else
                {
                    if (retFacility.AssetTypes == null)
                        retFacility.AssetTypes = new List<AssetType>();
                    retFacility.AssetTypes.Add(v.Validate((AssetType) null, retFacility));
                }
                ret |= v.HasFailures;
            }
            return ret;
        }

        /// <returns>true if it has failures</returns>
        private bool ValidateZones(Facility requirement, Facility submitted, Facility retFacility)
        {
            if (requirement.Zones == null) 
                return false;
            var ret = false;
            // hack: create a fake modelFacility candidates from spaces.
            var fakeSubmittedFacility = new Facility();
            fakeSubmittedFacility.Floors = fakeSubmittedFacility.Clone(submitted.Floors as IEnumerable<Floor>).ToList();
            fakeSubmittedFacility.Zones = new List<Zone>();
            var lSpaces = submitted.Get<Space>().ToList();

            foreach (var zoneRequirement in requirement.Zones)
            {
                var v = new CobieObjectValidator<Zone, Space>(zoneRequirement)
                {
                    TerminationMode = TerminationMode
                };
                if (! v.HasRequirements)
                    continue;

                // hack: now create a fake Zone based on candidates from spaces.
                var fakeZone = fakeSubmittedFacility.Create<Zone>();
                fakeZone.Categories = zoneRequirement.Categories.Clone().ToList();
                fakeZone.Name = zoneRequirement.Name;
                fakeZone.ExternalEntity = zoneRequirement.ExternalEntity;
                fakeZone.ExternalSystem = zoneRequirement.ExternalSystem;
                fakeZone.ExternalId = zoneRequirement.ExternalId;
                fakeZone.Spaces = new List<SpaceKey>();

                var candidateSpaces = v.GetCandidates(lSpaces).ToList();

                if (candidateSpaces.Any())
                {
                    foreach (var spaceMatch in candidateSpaces)
                    {
                        var mSpace = spaceMatch.MatchedObject as Space;
                        if (mSpace == null)
                            continue;
                        var sk = new SpaceKey {Name = mSpace.Name};
                        fakeZone.Spaces.Add(sk);
                    }
                    if (retFacility.Zones == null)
                        retFacility.Zones = new List<Zone>();
                    var validatedZone = v.Validate(fakeZone, retFacility);
                    retFacility.Zones.Add(validatedZone);
                    var tmpFloor = retFacility.Get<Floor>(fl => fl.Name == TemporaryFloorName).FirstOrDefault();
                    if (tmpFloor == null)
                        continue;
                    // ensure that the floor and spaces are avalialale in the report facility
                    foreach (var spaceKey in validatedZone.Spaces)
                    {
                        // 1) on the floor
                        var submissionOwningFloor =
                            submitted.Get<Floor>(f => f.Spaces != null && f.Spaces.Any(sp => sp.Name == spaceKey.Name)).FirstOrDefault();
                        if (submissionOwningFloor == null)
                            continue;
                        var destFloor = retFacility.Get<Floor>(f => f.Name == submissionOwningFloor.Name).FirstOrDefault();
                        if (destFloor == null)
                        {
                            destFloor = retFacility.Create<Floor>();
                            destFloor.Name = submissionOwningFloor.Name;
                            destFloor.ExternalEntity = submissionOwningFloor.ExternalEntity;
                            destFloor.ExternalId = submissionOwningFloor.ExternalId;
                            destFloor.ExternalSystem = submissionOwningFloor.ExternalSystem;
                            destFloor.Elevation = submissionOwningFloor.Elevation;
                            destFloor.Spaces = new List<Space>();
                            retFacility.Floors.Add(destFloor); // finally add it in the facility tree
                        }
                        // 2) now on the space.

                        var sourceSpace = tmpFloor.Spaces.FirstOrDefault(sp => sp.Name == spaceKey.Name);
                        if (sourceSpace != null)
                        {
                            destFloor.Spaces.Add(sourceSpace);        
                        }
                    }
                    retFacility.Floors.Remove(tmpFloor);
                }
                else
                {
                    if (retFacility.Zones == null)
                        retFacility.Zones = new List<Zone>();
                    retFacility.Zones.Add(v.Validate((Zone) null, retFacility));
                }
                ret |= v.HasFailures;   
            }
            return ret;
        }

        /// <summary>
        /// Determines behaviour that regulate the conclusion of the validation process.
        /// </summary>
        public TerminationMode TerminationMode { get; set; }

        /// <summary>
        /// true if the validator has encountered failures in the data.
        /// </summary>
        public bool HasFailures { get; private set; }
    }
}
