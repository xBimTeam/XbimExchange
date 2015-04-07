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
    public partial class BooleanAttributeValue
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
                if (AllowedValue == null) return null;
                return AllowedValue != null ? (AllowedValue??true).ToString(CultureInfo.InvariantCulture): "";
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    AllowedValue = null;
                bool bVal;
                if (bool.TryParse(value, out bVal))
                    AllowedValue = bVal;

            }
        }

        public override string GetStringValue()
        {
            return Value.HasValue
                ? Value.ToString()
                : "undefined boolean";
        }
    }
}
