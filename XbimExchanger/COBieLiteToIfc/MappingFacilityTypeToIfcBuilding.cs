using Xbim.COBieLite;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.Extensions;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingFacilityTypeToIfcBuilding : CoBieLiteIfcMappings<string, FacilityType, IfcBuilding>
    {

        protected override IfcBuilding Mapping(FacilityType facility, IfcBuilding ifcBuilding)
        {
            #region Properties

            ifcBuilding.Name = facility.FacilityName;
            ifcBuilding.Description = facility.FacilityDescription;
            
            #endregion

            #region Default units

            Exchanger.DefaultLinearUnit = facility.FacilityDefaultLinearUnitSpecified
                   ? new IfcUnitConverter(facility.FacilityDefaultLinearUnit.ToString())
                   : default(IfcUnitConverter);
            Exchanger.DefaultAreaUnit = facility.FacilityDefaultAreaUnitSpecified
                ? new IfcUnitConverter(facility.FacilityDefaultAreaUnit.ToString())
                : default(IfcUnitConverter);
            Exchanger.DefaultVolumeUnit = facility.FacilityDefaultVolumeUnitSpecified
                   ? new IfcUnitConverter(facility.FacilityDefaultVolumeUnit.ToString())
                   : default(IfcUnitConverter);
            Exchanger.DefaultCurrencyUnit = facility.FacilityDefaultCurrencyUnitSpecified
                ? (CurrencyUnitSimpleType?)facility.FacilityDefaultCurrencyUnit
                : null;

            #endregion

            #region Project
            var projectMapping = Exchanger.GetOrCreateMappings<MappingProjectTypeToIfcProject>();
            //COBie does nor require a project but Ifc does
            var ifcProject = facility.ProjectAssignment != null ? projectMapping.CreateTargetObject() : projectMapping.GetOrCreateTargetObject(facility.externalID);
            projectMapping.AddMapping(facility.ProjectAssignment, ifcProject); 
            #endregion

            #region Site
            //add the relationship between the site and the building if a site exists
            if (facility.SiteAssignment != null)
            {
                var siteMapping = Exchanger.GetOrCreateMappings<MappingSiteTypeToIfcSite>();
                var ifcSite = siteMapping.AddMapping(facility.SiteAssignment,
                    siteMapping.GetOrCreateTargetObject(facility.SiteAssignment.externalID));
                //add the relationship between the site and the project and the building
                ifcProject.AddSite(ifcSite);
                ifcSite.AddBuilding(ifcBuilding);
            }
            else //relate the building to the project
                ifcProject.AddBuilding(ifcBuilding); 
            #endregion

            #region Floors
            //write out the floors if we have any
            if (facility.Floors != null)
            {
                var floorMapping = Exchanger.GetOrCreateMappings<MappingFloorTypeToIfcBuildingStorey>();
                foreach (var floor in facility.Floors)
                {
                    var ifcFloor = floorMapping.AddMapping(floor, floorMapping.GetOrCreateTargetObject(floor.externalID));
                    ifcBuilding.AddToSpatialDecomposition(ifcFloor);
                }
            } 
            #endregion

            #region AssetTypes
            //write out the floors if we have any
            if (facility.AssetTypes != null)
            {
                var assetTypeMapping = Exchanger.GetOrCreateMappings<MappingAssetTypeInfoTypeToIfcTypeObject>();
                foreach (var assetType in facility.AssetTypes)
                {
                    var ifcFloor = assetTypeMapping.AddMapping(assetType, assetTypeMapping.GetOrCreateTargetObject(assetType.externalID));
                   
                }
            }
            #endregion

            #region Attributes
            if (facility.FacilityAttributes != null)
            {
                foreach (var attribute in facility.FacilityAttributes)
                    Exchanger.ConvertAttributeTypeToIfcObjectProperty(ifcBuilding, attribute);
            }
            #endregion

            return ifcBuilding;
        }
    }
}
