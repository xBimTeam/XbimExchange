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
    public partial class SystemAssignmentCollectionType : ICollectionType<SystemKeyType>
    {
        public IEnumerator<SystemKeyType> GetEnumerator()
        {
            return this.SystemAssignment.OfType<SystemKeyType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<SystemKeyType> InnerList
        {
            get { return this.SystemAssignment; }
        }
		
    }
}
