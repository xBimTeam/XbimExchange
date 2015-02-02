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
    public partial class ConnectionCollectionType : ICollectionType<ConnectionType>, IEnumerable<ConnectionType>
    {
        public IEnumerator<ConnectionType> GetEnumerator()
        {
            return this.Connection.OfType<ConnectionType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<ConnectionType> InnerList
        {
            get { return Connection; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Connection.OfType<ConnectionType>().GetEnumerator();
        }
    }
}
