using Xbim.COBieLite;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingSiteTypeToIfcSite : CoBieLiteIfcMappings<string, SiteType, IfcSite>
    {

        protected override IfcSite Mapping(SiteType siteType, IfcSite ifcSite)
        {
           
            ifcSite.Name = siteType.SiteName;
            ifcSite.Description = siteType.SiteDescription;
            ifcSite.CompositionType=IfcElementCompositionEnum.ELEMENT;
            return ifcSite;
        }
    }
}
