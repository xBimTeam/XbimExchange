using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.CobieExpress;
using Xbim.Ifc4.Interfaces;
using XbimExchanger.IfcHelpers;

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

                if (ifcSite != null)
                {
                     CobieSite site;
                    if(siteMapping.GetOrCreateTargetObject(ifcSite.EntityLabel, out site))
                        siteMapping.AddMapping(ifcSite, site);
                    
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
                Exchanger.ReportProgress.NextStage(helper.SystemAssignment.Keys.Count, 95); //finish progress at 95% 
                foreach (var ifcSystem in helper.SystemAssignment.Keys)
                {
                    CobieSystem system;
                    if (systemMappings.GetOrCreateTargetObject(ifcSystem.EntityLabel, out system))
                    {
                        system = systemMappings.AddMapping(ifcSystem, system);
                        system.Facility = facility;
                    }
                    Exchanger.ReportProgress.IncrementAndUpdate();
                }
            }

            //Get systems via propertySets
            if (helper.SystemMode.HasFlag(SystemExtractionMode.PropertyMaps) && helper.SystemViaPropAssignment.Any())
            {
                var systemMappings = Exchanger.GetOrCreateMappings<MappingSystemViaIfcPropertyToSystem>();
                Exchanger.ReportProgress.NextStage(helper.SystemAssignment.Keys.Count, 96); //finish progress at 95% 
                foreach (var ifcPropSet in helper.SystemViaPropAssignment.Keys)
                {
                    CobieSystem system;
                    if(systemMappings.GetOrCreateTargetObject(ifcPropSet.EntityLabel, out system))
                        system = systemMappings.AddMapping(ifcPropSet, system);

                    var duplicity = facility.Systems.FirstOrDefault(sys => sys.Name.Equals(system.Name, StringComparison.InvariantCultureIgnoreCase));
                    if (duplicity != null)
                    {
                        //move components to the original system
                        var components = duplicity.Components.ToList().Concat(system.Components).Distinct();
                        duplicity.Components.Clear();
                        duplicity.Components.AddRange(components);
                        system.Components.Clear();
                    }
                    else
                    {
                        //only assign system to facility if it is unique
                        system.Facility = facility;
                    }
                    Exchanger.ReportProgress.IncrementAndUpdate();
                }
            }
            

            //Contacts - all contacts are converted already in helper constructor
            Exchanger.ReportProgress.NextStage(helper.Contacts.Count, 97); //finish progress at 97% 
            

            ////assign all unallocated spaces to a zone
            //var spaces = Exchanger.TargetRepository.Instances.OfType<CobieSpace>().ToList();
            //var zones = Exchanger.TargetRepository.Instances.OfType<CobieZone>().ToList() ?? new List<CobieZone>();
            //var unAllocatedSpaces = spaces.Where(space => !zones.Any(z => z.Spaces != null && z.Spaces.Select(s => s.Name).Contains(space.Name))).ToList();
            //Exchanger.ReportProgress.NextStage(unAllocatedSpaces.Count, 98); //finish progress at 98% 
            //if (unAllocatedSpaces.Any())
            //{
            //    var defaultZone = helper.XbimDefaultZone;
            //    foreach (var space in unAllocatedSpaces)
            //    {
            //        defaultZone.Spaces.Add(space);
            //        Exchanger.ReportProgress.IncrementAndUpdate();
            //    }
            //}

            ////assign all assets that are not in a system to the default
            //if (helper.SystemMode.HasFlag(SystemExtractionMode.Types))
            //{
            //    var types = Exchanger.TargetRepository.Instances.OfType<CobieType>().ToList();
            //    var systemsWritten = Exchanger.TargetRepository.Instances.OfType<CobieSystem>().ToList();
            //    var componentsAssignedToSystem = new HashSet<string>(systemsWritten.SelectMany(s => s.Components).Select(a => a.Name));
            //    Exchanger.ReportProgress.NextStage(types.Count); //finish progress at 100% 
            //    //go over all unasigned assets
            //    foreach (var type in types)
            //    {
            //        var components = type.Components.Where(a => !componentsAssignedToSystem.Contains(a.Name)).ToList();
            //        if (components.Any())
            //        {
            //            var typeSystem = helper.XbimDefaultSystem;
            //            typeSystem.Name = string.Format("Type System {0} ", type.Name);
            //            typeSystem.Components.AddRange(components);
            //            typeSystem.Facility = facility;
            //        }
            //        Exchanger.ReportProgress.IncrementAndUpdate();
            //    } 
            //}
           
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
