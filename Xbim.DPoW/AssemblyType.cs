using System;
using System.Collections.Generic;
using System.Linq;

namespace Xbim.DPoW
{
    /// <summary>
    /// Assembly type represents aggregation of asset types
    /// </summary>
    public class AssemblyType : DPoWObject
    {
        /// <summary>
        /// List of Asset types which are agregated in this assembly. This set should always contain at least one asset type.
        /// </summary>
        public List<Guid> AggregationOfAssetTypes { get; set; }

        /// <summary>
        /// Gets real asset types which should live in the same project stage as this assembly
        /// </summary>
        /// <param name="stage">Project stage where this assembly lives alongside with asset types it aggregates.</param>
        /// <returns>Enumeration of aggregated asset types</returns>
        public IEnumerable<AssetType> GetAggregationOfAssetTypes(ProjectStage stage)
        {
            if (stage.AssetTypes == null || AggregationOfAssetTypes == null) return new AssetType[0];
            return stage.AssetTypes.Where(at => AggregationOfAssetTypes.Contains(at.Id));
        }
    }
}
