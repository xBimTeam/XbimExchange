using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingStringToExternalSystem : XbimMappings<IfcStore, IModel, string, string, CobieExternalSystem>
    {
        public override CobieExternalSystem CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieExternalSystem>();
        }

        protected override CobieExternalSystem Mapping(string source, CobieExternalSystem target)
        {
            target.Name = source;
            return target;
        }
    }
}
