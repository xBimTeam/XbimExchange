using Xbim.Ifc2x3.ProductExtension;

namespace Xbim.COBieLite
{
    public partial class SiteType
    {
        
       // private IfcSite _ifcSite;

        public SiteType()
        {
            
        }
        public SiteType(IfcSite ifcSite, CoBieLiteHelper helper)
            : this()
        {
          //  _ifcSite = ifcSite;
            externalEntityName = helper.ExternalEntityName(ifcSite);
            externalID = helper.ExternalEntityIdentity(ifcSite);
            externalSystemName = helper.ExternalSystemName(ifcSite);
            SiteName = ifcSite.LongName;
            SiteDescription = ifcSite.Description;
        }
    }
}
