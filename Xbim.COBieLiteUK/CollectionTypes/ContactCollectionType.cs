using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xbim.COBieLite.CollectionTypes;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLiteUK
{
    [JsonObject]
    public partial class ContactCollectionType : ICollectionType<ContactType>, IEnumerable<ContactType>
    {
        public IEnumerator<ContactType> GetEnumerator()
        {
            return Contact.OfType<ContactType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<ContactType> InnerList
        {
            get { return Contact; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Contact.OfType<ContactType>().GetEnumerator();
        }
    }
}
