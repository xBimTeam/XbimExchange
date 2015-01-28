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
    public partial class SpaceAssignmentCollectionType : ICollectionType<SpaceKeyType>
    {
        public IEnumerator<SpaceKeyType> GetEnumerator()
        {
            return this.SpaceAssignment.OfType<SpaceKeyType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<SpaceKeyType> InnerList
        {
            get { return SpaceAssignment; }
        }
    }
}
