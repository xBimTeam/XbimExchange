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
    public partial class ZoneAssignmentCollectionType : ICollectionType<ZoneKeyType>, IEnumerable<ZoneKeyType>
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


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.ZoneAssignment.OfType<ZoneKeyType>().GetEnumerator();
        }
    }
}
