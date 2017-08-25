using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingIfcSiteToSite : XbimMappings<IfcStore, IModel, int, IIfcSite, CobieSite>
    {
        protected override CobieSite Mapping(IIfcSite ifcSite, CobieSite site)
        {
            var helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper;
            site.ExternalObject = helper.GetExternalObject(ifcSite);
            site.ExternalId = helper.ExternalEntityIdentity(ifcSite);
            site.AltExternalId = ifcSite.GlobalId;
            site.Name = ifcSite.LongName;
            site.Description = FirstNonEmptyString(ifcSite.Description, ifcSite.LongName, ifcSite.Name);
            return site;
        }

        public override CobieSite CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieSite>();
        }
    }
}
