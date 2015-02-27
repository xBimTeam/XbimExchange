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
    public partial class SpareCollectionType : ICollectionType<SpareType>, IEnumerable<SpareType>
    {
        public IEnumerator<SpareType> GetEnumerator()
        {
            return Spare.OfType<SpareType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<SpareType> InnerList
        {
            get { return Spare ?? (Spare = new List<SpareType>()); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Spare.OfType<SpareType>().GetEnumerator();
        }
    }
}
