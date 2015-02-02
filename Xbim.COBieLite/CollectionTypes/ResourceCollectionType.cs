using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.COBieLite
{

    public partial class ResourceCollectionType : ICollectionType<ResourceType>, IEnumerable<ResourceType>
    {
        public IEnumerator<ResourceType> GetEnumerator()
        {
            return this.Resource.OfType<ResourceType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<ResourceType> InnerList
        {
            get { return Resource; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Resource.OfType<ResourceType>().GetEnumerator();
        }
    }
}
