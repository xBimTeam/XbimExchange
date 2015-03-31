using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.COBieLiteUkToIfc
{
    class MappingSiteToIfcSite : CoBieLiteUkIfcMappings<string, Site, IfcSite>
    {

        protected override IfcSite Mapping(Site siteType, IfcSite ifcSite)
        {
           
            ifcSite.Name = siteType.Name;
            ifcSite.Description = siteType.Description;
            ifcSite.CompositionType=IfcElementCompositionEnum.ELEMENT;
            return ifcSite;
        }
    }
}
