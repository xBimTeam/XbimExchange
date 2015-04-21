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

        public static AttributeValue CreateFromObject(object underlyingValue)
        {
            if (underlyingValue == null)
                return null;
            if (underlyingValue is AttributeValue)
                return (AttributeValue) underlyingValue;
            var asCobieValue = underlyingValue as CobieValue;
            if (asCobieValue != null)
            {
                return CreateFromObject(asCobieValue.ToObject());
            }

            var sw = underlyingValue.GetType().Name.ToLowerInvariant(); 

            switch (sw)
            {
                case "int32":
                case "int16":
                    return new IntegerAttributeValue() {Value = Convert.ToInt32(underlyingValue)};
                case "double":
                case "decimal":
                    return new DecimalAttributeValue(){ Value = Convert.ToDouble(underlyingValue) };
                case "string":
                    return new StringAttributeValue() {  Value  = underlyingValue.ToString() };
                case "boolean":
                    return new BooleanAttributeValue() { Value = underlyingValue as bool? };
                case "datetime":
                    return new DateTimeAttributeValue() { Value = Convert.ToDateTime(underlyingValue) };        
            }
            throw new ArgumentException(underlyingValue.GetType().Name + " cannot be converted to AttributeValue.");
        }
    }
}
