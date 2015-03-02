using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Xbim.COBieLite.CollectionTypes;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLite
{
    [JsonObject]
    public partial class AttributeCollectionType : ICollectionType<AttributeType>, IEnumerable<AttributeType>
    {
        public IEnumerator<AttributeType> GetEnumerator()
        {
            return Attribute.OfType<AttributeType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<AttributeType> InnerList
        {
            get { return Attribute ?? (Attribute = new List<AttributeType>()); }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return Attribute.OfType<AttributeType>().GetEnumerator();
        }
    }
}
