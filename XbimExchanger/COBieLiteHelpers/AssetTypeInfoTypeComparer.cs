using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
