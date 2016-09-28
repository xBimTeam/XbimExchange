using System;
using Xbim.Common;
using Xbim.Ifc;


namespace XbimExchanger.IfcHelpers
{
    public abstract class IfcMappings<TSourceRepository, TFromKey, TFromObject, TToObject> : XbimMappings<TSourceRepository, IfcStore, TFromKey, TFromObject, TToObject> where TToObject : IPersistEntity
    {
        public override TToObject CreateTargetObject()
        {
            if (!typeof(IInstantiableEntity).IsAssignableFrom(typeof(TToObject)))
                throw new Exception(string.Format("Invalid IFC type {0} is not assignable to an instantiable entity", typeof(TToObject).Name));
            return (TToObject) Exchanger.TargetRepository.Instances.New(typeof(TToObject));
        }

    }
}
