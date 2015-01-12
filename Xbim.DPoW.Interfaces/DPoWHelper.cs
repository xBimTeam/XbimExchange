using System;
using System.Linq;
using Xbim.COBieLite;
using Xbim.Common.Logging;

namespace Xbim.DPoW.Interfaces
{
    
    public class DpoWHelper
    {
        internal static readonly ILogger Logger = LoggerFactory.GetLogger();
        internal const string DpowSystemName = "DPoW";
        /// <summary>
        /// Processes the Plan of work and returns the number of warnings, these can be accessed through the Logger static member
        /// </summary>
        /// <param name="dpowPlanOfWork"></param>
        /// <returns>Number of warnings or -1 if an error occurred</returns>
        public int WriteCoBieLite(IPlanOfWork dpowPlanOfWork)
        {
          
            int warningCount = 0;
            if (!ValidatePlanOfWork(dpowPlanOfWork)) return -1;
            var facility = new FacilityType();
           
            int projectWarnings = ConvertProject(dpowPlanOfWork, facility);

            foreach (var classificationSystem in dpowPlanOfWork.ClassificationSystem)
            {
              //  Attrib
            }
            

            return warningCount;
        }

        private int ConvertProject(IPlanOfWork dpowPlanOfWork, FacilityType facility)
        {
            var warnings = ConvertProjectUnits(dpowPlanOfWork, facility);//this never generates an error
            var projectType = new ProjectType();
            if (string.IsNullOrWhiteSpace(dpowPlanOfWork.Project.ProjectURI))
            {
                Logger.Warn("The Project URI is not a valid identifier. It is required");
                warnings += 1;
            }
            projectType.externalID = dpowPlanOfWork.Project.ProjectURI;
            projectType.externalEntityName = "IPlanOfWork.Project";
            projectType.externalSystemName = DpowSystemName;
            if (string.IsNullOrWhiteSpace(dpowPlanOfWork.Project.ProjectName))
            {
                Logger.Warn("The Project Name is not a valid identifier. It is required");
                warnings += 1;
            }
            projectType.ProjectName = dpowPlanOfWork.Project.ProjectName;
            if (string.IsNullOrWhiteSpace(dpowPlanOfWork.Project.ProjectDescription))
            {
                Logger.Info("The Project Description has not been specified");
                warnings += 1;
            }
            projectType.ProjectDescription = dpowPlanOfWork.Project.ProjectDescription;
            facility.ProjectAssignment = projectType;
            return warnings;
        }

        private static int ConvertProjectUnits(IPlanOfWork dpowPlanOfWork, FacilityType facility)
        {
            int warnings = 0;
            AreaUnitSimpleType areaUnitType;
            if (Enum.TryParse(dpowPlanOfWork.Project.AreaUnits.ToString(), true, out areaUnitType))
            {
                facility.FacilityDefaultAreaUnit = areaUnitType;
                facility.FacilityDefaultAreaUnitSpecified = true;
            }
            else
            {
                Logger.WarnFormat("Default Area Unit of [{0}] is not a valid type.",
                    dpowPlanOfWork.Project.AreaUnits.ToString());
                warnings++;
            }

            LinearUnitSimpleType linearUnitType;
            if (Enum.TryParse(dpowPlanOfWork.Project.LinearUnits.ToString(), true, out linearUnitType))
            {
                facility.FacilityDefaultLinearUnit = linearUnitType;
                facility.FacilityDefaultLinearUnitSpecified = true;
            }
            else
            {
                Logger.WarnFormat("Default Linear Unit of [{0}] is not a valid type.",
                    dpowPlanOfWork.Project.LinearUnits.ToString());
                warnings++;
            }

            VolumeUnitSimpleType volumeUnitType;
            if (Enum.TryParse(dpowPlanOfWork.Project.VolumeUnits.ToString(), true, out volumeUnitType))
            {
                facility.FacilityDefaultVolumeUnit = volumeUnitType;
                facility.FacilityDefaultVolumeUnitSpecified = true;
            }
            else
            {
                Logger.WarnFormat("Default Volume Unit of [{0}] is not a valid type.",
                    dpowPlanOfWork.Project.VolumeUnits.ToString());
                warnings++;
            }

            CurrencyUnitSimpleType currencyUnitType;
            if (Enum.TryParse(dpowPlanOfWork.Project.CurrencyUnits.ToString(), true, out currencyUnitType))
            {
                facility.FacilityDefaultCurrencyUnit = currencyUnitType;
                facility.FacilityDefaultCurrencyUnitSpecified = true;
            }
            else
            {
                Logger.WarnFormat("Default Currency Unit of [{0}] is not a valid type.",
                    dpowPlanOfWork.Project.CurrencyUnits.ToString());
                warnings++;
            }
            return warnings;
        }

        public static bool ValidatePlanOfWork(IPlanOfWork dpowPlanOfWork)
        {
            //check all states
            if (dpowPlanOfWork.Project == null)
            {
                Logger.Error("The Digital Plan of Work does not contain a valid Project declaration");
                return false;
            }
            if (dpowPlanOfWork.Facility == null)
            {
                Logger.Error("The Digital Plan of Work does not contain a valid Facility declaration");
                return false;
            }
            if (dpowPlanOfWork.ProjectStages == null || !dpowPlanOfWork.ProjectStages.Any())
            {
                Logger.Error(
                    "The Digital Plan of Work does not contain any valid Project Stage declarations. There must be at least one");
                return false;
            }
            if (dpowPlanOfWork.ClassificationSystem == null || !dpowPlanOfWork.ClassificationSystem.Any())
            {
                Logger.Error(
                    "The Digital Plan of Work does not contain any valid Classification System declarations. There must be at least one");
                return false;
            }
            if (dpowPlanOfWork.Client == null)
            {
                Logger.Error("The Digital Plan of Work does not contain a valid Client declaration.");
                return false;
            }
            if (dpowPlanOfWork.Contacts == null || !dpowPlanOfWork.Contacts.Any())
            {
                Logger.Error(
                    "The Digital Plan of Work does not contain any valid Contact declarations. There must be at least one.");
                return false;
            }
            return true;
        }
    }
}
