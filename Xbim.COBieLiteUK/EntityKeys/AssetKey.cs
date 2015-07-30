using System;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLiteUK
{
    public partial class AssetKey : IEntityKey, IEquatable<AssetKey>
    {
        [XmlIgnore, JsonIgnore]
        public Type ForType
        {
            get { return typeof(Asset); }
        }

       

        public string GetSheet(string mapping)
        {
            var attr =
                ForType.GetCustomAttributes(typeof(SheetMappingAttribute), true)
                    .FirstOrDefault(a => ((SheetMappingAttribute)a).Type == mapping) as SheetMappingAttribute;
            return attr == null ? null : attr.Sheet;
        }


        public bool Equals(AssetKey other)
        {
            if (other == null) return false;
            return this.Name.Equals(other.Name);
        }

    }


    /// <summary>
    /// Compare Equality for linq statments using Distinct
    /// </summary>
    public class AssetKeyCompare : IEqualityComparer<AssetKey>
    {
        public bool Equals(AssetKey x, AssetKey y)
        {
            return String.Equals(x.Name, y.Name);
        }

        public int GetHashCode(AssetKey obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
