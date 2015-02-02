using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Xbim.COBieLite.CollectionTypes;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLite
{
    [JsonObject]
    public partial class ZoneAssignmentCollectionType : ICollectionType<ZoneKeyType>, IEnumerable<ZoneKeyType>
    {
        public IEnumerator<ZoneKeyType> GetEnumerator()
        {
            return ZoneAssignment.OfType<ZoneKeyType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<ZoneKeyType> InnerList
        {
            get { return ZoneAssignment; }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return ZoneAssignment.OfType<ZoneKeyType>().GetEnumerator();
        }
    }
}
