using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Xbim.COBieLite.CollectionTypes;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLiteUK
{
    [JsonObject]
    public partial class SystemAssignmentCollectionType : ICollectionType<SystemKeyType>, IEnumerable<SystemKeyType>
    {
        public IEnumerator<SystemKeyType> GetEnumerator()
        {
            return SystemAssignment.OfType<SystemKeyType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<SystemKeyType> InnerList
        {
            get { return SystemAssignment; }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return SystemAssignment.OfType<SystemKeyType>().GetEnumerator();
        }
    }
}
