using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xbim.COBieLite.CollectionTypes;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLite
{
    [JsonObject]
    public partial class SpaceAssignmentCollectionType : ICollectionType<SpaceKeyType>, IEnumerable<SpaceKeyType>
    {
        public IEnumerator<SpaceKeyType> GetEnumerator()
        {
            return SpaceAssignment.OfType<SpaceKeyType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<SpaceKeyType> InnerList
        {
            get { return SpaceAssignment ?? (SpaceAssignment = new List<SpaceKeyType>()); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return SpaceAssignment.OfType<SpaceKeyType>().GetEnumerator();
        }
    }
}
