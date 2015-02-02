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
    public partial class ContactAssignmentCollectionType : ICollectionType<ContactKeyType>, IEnumerable<ContactKeyType>
    {
        public IEnumerator<ContactKeyType> GetEnumerator()
        {
            return this.ContactAssignment.OfType<ContactKeyType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<ContactKeyType> InnerList
        {
            get { return ContactAssignment; }

        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.ContactAssignment.OfType<ContactKeyType>().GetEnumerator();
        }
    }
}
