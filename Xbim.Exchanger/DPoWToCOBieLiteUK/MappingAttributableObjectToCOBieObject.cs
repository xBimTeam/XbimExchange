using System.Collections.Generic;
using System.Linq;
using Xbim.CobieLiteUk;
using Xbim.DPoW;
using Attribute = Xbim.CobieLiteUk.Attribute;

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    abstract class MappingAttributableObjectToCOBieObject<TFrom, TTo> : DpoWtoCoBieLiteUkMapping<TFrom, TTo> where TFrom:DPoWAttributableObject where TTo : CobieObject, new()
    {
        protected override TTo Mapping(TFrom source, TTo target)
        {
            base.Mapping(source, target);

            if (source.Attributes == null || !source.Attributes.Any())
                return target;

            var tAttrs = source.GetCOBieAttributes(target.CreatedOn, target.CreatedBy.Email);
            if (target.Attributes == null) target.Attributes = new List<Attribute>();
            target.Attributes.AddRange(tAttrs);

            return target;
        }



        public override TTo CreateTargetObject()
        {
            return new TTo();
        }
    }
}
