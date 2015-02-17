using System;
namespace Xbim.DPoW
{
    /// <summary>
    /// Classification reference
    /// </summary>
    public class ClassificationReference 
    {
        /// <summary>
        /// Sort
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// Classification code like 12-45-98 or 78.78.65
        /// </summary>
        public string ClassificationCode { get; set; }
        /// <summary>
        /// Description of this classification node
        /// </summary>
        public string ClassificationDescription { get; set; }
        /// <summary>
        /// Unique ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Constructor initializes ID to new unique value.
        /// </summary>
        public ClassificationReference()
        {
            Id = Guid.NewGuid();
        }
    }
}
