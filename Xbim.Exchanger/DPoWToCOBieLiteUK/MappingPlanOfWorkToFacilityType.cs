using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;
using Xbim.DPoW;
using AssetType = Xbim.COBieLiteUK.AssetType;
using Attribute = Xbim.COBieLiteUK.Attribute;
using Contact = Xbim.COBieLiteUK.Contact;
using SpaceType = Xbim.DPoW.SpaceType;
using FacilityType = Xbim.COBieLiteUK.Facility;
using ProjectStage = Xbim.COBieLiteUK.ProjectStage;
using System = Xbim.COBieLiteUK.System;

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    internal class MappingPlanOfWorkToFacilityType : MappingAttributableObjectToCOBieObject<PlanOfWork, FacilityType>
    {
        protected override FacilityType Mapping(PlanOfWork source, FacilityType target)
        {
            //convert attributes
            base.Mapping(source, target);

            target.ExternalId = source.Id.ToString();
            target.ExternalSystem = "DPoW";
            
            //convert contacts and roles (roles are contacts with ContactCategory == 'Role')
            ConvertContacts(source, target);
            ConvertRoles(source, target);
            
            //convert fafility related information
            ConvertFacility(source, target);

            //convert project with units
            ConvertProject(source, target);

            

            //convert DPoW objects and related jobs from the specified stage
            var stage = Exchanger.Context as Xbim.DPoW.ProjectStage ?? source.Project.GetCurrentProjectStage(source);
            if (stage != null)
            {
                //convert all documents
                ConvertDocumentationSet(stage.DocumentationSet, target);

                //convert all asset types
                ConvertAssetTypes(stage.AssetTypes, target);

                //Convert all assembly types
                ConvertAssemblyTypes(stage.AssemblyTypes, stage);

                //convert all spaces into default floor
                ConvertSpaces(stage.SpaceTypes, target);

                //convert free stage attributes
                //add project free attributes to facility
                var stageAttrs = stage.GetCOBieAttributes(target.CreatedOn, target.CreatedBy.Email);
                var attrs = stageAttrs as Attribute[] ?? stageAttrs.ToArray();
                if (attrs.Any())
                {
                    foreach (var attr in attrs)
                        attr.ExternalEntity = "ProjectStageAttributes";
                    if (target.Attributes == null) target.Attributes = new List<Attribute>();
                    target.Attributes.AddRange(attrs);
                }

                //convert stage
                var stMapping = Exchanger.GetOrCreateMappings<MappingProjectStageToProjectStage>();
                var tStage = stMapping.GetOrCreateTargetObject(stage.Id.ToString());
                stMapping.AddMapping(stage, tStage);
                if(target.Stages == null) target.Stages = new List<ProjectStage>();
                target.Stages.Add(tStage);
            }

            

            //make sure all components live in at least one default system
            foreach (var assetType in target.AssetTypes ?? new List<AssetType>())
            {
                if (assetType.Assets == null) continue;
                foreach (var asset in assetType.Assets)
                {
                    if (target.Systems == null) target.Systems = new List<Xbim.COBieLiteUK.System>();
                    var isInSystem = target.Systems.Any(system => system.Components != null && system.Components.Any(k => k.Name == asset.Name));
                    if (isInSystem) continue;
                    
                    var defaultSystem = target.Systems.FirstOrDefault(s => s.Name == "Default system");
                    if (defaultSystem == null)
                    {
                        defaultSystem = new Xbim.COBieLiteUK.System{Name = "Default system"};
                        target.Systems.Add(defaultSystem);
                    }
                    if (defaultSystem.Components == null) defaultSystem.Components = new List<AssetKey>();
                    defaultSystem.Components.Add(new AssetKey{Name = asset.Name});
                }
            }

            return target;
        }

        private void ConvertFacility(PlanOfWork source, FacilityType target)
        {
            var sFacility = source.Facility;
            if (sFacility == null) return;
            target.Description = sFacility.Description;
            target.Name = sFacility.Name;
            var sFacilityCategory = sFacility.GetClassificationAndReference(source);
            if (sFacilityCategory != null)
            {
                target.Categories = new List<Category>
                {
                    new Category { Code = sFacilityCategory.Item2.ClassificationCode, Description = sFacilityCategory.Item2.ClassificationDescription, Classification = sFacilityCategory.Item1.Name},
                    new Category { Code = "required", Description = "DPoW"}
                };
            }
            if (!String.IsNullOrWhiteSpace(sFacility.SiteName))
                target.Site = new Site
                {
                    Name = sFacility.SiteName,
                    ExternalId = Guid.NewGuid().ToString()
                };
        }
        private void ConvertProject(PlanOfWork source, FacilityType target)
        {
            var sProject = source.Project;
            if (sProject == null) return;
            target.Project = new Xbim.COBieLiteUK.Project
            {
                ExternalId = sProject.Id.ToString(),
                Description = sProject.Description,
                Name = sProject.Name
            };

            //get project attributes which are convertable to COBieLite facility
            bool converted;
            target.AreaUnitsCustom = sProject.AreaUnits.ToString();
            target.CurrencyUnitCustom = sProject.CurrencyUnits.ToString();
            target.LinearUnitsCustom = sProject.LinearUnits.ToString();
            target.VolumeUnitsCustom = sProject.VolumeUnits.ToString();

            //add project free attributes to facility
            var attrs = sProject.GetCOBieAttributes(target.CreatedOn, target.CreatedBy.Email).ToArray();
            foreach (var attr in attrs)
                attr.PropertySetName = "ProjectAttributes";
            if (attrs.Any())
            {
                if (target.Attributes == null) target.Attributes = new List<Attribute>();
                target.Attributes.AddRange(attrs);
            }

            //add project stage attributes
            var stage = Exchanger.Context as Xbim.DPoW.ProjectStage;
            if (stage != null)
            {
                var stageAttrs = stage.GetCOBieAttributes(target.CreatedOn, target.CreatedBy.Email).ToArray();
                foreach (var attr in stageAttrs)
                    attr.PropertySetName = "ProjecStagetAttributes";
                if (stageAttrs.Any())
                {
                    if (target.Attributes == null) target.Attributes = new List<Attribute>();
                    target.Attributes.AddRange(attrs);
                }    
            }
        }

        private void ConvertRoles(PlanOfWork source, FacilityType target)
        {
            if (source.Roles == null || !source.Roles.Any()) return;
            var mappings = Exchanger.GetOrCreateMappings<MappingRoleToContact>();
            if (target.Contacts == null) target.Contacts = new List<Contact>();

            foreach (var sRole in source.Roles)
            {
                var key = sRole.Id.ToString();
                var tContact = mappings.GetOrCreateTargetObject(key);
                mappings.AddMapping(sRole, tContact);
                
                target.Contacts.Add(tContact);
            }
        }

        private void ConvertContacts(PlanOfWork source, FacilityType target)
        {
            if (source.Contacts == null || !source.Contacts.Any()) return;
            var mappings = Exchanger.GetOrCreateMappings<MappingContactToContact>();
            if (target.Contacts == null) target.Contacts = new List<Contact>();

            var clientId = source.Client != null ? source.Client.Id : new Guid();

            //Set the Client to be the first
            if (clientId != default(Guid))
            {
                var client = source.Contacts.FirstOrDefault(c => c.Id == clientId);
                if (client != null)
                {
                    source.Contacts.Remove(client);
                    source.Contacts.Insert(0, client);
                }
            }

            foreach (var sContact in source.Contacts)
            {
                var key = sContact.Id.ToString();
                var tContact = mappings.GetOrCreateTargetObject(key);
                mappings.AddMapping(sContact, tContact);

                //set client role as a contact category
                if (sContact.Id == clientId)
                {
                    tContact.Categories = new List<Category> {new Category() {Classification = "DPoW", Code = "Client"}};
                    tContact.CreatedBy = new ContactKey{Email = tContact.Email};
                }

                target.Contacts.Add(tContact);
            }
        }

        private void ConvertDocumentationSet(IEnumerable<Documentation> documentationSet, FacilityType target)
        {
            var sDocs = documentationSet as IList<Documentation> ?? documentationSet.ToList();
            if (documentationSet == null || !sDocs.Any()) return;
            if (target.Documents == null) target.Documents = new List<Document>();
            var dMap = Exchanger.GetOrCreateMappings<MappingDocumentToDocumentType>();
            foreach (var sDoc in sDocs)
            {
                var tDoc = dMap.GetOrCreateTargetObject(sDoc.Id.ToString());
                dMap.AddMapping(sDoc, tDoc);
                target.Documents.Add(tDoc);
            }
        }

        private void ConvertAssetTypes(IEnumerable<Xbim.DPoW.AssetType> types, FacilityType target)
        {
            var assetTypes = types as Xbim.DPoW.AssetType[] ?? types.ToArray();
            if (types == null || !assetTypes.Any()) return;
            if (target.AssetTypes == null) target.AssetTypes = new List<AssetType>();
            var aMap = Exchanger.GetOrCreateMappings<MappingAssetTypeToAssetType>();
            foreach (var sType in assetTypes)
            {
                var tType = aMap.GetOrCreateTargetObject(sType.Id.ToString());
                aMap.AddMapping(sType, tType);
                target.AssetTypes.Add(tType);
            }
        }

        private void ConvertSpaces(IEnumerable<SpaceType> spaces, FacilityType target)
        {
            var spaceTypes = spaces as IList<SpaceType> ?? spaces.ToList();
            if (spaces == null || !spaceTypes.Any()) return;

            //var tFloor = new Floor
            //{
            //    Name = "Default floor",
            //    Spaces = new List<Space>(),
            //    ExternalId = Guid.NewGuid().ToString(),
            //    ExternalSystem = "DPoW"
            //};
            //target.Floors = new List<Floor> {tFloor};

            var sMap = Exchanger.GetOrCreateMappings<MappingSpaceTypeToZone>();
            foreach (var spaceType in spaceTypes)
            {
                var tType = sMap.GetOrCreateTargetObject(spaceType.Id.ToString());
                sMap.AddMapping(spaceType, tType);

                if (target.Zones == null) target.Zones = new List<Zone>();
                target.Zones.Add(tType);
            }
        }

        private void ConvertAssemblyTypes(IEnumerable<AssemblyType> assemblyTypes, Xbim.DPoW.ProjectStage stage)
        {
            if (assemblyTypes == null || !assemblyTypes.Any()) return;

            var assemplyMap = Exchanger.GetOrCreateMappings<MappingAssemblyTypeToAssemblyType>();
            foreach (var assemblyType in stage.AssemblyTypes)
            {
                var tType = assemplyMap.GetOrCreateTargetObject(assemblyType.Id.ToString());
                assemplyMap.AddMapping(assemblyType, tType);
            }
        }

        private static TOut ConvertIdentEnum<TIn, TOut>(TIn value, TOut defaultResult, out bool converted)
            where TIn : struct, IConvertible
            where TOut : struct, IConvertible
        {
            TOut result;
            converted = true;
            if (Enum.TryParse(value.ToString(CultureInfo.InvariantCulture), out result))
                return result;
            converted = false;
            return defaultResult;
        }
    }
}