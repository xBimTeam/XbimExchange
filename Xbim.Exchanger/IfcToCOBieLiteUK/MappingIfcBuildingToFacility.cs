using System.Collections.Generic;
using System.Linq;
using Xbim.Common;
using Xbim.CobieLiteUk;
using Xbim.Ifc4.Interfaces;
using netSystem = System;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.IfcToCOBieLiteUK 
{
    class MappingIfcBuildingToFacility : XbimMappings<IModel, List<Facility>, string, IIfcBuilding,Facility> 
    {
        protected override Facility Mapping(IIfcBuilding ifcBuilding, Facility facility)
        {
            //Reset to prevent freezing in cases of various facilities
            Exchanger.ReportProgress.Reset();
            //Helper should do 10% of progress
            Exchanger.ReportProgress.NextStage(4, 42, string.Format("Creating Facility {0}", ifcBuilding.Name != null ? ifcBuilding.Name.ToString() : string.Empty));//finish progress at 42% 
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            var model = ifcBuilding.Model;
            facility.ExternalEntity = helper.ExternalEntityName(ifcBuilding);
            facility.ExternalId = helper.ExternalEntityIdentity(ifcBuilding);
            facility.AltExternalId = ifcBuilding.GlobalId;
            facility.ExternalSystem = helper.ExternalSystemName(ifcBuilding);
            facility.Name = helper.GetFacilityName(ifcBuilding);
            facility.Description = ifcBuilding.Description;
            facility.CreatedBy = helper.GetCreatedBy(ifcBuilding);
            facility.CreatedOn = helper.GetCreatedOn(ifcBuilding);
            facility.Categories = helper.GetCategories(ifcBuilding);
            var ifcProject = model.Instances.OfType<IIfcProject>().FirstOrDefault();
            if (ifcProject != null)
            {
                if (facility.Categories == null) //use the project Categories instead
                    facility.Categories = helper.GetCategories(ifcProject);
                facility.Project = new Project();
                var projectMapping = Exchanger.GetOrCreateMappings<MappingIfcProjectToProject>();
                projectMapping.AddMapping(ifcProject, facility.Project);
                Exchanger.ReportProgress.IncrementAndUpdate(); 
                var ifcSite = ifcProject.Sites.FirstOrDefault();
                var siteMapping = Exchanger.GetOrCreateMappings<MappingIfcSiteToSite>();

                //Facility Attributes
                facility.Attributes = helper.GetAttributes(ifcBuilding);
                
                if (ifcSite != null)
                {
                    facility.Site = new Site();
                    siteMapping.AddMapping(ifcSite, facility.Site);
                    
                    if(ifcSite.RefLatitude.HasValue && ifcSite.RefLongitude.HasValue)
                    {
                        facility.Attributes.Add(helper.MakeAttribute(ifcSite, "RefLatitude", ifcSite.RefLatitude.Value.AsDouble));
                        facility.Attributes.Add(helper.MakeAttribute(ifcSite, "RefLongtitude", ifcSite.RefLongitude.Value.AsDouble));
                    }
                }
                else //create a default "External area"
                {
                    facility.Site = new Site
                    {
                        Description = "Default area if no site has been defined in the model",
                        Name = "Default"
                    };
                    
                }
                Exchanger.ReportProgress.IncrementAndUpdate();
                facility.AreaUnits = helper.ModelAreaUnit ?? AreaUnit.notdefined;
                facility.LinearUnits = helper.ModelLinearUnit ?? LinearUnit.notdefined;
                facility.VolumeUnits = helper.ModelVolumeUnit ?? VolumeUnit.notdefined;
                facility.CurrencyUnit = helper.ModelCurrencyUnit ?? CurrencyUnit.notdefined;

                var storeys = ifcBuilding.BuildingStoreys;
                var cobieFloors = storeys.Cast<IIfcSpatialStructureElement>().ToList();
                if (ifcSite != null)
                    cobieFloors.Add(ifcSite);
                Exchanger.ReportProgress.IncrementAndUpdate();
                if (ifcBuilding != null)
                    cobieFloors.Add(ifcBuilding);
                Exchanger.ReportProgress.IncrementAndUpdate();
                facility.Floors = new List<Floor>(cobieFloors.Count);
                Exchanger.ReportProgress.NextStage(cobieFloors.Count, 50); //finish progress at 50% 
                var floorMappings = Exchanger.GetOrCreateMappings<MappingIfcSpatialStructureElementToFloor>();
                for (int i = 0; i < cobieFloors.Count; i++)
                {
                    var floor = new Floor();
                    floor = floorMappings.AddMapping(cobieFloors[i], floor);
                    facility.Floors.Add(floor);
                    Exchanger.ReportProgress.IncrementAndUpdate();
                }

            }
            

            //Documents
            var docsMappings = Exchanger.GetOrCreateMappings<MappingIfcDocumentSelectToDocument>();
            helper.AddDocuments(docsMappings, facility, ifcBuilding);
            if (helper.OrphanDocs.Any())
            {
                foreach (var docSel in helper.OrphanDocs)
                {
                    List<Document> docs = docsMappings.MappingMulti(docSel);
                    facility.Documents.AddRange(docs);
                }
            }
            
            //Zones
            
            var allSpaces = GetAllSpaces(ifcBuilding);
            var allZones = GetAllZones(allSpaces, helper);
            var ifcZones = allZones.ToArray();
            if (ifcZones.Any())
            {
                Exchanger.ReportProgress.NextStage(ifcZones.Count(), 65); //finish progress at 65% 
                facility.Zones = new List<Zone>(ifcZones.Length);
                var zoneMappings = Exchanger.GetOrCreateMappings<MappingIfcZoneToZone>();
                for (int i = 0; i < ifcZones.Length; i++)
                {
                    var zone = new Zone();
                    zone = zoneMappings.AddMapping(ifcZones[i], zone);
                    facility.Zones.Add(zone);
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
                facility.AssetTypes = new List<AssetType>(); 
                var assetTypeMappings = Exchanger.GetOrCreateMappings<MappingXbimIfcProxyTypeObjectToAssetType>();
                foreach (var elementsByType in allIfcTypes)
                {
                    if (elementsByType.Value.Any())
                    {
                        var assetType = new AssetType();
                        assetType = assetTypeMappings.AddMapping(elementsByType.Key, assetType);
                        facility.AssetTypes.Add(assetType);
                        Exchanger.ReportProgress.IncrementAndUpdate();
                    }
                }
            }

            //Systems
            
            facility.Systems = new List<Xbim.CobieLiteUk.System>();

            if (helper.SystemMode.HasFlag(SystemExtractionMode.System) && helper.SystemAssignment.Any())
            {
                var systemMappings = Exchanger.GetOrCreateMappings<MappingIfcSystemToSystem>();
                Exchanger.ReportProgress.NextStage(helper.SystemAssignment.Keys.Count(), 95); //finish progress at 95% 
                foreach (var ifcSystem in helper.SystemAssignment.Keys)
                {
                    var system = new Xbim.CobieLiteUk.System();
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
                    var system = new Xbim.CobieLiteUk.System();
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
            

            //Contacts
            var ifcActorSelects = helper.Contacts;
            if (ifcActorSelects!=null && ifcActorSelects.Any())
            {
                Exchanger.ReportProgress.NextStage(ifcActorSelects.Count(), 97); //finish progress at 97% 
                var cobieContacts = new List<Contact>(ifcActorSelects.Count());
                var contactMappings = Exchanger.GetOrCreateMappings<MappingIfcActorToContact>();
                foreach (var actor in ifcActorSelects)
                {
                    var contact = new Contact();
                    contact = contactMappings.AddMapping(actor, contact);
                    cobieContacts.Add(contact);
                    Exchanger.ReportProgress.IncrementAndUpdate();
                }
                facility.Contacts = cobieContacts.Distinct(new ContactComparer()).ToList();
            }

            //assign all unallocated spaces to a zone
            var spaces = facility.Get<Space>().ToList();
            var zones = facility.Zones ?? new List<Zone>();
            var unAllocatedSpaces = spaces.Where(space => !zones.Any(z => z.Spaces != null && z.Spaces.Select(s => s.Name).Contains(space.Name)));
            Exchanger.ReportProgress.NextStage(unAllocatedSpaces.Count(), 98); //finish progress at 98% 
            var defaultZone = helper.CreateXbimDefaultZone();
            foreach ( var space in unAllocatedSpaces )
            {           
                if (facility.Zones == null) facility.Zones = new List<Zone>();
               
                defaultZone.Spaces.Add(new SpaceKey { Name = space.Name });
                Exchanger.ReportProgress.IncrementAndUpdate();
            }
            if (facility.Zones != null) facility.Zones.Add(defaultZone);

            //assign all assets that are not in a system to the default
            if (helper.SystemMode.HasFlag(SystemExtractionMode.Types))
            {
                var assetTypes = facility.Get<AssetType>().ToList();
                var systemsWritten = facility.Get<Xbim.CobieLiteUk.System>();
                var assetsAssignedToSystem = new HashSet<string>(systemsWritten.SelectMany(s => s.Components).Select(a => a.Name));
                var systems = facility.Systems ?? new List<Xbim.CobieLiteUk.System>();
                var defaultSystem = helper.CreateUndefinedSystem();
                Exchanger.ReportProgress.NextStage(assetTypes.Count(), 100); //finish progress at 100% 
                //go over all unasigned assets
                foreach (var assetType in assetTypes)
                {
                    Xbim.CobieLiteUk.System assetTypeSystem = null;
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
                        facility.Systems = new List<Xbim.CobieLiteUk.System>();
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

        private IEnumerable<IIfcZone> GetAllZones(IEnumerable<IIfcSpace> allSpaces, CoBieLiteUkHelper helper)
        {
            var allZones = new HashSet<IIfcZone>();
            foreach (var space in allSpaces)
                foreach (var zone in helper.GetZones(space))
                    allZones.Add(zone);
            return allZones;
        }

        private IEnumerable<IIfcSpace> GetAllSpaces(IIfcBuilding ifcBuilding)
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

        public override Facility CreateTargetObject()
        {
            return new Facility();
        }
    }
}
