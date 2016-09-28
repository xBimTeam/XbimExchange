using System.Collections.Generic;
using Xbim.COBieLite;

namespace XbimExchanger.COBieLiteHelpers
{
    class AssetTypeInfoTypeComparer:IComparer<AssetTypeInfoType>
    {
        public int Compare(AssetTypeInfoType x, AssetTypeInfoType y)
        {
            return System.String.Compare(x.externalEntityName, y.externalEntityName, System.StringComparison.Ordinal);
        }
    }
}
