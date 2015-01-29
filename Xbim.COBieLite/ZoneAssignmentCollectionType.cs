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
    public partial class ZoneAssignmentCollectionType : ICollectionType<ZoneKeyType>
    {
        public IEnumerator<ZoneKeyType> GetEnumerator()
        {
            return this.ZoneAssignment.OfType<ZoneKeyType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<ZoneKeyType> InnerList
        {
            get { return ZoneAssignment; }
        }
		
    }
}
