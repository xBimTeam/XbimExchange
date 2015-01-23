using Xbim.IO;
using Xbim.XbimExtensions.Interfaces;

namespace XbimExchanger
{
    abstract class XbimIfcMappings<TFromKey, TFromObject, TToObject> : XbimMappings<XbimModel, TFromKey, TFromObject, TToObject> where TToObject : IPersistIfcEntity, new()
    {
        public override TToObject CreateTargetObject()
        {
            return MappingsCollection.Repository.Instances.New<TToObject>();
        }
    }
}
