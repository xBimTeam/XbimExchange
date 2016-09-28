using System.Linq;
using Xbim.COBieLite;
using Xbim.DPoW;
using Xbim.COBieLite.CollectionTypes;

namespace XbimExchanger.DPoWToCOBieLite
{
    abstract class MappingAttributableObjectToCOBieObject<TFrom, TTo> : DPoWToCOBieLiteMapping<TFrom, TTo> where TFrom:DPoWAttributableObject where TTo : ICOBieObject, new()
    {
        protected override TTo Mapping(TFrom source, TTo target)
        {
            if (source.Attributes == null || !source.Attributes.Any())
                return target;

            var tAttrs = source.GetCOBieAttributes();
            if (target.Attributes == null) target.Attributes = new AttributeCollectionType();
            target.Attributes.AddRange(tAttrs);

            return target;
        }

        
    }
}
