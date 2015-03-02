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
    public partial class ZoneCollectionType : ICollectionType<ZoneType>, IEnumerable<ZoneType>
    {
        public IEnumerator<ZoneType> GetEnumerator()
        {
            return Zone.OfType<ZoneType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<ZoneType> InnerList
        {
            get { return Zone ?? (Zone = new List<ZoneType>()); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Zone.OfType<ZoneType>().GetEnumerator();
        }
    }
}
