using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.XbimExtensions.Interfaces;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingAssetInfoTypeToIfcElement<TToObject> : CoBieLiteIfcMappings<string, AssetInfoType, TToObject> where TToObject : IfcElement, new()
    {

        protected override TToObject Mapping(AssetInfoType assetInfoType, TToObject ifcElement)
        {
            ifcElement.Name = assetInfoType.AssetName;
            ifcElement.Description = assetInfoType.AssetDescription;
            return ifcElement;
        }
    }
}
