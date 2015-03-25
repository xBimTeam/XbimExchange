
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    /// <summary>
    /// 
    /// </summary>
    public class SiteType:Site
    {
        
        /// <summary>
        /// 
        /// </summary>
        public SiteType()
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcSite"></param>
        /// <param name="helper"></param>
        public SiteType(IfcSite ifcSite, CoBieLiteUkHelper helper)
            : this()
        {
         
            ExternalEntity = helper.ExternalEntityName(ifcSite);
            ExternalId = helper.ExternalEntityIdentity(ifcSite);
            Name = ifcSite.LongName;
            Description = ifcSite.Description;
        }
    }
}
