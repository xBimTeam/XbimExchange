using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc2x3.MeasureResource;

namespace XbimExchanger.IfcHelpers
{
    public struct IfcUnitComparer : IEqualityComparer<IfcUnit>
    {
        public bool Equals(IfcUnit x, IfcUnit y)
        {
           
            if (x is IfcSIUnit && y is IfcSIUnit)
            {
                var xs = x as IfcSIUnit;
                var ys = y as IfcSIUnit;
                return xs.UnitType == ys.UnitType 
                    && xs.Name == ys.Name 
                    && xs.Prefix == ys.Prefix
                    && Equals(xs.Dimensions, ys.Dimensions);
            }
            if (x is IfcConversionBasedUnit && y is IfcConversionBasedUnit)
            {
                var xs = x as IfcConversionBasedUnit;
                var ys = y as IfcConversionBasedUnit;
                return xs.UnitType == ys.UnitType 
                    && xs.Name == ys.Name 
                    && xs.ConversionFactor.ValueComponent.Equals(ys.ConversionFactor.ValueComponent)  
                    && Equals(xs.ConversionFactor.UnitComponent, xs.ConversionFactor.UnitComponent)
                    && Equals(xs.Dimensions, ys.Dimensions);
            }
            if (x is IfcContextDependentUnit && y is IfcContextDependentUnit)
            {
                var xs = x as IfcContextDependentUnit;
                var ys = y as IfcContextDependentUnit;
                return xs.UnitType == ys.UnitType 
                    && xs.Name == ys.Name
                    && Equals(xs.Dimensions, ys.Dimensions);
            }
            if (x is IfcDerivedUnit && y is IfcDerivedUnit)
            {
                var xs = x as IfcDerivedUnit;
                var ys = y as IfcDerivedUnit;
                return xs.UnitType==ys.UnitType 
                    && xs.UserDefinedType==ys.UserDefinedType
                    && Equals(xs.Elements,ys.Elements);
            }
            if (x is IfcMonetaryUnit && y is IfcMonetaryUnit)
            {
                var xs = x as IfcMonetaryUnit;
                var ys = y as IfcMonetaryUnit;
                return xs.Currency == ys.Currency;
            }
            return false;
            
        }

        private bool Equals(IEnumerable<IfcDerivedUnitElement> a, IEnumerable<IfcDerivedUnitElement> b)
        {
            var ifcDerivedUnitElementsA = a as IList<IfcDerivedUnitElement> ?? a.ToList();
            var ifcDerivedUnitElementsB = a as IList<IfcDerivedUnitElement> ?? b.ToList();
            if (ifcDerivedUnitElementsA.Count != ifcDerivedUnitElementsB.Count)
                return false;
            for (int i = 0; i < ifcDerivedUnitElementsA.Count; i++)
            {
                if (ifcDerivedUnitElementsA[i].Exponent != ifcDerivedUnitElementsB[i].Exponent) return false;
                if (!Equals(ifcDerivedUnitElementsA[i].Unit, ifcDerivedUnitElementsB[i].Unit)) return false;
            }
            return true;
        }

        private bool Equals(IfcDimensionalExponents a, IfcDimensionalExponents b)
        {
            for (int i = 0; i < 7; i++)
            {
               if( a[i] != b[i])
                   return false;
            }
            return true;
        }
        public int GetHashCode(IfcUnit obj)
        {
            return obj.GetName().GetHashCode();
        }
    }
}
