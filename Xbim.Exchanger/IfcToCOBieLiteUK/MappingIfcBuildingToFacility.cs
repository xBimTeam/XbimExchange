using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;
using System = Xbim.COBieLiteUK.System;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    class MappingIfcBuildingToFacility : XbimMappings<XbimModel, List<Facility>, string, IfcBuilding,Facility> 
    {
        protected override Facility Mapping(IfcBuilding ifcBuilding, Facility facility)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            var model = ifcBuilding.ModelOf;
            facility.ExternalEntity = helper.ExternalEntityName(ifcBuilding);
            facility.ExternalId = helper.ExternalEntityIdentity(ifcBuilding);
            facility.ExternalSystem = helper.ExternalSystemName(ifcBuilding);
            facility.Name = helper.GetFacilityName(ifcBuilding);
            facility.Description = ifcBuilding.Description;
            facility.CreatedBy = helper.GetCreatedBy(ifcBuilding);
            facility.CreatedOn = helper.GetCreatedOn(ifcBuilding);
            facility.Categories = helper.GetCategories(ifcBuilding);
            var ifcProject = model.Instances.OfType<IfcProject>().FirstOrDefault();
            if (ifcProject != null)
            {
                if (facility.Categories == null) //use the project Categories instead
                    facility.Categories = helper.GetCategories(ifcProject);
                facility.Project = new Project();
                var projectMapping = Exchanger.GetOrCreateMappings<MappingIfcProjectToProject>();
                projectMapping.AddMapping(ifcProject, facility.Project);
                var ifcSite = ifcProject.GetSpatialStructuralElements().FirstOrDefault(p => p is IfcSite) as IfcSite;
                var siteMapping = Exchanger.GetOrCreateMappings<MappingIfcSiteToSite>();
                if (ifcSite != null)
                {
                    facility.Site = new Site();
                    siteMapping.AddMapping(ifcSite, facility.Site);
                }
                else //create a default "External area"
                {
                    facility.Site = new Site
                    {
                        Description = "Default  area if no site has been defined in the model",
                        Name = "Default"
                    };
                    
                }
                facility.AreaUnits = helper.ModelAreaUnit ?? AreaUnit.notdefined;
                facility.LinearUnits = helper.ModelLinearUnit ?? LinearUnit.notdefined;
                facility.VolumeUnits = helper.ModelVolumeUnit ?? VolumeUnit.notdefined;
                facility.CurrencyUnit = helper.ModelCurrencyUnit ?? CurrencyUnit.notdefined;

                var storeys = ifcBuilding.GetBuildingStoreys(true);
                var cobieFloors = storeys.Cast<IfcSpatialStructureElement>().ToList();
                cobieFloors.Add(ifcSite);
                cobieFloors.Add(ifcBuilding);

                facility.Floors = new List<Floor>(cobieFloors.Count);
                var floorMappings = Exchanger.GetOrCreateMappings<MappingIfcSpatialStructureElementToFloor>();
                for (int i = 0; i < cobieFloors.Count; i++)
                {
                    var floor = new Floor();
                    floor = floorMappings.AddMapping(cobieFloors[i], floor);
                    facility.Floors.Add(floor);
                }

            }
            //Attributes
            facility.Attributes = helper.GetAttributes(ifcBuilding);

            //Zones

            var allSpaces = GetAllSpaces(ifcBuilding);
            var allZones = GetAllZones(allSpaces, helper);
            var ifcZones = allZones.ToArray();
            if (ifcZones.Any())
            {
                facility.Zones = new List<Zone>(ifcZones.Length);
                var zoneMappings = Exchanger.GetOrCreateMappings<MappingIfcZoneToZone>();
                for (int i = 0; i < ifcZones.Length; i++)
                {
                    var zone = new Zone();
                    zone = zoneMappings.AddMapping(ifcZones[i], zone);
                    facility.Zones.Add(zone);
                }
            }

            //Assets
          //  var allIfcElementsinThisFacility = new HashSet<IfcElement>(helper.GetAllAssets(ifcBuilding));

            //AssetTypes
            //Get all assets that are in this facility/building
            //Asset Types are groups of assets that share a common typology
            //Some types are defined explicitly in the ifc file some have to be inferred

            var allIfcTypes = helper.DefiningTypeObjectMap.OrderBy(t=>t.Key.Name);
            if (allIfcTypes.Any())
            {
                facility.AssetTypes = new List<AssetType>(); 
                var assetTypeMappings = Exchanger.GetOrCreateMappings<MappingXbimIfcProxyTypeObjectToAssetType>();
                foreach (var elementsByType in allIfcTypes)
                {
                    if (elementsByType.Value.Any())
                    {
                        var assetType = new AssetType();
                        assetType = assetTypeMappings.AddMapping(elementsByType.Key, assetType);
                        facility.AssetTypes.Add(assetType);
                    }
                }
            }

            //Systems
           
            var ifcSystems = helper.SystemAssignment;
            if (ifcSystems.Any())
            {
                facility.Systems = new List<Xbim.COBieLiteUK.System>(ifcSystems.Count);
                var systemMappings = Exchanger.GetOrCreateMappings<MappingIfcSystemToSystem>();
                foreach (var ifcSystem in ifcSystems.Keys)
                {
                    var system = new Xbim.COBieLiteUK.System();
                    system = systemMappings.AddMapping(ifcSystem, system);
                    facility.Systems.Add(system);
                }
            }

            //Contacts
            var ifcActorSelects = helper.Contacts;

            if (ifcActorSelects!=null && ifcActorSelects.Any())
            {

                var cobieContacts = new List<Contact>(ifcActorSelects.Count());
                var contactMappings = Exchanger.GetOrCreateMappings<MappingIfcActorToContact>();
                foreach (var actor in ifcActorSelects)
                {
                    var contact = new Contact();
                    contact = contactMappings.AddMapping(actor, contact);
                    cobieContacts.Add(contact);
                }
                facility.Contacts = cobieContacts.Distinct(new ContactComparer()).ToList();
            }

            //assign all unallocated spaces to a zone
            var spaces = facility.Get<Space>().ToList();
            var zones = facility.Zones ?? new List<Zone>(); 
            var defaultZone = helper.CreateXbimDefaultZone();
            foreach (
                var space in
                    spaces.Where(
                        space => !zones.Any(z => z.Spaces != null && z.Spaces.Select(s => s.Name).Contains(space.Name)))
                )
            {           
                if (facility.Zones == null) facility.Zones = new List<Zone>();
               
                defaultZone.Spaces.Add(new SpaceKey { Name = space.Name });
            }
            if (facility.Zones != null) facility.Zones.Add(defaultZone);
            //assign all assets that are not in a system to the default
            var assetTypes = facility.Get<AssetType>().ToList();
            var systemsWritten = facility.Get<Xbim.COBieLiteUK.System>();
            var assetsAssignedToSystem = new HashSet<string>(systemsWritten.SelectMany(s => s.Components).Select(a => a.Name));
            var systems = facility.Systems ?? new List<Xbim.COBieLiteUK.System>();
            var defaultSystem = helper.CreateUndefinedSystem();
            //go over all unasigned assets
            foreach (var assetType in assetTypes)
            {
                Xbim.COBieLiteUK.System assetTypeSystem = null;
                foreach (var asset in assetType.Assets.Where(a=>!assetsAssignedToSystem.Contains(a.Name)))
                {
                    if (assetTypeSystem == null)
                    {
                        assetTypeSystem = helper.CreateUndefinedSystem();
                        assetTypeSystem.Name = string.Format("System {0} ", assetType.Name);
                        
                    }
                    assetTypeSystem.Components.Add(new AssetKey { Name = asset.Name });
                }

                //add to tle list only if it is not null
                if (assetTypeSystem == null) continue;
                if (facility.Systems == null) facility.Systems = new List<Xbim.COBieLiteUK.System>();
                facility.Systems.Add(assetTypeSystem);
            }
           

            //write out contacts created in the process
            if (helper.SundryContacts.Any())
            {
                 if(facility.Contacts==null) facility.Contacts = new List<Contact>();
                facility.Contacts.AddRange(helper.SundryContacts.Values);
            }
            
            helper.SundryContacts.Clear(); //clear ready for processing next facility
            return facility;
        }

        //private static HashSet<IfcTypeObject> AllAssetTypesInThisFacility(IfcBuilding ifcBuilding,
        //HashSet<IfcElement> allAssetsinThisFacility, CoBieLiteUkHelper helper)
        //{

        //    var allAssetTypes = helper.DefiningTypeObjectMap;
        //    var allAssetTypesInThisFacility = new HashSet<IfcTypeObject>();
        //    foreach (var assetTypeKeyValue in allAssetTypes)
        //    {
        //        //if any defining type has an object in this building/facility then we need to include it
        //        if (assetTypeKeyValue.Value.Any(allAssetsinThisFacility.Contains))
        //            allAssetTypesInThisFacility.Add(assetTypeKeyValue.Key);
        //    }
        //    return allAssetTypesInThisFacility;
        //}

        private IEnumerable<IfcZone> GetAllZones(IEnumerable<IfcSpace> allSpaces, CoBieLiteUkHelper helper)
        {
            var allZones = new HashSet<IfcZone>();
            foreach (var space in allSpaces)
                foreach (var zone in helper.GetZones(space))
                    allZones.Add(zone);
            return allZones;
        }

        private IEnumerable<IfcSpace> GetAllSpaces(IfcBuilding ifcBuilding)
        {
            var spaces = new HashSet<IfcSpace>();
            foreach (var space in ifcBuilding.GetSpaces().ToList())
                spaces.Add(space);
            foreach (var storey in ifcBuilding.GetBuildingStoreys().ToList())
            {
                foreach (var storeySpace in storey.GetSpaces().ToList())
                {
                    spaces.Add(storeySpace);
                    foreach (var spaceSpace in storeySpace.GetSpaces().ToList())
                        spaces.Add(spaceSpace); //get sub spaces
                }
            }
            return spaces;
        }
    }
}
