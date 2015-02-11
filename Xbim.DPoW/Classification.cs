using System;
using System.Collections.Generic;

namespace Xbim.DPoW
{
    public class Classification 
    {
        public string ClassificationURI { get; set; }
        public string ClassificationEditionDate { get; set; }
        public string ClassificationName { get; set; }
        public string ClassificationEdition { get; set; }
        public string ClassificationPublisher { get; set; }
        public List<ClassificationReference> ClassificationReferences { get; set; }

        public Guid Id { get; set; }

        public Classification()
        {
            Id = Guid.NewGuid();
            ClassificationReferences = new List<ClassificationReference>();
        }
    }
}
