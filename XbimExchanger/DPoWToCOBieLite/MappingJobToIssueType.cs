using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.DPoWToCOBieLite
{
    public class MappingJobToIssueType : DPoWToCOBieLiteMapping<Job, IssueType>
    {
        protected override IssueType Mapping(Job source, IssueType target)
        {
            target.externalID = Exchanger.GetStringIdentifier();
            target.IssueName = source.JobName;
            target.IssueDescription = source.JobDescription;
            
            //responsible person can only be saved as a set of attributes
            var respPers = source.ResponsibleFor;
            if (respPers != null)
            {
                var cMap = Exchanger.GetOrCreateMappings<MappingContactToContact>();
                target.ContactAssignment = new ContactKeyType() { ContactEmailReference = respPers.ContactEmail };
            }
            return target;
        }

        public static string GetKey(Job job)
        {
            return String.Format("{0} {1} {2}", job.JobName, job.JobDescription, job.DPoWObjects.Count);
        }
    }
}
