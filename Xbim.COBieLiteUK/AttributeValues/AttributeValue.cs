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
    public abstract partial class AttributeValue
    {
        /// <summary>
        /// This is an infrastructure property used for COBie serialization/deserialization. Use Minimal, Maximal, Allowed values to set up the constrains
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        internal abstract string AllowedValuesString { get; set; }

        public abstract string GetStringValue();
    }
}
