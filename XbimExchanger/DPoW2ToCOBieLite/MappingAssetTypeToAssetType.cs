using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLite;
using Xbim.DPoW;

namespace XbimExchanger.DPoW2ToCOBieLite
{
    public class MappingAssetTypeToAssetType: DPoWToCOBieLiteMapping<AssetType, AssetTypeInfoType>
    {
        protected override AssetTypeInfoType Mapping(AssetType source, AssetTypeInfoType target)
        {
            throw new NotImplementedException();
        }
    }
}
