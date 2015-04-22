using System;
using System.Linq;

namespace Xbim.DPoW
{
    /// <summary>
    /// Facility
    /// </summary>
    public class Facility
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Category ID
        /// </summary>
        public Guid CategoryId { get; set; }
        /// <summary>
        /// Sita name
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// Site description
        /// </summary>
        public string SiteDescription { get; set; }

        /// <summary>
        /// Gets category from actual plan of work by it's ID
        /// </summary>
        /// <param name="pow"></param>
        /// <returns></returns>
        public ClassificationReference GetCategory(PlanOfWork pow)
        {
            if (pow.ClassificationSystems == null) return null;
            return (from classification in pow.ClassificationSystems where classification.ClassificationReferences != null from reference in classification.ClassificationReferences where reference.Id == CategoryId select reference).FirstOrDefault();
        }

        /// <summary>
        /// Gets category from actual plan of work by it's ID
        /// </summary>
        /// <param name="pow"></param>
        /// <returns></returns>
        public Tuple<Classification, ClassificationReference> GetClassificationAndReference(PlanOfWork pow)
        {
            if (pow.ClassificationSystems == null) return null;
            foreach (var classification in pow.ClassificationSystems)
            {
                if (classification.ClassificationReferences == null) continue;
                var reference = classification.ClassificationReferences.FirstOrDefault(r => r.Id == CategoryId);
                if(reference == null) continue;
                return new Tuple<Classification, ClassificationReference>(classification, reference);
            }

            return null;
        }
    }
}
