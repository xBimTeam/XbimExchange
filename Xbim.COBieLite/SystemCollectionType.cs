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
    public partial class SystemCollectionType : ICollectionType<SystemType>
    {
        public IEnumerator<SystemType> GetEnumerator()
        {
            return this.System.OfType<SystemType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<SystemType> InnerList
        {
            get { return System; }
        }
    }
}
