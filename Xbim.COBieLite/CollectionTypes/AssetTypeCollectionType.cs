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
    public partial class AssetTypeCollectionType : ICollectionType<AssetTypeInfoType>, IEnumerable<AssetTypeInfoType>
    {
        public IEnumerator<AssetTypeInfoType> GetEnumerator()
        {
            return this.AssetType.OfType<AssetTypeInfoType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<AssetTypeInfoType> InnerList
        {
            get { return AssetType; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.AssetType.OfType<AssetTypeInfoType>().GetEnumerator();
        }
    }
}
