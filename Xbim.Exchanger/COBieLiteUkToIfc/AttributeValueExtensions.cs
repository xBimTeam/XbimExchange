using System.Linq;
using Xbim.CobieLiteUk;
using XbimExchanger.COBieLiteHelpers;

namespace XbimExchanger.COBieLiteUkToIfc
{
    /// <summary>
    /// 
    /// </summary>
    public static class AttributeValueExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public static XbimSimplePropertyType SimplePropertyType(this AttributeValue valueType)
        {
            if (valueType == null) return XbimSimplePropertyType.Null;
            var valueTypeString = valueType.GetType().Name;

            switch (valueTypeString)
            {
                case "BooleanAttributeValue":
                    return XbimSimplePropertyType.SimpleBoolean;
                case "DateTimeAttributeValue":
                    return XbimSimplePropertyType.SimpleDateTime;
                case "DecimalAttributeValue":
                    var dt = valueType as DecimalAttributeValue;
                    return dt != null && (dt.MinimalValue.HasValue || dt.MaximalValue.HasValue)
                        ? XbimSimplePropertyType.BoundedDecimal
                        : XbimSimplePropertyType.SimpleDecimal;
                case "IntegerAttributeValue":
                    var it = valueType as IntegerAttributeValue;
                    return it != null && (it.MinimalValue.HasValue || it.MaximalValue.HasValue)
                        ? XbimSimplePropertyType.BoundedInteger
                        : XbimSimplePropertyType.SimpleInteger;
                case "StringAttributeValue":
                    var st = valueType as StringAttributeValue;
                    return st != null && st.AllowedValues != null && st.AllowedValues.Any()
                        ? XbimSimplePropertyType.EnumerationString
                        : XbimSimplePropertyType.SimpleString;
                default:
                    return XbimSimplePropertyType.Null;
            }
        }
    }
}