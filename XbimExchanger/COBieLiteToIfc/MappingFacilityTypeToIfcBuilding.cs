using Xbim.COBieLite;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;
using Xbim.Ifc2x3.Extensions;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingFacilityTypeToIfcBuilding : XbimIfcMappings<string, FacilityType, IfcBuilding>
    {

        protected override IfcBuilding Mapping(FacilityType facility, IfcBuilding ifcBuilding)
        {
            ifcBuilding.Name = facility.FacilityName;
            ifcBuilding.Description = facility.FacilityDescription;

            var projectMapping = MappingsCollection.GetOrCreateMappings<MappingProjectTypeToIfcProject>();
            //COBie does nor require a project but Ifc does
            var ifcProject = facility.ProjectAssignment != null ? projectMapping.CreateTargetObject() : projectMapping.GetOrCreateTargetObject(facility.externalID);
            projectMapping.AddMapping(facility.ProjectAssignment, ifcProject);
            //add the relationship between the site and the building
            if (facility.SiteAssignment != null)
            {
                var siteMapping = MappingsCollection.GetOrCreateMappings<MappingSiteTypeToIfcSite>();
                var ifcSite = siteMapping.AddMapping(facility.SiteAssignment,
                    siteMapping.GetOrCreateTargetObject(facility.SiteAssignment.externalID));
                //add the relationship between the site and the project and the building
                ifcProject.AddSite(ifcSite);
                ifcSite.AddBuilding(ifcBuilding);
            }
            else //relate the building to the project
                ifcProject.AddBuilding(ifcBuilding);



            return ifcBuilding;
        }
    }
}
