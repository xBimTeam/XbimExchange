using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.DPoWToCOBieLite
{
    class MappingJobToJobType: DPoWToCOBieLiteMapping<Job, JobType>
    {
        protected override JobType Mapping(Job source, JobType target)
        {
            target.externalID = Exchanger.GetStringIdentifier();
            target.JobName = source.JobName;
            target.JobDescription = source.JobDescription;
            target.JobAttributes = new AttributeCollectionType();

            //responsible person can only be saved as a set of attributes
            var respPers = source.ResponsibleFor;
            if (respPers != null)
            {
                target.JobAttributes.Add("ResponsibleContactGivenName", "ResponsibleContactGivenName", respPers.ContactGivenName, "ResponsibleContact");
                target.JobAttributes.Add("ResponsibleContactFamilyName", "ResponsibleContactFamilyName", respPers.ContactFamilyName, "ResponsibleContact");
                target.JobAttributes.Add("ResponsibleContactEmail", "ResponsibleContactEmail", respPers.ContactEmail, "ResponsibleContact");
                target.JobAttributes.Add("ResponsibleContactCompanyName", "ResponsibleContactCompanyName", respPers.ContactCompanyName, "ResponsibleContact");
            }

            //add converted documents
            if (source.Documents != null && source.Documents.Any())
            {
                target.JobDocuments = new DocumentCollectionType();
                var dMap = Exchanger.GetOrCreateMappings<MappingDocumentToDocumentType>();
                foreach (var sDoc in source.Documents)
                {
                    DocumentType tDoc;
                    var dKey = MappingDocumentToDocumentType.GetKey(sDoc);
                    if (!dMap.GetTargetObject(dKey, out tDoc))
                    {
                        tDoc = dMap.GetOrCreateTargetObject(dKey);
                        dMap.AddMapping(sDoc, tDoc);
                    }
                    target.JobDocuments.Add(tDoc);

                }
            }
            return target;
        }

        public static string GetKey(Job job)
        {
            return MappingJobToIssueType.GetKey(job);
        }
    }
}
