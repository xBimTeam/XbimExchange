using System;
using System.Collections.Generic;

namespace Xbim.DPoW
{
    public class Document
    {
        public ScopeOfDocument Scope { get; set; }
        public string DocumentDescription { get; set; }
        public string DocumentURI { get; set; }
        public Guid DocumentClassificationReferenceId { get; set; }
        public string DocumentName { get; set; }

        public Guid Id { get; set; }

        /// <summary>
        /// Jobs associated with this document.
        /// </summary>
        public List<Job> Jobs { get; set; }

        public Document()
        {
            Jobs = new List<Job>();
            Id=Guid.NewGuid();
        }
    }
}
