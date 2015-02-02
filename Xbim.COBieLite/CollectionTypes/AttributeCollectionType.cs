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
    public partial class AttributeCollectionType : ICollectionType<AttributeType>, IEnumerable<AttributeType>
    {
        public IEnumerator<AttributeType> GetEnumerator()
        {
            return this.Attribute.OfType<AttributeType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<AttributeType> InnerList
        {
            get { return Attribute; }
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Attribute.OfType<AttributeType>().GetEnumerator();
        }
    }
}
