using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.DPoWToCOBieLite
{
    public class MappingPlanOfWorkToFacilityType : DPoWToCOBieLiteMapping<PlanOfWork, FacilityType>
    {
        protected override FacilityType Mapping(PlanOfWork source, FacilityType target)
        {
            var sFacility = source.Facility;
            target.externalID = Exchanger.GetStringIdentifier();
            if (sFacility != null)
            {
                target.FacilityDescription = sFacility.FacilityDescription;
                target.FacilityName = sFacility.FacilityName;
                target.FacilityCategory = sFacility.FacilityCategory != null ? sFacility.FacilityCategory.ClassificationCode : null;
            }
            if (!String.IsNullOrWhiteSpace(sFacility.FacilitySiteName))
            {
                target.SiteAssignment = new SiteType()
                {
                    SiteName = sFacility.FacilitySiteName,
                    SiteDescription = sFacility.FacilityDescription,
                    externalID = Exchanger.GetStringIdentifier()
                };
            }

            var sProject = source.Project;
            if (sProject != null)
            {
                target.ProjectAssignment = new ProjectType() { 
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
                var tContacts = target.Contacts.Contact != null ? target.Contacts.Contact.ToList() : new List<ContactTypeBase>();

                foreach (var sContact in source.Contacts)
                {
                    var key = MappingContactToContact.GetKey(sContact);
                    var tContact = mappings.GetOrCreateTargetObject(key);
                    mappings.AddMapping(sContact, tContact);
                    tContacts.Add(tContact);
                }

                //assign array back
                target.Contacts.Contact = tContacts.ToArray();
            }

            //set attributes for client
            if (source.Client != null)
            {
                target.FacilityAttributes.Add(new []{
                    new AttributeType()
                    {
                        AttributeName = "ProjectClientFamilyName",
                        AttributeDescription = "Client of this project as defined in DPoW.",
                        AttributeValue = new AttributeValueType() { Item = new AttributeStringValueType() { StringValue = source.Client.ContactFamilyName} }
                    },
                    new AttributeType()
                    {
                        AttributeName = "ProjectClientGivenName",
                        AttributeDescription = "Client of this project as defined in DPoW.",
                        AttributeValue = new AttributeValueType() { Item = new AttributeStringValueType() { StringValue = source.Client.ContactGivenName} }
                    },
                    new AttributeType()
                    {
                        AttributeName = "ProjectClientEmail",
                        AttributeDescription = "Client of this project as defined in DPoW.",
                        AttributeValue = new AttributeValueType() { Item = new AttributeStringValueType() { StringValue = source.Client.ContactEmail} }
                    },
                    new AttributeType()
                    {
                        AttributeName = "ProjectClientCompanyName",
                        AttributeDescription = "Client of this project as defined in DPoW.",
                        AttributeValue = new AttributeValueType() { Item = new AttributeStringValueType() { StringValue = source.Client.ContactCompanyName} }
                    },
                    new AttributeType()
                    {
                        AttributeName = "ProjectClientURL",
                        AttributeDescription = "Client of this project as defined in DPoW.",
                        AttributeValue = new AttributeValueType() { Item = new AttributeStringValueType() { StringValue = source.Client.ContactURL} }
                    }
                });
            }
            
            //convert DPoW objects and related jobs from the specified stage
            var stage = Exchanger.Context as ProjectStage;
            if (stage == null) stage = source.Project.CurrentProjectStage;
            if (stage != null)
            {
                if (stage.Jobs != null)
                {
                    foreach (var job in stage.Jobs)
                    {
                        //create job equivalent 
                        var jMap = Exchanger.GetOrCreateMappings<MappingJobToIssueType>();
                        var tIssue = jMap.GetOrCreateTargetObject(MappingJobToIssueType.GetKey(job));
                        jMap.AddMapping(job, tIssue);

                        //convert related documents and add them to documents of DPoW object

                        foreach (var dObject in job.DPoWObjects)
                        {
                            //branch for asset type, assembly type and zones
                            var zone = dObject as Zone;
                            if (zone != null)
                            { 
                            }

                            var assetType = dObject as AssetType;
                            if (assetType != null)
                            { 
                            }

                            var assemblyType = dObject as Xbim.DPoW.Interfaces.AssemblyType;
                            if (assemblyType != null)
                            { 
                            }

                        }
                    }
                }
            }

            return target;
        }
    }
}
