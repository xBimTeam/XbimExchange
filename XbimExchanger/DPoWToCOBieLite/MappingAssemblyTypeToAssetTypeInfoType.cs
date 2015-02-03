using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.DPoWToCOBieLite
{
    class MappingAssemblyTypeToAssetTypeInfoType : DPoWToCOBieLiteMapping<Xbim.DPoW.Interfaces.AssemblyType, AssetTypeInfoType>
    {
        protected override AssetTypeInfoType Mapping(Xbim.DPoW.Interfaces.AssemblyType source, AssetTypeInfoType target)
        {
            target.externalID = Exchanger.GetStringIdentifier();
            target.AssetTypeName = source.DPoWObjectName;
            target.AssetTypeDescription = source.DPoWObjectDescription;
            target.AssetTypeCategory = source.DPoWObjectCategory != null ? source.DPoWObjectCategory.ClassificationCode : null;
            target.AssetTypeAttributes = new AttributeCollectionType();
            if (source.RequiredLOD != null)
            {
                var lod = source.RequiredLOD;
                target.AssetTypeShapeDescription = lod.RequiredLODCode;
                target.AssetTypeAttributes.Add("RequiredLODCode", "Required LOD Code", lod.RequiredLODCode, "RequiredLOD");
                target.AssetTypeAttributes.Add("RequiredLODDescription", "Required LOD Description", lod.RequiredLODDescription, "RequiredLOD");
                target.AssetTypeAttributes.Add("RequiredLODURI", "Required LOD URI", lod.RequiredLODURI, "RequiredLOD");
            }
            if (source.RequiredAttributes != null)
            {
                var attrs = new List<AttributeType>();
                foreach (var sAttr in source.RequiredAttributes)
                    attrs.Add(new AttributeType() { propertySetName = "RequiredAttributes", AttributeName = sAttr.AttributeName, AttributeDescription = sAttr.AttributeDescription });
                target.AssetTypeAttributes.AddRange(attrs);
            }


            return target;
        }

        public static string GetKey(Xbim.DPoW.Interfaces.AssemblyType assembly)
        {
            var attrCount = assembly.RequiredAttributes != null ? assembly.RequiredAttributes.Count : 0;
            var lodCode = assembly.RequiredLOD != null ? assembly.RequiredLOD.RequiredLODCode : "";
            return String.Format("{0} {1} {2} {3}", assembly.DPoWObjectName, assembly.DPoWObjectDescription, attrCount, lodCode);
        }
    }
}
