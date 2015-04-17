using System;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLiteUK
{
    public partial class EntityKey : IEntityKey
    {
        [XmlIgnore, JsonIgnore]
        public Type ForType
        {
            get {
                switch (KeyType)
                {
                    case EntityType.Contact:
                        return typeof(Contact);
                    case EntityType.Facility:
                        return typeof(Facility);
                    case EntityType.Floor:
                        return typeof(Floor);
                    case EntityType.Space:
                        return typeof(Space);
                    case EntityType.Zone:
                        return typeof(Zone);
                    case EntityType.AssetType:
                        return typeof(AssetType);
                    case EntityType.Asset:
                        return typeof(Asset);
                    case EntityType.System:
                        return typeof(System);
                    case EntityType.Spare:
                        return typeof(Spare);
                    case EntityType.Resource:
                        return typeof(Resource);
                    case EntityType.ProjectStage:
                        return typeof(ProjectStage);
                    case EntityType.Notdefined:
                        return typeof(CobieObject);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
             }
        }

        public string GetSheet(string mapping)
        {
            var attr =
                ForType.GetCustomAttributes(typeof(SheetMappingAttribute), true)
                    .FirstOrDefault(a => ((SheetMappingAttribute)a).Type == mapping) as SheetMappingAttribute;
            return attr == null ? null : attr.Sheet;
        }
    }
}
