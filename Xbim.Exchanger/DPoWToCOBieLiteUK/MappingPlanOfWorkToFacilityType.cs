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

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    internal class MappingPlanOfWorkToFacilityType : MappingAttributableObjectToCOBieObject<PlanOfWork, FacilityType>
    {
        protected override FacilityType Mapping(PlanOfWork source, FacilityType target)
        {
            

            target.ExternalId = source.Id.ToString();
            target.ExternalSystem = "DPoW";
            
            //convert fafility related information
            ConvertFacility(source, target);

            //convert project with units
            ConvertProject(source, target);

            //convert contacts and roles (roles are contacts with ContactCategory == 'Role')
            ConvertContacts(source, target);
            ConvertRoles(source, target);

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
                var stageAttrs = stage.GetCOBieAttributes();
                var attrs = stageAttrs as Attribute[] ?? stageAttrs.ToArray();
                if (attrs.Any())
                {
                    foreach (var attr in attrs)
                        attr.ExternalEntity = "ProjectStageAttributes";
                    if (target.Attributes == null) target.Attributes = new List<Attribute>();
                    target.Attributes.AddRange(attrs);
                }
            }

            //convert attributes
            base.Mapping(source, target);

            return target;
        }

        private void ConvertFacility(PlanOfWork source, FacilityType target)
        {
            var sFacility = source.Facility;
            if (sFacility == null) return;
            target.Description = sFacility.Description;
            target.Name = sFacility.Name;
            var sFacilityCategory = sFacility.GetCategory(source);
            target.Categories = new List<Category> { new Category { Code = sFacilityCategory != null ? sFacilityCategory.ClassificationCode : null } };
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
            target.AreaUnits = ConvertIdentEnum(sProject.AreaUnits, AreaUnit.squaremeters,
                out converted);
            target.CurrencyUnit = ConvertIdentEnum(sProject.CurrencyUnits,
                CurrencyUnit.GBP, out converted);
            target.LinearUnits = ConvertIdentEnum(sProject.LinearUnits,
                LinearUnit.millimeters, out converted);
            target.VolumeUnits = ConvertIdentEnum(sProject.VolumeUnits,
                VolumeUnit.cubicmeters, out converted);

            //add project free attributes to facility
            var projAttrs = sProject.GetCOBieAttributes();
            var attrs = projAttrs as Attribute[] ?? projAttrs.ToArray();
            foreach (var attr in attrs)
                attr.ExternalEntity = "ProjectAttributes";
            if (attrs.Any())
            {
                if (target.Attributes == null) target.Attributes = new List<Attribute>();
                target.Attributes.AddRange(attrs);
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

            foreach (var sContact in source.Contacts)
            {
                var key = sContact.Id.ToString();
                var tContact = mappings.GetOrCreateTargetObject(key);

                //set client role as a contact category
                if (sContact.Id == clientId)
                {
                    if (tContact.Categories == null) tContact.Categories = new List<Category>();
                    tContact.Categories.Add(new Category() { Classification = "DPoW", Code = "Client" });
                }

                mappings.AddMapping(sContact, tContact);
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

            var tFloor = new Floor
            {
                Name = "Default floor",
                Spaces = new List<Space>(),
                ExternalId = Guid.NewGuid().ToString(),
                ExternalSystem = "DPoW"
            };
            target.Floors = new List<Floor> {tFloor};

            var sMap = Exchanger.GetOrCreateMappings<MappingSpaceTypeToSpaceType>();
            foreach (var spaceType in spaceTypes)
            {
                var tType = sMap.GetOrCreateTargetObject(spaceType.Id.ToString());
                sMap.AddMapping(spaceType, tType);
                tFloor.Spaces.Add(tType);
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