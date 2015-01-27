using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.DPoWToCOBieLite
{
    class MappingAssetTypeToAssetTypeInfoType: DPoWToCOBieLiteMapping<AssetType, AssetTypeInfoType>
    {
        protected override AssetTypeInfoType Mapping(AssetType source, AssetTypeInfoType target)
        {
            target.externalID = Exchanger.GetStringIdentifier();
            target.AssetTypeName = source.DPoWObjectName;
            target.AssetTypeDescription = source.DPoWObjectDescription;
            target.AssetTypeCategory = source.DPoWObjectCategory != null ? source.DPoWObjectCategory.ClassificationCode : null;
            target.AssetTypeAttributes = new AttributeCollectionType();
            if (source.RequiredLOD != null)
            {
                var lod = source.RequiredLOD;
                target.AssetTypeAttributes.Add("RequiredLODCode", "Required LOD Code", lod.RequiredLODCode, "RequiredLOD");
                target.AssetTypeAttributes.Add("RequiredLODDescription", "Required LOD Description", lod.RequiredLODDescription, "RequiredLOD");
                target.AssetTypeAttributes.Add("RequiredLODURI", "Required LOD URI", lod.RequiredLODURI, "RequiredLOD");
            }
            if (source.RequiredAttributes != null)
            {
                var attrs = new List<AttributeType>();
                foreach (var sAttr in source.RequiredAttributes)
                    attrs.Add(new AttributeType() { propertySetName = "RequiredAttributes", AttributeName = sAttr.AttributeName, AttributeDescription = sAttr.AttributeDescription});
                target.AssetTypeAttributes.Add(attrs);
            }
                

            return target;
        }

        public static string GetKey(AssetType asset)
        {
            var attrCount = asset.RequiredAttributes != null? asset.RequiredAttributes.Count : 0;
            var lodCode = asset.RequiredLOD != null ? asset.RequiredLOD.RequiredLODCode : "";
            return String.Format("{0} {1} {2} {3}", asset.DPoWObjectName, asset.DPoWObjectDescription, attrCount, lodCode);
        }
    }
}
