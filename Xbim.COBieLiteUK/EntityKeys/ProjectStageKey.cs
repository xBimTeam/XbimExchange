using System;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLiteUK
{
    public partial class ProjectStageKey : IEntityKey
    {
        [XmlIgnore, JsonIgnore]
        public Type ForType
        {
            get { return typeof(ProjectStage); }
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
