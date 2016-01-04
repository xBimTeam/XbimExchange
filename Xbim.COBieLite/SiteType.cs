using Xbim.Ifc4.Interfaces;

namespace Xbim.COBieLite
{
    public partial class SiteType
    {
        
       // private IfcSite _ifcSite;

        public SiteType()
        {
            
        }
        public SiteType(IIfcSite ifcSite, CoBieLiteHelper helper)
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
