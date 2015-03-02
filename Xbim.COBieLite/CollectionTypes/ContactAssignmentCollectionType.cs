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
    public partial class ContactAssignmentCollectionType : ICollectionType<ContactKeyType>, IEnumerable<ContactKeyType>
    {
        public IEnumerator<ContactKeyType> GetEnumerator()
        {
            return ContactAssignment.OfType<ContactKeyType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<ContactKeyType> InnerList
        {
            get { return ContactAssignment ?? (ContactAssignment = new List<ContactKeyType>()); }

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ContactAssignment.OfType<ContactKeyType>().GetEnumerator();
        }
    }
}
