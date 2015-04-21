using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLiteUK
{
    public partial class StringAttributeValue
    {
        /// <summary>
        /// This is an infrastructure property used for COBie serialization/deserialization. Use Minimal, Maximal, Allowed values to set up the constrains
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        internal override string AllowedValuesString
        {
            get
            {
                if (AllowedValues == null || !AllowedValues.Any())
                    return null;
                return String.Join(",", AllowedValues);
            }
            set {
                AllowedValues = value == null ? new List<string>() : value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();
                for (var i = 0; i < AllowedValues.Count; i++)
                    AllowedValues[i] = AllowedValues[i].Trim();
            }
        }
        public override string GetStringValue()
        {
            return Value;
        }
    }
}
