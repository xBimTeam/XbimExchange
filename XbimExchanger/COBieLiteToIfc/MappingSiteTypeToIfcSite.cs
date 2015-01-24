using Xbim.COBieLite;
using Xbim.Ifc2x3.ProductExtension;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingSiteTypeToIfcSite : COBieLiteIfcMappings<string, SiteType, IfcSite>
    {

        protected override IfcSite Mapping(SiteType source, IfcSite target)
        {
           
            target.Name = source.SiteName;
            target.Description = source.SiteDescription;
            return target;
        }
    }
}
