using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xbim.DPoW
{
    [XmlInclude(typeof(SpaceTypes))]
    [XmlInclude( typeof(AssemblyType))]
    [XmlInclude(typeof(AssetType))]
    public abstract class DPoWObject 
    {
        public List<Guid> ClassificationReferenceIds { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RequiredAttribute> RequiredAttributes { get; set; }
        public RequiredLOD RequiredLOD { get; set; }

        public Guid Id { get; set; }

        public List<Job> Jobs { get; set; }

        public DPoWObject()
        {
            Id = Guid.NewGuid();
            Jobs = new List<Job>();
            RequiredAttributes = new List<RequiredAttribute>();
            ClassificationReferenceIds = new List<Guid>();
        }
    }
}
