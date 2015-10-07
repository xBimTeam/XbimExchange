using System.Collections.Generic;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    class MappingIfcSiteToSite : XbimMappings<XbimModel, List<Facility>, string, IfcSite, Site>
    {
        protected override Site Mapping(IfcSite ifcSite, Site site)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            site.ExternalEntity = helper.ExternalEntityName(ifcSite);
            site.ExternalId = helper.ExternalEntityIdentity(ifcSite);
            site.AltExternalId = ifcSite.GlobalId;
            site.Name = ifcSite.LongName;
            site.Description = ifcSite.Description;
            return site;
        }
    }
}
