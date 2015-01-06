using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.XbimExtensions.SelectTypes;

namespace Xbim.COBieLite
{

    public partial class FacilityType
    {
      
       // private IfcBuilding _ifcBuilding;
       
        public FacilityType()
        {
           
        }
        public FacilityType(IfcBuilding ifcBuilding, CoBieLiteHelper helper)
            : this()
        {
            //   _ifcBuilding = ifcBuilding;
            var model = ifcBuilding.ModelOf;
            externalEntityName = helper.ExternalEntityName(ifcBuilding);
            externalID = helper.ExternalEntityIdentity(ifcBuilding);
            externalSystemName = helper.ExternalSystemName(ifcBuilding);
            FacilityName = ifcBuilding.Name;
            FacilityDescription = ifcBuilding.Description;
            FacilityCategory = helper.GetClassification(ifcBuilding);
            var ifcProject = model.Instances.OfType<IfcProject>().FirstOrDefault();
            if (ifcProject != null)
            {
                ProjectAssignment = new ProjectType(ifcProject, helper);
                var ifcSite = ifcProject.GetSpatialStructuralElements().FirstOrDefault(p => p is IfcSite) as IfcSite;
                if (ifcSite != null) SiteAssignment = new SiteType(ifcSite, helper);
                SetDefaultUnits(helper);
                FacilityDeliverablePhaseName = ifcProject.Phase;
                var storeys = ifcBuilding.GetBuildingStoreys(true);
                var ifcBuildingStories = storeys as IList<IfcBuildingStorey> ?? storeys.ToList();
                if (ifcBuildingStories.Any())
                {
                    Floors = new FloorCollectionType {Floor = new FloorTypeBase[ifcBuildingStories.Count]};
                    for (int i = 0; i < ifcBuildingStories.Count; i++)
                    {
                        Floors.Floor[i] = new FloorType(ifcBuildingStories[i], helper);
                    }
                }

            }
            //Attributes
            AttributeType[] ifcAttributes = helper.GetAttributes(ifcBuilding);
            if (ifcAttributes != null && ifcAttributes.Length > 0)
                FacilityAttributes = new AttributeCollectionType { Attribute = ifcAttributes };
           
            //Zones

            var allSpaces = GetAllSpaces(ifcBuilding);
            var allZones = GetAllZones(allSpaces, helper);
            var ifcZones = allZones.ToArray();
            if (ifcZones.Any())
            {
                Zones = new ZoneCollectionType { Zone = new ZoneTypeBase[ifcZones.Length] };
                for (int i = 0; i < ifcZones.Length; i++)
                {
                    Zones.Zone[i] = new ZoneType(ifcZones[i], helper);
                }
            }
            
            //Assets
            var allAssetsinThisFacility = new HashSet<IfcElement>(helper.GetAllAssets(ifcBuilding));
            
            //AssetTypes
            //Get all assets that are in this facility/building
            var allAssetTypesInThisFacility = AllAssetTypesInThisFacility(ifcBuilding, allAssetsinThisFacility, helper);
            if (allAssetTypesInThisFacility.Any())
            {
                AssetTypes = new AssetTypeCollectionType
                {
                    AssetType = new AssetTypeInfoTypeBase[allAssetTypesInThisFacility.Count]
                };
                for (int i = 0; i < allAssetTypesInThisFacility.Count; i++)
                {
                    AssetTypes.AssetType[i] = new AssetTypeInfoType(allAssetTypesInThisFacility[i], helper); 
                }
            }

            //Systems
            var allSystemsInThisFacility = helper.SystemAssignment
                .Where(v => v.Value.Any(allAssetsinThisFacility.Contains))
                .Select(k => k.Key).ToArray();
            if (allSystemsInThisFacility.Any())
            {
                Systems = new SystemCollectionType
                {
                    System = new SystemTypeBase[allSystemsInThisFacility.Length]
                };

                for (int i = 0; i < allSystemsInThisFacility.Length; i++)
                {
                    Systems.System[i] = new SystemType(allSystemsInThisFacility[i], helper);
                }
            }

            //Contacts
            var contacts = helper.GetContacts();
            var ifcActors = contacts as IfcActorSelect[] ?? contacts.ToArray();
            if (ifcActors.Any())
            {
                Contacts = new ContactCollectionType
                {
                    Contact = new ContactTypeBase[ifcActors.Length]
                };

                for (int i = 0; i < ifcActors.Length; i++)
                {
                    Contacts.Contact[i] = new ContactType(ifcActors[i], helper);
                }
            }

        }

        

        private static List<IfcTypeObject> AllAssetTypesInThisFacility(IfcBuilding ifcBuilding, HashSet<IfcElement> allAssetsinThisFacility,  CoBieLiteHelper helper)
        {
           
            var allAssetTypes = helper.DefiningTypeObjectMap;
            var allAssetTypesInThisFacility = new List<IfcTypeObject>(allAssetTypes.Count);
            foreach (var assetTypeKeyValue in allAssetTypes)
            {
                //if any defining type has an object in this building/facility then we need to include it
                if (assetTypeKeyValue.Value.Any(allAssetsinThisFacility.Contains))
                    allAssetTypesInThisFacility.Add(assetTypeKeyValue.Key);
            }
            return allAssetTypesInThisFacility;
        }

        private IEnumerable<IfcZone> GetAllZones(IEnumerable<IfcSpace> allSpaces, CoBieLiteHelper helper)
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

        private void SetDefaultUnits(CoBieLiteHelper helper)
        {
            if ((FacilityDefaultAreaUnitSpecified = helper.HasAreaUnit) == true)
                facilityDefaultAreaUnitField = helper.ModelAreaUnit;
            if ((FacilityDefaultLinearUnitSpecified = helper.HasLinearUnit) == true)
                facilityDefaultLinearUnitField = helper.ModelLinearUnit;
            if ((FacilityDefaultVolumeUnitSpecified = helper.HasVolumeUnit) == true)
                facilityDefaultVolumeUnitField = helper.ModelVolumeUnit;
            if ((FacilityDefaultCurrencyUnitSpecified = helper.HasCurrencyUnit) == true)
                facilityDefaultCurrencyUnitField = helper.ModelCurrencyUnit;
        }
    }
}
