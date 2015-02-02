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
    public partial class AssemblyAssignmentCollectionType : ICollectionType<AssemblyType>, IEnumerable<AssemblyType>
    {
        public IEnumerator<AssemblyType> GetEnumerator()
        {
            return this.AssemblyAssignment.OfType<AssemblyType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<AssemblyType> InnerList
        {
            get { return AssemblyAssignment; }
        }
        
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.AssemblyAssignment.OfType<AssemblyType>().GetEnumerator();
        }
    }
}
