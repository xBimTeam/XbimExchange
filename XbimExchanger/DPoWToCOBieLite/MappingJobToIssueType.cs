using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.DPoWToCOBieLite
{
    public class MappingJobToIssueType : DPoWToCOBieLiteMapping<Job, JobType>
    {
        protected override JobType Mapping(Job source, JobType target)
        {
            throw new NotImplementedException();
        }

        public static string GetKey(Job job)
        {
            return String.Format("{0} {1} {2}", job.JobName, job.JobDescription, job.DPoWObjects.Count);
        }
    }
}
