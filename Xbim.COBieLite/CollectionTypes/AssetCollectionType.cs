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
    public partial class AssetCollectionType : ICollectionType<AssetInfoType>, IEnumerable<AssetInfoType>
    {
        public IEnumerator<AssetInfoType> GetEnumerator()
        {
            return Asset.OfType<AssetInfoType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<AssetInfoType> InnerList
        {
            get { return Asset ?? (Asset = new List<AssetInfoType>()); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Asset.OfType<AssetInfoType>().GetEnumerator();
        }
    }
}
