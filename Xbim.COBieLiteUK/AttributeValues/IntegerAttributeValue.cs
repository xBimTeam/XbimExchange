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
    public partial class IntegerAttributeValue
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
                if (MinimalValue == null && MaximalValue == null) return null;
                var min = MinimalValue == null ? "-∞" : MinimalValue.ToString();
                var max = MaximalValue == null ? "∞" : MaximalValue.ToString();
                return String.Format("<{0},{1}>", min, max);
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    MinimalValue = null;
                    MaximalValue = null;
                    return;
                }
                var vals = value.Trim('<', '>').Split(',');
                {
                    int i;
                    if (int.TryParse(vals[0], out i))
                        MinimalValue = i;
                    else
                        MinimalValue = null;
                }
                {
                    int i;
                    if (int.TryParse(vals[1], out i))
                        MaximalValue = i;
                    else
                        MaximalValue = null;
                }
            }
        }

        public override string GetStringValue()
        {
            return Value.HasValue
                ? Value.ToString()
                : "undefined integer";
        }
    }
}