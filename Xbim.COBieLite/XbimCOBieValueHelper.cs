using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.XbimExtensions.SelectTypes;

namespace Xbim.COBieLite
{
    /// <summary>
    /// This class makes it easier for exchangers to interact with COBie values and attributes
    /// </summary>
    public struct XbimCoBieValueHelper
    {
        public readonly IfcSIPrefix? SiPrefix;
        public readonly IfcSIUnitName SiUnitName;
        public readonly object Value;
        public readonly bool IsNull;
        public XbimCoBieValueHelper(DecimalValueType decimalValue, IfcSIUnitName defaultSiUnitName, IfcSIPrefix? defaultSiPrefix = null)
        {
            //set regardless
            SiPrefix = defaultSiPrefix;
            SiUnitName = defaultSiUnitName;

            if (decimalValue == null || !decimalValue.DecimalValueSpecified)
            {
                IsNull = true;
                Value = null;
            }
            else
            {
                IsNull = false;
                Value = decimalValue.DecimalValue;
                if (!string.IsNullOrWhiteSpace(decimalValue.UnitName))
                {
                    //acording the COBie standard units should start with the SI prefix and end with the unit name
                    var trimmedName = decimalValue.UnitName.Trim();
                    foreach (IfcSIUnitName unit in Enum.GetValues(typeof (IfcSIUnitName)))
                    {
                        if (trimmedName.EndsWith(unit.ToString(), true, CultureInfo.InvariantCulture))
                        {
                            SiUnitName = unit;
                            break;
                        }
                    }
                    foreach (IfcSIPrefix prefix in Enum.GetValues(typeof(IfcSIPrefix)))
                    {
                        if (trimmedName.StartsWith(prefix.ToString(), true, CultureInfo.InvariantCulture))
                        {
                            defaultSiPrefix = prefix;
                            break;
                        }
                    }
                }
            }
        }
    }
}
