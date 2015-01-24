using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.DPoWToCOBieLite
{
    public class MappingPlanOfWorkToFacilityType : DPoWToCOBieLiteMapping<PlanOfWork, FacilityType>
    {
        protected override FacilityType Mapping(PlanOfWork source, FacilityType target)
        {
            var sFacility = source.Facility;
            target.externalID = Exchanger.GetStringIdentifier();
            if (sFacility != null)
            {
                target.FacilityDescription = sFacility.FacilityDescription;
                target.FacilityName = sFacility.FacilityName;
                target.FacilityCategory = sFacility.FacilityCategory != null ? sFacility.FacilityCategory.ClassificationCode : null;
            }
            if (!String.IsNullOrWhiteSpace(sFacility.FacilitySiteName))
            {
                target.SiteAssignment = new SiteType()
                {
                    SiteName = sFacility.FacilitySiteName,
                    SiteDescription = sFacility.FacilityDescription,
                    externalID = Exchanger.GetStringIdentifier()
                };
            }

            var sProject = source.Project;
            if (sProject != null)
            {
                target.ProjectAssignment = new ProjectType() { 
                    externalID = Exchanger.GetStringIdentifier(),
                    ProjectDescription = sProject.ProjectDescription,
                    ProjectName = sProject.ProjectName
                };

                //get project attributes which are convertable to COBieLite facility
                target.FacilityDefaultAreaUnit = DPoWToCOBieLiteExchanger.GetAreaUnit(sProject.AreaUnits);
                target.FacilityDefaultAreaUnitSpecified = true;
                target.FacilityDefaultCurrencyUnit = DPoWToCOBieLiteExchanger.GetCurrency(sProject.CurrencyUnits);
                target.FacilityDefaultCurrencyUnitSpecified = true;
                target.FacilityDefaultLinearUnit = DPoWToCOBieLiteExchanger.GetLinearUnit(sProject.LinearUnits);
                target.FacilityDefaultLinearUnitSpecified = true;
                target.FacilityDefaultVolumeUnit = DPoWToCOBieLiteExchanger.GetVolumeUnit(sProject.VolumeUnits);
                target.FacilityDefaultVolumeUnitSpecified = true;
            }

            return target;
        }
    }
}
