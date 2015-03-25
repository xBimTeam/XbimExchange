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

            if(tObject.Categories == null) tObject.Categories = new List<Category>();
            tObject.Categories.AddRange(Exchanger.SourceRepository.GetEncodedClassification(sObject.ClassificationReferenceIds));
                
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
