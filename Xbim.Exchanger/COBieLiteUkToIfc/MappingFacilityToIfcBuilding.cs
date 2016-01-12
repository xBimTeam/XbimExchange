using System;
using System.Linq;
using Xbim.Common;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.Extensions;
using XbimExchanger.IfcHelpers;
using Xbim.Ifc2x3.ActorResource;

namespace XbimExchanger.COBieLiteUkToIfc
{
    class MappingFacilityToIfcBuilding : CoBieLiteUkIfcMappings<string, Facility, IfcBuilding>
    {

        protected override IfcBuilding Mapping(Facility facility, IfcBuilding ifcBuilding)
        {
            #region Properties

            ifcBuilding.Name = facility.Name;
            ifcBuilding.Description = facility.Description;
            ifcBuilding.CompositionType=IfcElementCompositionEnum.ELEMENT;
            #endregion
           
            #region Default units

            Exchanger.DefaultLinearUnit = new IfcUnitConverter(facility.LinearUnits.ToString());       
            Exchanger.DefaultAreaUnit = new IfcUnitConverter(facility.AreaUnits.ToString());
            Exchanger.DefaultVolumeUnit = new IfcUnitConverter(facility.VolumeUnits.ToString());
            Exchanger.DefaultCurrencyUnit = facility.CurrencyUnit;

            #endregion

            #region Contacts
            if (facility.Contacts != null && facility.Contacts.Any())
            {
                var ContactMapping = Exchanger.GetOrCreateMappings<MappingContactToIfcPersonAndOrganization>();
                foreach (var contact in facility.Contacts)
                {
                    IfcPersonAndOrganization ifcPersonAndOrganization = ContactMapping.AddMapping(contact, ContactMapping.GetOrCreateTargetObject(contact.ExternalId));
                    //assign relationship
                    //create IfcActor to set CreatedBy and CreatedOn for next time ifc is imported as IfcActor is derived from IfcRoot
                    IfcActor actor = Exchanger.TargetRepository.Instances.New<IfcActor>();
                    Exchanger.SetUserHistory(actor, contact.ExternalSystem, (contact.CreatedBy == null) ? null : contact.CreatedBy.Email, (contact.CreatedOn == null) ? DateTime.Now : (DateTime)contact.CreatedOn);
                    using (OwnerHistoryEditScope context = new OwnerHistoryEditScope(Exchanger.TargetRepository, actor.OwnerHistory))
                    {
                        actor.TheActor = ifcPersonAndOrganization;
                    }
                    //assign the actor to the building
                    IfcRelAssignsToActor ifcRelAssignsToActor = Exchanger.TargetRepository.Instances.New<IfcRelAssignsToActor>();
                    Exchanger.SetUserHistory(ifcRelAssignsToActor, contact.ExternalSystem, (contact.CreatedBy == null) ? null : contact.CreatedBy.Email, (contact.CreatedOn == null) ? DateTime.Now : (DateTime)contact.CreatedOn);
                    using (OwnerHistoryEditScope context = new OwnerHistoryEditScope(Exchanger.TargetRepository, ifcRelAssignsToActor.OwnerHistory))
                    {
                        ifcRelAssignsToActor.RelatingActor = actor;
                        ifcRelAssignsToActor.RelatedObjects.Add(ifcBuilding);
                    }
                } 
            }
            #endregion

            #region Categories
            if (facility.Categories != null)
                foreach (var category in facility.Categories)
                {
                    Exchanger.ConvertCategoryToClassification(category, ifcBuilding);
                }

            #endregion


            #region Project
            
            
            
            //COBie does nor require a project but Ifc does
            var ifcProject = Exchanger.TargetRepository.Instances.OfType<IfcProject>().FirstOrDefault();
            if (ifcProject == null)
               ifcProject= Exchanger.TargetRepository.Instances.New<IfcProject>();
            var projectMapping = Exchanger.GetOrCreateMappings<MappingProjectToIfcProject>();
            projectMapping.AddMapping(facility.Project, ifcProject);
            InitialiseUnits(ifcProject);
            #endregion

            #region Site
            //add the relationship between the site and the building if a site exists
            if (facility.Site != null)
            {
                var siteMapping = Exchanger.GetOrCreateMappings<MappingSiteToIfcSite>();
                var ifcSite = siteMapping.AddMapping(facility.Site,
                    siteMapping.GetOrCreateTargetObject(facility.Site.ExternalId ?? Guid.NewGuid().ToString()));
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
                var floorMapping = Exchanger.GetOrCreateMappings<MappingFloorToIfcBuildingStorey>();
                foreach (var floor in facility.Floors)
                {
                    var ifcFloor = floorMapping.AddMapping(floor, floorMapping.GetOrCreateTargetObject(floor.ExternalId));
                    ifcBuilding.AddToSpatialDecomposition(ifcFloor);
                }
            } 
            #endregion


            #region AssetTypes
            //write out the floors if we have any
            if (facility.AssetTypes != null)
            {
                var assetTypeMapping = Exchanger.GetOrCreateMappings<MappingAssetTypeToIfcTypeObject>();
                foreach (var assetType in facility.AssetTypes.OrderBy(a=>a.ExternalEntity))
                {
                    Exchanger.BeginAssetTypeInfoType();
                    assetTypeMapping.AddMapping(assetType, assetTypeMapping.GetOrCreateTargetObject(assetType.ExternalId));
                    Exchanger.EndAssetTypeInfoType();
                   
                }
            }
            #endregion

            #region Attributes
            if (facility.Attributes != null)
            {
                foreach (var attribute in facility.Attributes)
                    Exchanger.ConvertAttributeTypeToIfcObjectProperty(ifcBuilding, attribute);
            }
            #endregion

            #region Zones

            if (facility.Zones != null)
            {
                var zoneTypeMapping = Exchanger.GetOrCreateMappings<MappingZoneToIfcZone>();
                foreach (var zone in facility.Zones)
                {

                    zoneTypeMapping.AddMapping(zone, zoneTypeMapping.GetOrCreateTargetObject(zone.ExternalId));
                   
                }
            }

            #endregion

            #region Documents
            if (facility.Documents != null && facility.Documents.Any())
            {
                Exchanger.ConvertDocumentsToDocumentSelect(ifcBuilding, facility.Documents);
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
