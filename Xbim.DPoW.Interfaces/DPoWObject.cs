using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xbim.DPoW.Interfaces
{
    [XmlInclude(typeof(Zone))]
    [XmlInclude( typeof(Assembly))]
    [XmlInclude(typeof(AssetType))]
    public abstract class DPoWObject 
    {
        public ClassificationReference DPoWObjectCategory { get; set; }
        public string DPoWObjectName { get; set; }
        public string DPoWObjectDescription { get; set; }
        public List<RequiredAttribute> RequiredAttributes { get; set; }
        public RequiredLOD RequiredLOD { get; set; }

        public DPoWObject()
        {
            RequiredAttributes = new List<RequiredAttribute>();
        }
    }
}
