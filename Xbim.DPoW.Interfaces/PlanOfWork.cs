using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xbim.DPoW.Interfaces
{
    public class PlanOfWork 
    {
        public List<Classification> ClassificationSystem { get; set; }
        public Project Project { get; set; }
        public Facility Facility { get; set; }
        public List<ProjectStage> ProjectStages { get; set; }
        public Contact Client { get; set; }
        public List<Contact> Contacts { get; set; }

        public PlanOfWork()
        {
            Contacts = new List<Contact>();
            ProjectStages = new List<ProjectStage>();
            ClassificationSystem = new List<Classification>();
        }

    }
}
