using System.Collections.Generic;
using System.Linq;
using Xbim.CobieExpress;
using Xbim.COBieLiteUK;
using Xbim.Ifc4.Interfaces;
using netSystem = System;

namespace XbimExchanger.IfcToCOBieExpress 
{
    internal class MappingIfcBuildingToFacility : MappingIfcObjectToAsset<IIfcBuilding,CobieFacility> 
    {
        protected override CobieFacility Mapping(IIfcBuilding ifcBuilding, CobieFacility facility)
        {
            base.Mapping(ifcBuilding, facility);

            //Helper should do 10% of progress
            Exchanger.ReportProgress.NextStage(4, 42, string.Format("Creating Facility {0}", ifcBuilding.Name != null ? ifcBuilding.Name.ToString() : string.Empty));//finish progress at 42% 
            var helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper;
            var model = ifcBuilding.Model;

            var ifcProject = model.Instances.OfType<IIfcProject>().FirstOrDefault();
            if (ifcProject != null)
            {
                if (!facility.Categories.Any()) //use the project Categories instead
                    facility.Categories.AddRange(helper.GetCategories(ifcProject));
                var projectMapping = Exchanger.GetOrCreateMappings<MappingIfcProjectToProject>();
                CobieProject cProject;
                if(projectMapping.GetOrCreateTargetObject(ifcProject.EntityLabel, out cProject))
                    projectMapping.AddMapping(ifcProject, cProject);
                facility.Project = cProject;

                Exchanger.ReportProgress.IncrementAndUpdate(); 
                var ifcSite = ifcProject.Sites.FirstOrDefault();
                var siteMapping = Exchanger.GetOrCreateMappings<MappingIfcSiteToSite>();

                //Facility Attributes
                foreach (var attr in helper.GetAttributes(ifcBuilding))
                    facility.Attributes.Add(attr);

                
                if (ifcSite != null)
                {
                     CobieSite site;
                    if(siteMapping.GetOrCreateTargetObject(ifcSite.EntityLabel, out site))
                        siteMapping.AddMapping(ifcSite, facility.Site);
                    
                    if(ifcSite.RefLatitude.HasValue && ifcSite.RefLongitude.HasValue)
                    {
                        facility.Attributes.Add(helper.MakeAttribute(ifcSite, "RefLatitude", ifcSite.RefLatitude.Value.AsDouble));
                        facility.Attributes.Add(helper.MakeAttribute(ifcSite, "RefLongtitude", ifcSite.RefLongitude.Value.AsDouble));
                    }
                    facility.Site = site;
                }
                else //create a default "External area"
                {
                    facility.Site = Exchanger.TargetRepository.Instances.New<CobieSite>(s =>
                    {
                        s.Description = "Default area if no site has been defined in the model";
                        s.Name = "Default";
                    });
                    
                }
                Exchanger.ReportProgress.IncrementAndUpdate();
                facility.AreaUnits = helper.ModelAreaUnit;
                facility.LinearUnits = helper.ModelLinearUnit;
                facility.VolumeUnits = helper.ModelVolumeUnit;
                facility.CurrencyUnit = helper.ModelCurrencyUnit;

                var storeys = ifcBuilding.BuildingStoreys;
                var cobieFloors = storeys.Cast<IIfcSpatialStructureElement>().ToList();
                if (ifcSite != null)
                    cobieFloors.Add(ifcSite);
                Exchanger.ReportProgress.IncrementAndUpdate();
                if (ifcBuilding != null)
                    cobieFloors.Add(ifcBuilding);
                Exchanger.ReportProgress.IncrementAndUpdate();
                Exchanger.ReportProgress.NextStage(cobieFloors.Count, 50); //finish progress at 50% 
                var floorMappings = Exchanger.GetOrCreateMappings<MappingIfcSpatialElementToFloor>();
                foreach (var ifcFloor in cobieFloors)
                {
                    CobieFloor floor;
                    if(floorMappings.GetOrCreateTargetObject(ifcFloor.EntityLabel, out floor))
                        floor = floorMappings.AddMapping(ifcFloor, floor);
                    floor.Facility = facility;
                    Exchanger.ReportProgress.IncrementAndUpdate();
                }

            }
            

            //attach orphan documents to the root facility
            if (helper.OrphanDocs.Any())
            {
                var docsMappings = Exchanger.GetOrCreateMappings<MappingIfcDocumentSelectToDocument>();
                var documents = helper.OrphanDocs
                    .Select(docSel => docsMappings.MappingMulti(docSel))
                    .SelectMany(docs => docs);
                facility.Documents.AddRange(documents);
            }
            
            //Zones
            
            var allSpaces = GetAllSpaces(ifcBuilding);
            var allZones = GetAllZones(allSpaces, helper);
            var ifcZones = allZones.ToArray();
            if (ifcZones.Any())
            {
                Exchanger.ReportProgress.NextStage(ifcZones.Length, 65); //finish progress at 65% 
                var zoneMappings = Exchanger.GetOrCreateMappings<MappingIfcZoneToZone>();
                foreach (var ifcZone in ifcZones)
                {
                    CobieZone zone;
                    if(zoneMappings.GetOrCreateTargetObject(ifcZone.EntityLabel, out zone))
                        zoneMappings.AddMapping(ifcZone, zone);
                    Exchanger.ReportProgress.IncrementAndUpdate();
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
                Exchanger.ReportProgress.NextStage(allIfcTypes.Count(), 90); //finish progress at 90% 
                var assetTypeMappings = Exchanger.GetOrCreateMappings<MappingXbimIfcProxyTypeObjectToAssetType>();
                foreach (var elementsByType in allIfcTypes)
                {
                    if (!elementsByType.Value.Any()) continue;

                    var type = assetTypeMappings.CreateTargetObject();
                    assetTypeMappings.AddMapping(elementsByType.Key, type);
                    Exchanger.ReportProgress.IncrementAndUpdate();
                }
            }

            //Systems
            if (helper.SystemMode.HasFlag(SystemExtractionMode.System) && helper.SystemAssignment.Any())
            {
                var systemMappings = Exchanger.GetOrCreateMappings<MappingIfcSystemToSystem>();
                Exchanger.ReportProgress.NextStage(helper.SystemAssignment.Keys.Count(), 95); //finish progress at 95% 
                foreach (var ifcSystem in helper.SystemAssignment.Keys)
                {
                    var system = new Xbim.COBieLiteUK.System();
                    system = systemMappings.AddMapping(ifcSystem, system);
                    facility.Systems.Add(system);
                    Exchanger.ReportProgress.IncrementAndUpdate();
                }
            }

            //Get systems via propertySets
            if (helper.SystemMode.HasFlag(SystemExtractionMode.PropertyMaps) && helper.SystemViaPropAssignment.Any())
            {
                var systemMappings = Exchanger.GetOrCreateMappings<MappingSystemViaIfcPropertyToSystem>();
                Exchanger.ReportProgress.NextStage(helper.SystemAssignment.Keys.Count(), 96); //finish progress at 95% 
                foreach (var ifcPropSet in helper.SystemViaPropAssignment.Keys)
                {
                    var system = new Xbim.COBieLiteUK.System();
                    system = systemMappings.AddMapping(ifcPropSet, system);
                    var init = facility.Systems.Where(sys => sys.Name.Equals(system.Name, netSystem.StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    if (init != null)
                    {
                        var idx = facility.Systems.IndexOf(init);
                        facility.Systems[idx].Components = facility.Systems[idx].Components.Concat(system.Components).Distinct(new AssetKeyCompare()).ToList();
                    }
                    else
                    {
                        facility.Systems.Add(system);
                    }
                    Exchanger.ReportProgress.IncrementAndUpdate();
                }
            }
            

            //Contacts - all contacts are converted already in helper constructor
            Exchanger.ReportProgress.NextStage(helper.Contacts.Count, 97); //finish progress at 97% 
            

            //assign all unallocated spaces to a zone
            var spaces = Exchanger.TargetRepository.Instances.OfType<CobieSpace>().ToList();
            var zones = Exchanger.TargetRepository.Instances.OfType<CobieZone>().ToList() ?? new List<CobieZone>();
            var unAllocatedSpaces = spaces.Where(space => !zones.Any(z => z.Spaces != null && z.Spaces.Select(s => s.Name).Contains(space.Name))).ToList();
            Exchanger.ReportProgress.NextStage(unAllocatedSpaces.Count(), 98); //finish progress at 98% 
            if (unAllocatedSpaces.Any())
            {
                var defaultZone = helper.XbimDefaultZone;
                foreach (var space in unAllocatedSpaces)
                {
                    defaultZone.Spaces.Add(space);
                    Exchanger.ReportProgress.IncrementAndUpdate();
                }
            }

            //assign all assets that are not in a system to the default
            if (helper.SystemMode.HasFlag(SystemExtractionMode.Types))
            {
                var assetTypes = facility.Get<AssetType>().ToList();
                var systemsWritten = facility.Get<Xbim.COBieLiteUK.System>();
                var assetsAssignedToSystem = new HashSet<string>(systemsWritten.SelectMany(s => s.Components).Select(a => a.Name));
                var systems = facility.Systems ?? new List<Xbim.COBieLiteUK.System>();
                var defaultSystem = helper.CreateUndefinedSystem();
                Exchanger.ReportProgress.NextStage(assetTypes.Count(), 100); //finish progress at 100% 
                //go over all unasigned assets
                foreach (var assetType in assetTypes)
                {
                    Xbim.COBieLiteUK.System assetTypeSystem = null;
                    foreach (var asset in assetType.Assets.Where(a => !assetsAssignedToSystem.Contains(a.Name)))
                    {
                        if (assetTypeSystem == null)
                        {
                            assetTypeSystem = helper.CreateUndefinedSystem();
                            assetTypeSystem.Name = string.Format("Type System {0} ", assetType.Name);

                        }
                        assetTypeSystem.Components.Add(new AssetKey { Name = asset.Name });
                    }

                    //add to tle list only if it is not null
                    if (assetTypeSystem == null)
                        continue;
                    if (facility.Systems == null)
                        facility.Systems = new List<Xbim.COBieLiteUK.System>();
                    facility.Systems.Add(assetTypeSystem);
                    Exchanger.ReportProgress.IncrementAndUpdate();
                } 
            }
           

            //write out contacts created in the process
            if (helper.SundryContacts.Any())
            {
                 if(facility.Contacts==null) facility.Contacts = new List<Contact>();
                facility.Contacts.AddRange(helper.SundryContacts.Values);
            }
            
            helper.SundryContacts.Clear(); //clear ready for processing next facility

            Exchanger.ReportProgress.Finalise(500); //finish with 500 millisecond delay
            
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

        private static IEnumerable<IIfcZone> GetAllZones(IEnumerable<IIfcSpace> allSpaces, COBieExpressHelper helper)
        {
            var allZones = new HashSet<IIfcZone>();
            foreach (var space in allSpaces)
                foreach (var zone in helper.GetZones(space))
                    allZones.Add(zone);
            return allZones;
        }

        private static IEnumerable<IIfcSpace> GetAllSpaces(IIfcBuilding ifcBuilding)
        {
            var spaces = new HashSet<IIfcSpace>();
            foreach (var space in ifcBuilding.Spaces.ToList())
                spaces.Add(space);
            foreach (var storey in ifcBuilding.BuildingStoreys.ToList())
            {
                foreach (var storeySpace in storey.Spaces.ToList())
                {
                    spaces.Add(storeySpace);
                    foreach (var spaceSpace in storeySpace.Spaces.ToList())
                        spaces.Add(spaceSpace); //get sub spaces
                }
            }
            return spaces;
        }

        public override CobieFacility CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieFacility>();
        }
    }
}
