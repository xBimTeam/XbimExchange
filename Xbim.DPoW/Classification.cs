using System;
using System.Collections.Generic;

namespace Xbim.DPoW
{
    public class Classification 
    {
        public string URI { get; set; }
        public string EditionDate { get; set; }
        public string Name { get; set; }
        public string Edition { get; set; }
        public string Publisher { get; set; }
        public List<ClassificationReference> ClassificationReferences { get; set; }

        public Guid Id { get; set; }

        public Classification()
        {
            Id = Guid.NewGuid();
            ClassificationReferences = new List<ClassificationReference>();
        }
    }
}
