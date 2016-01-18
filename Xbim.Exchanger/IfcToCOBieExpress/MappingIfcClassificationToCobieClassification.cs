using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingIfcClassificationToCobieClassification : XbimMappings<IfcStore, IModel, string, IIfcClassification, CobieClassification>
    {
        public override CobieClassification CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieClassification>();
        }

        protected override CobieClassification Mapping(IIfcClassification source, CobieClassification target)
        {
            target.Name = source.Name;
            target.Description = source.Description;
            return target;
        }
    }
}
