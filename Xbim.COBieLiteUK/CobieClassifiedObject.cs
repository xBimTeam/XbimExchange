using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.COBieLiteUK
{
    public abstract partial class CobieClassifiedObject
    {
        [XmlIgnore, JsonIgnore]
        public override string ObjectType
        {
            get
            {
                if (Categories == null || !Categories.Any()) return null;
                var typeCat = Categories.FirstOrDefault(c => c.Classification == null);
                return typeCat != null ? typeCat.Code : null;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value) && (Categories == null || !Categories.Any())) return;

                var typeCat = Categories != null ? Categories.FirstOrDefault(c => c.Classification == null) : null;
                //remove default category if defined
                if (String.IsNullOrWhiteSpace(value) && typeCat != null)
                    Categories.Remove(typeCat);

                if (typeCat == null)
                {
                    typeCat = new Category();
                    if (Categories == null)
                        Categories = new List<Category> { typeCat };
                    else
                     Categories.Add(typeCat);   
                }
                typeCat.Code = value;
            }
        }
    }
}