using System;
using System.Linq;
using Xbim.Common;
using Xbim.COBieLite;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ProductExtension;
using XbimExchanger.IfcHelpers;
using XbimExchanger.IfcHelpers.Ifc2x3;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingFacilityTypeToIfcBuilding : CoBieLiteIfcMappings<string, FacilityType, IfcBuilding>
    {

        protected override IfcBuilding Mapping(FacilityType facility, IfcBuilding ifcBuilding)
        {
            #region Properties

            ifcBuilding.Name = facility.FacilityName;
            ifcBuilding.Description = facility.FacilityDescription;
            ifcBuilding.CompositionType=IfcElementCompositionEnum.ELEMENT;
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

            var ifcProject = Exchanger.TargetRepository.Instances.OfType<IfcProject>().FirstOrDefault();
            

            //COBie does nor require a project but Ifc does

            if (ifcProject == null)
               ifcProject= Exchanger.TargetRepository.Instances.New<IfcProject>();
     

            #region Project
                var projectMapping = Exchanger.GetOrCreateMappings<MappingProjectTypeToIfcProject>();
                projectMapping.AddMapping(facility.ProjectAssignment, ifcProject);
                InitialiseUnits(ifcProject);

                #endregion

                #region Site
                //add the relationship between the site and the building if a site exists
                if (facility.SiteAssignment != null)
                {
                    var siteMapping = Exchanger.GetOrCreateMappings<MappingSiteTypeToIfcSite>();
                    var ifcSite = siteMapping.AddMapping(facility.SiteAssignment,
                        siteMapping.GetOrCreateTargetObject(facility.SiteAssignment.externalID ?? Guid.NewGuid().ToString()));
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
                    foreach (var assetType in facility.AssetTypes.OrderBy(a => a.externalEntityName))
                    {
                        Exchanger.BeginAssetTypeInfoType();
                        assetTypeMapping.AddMapping(assetType, assetTypeMapping.GetOrCreateTargetObject(assetType.externalID));
                        Exchanger.EndAssetTypeInfoType();

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

                #region Add Space Geometry

                CreateSpaceProxies();

                #endregion
            
            return ifcBuilding;
        }

        /// <summary>
        /// Creates proxy object for each type of space based on the space category
        /// </summary>
        private void CreateSpaceProxies()
        {
          
        }

        private void InitialiseUnits(IfcProject ifcProject)
        {      
            ifcProject.Initialize(ProjectUnits.SIUnitsUK);
            //Area
            var areaUnit = ifcProject.UnitsInContext.AreaUnit as IfcSIUnit; //they always are as we are initialising to this
            if (areaUnit!=null && Exchanger.DefaultAreaUnit.HasValue)
            {
                var defaultAreaUnit = Exchanger.DefaultAreaUnit.Value;
                areaUnit.UnitType = defaultAreaUnit.UnitName;
                areaUnit.Prefix = defaultAreaUnit.SiPrefix;
                if (defaultAreaUnit.SiUnitName != null) areaUnit.Name = defaultAreaUnit.SiUnitName.Value;
                if (Math.Abs(defaultAreaUnit.ConversionFactor - 1) > 1e-9) //need to create conversion units
                {
                    var convBasedUnit = Exchanger.TargetRepository.Instances.New<IfcConversionBasedUnit>();
                    var measureWithUnit = Exchanger.TargetRepository.Instances.New<IfcMeasureWithUnit>();
                    var dimensionalUnits = Exchanger.TargetRepository.Instances.New<IfcDimensionalExponents>();
                    dimensionalUnits.LengthExponent = 2;
                    convBasedUnit.Dimensions = dimensionalUnits;
                    measureWithUnit.ValueComponent = new IfcRatioMeasure(defaultAreaUnit.ConversionFactor);
                    measureWithUnit.UnitComponent = areaUnit;
                    convBasedUnit.ConversionFactor = measureWithUnit;
                    convBasedUnit.UnitType = areaUnit.UnitType;
                    convBasedUnit.Name = defaultAreaUnit.UserDefinedSiUnitName;
                }
            }

            //Length
            var linearUnit = ifcProject.UnitsInContext.LengthUnit as IfcSIUnit; //they always are as we are initialising to this
            if (linearUnit != null && Exchanger.DefaultLinearUnit.HasValue)
            {
                var defaultLinearUnit = Exchanger.DefaultLinearUnit.Value;
                linearUnit.UnitType = defaultLinearUnit.UnitName;
                linearUnit.Prefix = defaultLinearUnit.SiPrefix;
                if (defaultLinearUnit.SiUnitName != null) linearUnit.Name = defaultLinearUnit.SiUnitName.Value;
                if (Math.Abs(defaultLinearUnit.ConversionFactor - 1) > 1e-9) //need to create conversion units
                {
                    var convBasedUnit = Exchanger.TargetRepository.Instances.New<IfcConversionBasedUnit>();
                    var dimensionalUnits = Exchanger.TargetRepository.Instances.New<IfcDimensionalExponents>();
                    dimensionalUnits.LengthExponent = 1;
                    convBasedUnit.Dimensions = dimensionalUnits;
                    var measureWithUnit = Exchanger.TargetRepository.Instances.New<IfcMeasureWithUnit>();
                    measureWithUnit.ValueComponent = new IfcRatioMeasure(defaultLinearUnit.ConversionFactor);
                    measureWithUnit.UnitComponent = linearUnit;
                    convBasedUnit.ConversionFactor = measureWithUnit;
                    convBasedUnit.UnitType = linearUnit.UnitType;
                    convBasedUnit.Name = defaultLinearUnit.UserDefinedSiUnitName;
                }
            }
            //Volume
            var volumeUnit = ifcProject.UnitsInContext.VolumeUnit as IfcSIUnit; //they always are as we are initialising to this
            if (volumeUnit != null && Exchanger.DefaultVolumeUnit.HasValue)
            {
                var defaultVolumeUnit = Exchanger.DefaultVolumeUnit.Value;
                volumeUnit.UnitType = defaultVolumeUnit.UnitName;
                volumeUnit.Prefix = defaultVolumeUnit.SiPrefix;
                if (defaultVolumeUnit.SiUnitName != null) volumeUnit.Name = defaultVolumeUnit.SiUnitName.Value;
                if (Math.Abs(defaultVolumeUnit.ConversionFactor - 1) > 1e-9) //need to create conversion units
                {
                    var convBasedUnit = Exchanger.TargetRepository.Instances.New<IfcConversionBasedUnit>();
                    var dimensionalUnits = Exchanger.TargetRepository.Instances.New<IfcDimensionalExponents>();
                    dimensionalUnits.LengthExponent = 3;
                    convBasedUnit.Dimensions = dimensionalUnits;
                    var measureWithUnit = Exchanger.TargetRepository.Instances.New<IfcMeasureWithUnit>();
                    measureWithUnit.ValueComponent = new IfcRatioMeasure(defaultVolumeUnit.ConversionFactor);
                    measureWithUnit.UnitComponent = volumeUnit;
                    convBasedUnit.ConversionFactor = measureWithUnit;
                    convBasedUnit.UnitType = volumeUnit.UnitType;
                    convBasedUnit.Name = defaultVolumeUnit.UserDefinedSiUnitName;
                }
            }
        }
    }
}
