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
        /// <summary>
        /// Constructs new exchanger. When converting data from DPoW to COBieLite it is possible to convert only one stage to COBieLite
        /// as there is no concept of project stages in COBieLite schema which is more focused on exchange of actual existing data at certain
        /// project stage rather than modelling multiple stages.
        /// </summary>
        /// <param name="source">Source DPoW model</param>
        /// <param name="target">Target COBieLite model</param>
        /// <param name="stage">Specific project stage</param>
        public DPoWToCOBieLiteExchanger(PlanOfWork source, FacilityType target, ProjectStage stage) : base(source, target)
        {
            Context = stage;
        }

        /// <summary>
        /// Constructs new exchanger. When converting data from DPoW to COBieLite it is possible to convert only one stage to COBieLite
        /// as there is no concept of project stages in COBieLite schema which is more focused on exchange of actual existing data at certain
        /// project stage rather than modelling multiple stages. If you don't specify a project stage only current project stage will be converted.
        /// </summary>
        /// <param name="source">Source DPoW model</param>
        /// <param name="target">Target COBieLite model</param>
        public DPoWToCOBieLiteExchanger(PlanOfWork source, FacilityType target)
            : base(source, target)
        {
            Context = source.Project.CurrentProjectStage;
        }

        /// <summary>
        /// Converts DPoW model to COBieLite where FacilityType is the root element of the data model
        /// </summary>
        /// <returns>COBieLite root element</returns>
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
