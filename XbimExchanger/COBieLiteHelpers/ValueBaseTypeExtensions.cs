using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.XbimExtensions.SelectTypes;

namespace XbimExchanger.COBieLiteHelpers
{
    public static class ValueBaseTypeExtensions
    {
        /// <summary>
        /// Returns true if the value base type has a value, empty strings return false
        /// </summary>
        /// <param name="valueBaseType"></param>
        /// <returns></returns>
        public static bool HasValue(this ValueBaseType valueBaseType)
        {
            if (valueBaseType == null) return false;
            var decimalType = valueBaseType as DecimalValueType;
            if (decimalType != null) return decimalType.DecimalValueSpecified;
            var stringType = valueBaseType as StringValueType;
            if (stringType != null) return string.IsNullOrEmpty(stringType.StringValue);
            var integerType = valueBaseType as IntegerValueType;
            if (integerType != null) return true; //for some reason these are always OK
            var booleanType = valueBaseType as BooleanValueType;
            if (booleanType != null) return booleanType.BooleanValueSpecified;
            return false;
        }


        /// <summary>
        /// Converts to the template type, valid types are double, int, string, bool
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="valueBaseType"></param>
        /// <returns></returns>
        public static TType ConvertTo<TType>(this ValueBaseType valueBaseType)
        {
            var decimalType = valueBaseType as DecimalValueType;
            if (decimalType != null && decimalType.DecimalValueSpecified)
                return (TType)Convert.ChangeType(decimalType.DecimalValue, typeof(TType));
            var stringType = valueBaseType as StringValueType;
            if (stringType != null)
                return (TType)Convert.ChangeType(stringType.StringValue, typeof(TType));
            var integerType = valueBaseType as IntegerValueType;
            if (integerType != null)
                return (TType)Convert.ChangeType(integerType.IntegerValue, typeof(TType));
            var booleanType = valueBaseType as BooleanValueType;
            if (booleanType != null && booleanType.BooleanValueSpecified)
                return (TType)Convert.ChangeType(booleanType.BooleanValue, typeof(TType));
            return default(TType);
        }

        /// <summary>
        /// Converts to an IfcValue type, IfcText, IfcInteger, IfcBoolean or IfcReal
        /// </summary>
        /// <param name="valueBaseType"></param>
        /// <returns></returns>
        public static IfcValue ConvertToIfcValue(this ValueBaseType valueBaseType)
        {
            var decimalType = valueBaseType as DecimalValueType;
            if (decimalType != null && decimalType.DecimalValueSpecified)
                return new IfcReal((double)Convert.ChangeType(decimalType.DecimalValue, typeof(double)));
            var stringType = valueBaseType as StringValueType;
            if (stringType != null)
                return new IfcText((string)Convert.ChangeType(stringType.StringValue, typeof(string)));
            var integerType = valueBaseType as IntegerValueType;
            if (integerType != null)
                return new IfcInteger((int)Convert.ChangeType(integerType.IntegerValue, typeof(int)));
            var booleanType = valueBaseType as BooleanValueType;
            if (booleanType != null && booleanType.BooleanValueSpecified)
                return new IfcBoolean((bool)Convert.ChangeType(booleanType.BooleanValue, typeof(bool)));
            return default(IfcText);
        }
    }
}
