using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xbim.DPoW
{
    /// <summary>
    /// Abstract class for space types, asset types and assembly types
    /// </summary>
    [XmlInclude(typeof(SpaceType))]
    [XmlInclude( typeof(AssemblyType))]
    [XmlInclude(typeof(AssetType))]
    public abstract class DPoWObject :DPoWAttributableObject
    {
        /// <summary>
        /// Classification reference IDs. These hold IDs of classification references from diferent classification systems
        /// </summary>
        public List<Guid> ClassificationReferenceIds { get; set; }
        /// <summary>
        /// Name of the object
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of the object
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Required LOD of this object representing detail of geometry as a code
        /// </summary>
        public RequiredLOD RequiredLOD { get; set; }
        /// <summary>
        /// Required LOI of this object representing required attributes as an ammount of information for specific level
        /// </summary>
        public RequiredLOI RequiredLOI { get; set; }
        /// <summary>
        /// Unique ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Jobs related to this object. This is used to assign responsibility to people and/or roles for specific actions.
        /// </summary>
        public List<Job> Jobs { get; set; }
        /// <summary>
        /// Constructor initializes ID to new unique value
        /// </summary>
        public DPoWObject()
        {
            Id = Guid.NewGuid();
        }
    }
}
