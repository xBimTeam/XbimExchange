using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Xml.Serialization;
using Xbim.COBieLite.CollectionTypes;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLite
{
    [JsonObject]
    public partial class FloorCollectionType : ICollectionType<FloorType>, IEnumerable<FloorType>
    {
        public IEnumerator<FloorType> GetEnumerator()
        {
            return  Floor.OfType<FloorType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<FloorType> InnerList => Floor ?? (Floor = new List<FloorType>());

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Floor.OfType<FloorType>().GetEnumerator();
        }
    }
}
