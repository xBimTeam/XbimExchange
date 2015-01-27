using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.DPoWToCOBieLite
{
    class MappingZoneToZoneType : DPoWToCOBieLiteMapping<Zone, ZoneType>
    {
        protected override ZoneType Mapping(Zone source, ZoneType target)
        {
            target.externalID = Exchanger.GetStringIdentifier();
            target.ZoneName = source.DPoWObjectName;
            target.ZoneDescription = source.DPoWObjectDescription;
            target.ZoneCategory = source.DPoWObjectCategory != null ? source.DPoWObjectCategory.ClassificationCode : null;
            target.ZoneAttributes = new AttributeCollectionType();

            if (source.RequiredLOD != null)
            {
                var lod = source.RequiredLOD;
                target.ZoneAttributes.Add("RequiredLODCode", "Required LOD Code", lod.RequiredLODCode);
                target.ZoneAttributes.Add("RequiredLODDescription", "Required LOD Description", lod.RequiredLODDescription);
                target.ZoneAttributes.Add("RequiredLODURI", "Required LOD URI", lod.RequiredLODURI);
            }
            if (source.RequiredAttributes != null)
                foreach (var sArrt in source.RequiredAttributes)
                    target.ZoneAttributes.Add(sArrt.AttributeName, sArrt.AttributeDescription);

            return target;
        }

        public static string GetKey(Zone zone)
        {
            return string.Format("{2} {0} {1}", zone.DPoWObjectName, zone.DPoWObjectDescription, zone.DPoWObjectCategory != null? zone.DPoWObjectCategory.ClassificationCode : "");
        }
    }
}
