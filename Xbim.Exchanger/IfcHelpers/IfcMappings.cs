using Xbim.IO;

namespace XbimExchanger.IfcHelpers
{
    public abstract class IfcMappings<TSourceRepository, TFromKey, TFromObject, TToObject> : XbimMappings<TSourceRepository, XbimModel, TFromKey, TFromObject, TToObject> where TToObject : IPersistIfcEntity, new()
    {
        public override TToObject CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<TToObject>();
        }

    }
}
