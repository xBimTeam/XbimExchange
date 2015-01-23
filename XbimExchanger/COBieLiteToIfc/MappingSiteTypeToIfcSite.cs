using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;

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
