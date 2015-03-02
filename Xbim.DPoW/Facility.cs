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
    }
}
