using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingIfcProjectToProject : XbimMappings<IfcStore, IModel, int, IIfcProject, CobieProject>
    {
        protected override CobieProject Mapping(IIfcProject source, CobieProject target)
        {
            var helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper;
            target.ExternalObject = helper.GetExternalObject(source);
            target.ExternalId = helper.ExternalEntityIdentity(source);
            target.AltExternalId = source.GlobalId;
            target.Name = source.Name;
            target.Description = FirstNonEmptyString(source.Description, source.LongName, source.Name);
            return target;
        }

        public override CobieProject CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieProject>();
        }
    }
}
