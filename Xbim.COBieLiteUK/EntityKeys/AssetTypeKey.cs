using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLiteUK
{
    public partial class AssetTypeKey : IEntityKey
    {
        [XmlIgnore, JsonIgnore]
        Type IEntityKey.ForType
        {
            get { return typeof(AssetType); }
        }
    }
}
