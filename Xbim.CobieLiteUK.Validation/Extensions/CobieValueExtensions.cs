using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.Extensions
{
    internal static class CobieValueExtensions
    {
        internal static object ToObject(this CobieValue inValue)
        {
            switch (inValue.ValueType)
            {
                case CobieValueType.Boolean:
                    return inValue.BooleanValue;
                case CobieValueType.DateTime:
                    return inValue.DateTimeValue;
                case CobieValueType.Double:
                    return inValue.DoubleValue;
                case CobieValueType.Integer:
                    return inValue.IntegerValue;
                case CobieValueType.String:
                    return inValue.BooleanValue;
                default:
                    return null;
            }
        }
    }
}
