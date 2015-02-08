using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xbim.COBieLite.CollectionTypes;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLiteUK
{
    [JsonObject]
    public partial class AssemblyAssignmentCollectionType : ICollectionType<AssemblyType>, IEnumerable<AssemblyType>
    {
        public IEnumerator<AssemblyType> GetEnumerator()
        {
            return AssemblyAssignment.OfType<AssemblyType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<AssemblyType> InnerList
        {
            get { return AssemblyAssignment; }
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return AssemblyAssignment.OfType<AssemblyType>().GetEnumerator();
        }
    }
}
