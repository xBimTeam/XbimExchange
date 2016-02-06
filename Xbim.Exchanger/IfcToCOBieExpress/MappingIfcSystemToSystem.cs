using System.Linq;
using Xbim.CobieExpress;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingIfcSystemToSystem : MappingIfcObjectToAsset<IIfcSystem, CobieSystem>
    {
        private MappingIfcElementToComponent _elementMapping;

        public MappingIfcElementToComponent ElementMapping
        {
            get { return _elementMapping ?? (_elementMapping = Exchanger.GetOrCreateMappings<MappingIfcElementToComponent>()); }
        }

        protected override CobieSystem Mapping(IIfcSystem ifcSystem, CobieSystem target)
        {
            base.Mapping(ifcSystem, target);

            //Add Assets
            var systemAssignments = Helper.GetSystemAssignments(ifcSystem);
            var elements = systemAssignments.OfType<IIfcElement>();
            foreach (var element in elements)
            {
                CobieComponent component;
                if (ElementMapping.GetOrCreateTargetObject(element.EntityLabel, out component))
                    ElementMapping.AddMapping(element, component);
                target.Components.Add(component);
            }

            //TODO: System Issues

            return target;
        }

        public override CobieSystem CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieSystem>();
        }
    }
}
