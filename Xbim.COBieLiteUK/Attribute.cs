using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xbim.COBieLiteUK.Converters;
using System.IO;
using System.Linq;

namespace Xbim.COBieLiteUK
{
    public partial class Attribute
    {
        /// <summary>
        /// PropertySetName is a proxy property for ExternalEntity. It is not a part of the schema but it is part of the API to make sure the
        /// data schema is used in an uniform way.
        /// </summary>
        [XmlIgnore][JsonIgnore]
        public string PropertySetName { get { return ExternalEntity; } set { ExternalEntity = value; } }

        [JsonIgnore]
        [global::System.Runtime.Serialization.DataMember]
        public StageType StageType
        {
            get
            {
                if (Categories == null || !Categories.Any()) return StageType.notdefined;
                var type = Categories.FirstOrDefault(c => c.Classification == "StageType" || c.Classification == null);
                return type == null ? StageType.notdefined : GetEnumeration<StageType>(type.Code);
            }
            set
            {
                SetEnumeration(value, s =>
                {
                    if (Categories == null) Categories = new List<Category>();
                    var type = Categories.FirstOrDefault(c => c.Classification == "StageType" || c.Classification == null);
                    if (type == null)
                    {
                        type = new Category ();
                        Categories.Add(type);
                    }
                    type.Code = s;
                });
            }
        }
    }
}
