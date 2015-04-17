using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLiteUK
{
    public partial class DateTimeAttributeValue
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
                var min = MinimalValue == null ? "-∞" : (MinimalValue ?? new DateTime()).ToString("O").Substring(0, 19);
                var max = MaximalValue == null ? "∞" : (MaximalValue ?? new DateTime()).ToString("O").Substring(0, 19);
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
                    DateTime d;
                    if (DateTime.TryParse(vals[0], null, DateTimeStyles.RoundtripKind , out d))
                        MinimalValue = d;
                    else
                        MinimalValue = null;
                }
                {
                    DateTime d;
                    if (DateTime.TryParse(vals[1], null, DateTimeStyles.RoundtripKind, out d))
                        MaximalValue = d;
                    else
                        MaximalValue = null;
                }
            }
        }

        public override string GetStringValue()
        {
            return Value.HasValue
                ? Value.ToString()
                : "undefined date/time";
        }
    }
}
