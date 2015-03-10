using System.Linq;
using Xbim.COBieLite;

namespace XbimExchanger.COBieLiteHelpers
{
    public static class AttributeValueTypeExtensions
    {

        public static XbimSimplePropertyType SimplePropertyType(this AttributeValueType valueType)
        {
            if (valueType == null || valueType.Item == null) return XbimSimplePropertyType.Null;
            string valueTypeString = valueType.Item.GetType().Name;
            
            switch (valueTypeString)
            {
                case "BooleanValueType":
                    return XbimSimplePropertyType.SimpleBoolean;
                case "DateTime":
                    return XbimSimplePropertyType.SimpleDateTime;
                case "AttributeDecimalValueType":
                    var dt = valueType.Item as AttributeDecimalValueType;
                    return dt != null && (dt.MaxValueDecimalSpecified || dt.MinValueDecimalSpecified)
                    ? XbimSimplePropertyType.BoundedDecimal
                    : XbimSimplePropertyType.SimpleDecimal;
                case "AttributeIntegerValueType":
                    var it = valueType.Item as AttributeIntegerValueType;
                    return it != null && (it.MaxValueIntegerSpecified || it.MinValueIntegerSpecified)
                    ? XbimSimplePropertyType.BoundedInteger
                    : XbimSimplePropertyType.SimpleInteger;
                case "AttributeMonetaryValueType":
                    return XbimSimplePropertyType.SimpleMonetary;
                case "AttributeStringValueType":
                    var st = valueType.Item as AttributeStringValueType;
                    return st != null && st.AllowedValues != null && st.AllowedValues.Any()
                    ? XbimSimplePropertyType.EnumerationString
                    : XbimSimplePropertyType.SimpleString;
                default:
                    return XbimSimplePropertyType.Null;
            }
        }   
    }
}
