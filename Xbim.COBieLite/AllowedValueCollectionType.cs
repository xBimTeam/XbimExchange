using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.COBieLite
{
    [JsonObject]
    public  partial class AllowedValueCollectionType : ICollectionType<string>
    {
        public IEnumerator<string> GetEnumerator()
        {
            return this.AttributeAllowedValue.OfType<string>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<string> InnerList
        {
            get { return AttributeAllowedValue; }

        }
    }
}
