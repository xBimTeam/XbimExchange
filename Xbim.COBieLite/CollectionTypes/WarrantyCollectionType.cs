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
    public partial class WarrantyCollectionType : ICollectionType<WarrantyType>, IEnumerable<WarrantyType>
    {
        public IEnumerator<WarrantyType> GetEnumerator()
        {
            return Warranty.OfType<WarrantyType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<WarrantyType> InnerList
        {
            get { return Warranty; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Warranty.OfType<WarrantyType>().GetEnumerator();            
        }
    }
}
