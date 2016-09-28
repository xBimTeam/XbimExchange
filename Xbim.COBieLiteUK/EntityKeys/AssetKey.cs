using System;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Xbim.CobieLiteUk
{
    public partial class AssetKey : IEntityKey
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
    }


    /// <summary>
    /// Compare Equality for linq statments using Distinct
    /// </summary>
    public class AssetKeyCompare : IEqualityComparer<AssetKey>
    {
        public bool Equals(AssetKey x, AssetKey y)
        {
            if (Object.ReferenceEquals(x, y)) //same instance
                return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null)) //one is null
                return false;

            return String.Equals(x.Name, y.Name);
        }

        public int GetHashCode(AssetKey obj)
        {
            if (Object.ReferenceEquals(obj, null)) //obj is null
                return 0;

            return obj.Name == null ? 0 : obj.Name.GetHashCode();
        }
    }
}
