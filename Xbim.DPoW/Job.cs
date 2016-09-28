using System;

namespace Xbim.DPoW
{
    /// <summary>
    /// Job is used to represent required action with responsibility and other related information.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Name of the job
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of the job
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Responsibility assignment
        /// </summary>
        public Responsibility Responsibility { get; set; }
        /// <summary>
        /// Unique ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// ID initialized to unique value
        /// </summary>
        public Job()
        {
            Id = Guid.NewGuid();
        }
    }
}
