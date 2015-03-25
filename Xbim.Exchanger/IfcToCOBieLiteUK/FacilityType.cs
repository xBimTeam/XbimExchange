using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.XbimExtensions.SelectTypes;


namespace XbimExchanger.IfcToCOBieLiteUK
{

    /// <summary>
    /// 
    /// </summary>
    public class FacilityType: Facility
    {
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcBuilding"></param>
        /// <param name="helper"></param>
        public FacilityType(IfcBuilding ifcBuilding, CoBieLiteUkHelper helper) 
        {
            //   _ifcBuilding = ifcBuilding;
            var model = ifcBuilding.ModelOf;
            ExternalEntity = helper.ExternalEntityName(ifcBuilding);
            ExternalId = helper.ExternalEntityIdentity(ifcBuilding);
            ExternalSystem = helper.ExternalSystemName(ifcBuilding);
            Name = ifcBuilding.Name;
            Description = ifcBuilding.Description;
            Categories =helper.GetCategories(ifcBuilding);
            var ifcProject = model.Instances.OfType<IfcProject>().FirstOrDefault();
            if (ifcProject != null)
            {
                Project = new ProjectType(ifcProject, helper);
                var ifcSite = ifcProject.GetSpatialStructuralElements().FirstOrDefault(p => p is IfcSite) as IfcSite;
                if (ifcSite != null) Site = new SiteType(ifcSite, helper);
                else //create a default "External area"
                {
                    Site = new Site();
                    Site.Description = "Default external area if no site has been defined in the model";
                    Site.Name = "Default External";
                }
                SetDefaultUnits(helper);
                
                var storeys = ifcBuilding.GetBuildingStoreys(true);
                var ifcBuildingStories = storeys as IList<IfcBuildingStorey> ?? storeys.ToList();
                if (ifcBuildingStories.Any())
                {
                    Floors = new List<Floor>(ifcBuildingStories.Count);
                    for (int i = 0; i < ifcBuildingStories.Count; i++)
                    {
                        Floors.Add(new FloorType(ifcBuildingStories[i], helper));
                    }
                }
            }
            //Attributes
            Attributes = helper.GetAttributes(ifcBuilding);
           
            //Zones

            var allSpaces = GetAllSpaces(ifcBuilding);
            var allZones = GetAllZones(allSpaces, helper);
            var ifcZones = allZones.ToArray();
            if (ifcZones.Any())
            {
                Zones = new List<Zone>(ifcZones.Length);
                for (int i = 0; i < ifcZones.Length; i++)
                {
                    Zones.Add(new ZoneType(ifcZones[i], helper));
                }
            }

            //Assets
            var allAssetsinThisFacility = new HashSet<IfcElement>(helper.GetAllAssets(ifcBuilding));

            //AssetTypes
            //Get all assets that are in this facility/building
            var allAssetTypesInThisFacility = AllAssetTypesInThisFacility(ifcBuilding, allAssetsinThisFacility, helper);
            if (allAssetTypesInThisFacility.Any())
            {
                AssetTypes = new List<AssetType>(allAssetTypesInThisFacility.Count);
                
                for (int i = 0; i < allAssetTypesInThisFacility.Count; i++)
                {
                    AssetTypes.Add(new AssetTypeInfoType(allAssetTypesInThisFacility[i], helper));
                }
            }

            //Systems
            var allSystemsInThisFacility = helper.SystemAssignment
                .Where(v => v.Value.Any(allAssetsinThisFacility.Contains))
                .Select(k => k.Key).ToArray();
            if (allSystemsInThisFacility.Any())
            {
                Systems = new List<Xbim.COBieLiteUK.System>(allSystemsInThisFacility.Length);

                for (int i = 0; i < allSystemsInThisFacility.Length; i++)
                {
                    Systems.Add(new SystemType(allSystemsInThisFacility[i], helper));
                }
            }

            //Contacts
            var contacts = helper.GetContacts();
            var ifcActors = contacts as IfcActorSelect[] ?? contacts.ToArray();
            if (ifcActors.Any())
            {

                Contacts = new List<Contact>(ifcActors.Length);
                for (int i = 0; i < ifcActors.Length; i++)
                {
                    Contacts.Add(new ContactType(ifcActors[i], helper));
                }
            }

        }



        private static List<IfcTypeObject> AllAssetTypesInThisFacility(IfcBuilding ifcBuilding,
            HashSet<IfcElement> allAssetsinThisFacility, CoBieLiteUkHelper helper)
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

        private void SetDefaultUnits(CoBieLiteUkHelper helper)
        {
            AreaUnits = helper.ModelAreaUnit??AreaUnit.notdefined;
            LinearUnits = helper.ModelLinearUnit??LinearUnit.notdefined;
            VolumeUnits = helper.ModelVolumeUnit??VolumeUnit.notdefined;
            CurrencyUnit = helper.ModelCurrencyUnit??CurrencyUnit.notdefined;
        }

        
    }
}
