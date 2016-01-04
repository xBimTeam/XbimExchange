using System.Collections.Generic;
using Xbim.COBieLiteUK;
using Xbim.Ifc;

using Xbim.Ifc4.Interfaces;


namespace XbimExchanger.IfcToCOBieLiteUK
{
    class MappingIfcSiteToSite : XbimMappings<IfcStore, List<Facility>, string, IIfcSite, Site>
    {
        protected override Site Mapping(IIfcSite ifcSite, Site site)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            site.ExternalEntity = helper.ExternalEntityName(ifcSite);
            site.ExternalId = helper.ExternalEntityIdentity(ifcSite);
            site.AltExternalId = ifcSite.GlobalId;
            site.Name = ifcSite.LongName;
            site.Description = ifcSite.Description;
            return site;
        }

        public override Site CreateTargetObject()
        {
            return new Site();
        }
    }
}
