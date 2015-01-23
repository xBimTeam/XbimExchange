using Xbim.COBieLite;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingSiteTypeToIfcSite : XbimIfcMappings<string, SiteType, IfcSite>
    {

        protected override IfcSite Mapping(SiteType source, IfcSite target)
        {
           
            target.Name = source.SiteName;
            target.Description = source.SiteDescription;
            return target;
        }
    }
}
