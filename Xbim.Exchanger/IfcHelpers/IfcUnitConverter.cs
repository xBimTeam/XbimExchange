using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.IO;

namespace XbimExchanger.IfcHelpers
{
    /// <summary>
    /// Helper to convert text descriptions to formal Units
    /// </summary>
    public struct IfcUnitConverter
    {
        
        /// <summary>
        /// The name of the Unit, VOLUMEUNIT, LENGTHUNIT etc, 
        /// </summary>
        public IfcUnitEnum UnitName;
        /// <summary>
        /// Name of the SI Unit, METRE, SQUARE_METRE etc null if not SI
        /// </summary>
        public IfcSIUnitName? SiUnitName;
        /// <summary>
        /// User defined unit name
        /// </summary>
        public string UserDefinedSiUnitName;
        /// <summary>
        /// Sub-dicsion of unit MILLI, PICA etc, null if none
        /// </summary>
        public IfcSIPrefix? SiPrefix;
        /// <summary>
        /// Conversion factor between unit
        /// </summary>
        public double ConversionFactor;


        /// <summary>
        /// The unit has no SIUnitName
        /// </summary>
        public bool IsUndefined
        {
            get { return !SiUnitName.HasValue; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public IfcUnitConverter(string name)
        {
           
            SiUnitName = null;
            SiPrefix = null;
            UnitName = IfcUnitEnum.USERDEFINED;
            UserDefinedSiUnitName = name; //default if all fails
            ConversionFactor = 1;
            Convert(name);
        }
        

        /// <summary>
        /// Converts a string
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Convert(string name)
        {
            Init(name);
            if (string.IsNullOrWhiteSpace(name)) return;
            var trimmedName = name.ToLowerInvariant();
            trimmedName = Regex.Replace(trimmedName, "[^a-z0-9]+", string.Empty, RegexOptions.Compiled);
            //trimmedName = Regex.Replace(trimmedName, @"\s+", "");
            //check for a metric SI value
            switch (trimmedName)
            {
                case "millimeter":
                case "millimeters":
                case "millimetre":
                case "millimetres":
                case "mm":
                    SiUnitName = IfcSIUnitName.METRE;
                    SiPrefix = IfcSIPrefix.MILLI;
                    break;
                case "centimeter":
                case "centimeters":
                case "centimetre":
                case "centimetres":
                case "cm":
                    SiUnitName = IfcSIUnitName.METRE;
                    SiPrefix = IfcSIPrefix.CENTI;
                    break;
                case "meter":
                case "meters":
                case "metre":
                case "metres":
                case "m":
                    SiUnitName = IfcSIUnitName.METRE;
                    SiPrefix = null;
                    break;
                case "kilometer":
                case "kilometers":
                case "kilometre":
                case "kilometres":
                case "km":
                    SiUnitName = IfcSIUnitName.METRE;
                    SiPrefix = IfcSIPrefix.KILO;
                    break;
                case "squaremillimeter":
                case "squaremillimeters":
                case "squaremillimetre":
                case "squaremillimetres":
                case "mm2":
                    SiUnitName = IfcSIUnitName.SQUARE_METRE;
                    SiPrefix = IfcSIPrefix.MILLI;
                    break;
                case "squarecentimeter":
                case "squarecentimeters":
                case "squarecentimetre":
                case "squarecentimetres":
                case "cm2":
                    SiUnitName = IfcSIUnitName.SQUARE_METRE;
                    SiPrefix = IfcSIPrefix.CENTI;
                    break;
                case "squaremeter":
                case "squaremeters":
                case "squaremetre":
                case "squaremetres":
                case "m2":
                    SiUnitName = IfcSIUnitName.SQUARE_METRE;
                    SiPrefix = null;
                    break;
                case "squarekilometer":
                case "squarekilometers":
                case "squarekilometre":
                case "squarekilometres":
                    SiUnitName = IfcSIUnitName.SQUARE_METRE;
                    SiPrefix = IfcSIPrefix.KILO;
                    break;
                case "cubicmillimeter":
                case "cubicmillimeters":
                case "cubicmillimetre":
                case "cubicmillimetres":
                case "mm3":
                    SiUnitName = IfcSIUnitName.CUBIC_METRE;
                    SiPrefix = IfcSIPrefix.MILLI;
                    break;
                case "cubiccentimeter":
                case "cubiccentimeters":
                case "cubiccentimetre":
                case "cubiccentimetres":
                case "cm3":
                    SiUnitName = IfcSIUnitName.CUBIC_METRE;
                    SiPrefix = IfcSIPrefix.CENTI;
                    break;
                case "cubicmeter":
                case "cubicmeters":
                case "cubicmetre":
                case "cubicmetres":
                case "m3":
                    SiUnitName = IfcSIUnitName.CUBIC_METRE;
                    SiPrefix = null;
                    break;
                case "cubickilometer":
                case "cubickilometers":
                case "cubickilometre":
                case "cubickilometres":
                case "km3":
                    SiUnitName = IfcSIUnitName.CUBIC_METRE;
                    SiPrefix = IfcSIPrefix.KILO;
                    break;
            }


            if (!SiUnitName.HasValue) //see if it is imperial
            {
                switch (trimmedName)
                {
                    case "inches":
                    case "inch":
                    case "\"":
                        SiUnitName = IfcSIUnitName.METRE;
                        ConversionFactor = 0.0254;
                        break;
                    case "feet":
                    case "foot":
                    case "ft":
                    case "'":
                        SiUnitName = IfcSIUnitName.METRE;
                        ConversionFactor = 0.3048;
                        break;
                    case "yards":
                    case "yard":
                    case "yds":
                    case "yd":
                        SiUnitName = IfcSIUnitName.METRE;
                        ConversionFactor = 0.9144;
                        break;
                    case "miles":
                    case "mile":
                        SiUnitName = IfcSIUnitName.METRE;
                        ConversionFactor = 1609.344;
                        break;
                    case "squareinches":
                    case "squareinch":
                        SiUnitName = IfcSIUnitName.SQUARE_METRE;
                        ConversionFactor = 0.00064516;
                        break;
                    case "squarefeet":
                    case "squarefoot":
                    case "sqft":
                    case "ft2":
                        SiUnitName = IfcSIUnitName.SQUARE_METRE;
                        ConversionFactor = 0.09290304;
                        break;
                    case "squareyards":
                    case "squareyard":
                    case "sqyd":
                    case "yd2":
                    case "yard2":
                        SiUnitName = IfcSIUnitName.SQUARE_METRE;
                        ConversionFactor = 0.83612736;
                        break;
                    case "squaremiles":
                    case "squaremile":
                        SiUnitName = IfcSIUnitName.SQUARE_METRE;
                        ConversionFactor = 2589988.11;
                        break;
                    case "cubicinches":
                    case "cubicinch":
                        SiUnitName = IfcSIUnitName.CUBIC_METRE;
                        ConversionFactor = 1.6387064e-5;
                        break;
                    case "cubicfeet":
                    case "cubicfoot":
                    case "foot3":
                    case "ft3":
                        SiUnitName = IfcSIUnitName.CUBIC_METRE;
                        ConversionFactor = 0.0283168466;
                        break;
                    case "cubicyards":
                    case "cubicyard":
                    case "yard3":
                    case "yd3":
                        SiUnitName = IfcSIUnitName.CUBIC_METRE;
                        ConversionFactor = 0.764554858;
                        break;
                    case "cubicmiles":
                    case "cubicmile":
                        SiUnitName = IfcSIUnitName.CUBIC_METRE;
                        ConversionFactor = 4.16818183e9;
                        break;
                }
            }

            if (SiUnitName.HasValue)
            {
                switch (SiUnitName)
                {
                    case IfcSIUnitName.AMPERE:
                        break;
                    case IfcSIUnitName.BECQUEREL:
                        break;
                    case IfcSIUnitName.CANDELA:
                        break;
                    case IfcSIUnitName.COULOMB:
                        break;
                    case IfcSIUnitName.CUBIC_METRE:
                        UnitName = IfcUnitEnum.VOLUMEUNIT;
                        break;
                    case IfcSIUnitName.DEGREE_CELSIUS:
                        break;
                    case IfcSIUnitName.FARAD:
                        break;
                    case IfcSIUnitName.GRAM:
                        break;
                    case IfcSIUnitName.GRAY:
                        break;
                    case IfcSIUnitName.HENRY:
                        break;
                    case IfcSIUnitName.HERTZ:
                        break;
                    case IfcSIUnitName.JOULE:
                        break;
                    case IfcSIUnitName.KELVIN:
                        break;
                    case IfcSIUnitName.LUMEN:
                        break;
                    case IfcSIUnitName.LUX:
                        break;
                    case IfcSIUnitName.METRE:
                        UnitName = IfcUnitEnum.LENGTHUNIT;
                        break;
                    case IfcSIUnitName.MOLE:
                        break;
                    case IfcSIUnitName.NEWTON:
                        break;
                    case IfcSIUnitName.OHM:
                        break;
                    case IfcSIUnitName.PASCAL:
                        break;
                    case IfcSIUnitName.RADIAN:
                        break;
                    case IfcSIUnitName.SECOND:
                        break;
                    case IfcSIUnitName.SIEMENS:
                        break;
                    case IfcSIUnitName.SIEVERT:
                        break;
                    case IfcSIUnitName.SQUARE_METRE:
                        UnitName = IfcUnitEnum.AREAUNIT;
                        break;
                    case IfcSIUnitName.STERADIAN:
                        break;
                    case IfcSIUnitName.TESLA:
                        break;
                    case IfcSIUnitName.VOLT:
                        break;
                    case IfcSIUnitName.WATT:
                        break;
                    case IfcSIUnitName.WEBER:
                        break;
                    case null:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("name");
                }
            }
        }

        private void Init(string name)
        {
           
            SiUnitName = null;
            SiPrefix = null;
            UnitName = IfcUnitEnum.USERDEFINED;
            UserDefinedSiUnitName = name; //default if all fails
            ConversionFactor = 1;
        }


        /// <summary>
        /// Creates an IfcUnit from the this object and adds it to the database, only works for SI at the moment
        /// </summary>
        /// <param name="units"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public IfcUnit IfcUnit(Dictionary<string, IfcUnit> units, XbimModel model)
        {
            if (SiUnitName == null) return null;
            IfcUnit unit;
            var key = SiPrefix.HasValue? SiPrefix.Value.ToString()  :"";
            key += SiUnitName.Value;
            if (units.TryGetValue(key, out unit)) return unit;
            var siUnit = model.Instances.New<IfcSIUnit>();
            siUnit.Name = SiUnitName.Value;
            siUnit.Prefix = SiPrefix;
            siUnit.UnitType = UnitName;
            siUnit.Dimensions = IfcDimensionalExponents.DimensionsForSiUnit(SiUnitName.Value);
            units.Add(key,siUnit);
            return siUnit;
        }
    }
}
