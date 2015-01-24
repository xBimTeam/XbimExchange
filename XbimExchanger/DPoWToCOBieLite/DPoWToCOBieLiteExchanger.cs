using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.DPoWToCOBieLite
{
    public class DPoWToCOBieLiteExchanger : XbimExchanger<PlanOfWork, FacilityType>
    {
        public DPoWToCOBieLiteExchanger(PlanOfWork source, FacilityType target) : base(source, target)
        {

        }

        public override FacilityType Convert()
        {
            var mappings = GetOrCreateMappings<MappingPlanOfWorkToFacilityType>();
            return mappings.AddMapping(SourceRepository, TargetRepository);
        }

        public static AreaUnitSimpleType GetAreaUnit(AreaUnits dpowUnits)
        {
            switch (dpowUnits)
            {
                case AreaUnits.squaremeters:
                    return AreaUnitSimpleType.squaremeters;
                case AreaUnits.squarecentimeters:
                    return AreaUnitSimpleType.squarecentimeters;
                case AreaUnits.squaremillimeters:
                    return AreaUnitSimpleType.squaremillimeters;
                case AreaUnits.squarekilometers:
                    return AreaUnitSimpleType.squarekilometers;
                default:
                    throw new ArgumentException("Unexpected unit");
            }
        }

        public static CurrencyUnitSimpleType GetCurrency(CurrencyUnits units)
        {
            switch (units)
            {
                case CurrencyUnits.USD:
                    return CurrencyUnitSimpleType.USD;
                case CurrencyUnits.EUR:
                    return CurrencyUnitSimpleType.EUR;
                case CurrencyUnits.GBP:
                    return CurrencyUnitSimpleType.GBP;
                default:
                    throw new ArgumentException("Unexpected unit");
            }
        }

        public static LinearUnitSimpleType GetLinearUnit(LinearUnits units)
        {
            switch (units)
            {
                case LinearUnits.meters:
                    return LinearUnitSimpleType.meters;
                case LinearUnits.millimeters:
                    return LinearUnitSimpleType.millimeters;
                case LinearUnits.centimeters:
                    return LinearUnitSimpleType.centimeters;
                case LinearUnits.kilometers:
                    return LinearUnitSimpleType.kilometers;
                default:
                    throw new ArgumentException("Unexpected unit");
            }
        }

        public static VolumeUnitSimpleType GetVolumeUnit(VolumeUnits units)
        {
            switch (units)
            {
                case VolumeUnits.cubicmeters:
                    return VolumeUnitSimpleType.cubicmeters;
                case VolumeUnits.cubicmillimeters:
                    return VolumeUnitSimpleType.cubicmillimeters;
                case VolumeUnits.cubiccentimeters:
                    return VolumeUnitSimpleType.cubiccentimeters;
                default:
                    throw new ArgumentException("Unexpected unit");
            }
        }

        public static List<string> CategoryValues = new List<string>() { "Design Review" , "Bidder Inquiry" , "Change" , "Claim" , "Coordination" , "Environmental" , "Functional" , "Indoor Air Quality" , "Installation" , "Quality Assurance" , "Quality Control" , "RFI" , "Safety" , "Specification" , "Deviation"};
    }
}
