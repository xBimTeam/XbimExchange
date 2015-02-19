using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.COBieLite.CollectionTypes;
using Xbim.DPoW;

namespace XbimExchanger.DPoW2ToCOBieLite
{
    public class MappingPlanOfWorkToFacilityType : DPoWToCOBieLiteMapping<PlanOfWork, FacilityType>
    {
        protected override FacilityType Mapping(PlanOfWork source, FacilityType target)
        {
            target.externalID = source.Id.ToString();
            var sFacility = source.Facility;
            if (sFacility != null)
            {
                target.FacilityDescription = sFacility.Description;
                target.FacilityName = sFacility.Name;
                var sFacilityCategory = sFacility.GetCategory(source);
                target.FacilityCategory = sFacilityCategory != null ? sFacilityCategory.ClassificationCode : null;
                if (!String.IsNullOrWhiteSpace(sFacility.SiteName))
                    target.SiteAssignment = new SiteType
                    {
                        SiteName = sFacility.SiteName
                    };
            }


            var sProject = source.Project;
            if (sProject != null)
            {
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
            }

            //convert contacts
            if (source.Contacts != null)
            {
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


            //convert DPoW objects and related jobs from the specified stage
            var stage = Exchanger.Context as ProjectStage ?? source.Project.GetCurrentProjectStage(source);
            if (stage != null)
            {
                //convert all documents
                if (stage.DocumentationSet != null && stage.DocumentationSet.Any())
                {
                    if (target.FacilityDocuments == null) target.FacilityDocuments = new DocumentCollectionType();
                    var dMap = Exchanger.GetOrCreateMappings<MappingDocumentToDocumentType>();
                    foreach (var sDoc in stage.DocumentationSet)
                    {
                        var tDoc = dMap.GetOrCreateTargetObject(sDoc.Id.ToString());
                        dMap.AddMapping(sDoc, tDoc);
                        target.FacilityDocuments.Add(tDoc);
                    }
                }

                //convert all asset types
                if (stage.AssetTypes != null && stage.AssetTypes.Any())
                {
                    if (target.AssetTypes == null) target.AssetTypes = new AssetTypeCollectionType();
                    var aMap = Exchanger.GetOrCreateMappings<MappingAssetTypeToAssetType>();
                    foreach (var sType in stage.AssetTypes)
                    {
                        var tType = aMap.GetOrCreateTargetObject(sType.Id.ToString());
                        aMap.AddMapping(sType, tType);
                        target.AssetTypes.Add(tType);
                    }
                }

                //Convert all assembly types
                if (stage.AssemblyTypes != null && stage.AssetTypes.Any())
                {
                    var assemplyMap = Exchanger.GetOrCreateMappings<MappingAssemblyTypeToAssemblyType>();
                    var assetMap = Exchanger.GetOrCreateMappings<MappingAssetTypeToAssetType>();
                    foreach (var assemblyType in stage.AssemblyTypes)
                    {
                        var tType = assemplyMap.GetOrCreateTargetObject(assemblyType.Id.ToString());
                        assemplyMap.AddMapping(assemblyType, tType);

                        var assetTypeIds = assemblyType.AggregationOfAssetTypes;
                        foreach (var id in assetTypeIds)
                        {
                            AssetTypeInfoType tAsset;
                            if (!assetMap.GetTargetObject(id.ToString(), out tAsset)) continue;
                            if (tAsset.AssemblyAssignments == null) tAsset.AssemblyAssignments = new AssemblyAssignmentCollectionType();
                            tAsset.AssemblyAssignments.Add(tType);
                        }

                    }
                }

                //convert all spaces into default floor
                if (stage.SpaceTypes != null && stage.SpaceTypes.Any())
                {
                    var tFloor = new FloorType
                    {
                        FloorName = "Default floor",
                        Spaces = new SpaceCollectionType()
                    };
                    target.Floors = new FloorCollectionType();
                    target.Floors.Add(tFloor);

                    var sMap = Exchanger.GetOrCreateMappings<MappingSpaceTypeToSpaceType>();
                    foreach (var spaceType in stage.SpaceTypes)
                    {
                        var tType = sMap.GetOrCreateTargetObject(spaceType.Id.ToString());
                        sMap.AddMapping(spaceType, tType);
                        tFloor.Spaces.Add(tType);
                    }
                }
            }

            return target;
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