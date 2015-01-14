using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.DPoW.Interfaces
{
    public class ProjectStage
    {
        public string ProjectStageName { get; set; }
        public string ProjectStageCode { get; set; }
        public string ProjectStageDescription { get; set; }
        public List<Job> Jobs { get; set; }

        public ProjectStage()
        {
            Jobs = new List<Job>();
        }
    }
}
