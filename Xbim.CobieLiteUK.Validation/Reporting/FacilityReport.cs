using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;
using Xbim.CobieLiteUK.Validation.Extensions;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    public class FacilityReport
    {
        /// <summary>
        /// The specified classification takes precedence over others for reporting purposes, when appropriate.
        /// </summary>
        public string PreferredClassification = "Uniclass2015";

        private Facility _facility;

        public FacilityReport(Facility facility)
        {
            _facility = facility;
        }

        private List<AssetTypeRequirementPointer> _RequirementGroups;

        internal List<AssetTypeRequirementPointer> RequirementGroups
        {
            get
            {
                if (_RequirementGroups == null)
                {
                    var tmp = new Dictionary<Tuple<string, string>, AssetTypeRequirementPointer>();
                    _RequirementGroups = new List<AssetTypeRequirementPointer>();
                    if (_facility.AssetTypes != null)
                    {
                        foreach (var providedAsset in _facility.AssetTypes)
                        {
                            var sys = providedAsset.GetRequirementExternalSystem();
                            var id = providedAsset.GetRequirementExternalId();
                            var nm = providedAsset.GetRequirementName();

                            var tryReq = new Tuple<string, string>(sys, id);
                            if (tmp.ContainsKey(tryReq))
                                tmp[tryReq].AddSumission(providedAsset);
                            else
                            {
                                var newP = new AssetTypeRequirementPointer(sys, id, nm);
                                newP.AddSumission(providedAsset);
                                tmp.Add(tryReq, newP);
                            }
                        }
                        _RequirementGroups = tmp.Values.ToList();
                    }
                }
                return _RequirementGroups;
            }
        }
    }
}
