using System.Collections.Generic;
using System.Linq;
using Xbim.CobieLiteUk;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    internal class MappingIfcRelAggregatesToAssembly : XbimMappings<IModel, List<Facility>, string, IIfcRelAggregates, Assembly>
    {
        public EntityType EntityType
        { get; set; }

        protected override Assembly Mapping(IIfcRelAggregates source, Assembly target)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;

            target.ExternalEntity = helper.ExternalEntityName(source);
            target.ExternalId = helper.ExternalEntityIdentity(source);
            target.AltExternalId = source.GlobalId;
            target.ExternalSystem = helper.ExternalSystemName(source);
            target.Description = source.Description;
            target.Categories = CoBieLiteUkHelper.UnknownCategory;
            target.CreatedBy = helper.GetCreatedBy(source);
            target.CreatedOn = helper.GetCreatedOn(source);
            target.Name = source.Name;

            if (source.RelatedObjects.Any())
            {
                List<IIfcObjectDefinition> children = source.RelatedObjects.ToList();

                List<EntityKey> entityKeys = new List<EntityKey>();
                foreach (IIfcObjectDefinition child in children)
                {
                    EntityKey entityKey = new EntityKey();
                    entityKey.KeyType = EntityType;

                    if(EntityType.AssetType == EntityType && helper.TypeEntityKeyLookup.ContainsKey(child))
                    {
                        entityKey.Name = helper.TypeEntityKeyLookup[child];
                    } else
                    {
                        entityKey.Name = child.Name;
                    }
                    entityKeys.Add(entityKey);
                }

                target.ChildAssetsOrTypes = entityKeys;
            }

            return target;
        }

        public override Assembly CreateTargetObject()
        {
            return new Assembly();
        }
    }
}
