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
    public partial class AssetCollectionType : ICollectionType<AssetInfoType>, IEnumerable<AssetInfoType>
    {
        public IEnumerator<AssetInfoType> GetEnumerator()
        {
            return this.Asset.OfType<AssetInfoType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<AssetInfoType> InnerList
        {
            get { return Asset; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Asset.OfType<AssetInfoType>().GetEnumerator();
        }
    }
}
