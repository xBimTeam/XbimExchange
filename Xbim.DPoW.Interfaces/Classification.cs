using System.Collections.Generic;

namespace Xbim.DPoW.Interfaces
{
    public class Classification 
    {
        public string ClassificationURI { get; set; }
        public string ClassificationEditionDate { get; set; }
        public string ClassificationName { get; set; }
        public string ClassificationEdition { get; set; }
        public string ClassificationPublisher { get; set; }
        public IEnumerable<ClassificationReference> ClassificationReferences { get; set; }
    }
}
