using System;
using System.Collections.Generic;
using System.Linq;

namespace Xbim.DPoW
{
    public class Documentation
    {
        /// <summary>
        /// Description of documentation
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// URI identifying this documentation
        /// </summary>
        public string URI { get; set; }
        /// <summary>
        /// Classification references assigned to this documentation. It is possible to assign multiple classification
        /// references from multiple classification systems if necessary. Use 'GetClassificationReferences' method
        /// to get real objects instead of their IDs.
        /// </summary>
        public List<Guid> ClassificationReferenceIds { get; set; }
        /// <summary>
        /// Name of this documentation
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ID of this documentation object
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets all classification references based on IDs
        /// </summary>
        /// <param name="pow">Plan of work in which this object and classification references live</param>
        /// <returns>Enumeration of classification references</returns>
        public IEnumerable<ClassificationReference> GetClassificationReferences(PlanOfWork pow)
        {
            if (ClassificationReferenceIds == null || pow.ClassificationSystems == null) yield break;
            foreach (var reference in from classification in pow.ClassificationSystems where classification.ClassificationReferences != null from reference in classification.ClassificationReferences where ClassificationReferenceIds.Contains(reference.Id) select reference)
                yield return reference;
        }

        /// <summary>
        /// Jobs associated with this document.
        /// </summary>
        public List<Job> Jobs { get; set; }

        /// <summary>
        /// Constructor of documentation object. It sets Id to new GUID.
        /// </summary>
        public Documentation()
        {
            Id=Guid.NewGuid();
        }
    }
}
