using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    public class AssetTypeSummaryReport
    {
        private IEnumerable<AssetType> _requirementAssets;

        public AssetTypeSummaryReport(IEnumerable<AssetType> requirementAssets)
        {
            _requirementAssets = requirementAssets;
        }
    }
}
