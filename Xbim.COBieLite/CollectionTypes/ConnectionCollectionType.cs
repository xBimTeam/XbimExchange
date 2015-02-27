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
    public partial class ConnectionCollectionType : ICollectionType<ConnectionType>, IEnumerable<ConnectionType>
    {
        public IEnumerator<ConnectionType> GetEnumerator()
        {
            return Connection.OfType<ConnectionType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<ConnectionType> InnerList
        {
            get { return Connection ?? (Connection = new List<ConnectionType>()); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Connection.OfType<ConnectionType>().GetEnumerator();
        }
    }
}
