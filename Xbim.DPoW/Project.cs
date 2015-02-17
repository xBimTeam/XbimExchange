using System;
using System.Linq;

namespace Xbim.DPoW
{
    /// <summary>
    /// This holds information which is supposed to be invariant during the project
    /// </summary>
    public class Project :DPoWAttributableObject
    {
        /// <summary>
        /// Code of the project
        /// </summary>
        public string Code { get; private set; }
        /// <summary>
        /// Linear units used on the project by default
        /// </summary>
        public LinearUnits LinearUnits { get; set; }
        /// <summary>
        /// Area units used on the project by default
        /// </summary>
        public AreaUnits AreaUnits { get; set; }
        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Volume units used on the project by default
        /// </summary>
        public VolumeUnits VolumeUnits { get; set; }
        /// <summary>
        /// Project URI
        /// </summary>
        public string ProjectURI { get; set; }
        /// <summary>
        /// Brief description of the project
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Currency units used on the project by default
        /// </summary>
        public CurrencyUnits CurrencyUnits { get; set; }
        /// <summary>
        /// Pointer to current project stage
        /// </summary>
        public Guid CurrentProjectStageId { get; set; }
        /// <summary>
        /// Gets current project stage. Use 'CurrentProjectStageId' to set current project stage.
        /// </summary>
        /// <param name="pow">Plan of work</param>
        /// <returns>Current project stage</returns>
        public ProjectStage GetProjectStage(PlanOfWork pow)
        {
            return pow.ProjectStages != null ? pow.ProjectStages.FirstOrDefault(ps => ps.Id == CurrentProjectStageId) :
            null;
        }

        /// <summary>
        /// Unique ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Initializes ID to new unique ID
        /// </summary>
        public Project()
        {
            Id = Guid.NewGuid();
        }
    }
}
