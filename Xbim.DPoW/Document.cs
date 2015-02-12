using System;
using System.Collections.Generic;

namespace Xbim.DPoW
{
    public class Document
    {
        public ScopeOfDocument Scope { get; set; }
        public string Description { get; set; }
        public string URI { get; set; }
        public List<Guid> ClassificationReferenceIds { get; set; }
        public string Name { get; set; }

        public Guid Id { get; set; }

        /// <summary>
        /// Jobs associated with this document.
        /// </summary>
        public List<Job> Jobs { get; set; }

        public Document()
        {
            ClassificationReferenceIds = new List<Guid>();
            Jobs = new List<Job>();
            Id=Guid.NewGuid();
        }
    }
}
