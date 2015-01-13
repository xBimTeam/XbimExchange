using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xbim.DPoW.Interfaces
{
    public class PlanOfWork 
    {
        public IEnumerable<Classification> ClassificationSystem { get; set; }
        public Project Project { get; set; }
        public Facility Facility { get; set; }
        public IEnumerable<ProjectStage> ProjectStages { get; set; }
        public Contact Client { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
    }
}
