using System.Collections.Generic;
using System.Linq;

using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcHelpers
{
    public struct IfcUnitComparer : IEqualityComparer<IIfcUnit>
    {
        public bool Equals(IIfcUnit x, IIfcUnit y)
        {
           
            if (x is IIfcSIUnit && y is IIfcSIUnit)
            {
                var xs = x as IIfcSIUnit;
                var ys = y as IIfcSIUnit;
                return xs.UnitType == ys.UnitType 
                    && xs.Name == ys.Name 
                    && xs.Prefix == ys.Prefix
                    && Equals(xs.Dimensions, ys.Dimensions);
            }
            if (x is IIfcConversionBasedUnit && y is IIfcConversionBasedUnit)
            {
                var xs = x as IIfcConversionBasedUnit;
                var ys = y as IIfcConversionBasedUnit;
                return xs.UnitType == ys.UnitType 
                    && xs.Name == ys.Name 
                    && xs.ConversionFactor.ValueComponent.Equals(ys.ConversionFactor.ValueComponent)  
                    && Equals(xs.ConversionFactor.UnitComponent, xs.ConversionFactor.UnitComponent)
                    && Equals(xs.Dimensions, ys.Dimensions);
            }
            if (x is IIfcContextDependentUnit && y is IIfcContextDependentUnit)
            {
                var xs = x as IIfcContextDependentUnit;
                var ys = y as IIfcContextDependentUnit;
                return xs.UnitType == ys.UnitType 
                    && xs.Name == ys.Name
                    && Equals(xs.Dimensions, ys.Dimensions);
            }
            if (x is IIfcDerivedUnit && y is IIfcDerivedUnit)
            {
                var xs = x as IIfcDerivedUnit;
                var ys = y as IIfcDerivedUnit;
                return xs.UnitType==ys.UnitType 
                    && xs.UserDefinedType==ys.UserDefinedType
                    && Equals(xs.Elements,ys.Elements);
            }
            if (x is IIfcMonetaryUnit && y is IIfcMonetaryUnit)
            {
                var xs = x as IIfcMonetaryUnit;
                var ys = y as IIfcMonetaryUnit;
                return xs.Currency == ys.Currency;
            }
            return false;
            
        }

        private bool Equals(IEnumerable<IIfcDerivedUnitElement> a, IEnumerable<IIfcDerivedUnitElement> b)
        {
            var ifcDerivedUnitElementsA = a as IList<IIfcDerivedUnitElement> ?? a.ToList();
            var ifcDerivedUnitElementsB = a as IList<IIfcDerivedUnitElement> ?? b.ToList();
            if (ifcDerivedUnitElementsA.Count != ifcDerivedUnitElementsB.Count)
                return false;
            for (int i = 0; i < ifcDerivedUnitElementsA.Count; i++)
            {
                if (ifcDerivedUnitElementsA[i].Exponent != ifcDerivedUnitElementsB[i].Exponent) return false;
                if (!Equals(ifcDerivedUnitElementsA[i].Unit, ifcDerivedUnitElementsB[i].Unit)) return false;
            }
            return true;
        }

        private bool Equals(IIfcDimensionalExponents a, IIfcDimensionalExponents b)
        {
            return (
                a.AmountOfSubstanceExponent == b.AmountOfSubstanceExponent &&
                a.ElectricCurrentExponent == b.ElectricCurrentExponent &&
                a.LengthExponent == b.LengthExponent &&
                a.LuminousIntensityExponent == b.LuminousIntensityExponent &&
                a.MassExponent == b.MassExponent &&
                a.ThermodynamicTemperatureExponent == b.ThermodynamicTemperatureExponent &&
                a.TimeExponent == b.TimeExponent
            );          
        }
        public int GetHashCode(IIfcUnit obj)
        {
            return obj.FullName.GetHashCode();
        }
    }
}
