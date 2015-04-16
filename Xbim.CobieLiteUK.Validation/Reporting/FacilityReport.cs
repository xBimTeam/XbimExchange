using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.CobieLiteUK.Validation.Extensions;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    /// <summary>
    /// Used to produce facility reports in formats other than the standards provided.
    /// See ExcelValidationReport for the excel format.
    /// </summary>
    public class FacilityReport
    {
        /// <summary>
        /// The specified classification takes precedence over others for reporting purposes, when appropriate.
        /// </summary>
        public string PreferredClassification = "Uniclass2015";

        private readonly Facility _validationResult;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="validationResultFacility">The result of a validation process to report over.</param>
        public FacilityReport(Facility validationResultFacility)
        {
            _validationResult = validationResultFacility;
        }

        private List<AssetTypeRequirementPointer<AssetType, Asset>> _assetRequirementGroups;

        internal List<AssetTypeRequirementPointer<AssetType, Asset>> AssetRequirementGroups
        {
            get
            {
                if (_assetRequirementGroups != null) 
                    return _assetRequirementGroups;
                var tmp = new Dictionary<Tuple<string, string>, AssetTypeRequirementPointer<AssetType, Asset>>();
                _assetRequirementGroups = new List<AssetTypeRequirementPointer<AssetType, Asset>>();
                if (_validationResult.AssetTypes == null) 
                    return _assetRequirementGroups;
                foreach (var providedAsset in _validationResult.AssetTypes)
                {
                    var sys = providedAsset.GetRequirementExternalSystem();
                    var id = providedAsset.GetRequirementExternalId();
                    var nm = providedAsset.GetRequirementName();

                    var tryReq = new Tuple<string, string>(sys, id);
                    if (tmp.ContainsKey(tryReq))
                        tmp[tryReq].AddSumission(providedAsset);
                    else
                    {
                        var newP = new AssetTypeRequirementPointer<AssetType, Asset>(sys, id, nm);
                        newP.AddSumission(providedAsset);
                        tmp.Add(tryReq, newP);
                    }
                }
                _assetRequirementGroups = tmp.Values.ToList();
                return _assetRequirementGroups;
            }
        }

        private List<AssetTypeRequirementPointer<Zone, Space>> _zoneRequirementGroups;

        internal List<AssetTypeRequirementPointer<Zone, Space>> ZoneRequirementGroups
        {
            get
            {
                if (_zoneRequirementGroups != null)
                    return _zoneRequirementGroups;
                var tmp = new Dictionary<Tuple<string, string>, AssetTypeRequirementPointer<Zone, Space>>();
                _zoneRequirementGroups = new List<AssetTypeRequirementPointer<Zone, Space>>();
                if (_validationResult.Zones == null)
                    return _zoneRequirementGroups;
                foreach (var providedAsset in _validationResult.Zones)
                {
                    var sys = providedAsset.GetRequirementExternalSystem();
                    var id = providedAsset.GetRequirementExternalId();
                    var nm = providedAsset.GetRequirementName();

                    var tryReq = new Tuple<string, string>(sys, id);
                    if (tmp.ContainsKey(tryReq))
                        tmp[tryReq].AddSumission(providedAsset);
                    else
                    {
                        var newP = new AssetTypeRequirementPointer<Zone, Space>(sys, id, nm);
                        newP.AddSumission(providedAsset);
                        tmp.Add(tryReq, newP);
                    }
                }
                _zoneRequirementGroups = tmp.Values.ToList();
                return _zoneRequirementGroups;
            }
        }
    }
}
