using System;
using System.Collections.Generic;

namespace Xbim.DPoW
{
    /// <summary>
    /// Classification system like Uniclass2, NRM, Omniclass or any other. It contains classification references
    /// which represent single classification nodes
    /// </summary>
    public class Classification 
    {
        /// <summary>
        /// URI representing classification
        /// </summary>
        public string URI { get; set; }
        /// <summary>
        /// Edition date
        /// </summary>
        public string EditionDate { get; set; }
        /// <summary>
        /// Name of the classification
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Edition of this classification
        /// </summary>
        public string Edition { get; set; }
        /// <summary>
        /// Publisher of this classification
        /// </summary>
        public string Publisher { get; set; }
        /// <summary>
        /// List of classification references. These references are referenced in DPoW by their IDs
        /// </summary>
        public List<ClassificationReference> ClassificationReferences { get; set; }

        /// <summary>
        /// Id of this classification. This value is initialized to new unique ID in constructor.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Constructor initializes new unique ID
        /// </summary>
        public Classification()
        {
            Id = Guid.NewGuid();
        }
    }
}
