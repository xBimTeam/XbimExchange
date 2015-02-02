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
    public partial class SpaceCollectionType : ICollectionType<SpaceType>, IEnumerable<SpaceType>
    {
        public IEnumerator<SpaceType> GetEnumerator()
        {
            return Space.OfType<SpaceType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<SpaceType> InnerList
        {
            get { return Space; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Space.OfType<SpaceType>().GetEnumerator();
        }
    }
}
