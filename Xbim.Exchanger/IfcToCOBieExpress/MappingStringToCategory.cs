using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingStringToCategory : XbimMappings<IfcStore, IModel, string, string, CobieCategory>
    {
        public override CobieCategory CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieCategory>();
        }

        protected override CobieCategory Mapping(string source, CobieCategory target)
        {
            target.Value = source;
            target.Description = source;
            return target;
        }

        public CobieCategory GetOrCreate(string value)
        {
            CobieCategory result;
            if (GetOrCreateTargetObject(value, out result))
                Mapping(value, result);
            return result;
        }
    }
}
