using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLite;
using Xbim.COBieLite.CollectionTypes;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.DPoWToCOBieLite
{
    public class MappingPlanOfWorkToFacilityType : DPoWToCOBieLiteMapping<PlanOfWork, FacilityType>
    {
        protected override FacilityType Mapping(PlanOfWork source, FacilityType target)
        {
            target.externalID = Exchanger.GetStringIdentifier();

            //init collections
            target.FacilityAttributes = new AttributeCollectionType();
            target.Zones = new ZoneCollectionType();
            target.AssetTypes = new AssetTypeCollectionType();
            target.FacilityDocuments = new DocumentCollectionType();
            target.FacilityIssues = new IssueCollectionType();

            var sFacility = source.Facility;
            if (sFacility != null)
            {
                target.FacilityDescription = sFacility.FacilityDescription;
                target.FacilityName = sFacility.FacilityName;
                target.FacilityCategory = sFacility.FacilityCategory != null ? sFacility.FacilityCategory.ClassificationCode : null;
                if (!String.IsNullOrWhiteSpace(sFacility.FacilitySiteName))
                {
                    target.SiteAssignment = new SiteType
                    {
                        SiteName = sFacility.FacilitySiteName,
                        SiteDescription = sFacility.FacilityDescription,
                        externalID = Exchanger.GetStringIdentifier()
                    };
                }
            }
           

            var sProject = source.Project;
            if (sProject != null)
            {
                target.ProjectAssignment = new ProjectType
                { 
                    externalID = Exchanger.GetStringIdentifier(),
                    ProjectDescription = sProject.ProjectDescription,
                    ProjectName = sProject.ProjectName
                };

                //get project attributes which are convertable to COBieLite facility
                target.FacilityDefaultAreaUnit = DPoWToCOBieLiteExchanger.GetAreaUnit(sProject.AreaUnits);
                target.FacilityDefaultAreaUnitSpecified = true;
                target.FacilityDefaultCurrencyUnit = DPoWToCOBieLiteExchanger.GetCurrency(sProject.CurrencyUnits);
                target.FacilityDefaultCurrencyUnitSpecified = true;
                target.FacilityDefaultLinearUnit = DPoWToCOBieLiteExchanger.GetLinearUnit(sProject.LinearUnits);
                target.FacilityDefaultLinearUnitSpecified = true;
                target.FacilityDefaultVolumeUnit = DPoWToCOBieLiteExchanger.GetVolumeUnit(sProject.VolumeUnits);
                target.FacilityDefaultVolumeUnitSpecified = true;
            }

            //convert contacts
            if (source.Contacts != null)
            {
                var mappings = Exchanger.GetOrCreateMappings<MappingContactToContact>();
                if (target.Contacts == null) target.Contacts = new ContactCollectionType();
                
                foreach (var sContact in source.Contacts)
                {
                    var key = MappingContactToContact.GetKey(sContact);
                    var tContact = mappings.GetOrCreateTargetObject(key);
                    mappings.AddMapping(sContact, tContact);
                    target.Contacts.Contact.Add(tContact);
                }
            }

            //set attributes for client
            if (source.Client != null)
            {
                target.FacilityAttributes.AddRange(
                    new List<AttributeType>
                    {
                    new AttributeType
                    {
                        AttributeName = "ProjectClientFamilyName",
                        AttributeDescription = "Client of this project as defined in DPoW.",
                        AttributeValue = new AttributeValueType { Item = new AttributeStringValueType { StringValue = source.Client.ContactFamilyName} },
                        propertySetName = "ProjectClient"
                    },
                    new AttributeType
                    {
                        AttributeName = "ProjectClientGivenName",
                        AttributeDescription = "Client of this project as defined in DPoW.",
                        AttributeValue = new AttributeValueType { Item = new AttributeStringValueType { StringValue = source.Client.ContactGivenName} },
                        propertySetName = "ProjectClient"
                    },
                    new AttributeType
                    {
                        AttributeName = "ProjectClientEmail",
                        AttributeDescription = "Client of this project as defined in DPoW.",
                        AttributeValue = new AttributeValueType { Item = new AttributeStringValueType { StringValue = source.Client.ContactEmail} },
                        propertySetName = "ProjectClient"
                    },
                    new AttributeType
                    {
                        AttributeName = "ProjectClientCompanyName",
                        AttributeDescription = "Client of this project as defined in DPoW.",
                        AttributeValue = new AttributeValueType { Item = new AttributeStringValueType { StringValue = source.Client.ContactCompanyName} },
                        propertySetName = "ProjectClient"
                    },
                    new AttributeType
                    {
                        AttributeName = "ProjectClientURL",
                        AttributeDescription = "Client of this project as defined in DPoW.",
                        AttributeValue = new AttributeValueType { Item = new AttributeStringValueType { StringValue = source.Client.ContactURL} },
                        propertySetName = "ProjectClient"
                    }
                });
            }
            
            //convert DPoW objects and related jobs from the specified stage
            var stage = Exchanger.Context as ProjectStage ?? source.Project.CurrentProjectStage;
            if (stage != null)
            {
                if (stage.Jobs != null)
                {
                    foreach (var job in stage.Jobs)
                    {
                        //create job equivalent as an issue for zone and assembly which doesn't have a job equivalent
                        //IssueType doesn't contain documents so these should be assigned to the target type on the level of the type (Zone or Assembly)
                        var jiMap = Exchanger.GetOrCreateMappings<MappingJobToIssueType>();
                        var tIssue = jiMap.GetOrCreateTargetObject(MappingJobToIssueType.GetKey(job));
                        jiMap.AddMapping(job, tIssue);

                        //create job which can be assigned to asset type. JobType also contains related documents
                        var jjMap = Exchanger.GetOrCreateMappings<MappingJobToJobType>();
                        var tJob = jjMap.GetOrCreateTargetObject(MappingJobToJobType.GetKey(job));
                        jjMap.AddMapping(job, tJob);

                        //convert related documents and add them to documents of DPoW object
                        var dMap = Exchanger.GetOrCreateMappings<MappingDocumentToDocumentType>();
                        var tDocs = new List<DocumentType>();
                        foreach (var sDoc in job.Documents)
                        {
                            var dKey = MappingDocumentToDocumentType.GetKey(sDoc);
                            var tDoc = dMap.GetOrCreateTargetObject(dKey);
                            dMap.AddMapping(sDoc, tDoc);
                            tDocs.Add(tDoc);
                        }

                        if (job.DPoWObjects != null && job.DPoWObjects.Any())
                        {
                            var zones = new List<ZoneType>();
                            var assetTypes = new List<AssetTypeInfoType>();
                            var zoneMapping = Exchanger.GetOrCreateMappings<MappingZoneToZoneType>();
                            var assetTypeMapping = Exchanger.GetOrCreateMappings<MappingAssetTypeToAssetTypeInfoType>();
                            var assemblyTypeMapping = Exchanger.GetOrCreateMappings<MappingAssemblyTypeToAssetTypeInfoType>();
                            //branch for asset type, assembly type and zones
                            foreach (var dObject in job.DPoWObjects)
                            {
                                var zone = dObject as Zone;
                                if (zone != null)
                                {
                                    var zKey = MappingZoneToZoneType.GetKey(zone);
                                    var tZone = zoneMapping.GetOrCreateTargetObject(zKey);
                                    zoneMapping.AddMapping(zone, tZone);
                                    tZone.ZoneDocuments = new DocumentCollectionType();
                                    tZone.ZoneDocuments.AddRange(tDocs);
                                    tZone.ZoneIssues = new IssueCollectionType();
                                    tZone.ZoneIssues.Add(tIssue);
                                    zones.Add(tZone);
                                }

                                var assetType = dObject as AssetType;
                                if (assetType != null)
                                {
                                    var aKey = MappingAssetTypeToAssetTypeInfoType.GetKey(assetType);
                                    var tAssetType = assetTypeMapping.GetOrCreateTargetObject(aKey);
                                    assetTypeMapping.AddMapping(assetType, tAssetType);
                                    tAssetType.Jobs = new JobCollectionType();
                                    tAssetType.Jobs.Add(tJob); 
                                    //don't have to add documents as they are defined within the job
                                    assetTypes.Add(tAssetType);
                                }

                                var assemblyType = dObject as Xbim.DPoW.Interfaces.AssemblyType;
                                if (assemblyType != null)
                                {
                                    var aKey = MappingAssemblyTypeToAssetTypeInfoType.GetKey(assemblyType);
                                    var tAssetType = assetTypeMapping.GetOrCreateTargetObject(aKey);
                                    assemblyTypeMapping.AddMapping(assemblyType, tAssetType);
                                    tAssetType.Jobs = new JobCollectionType();
                                    tAssetType.Jobs.Add(tJob);
                                    //don't have to add documents as they are defined within the job
                                    assetTypes.Add(tAssetType);
                                }

                            }
                            //assign object sets to facility
                            target.Zones.AddRange(zones);
                            target.AssetTypes.AddRange(assetTypes);
                            
                        }
                        else
                        {
                            //assign documents and issues to the facility type (root)
                            target.FacilityDocuments.AddRange(tDocs);
                            target.FacilityIssues.Add(tIssue);
                        }
                   }
                }
            }

            return target;
        }
    }
}
