using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Xbim.COBieLite
{
    [JsonObject]
    public  partial class AllowedValueCollectionType : ICollectionType<string>, IEnumerable<string>
    {
        public IEnumerator<string> GetEnumerator()
        {
            return AttributeAllowedValue.OfType<string>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<string> InnerList
        {
            get { return AttributeAllowedValue; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return AttributeAllowedValue.OfType<string>().GetEnumerator();
        }
    }
}
