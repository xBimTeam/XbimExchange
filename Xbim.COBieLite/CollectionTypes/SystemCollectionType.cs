using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xbim.COBieLite.CollectionTypes;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLite
{
    [JsonObject]
    public partial class SystemCollectionType : ICollectionType<SystemType>, IEnumerable<SystemType>
    {
        public IEnumerator<SystemType> GetEnumerator()
        {
            return System.OfType<SystemType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<SystemType> InnerList
        {
            get { return System ?? (System = new List<SystemType>()); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return System.OfType<SystemType>().GetEnumerator();
        }
    }
}
