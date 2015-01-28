using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.COBieLite
{
    [JsonObject]
    public partial class ContactCollectionType : ICollectionType<ContactType>
    {
        public IEnumerator<ContactType> GetEnumerator()
        {
            return this.Contact.OfType<ContactType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<ContactType> InnerList
        {
            get { return Contact; }
        }
    }
}
