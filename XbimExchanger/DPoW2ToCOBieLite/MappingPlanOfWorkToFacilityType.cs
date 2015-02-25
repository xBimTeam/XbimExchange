using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.COBieLite.CollectionTypes;
using Xbim.DPoW;
using SpaceType = Xbim.DPoW.SpaceType;

namespace XbimExchanger.DPoW2ToCOBieLite
{
    internal class MappingPlanOfWorkToFacilityType : MappingAttributableObjectToCOBieObject<PlanOfWork, FacilityType>
    {
        protected override FacilityType Mapping(PlanOfWork source, FacilityType target)
        {
            

            target.externalID = source.Id.ToString();
            
            //convert fafility related information
            ConvertFacility(source, target);

            //convert project with units
            ConvertProject(source, target);

            //convert contacts and roles (roles are contacts with ContactCategory == 'Role')
            ConvertContacts(source, target);
            ConvertRoles(source, target);

            //convert DPoW objects and related jobs from the specified stage
            var stage = Exchanger.Context as ProjectStage ?? source.Project.GetCurrentProjectStage(source);
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
                var attrs = stageAttrs as AttributeType[] ?? stageAttrs.ToArray();
                if (attrs.Any())
                {
                    foreach (var attr in attrs)
                        attr.propertySetName = "ProjectStageAttributes";
                    if (target.FacilityAttributes == null) target.FacilityAttributes = new AttributeCollectionType();
                    target.FacilityAttributes.AddRange(attrs);
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
            target.FacilityDescription = sFacility.Description;
            target.FacilityName = sFacility.Name;
            var sFacilityCategory = sFacility.GetCategory(source);
            target.FacilityCategory = sFacilityCategory != null ? sFacilityCategory.ClassificationCode : null;
            if (!String.IsNullOrWhiteSpace(sFacility.SiteName))
                target.SiteAssignment = new SiteType
                {
                    SiteName = sFacility.SiteName,
                    externalID = Guid.NewGuid().ToString()
                };
        }
        private void ConvertProject(PlanOfWork source, FacilityType target)
        {
            var sProject = source.Project;
            if (sProject == null) return;
            target.ProjectAssignment = new ProjectType
            {
                externalID = sProject.Id.ToString(),
                ProjectDescription = sProject.Description,
                ProjectName = sProject.Name
            };

            //get project attributes which are convertable to COBieLite facility
            bool converted;
            target.FacilityDefaultAreaUnit = ConvertIdentEnum(sProject.AreaUnits, AreaUnitSimpleType.Item,
                out converted);
            target.FacilityDefaultAreaUnitSpecified = converted;
            target.FacilityDefaultCurrencyUnit = ConvertIdentEnum(sProject.CurrencyUnits,
                CurrencyUnitSimpleType.Item, out converted);
            target.FacilityDefaultCurrencyUnitSpecified = converted;
            target.FacilityDefaultLinearUnit = ConvertIdentEnum(sProject.LinearUnits,
                LinearUnitSimpleType.millimeters, out converted);
            target.FacilityDefaultLinearUnitSpecified = converted;
            target.FacilityDefaultVolumeUnit = ConvertIdentEnum(sProject.VolumeUnits,
                VolumeUnitSimpleType.cubicmeters, out converted);
            target.FacilityDefaultVolumeUnitSpecified = converted;

            //add project free attributes to facility
            var projAttrs = sProject.GetCOBieAttributes();
            var attrs = projAttrs as AttributeType[] ?? projAttrs.ToArray();
            foreach (var attr in attrs)
                attr.propertySetName = "ProjectAttributes";
            if (attrs.Any())
            {
                if (target.FacilityAttributes == null) target.FacilityAttributes = new AttributeCollectionType();
                target.FacilityAttributes.AddRange(attrs);
            }
            
        }

        private void ConvertRoles(PlanOfWork source, FacilityType target)
        {
            if (source.Roles == null || !source.Roles.Any()) return;
            var mappings = Exchanger.GetOrCreateMappings<MappingRoleToContact>();
            if (target.Contacts == null) target.Contacts = new ContactCollectionType();

            foreach (var sRole in source.Roles)
            {
                var key = sRole.Id.ToString();
                var tContact = mappings.GetOrCreateTargetObject(key);
                mappings.AddMapping(sRole, tContact);
                
                target.Contacts.Contact.Add(tContact);
            }
        }

        private void ConvertContacts(PlanOfWork source, FacilityType target)
        {
            if (source.Contacts == null || !source.Contacts.Any()) return;
            var mappings = Exchanger.GetOrCreateMappings<MappingContactToContact>();
            if (target.Contacts == null) target.Contacts = new ContactCollectionType();

            var clientId = source.Client != null ? source.Client.Id : new Guid();

            foreach (var sContact in source.Contacts)
            {
                var key = sContact.Id.ToString();
                var tContact = mappings.GetOrCreateTargetObject(key);

                //set client role as a contact category
                if (sContact.Id == clientId)
                    tContact.ContactCategory = "Client";

                mappings.AddMapping(sContact, tContact);
                target.Contacts.Contact.Add(tContact);
            }
        }

        private void ConvertDocumentationSet(IEnumerable<Documentation> documentationSet, FacilityType target)
        {
            var sDocs = documentationSet as IList<Documentation> ?? documentationSet.ToList();
            if (documentationSet == null || !sDocs.Any()) return;
            if (target.FacilityDocuments == null) target.FacilityDocuments = new DocumentCollectionType();
            var dMap = Exchanger.GetOrCreateMappings<MappingDocumentToDocumentType>();
            foreach (var sDoc in sDocs)
            {
                var tDoc = dMap.GetOrCreateTargetObject(sDoc.Id.ToString());
                dMap.AddMapping(sDoc, tDoc);
                target.FacilityDocuments.Add(tDoc);
            }
        }

        private void ConvertAssetTypes(IEnumerable<AssetType> types, FacilityType target)
        {
            var assetTypes = types as AssetType[] ?? types.ToArray();
            if (types == null || !assetTypes.Any()) return;
            if (target.AssetTypes == null) target.AssetTypes = new AssetTypeCollectionType();
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

            var tFloor = new FloorType
            {
                FloorName = "Default floor",
                Spaces = new SpaceCollectionType(),
                externalID = Guid.NewGuid().ToString()
            };
            target.Floors = new FloorCollectionType();
            target.Floors.Add(tFloor);

            var sMap = Exchanger.GetOrCreateMappings<MappingSpaceTypeToSpaceType>();
            foreach (var spaceType in spaceTypes)
            {
                var tType = sMap.GetOrCreateTargetObject(spaceType.Id.ToString());
                sMap.AddMapping(spaceType, tType);
                tFloor.Spaces.Add(tType);
            }
        }

        private void ConvertAssemblyTypes(IEnumerable<Xbim.DPoW.AssemblyType> assemblyTypes, ProjectStage stage)
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