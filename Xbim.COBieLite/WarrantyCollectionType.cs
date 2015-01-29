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
    public partial class WarrantyCollectionType : ICollectionType<WarrantyType>
    {
        public IEnumerator<WarrantyType> GetEnumerator()
        {
            return this.Warranty.OfType<WarrantyType>().GetEnumerator();
        }

        [XmlIgnore][JsonIgnore]
        public List<WarrantyType> InnerList
        {
            get { return Warranty; }
        }
    }
}
