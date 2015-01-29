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
    public partial class SpareCollectionType : ICollectionType<SpareType>
    {
        public IEnumerator<SpareType> GetEnumerator()
        {
            return this.Spare.OfType<SpareType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<SpareType> InnerList
        {
            get { return Spare; }
        }
    }
}
