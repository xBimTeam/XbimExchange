using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;
using Xbim.DPoW;

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    class MappingDocumentToDocumentType : MappingAttributableObjectToCOBieObject<Documentation, Document>
    {
        protected override Document Mapping(Documentation sObject, Document tObject)
        {
            //perform base mapping where free attributes are converted
            base.Mapping(sObject, tObject);

            tObject.ExternalId = sObject.Id.ToString();
            tObject.Name = sObject.Name;
            tObject.Description = sObject.Description;
            tObject.Reference = sObject.URI;


            tObject.Category =
                Exchanger.SourceRepository.GetEncodedClassification(sObject.ClassificationReferenceIds);

            //classification and classification code encoded with ';' separator
            if (sObject.ClassificationReferenceIds != null && sObject.ClassificationReferenceIds.Any())
            {
                var reference = sObject.GetClassificationReferences(Exchanger.SourceRepository).FirstOrDefault();
                var classification =
                    Exchanger.SourceRepository.ClassificationSystems.FirstOrDefault(c => c.ClassificationReferences.Contains(reference));
                if (reference != null && classification != null)
                    tObject.Category = String.Format("{0};{1}", classification.Name, reference.ClassificationCode);
            }


            //Issues from Jobs + Responsibilities
            if (sObject.Jobs != null && sObject.Jobs.Any())
            {
                if (tObject.Issues == null) tObject.Issues = new List<Issue>();
                var jMap = Exchanger.GetOrCreateMappings<MappingJobToIssueType>();
                foreach (var job in sObject.Jobs)
                {
                    var issue = jMap.GetOrCreateTargetObject(job.Id.ToString());
                    jMap.AddMapping(job, issue);
                    tObject.Issues.Add(issue);
                }
            }

            return tObject;
        }
    }
}
