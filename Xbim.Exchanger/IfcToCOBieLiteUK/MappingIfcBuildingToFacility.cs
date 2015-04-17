using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;
using Xbim.XbimExtensions.SelectTypes;

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
            facility.Categories = helper.GetCategories(ifcBuilding);
            var ifcProject = model.Instances.OfType<IfcProject>().FirstOrDefault();
            if (ifcProject != null)
            {
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
                var ifcBuildingStories = storeys as IList<IfcBuildingStorey> ?? storeys.ToList();
                if (ifcBuildingStories.Any())
                {
                    facility.Floors = new List<Floor>(ifcBuildingStories.Count);
                    var floorMappings = Exchanger.GetOrCreateMappings<MappingIfcBuildingStoreyToFloor>();
                    for (int i = 0; i < ifcBuildingStories.Count; i++)
                    {
                        var floor = new Floor();
                        floor = floorMappings.AddMapping(ifcBuildingStories[i], floor);
                        facility.Floors.Add(floor);
                    }
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
            //var allSystemsInThisFacility = helper.SystemAssignment
            //    .Where(v => v.Value.Any(allIfcTypes.Contains))
            //    .Select(k => k.Key).ToArray();
            var ifcSystems = helper.SystemAssignment;
            if (helper.SystemAssignment.Any())
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
            var contacts = helper.GetContacts();
            var ifcActors = contacts as IfcActorSelect[] ?? contacts.ToArray();
            if (ifcActors.Any())
            {

                facility.Contacts = new List<Contact>(ifcActors.Length);
                var contactMappings = Exchanger.GetOrCreateMappings<MappingIfcActorToContact>();
                for (int i = 0; i < ifcActors.Length; i++)
                {
                    var contact = new Contact();
                    contact = contactMappings.AddMapping(ifcActors[i], contact);
                    facility.Contacts.Add(contact);
                }
            }

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
