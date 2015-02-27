using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xbim.COBieLite.CollectionTypes;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLite
{

    public partial class ResourceCollectionType : ICollectionType<ResourceType>, IEnumerable<ResourceType>
    {
        public IEnumerator<ResourceType> GetEnumerator()
        {
            return Resource.OfType<ResourceType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<ResourceType> InnerList
        {
            get { return Resource ?? (Resource = new List<ResourceType>()); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Resource.OfType<ResourceType>().GetEnumerator();
        }
    }
}
