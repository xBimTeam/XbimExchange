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
    public partial class ZoneCollectionType : ICollectionType<ZoneType>
    {
        public IEnumerator<ZoneType> GetEnumerator()
        {
            return this.Zone.OfType<ZoneType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<ZoneType> InnerList
        {
            get { return Zone; }
        }
    }
}
