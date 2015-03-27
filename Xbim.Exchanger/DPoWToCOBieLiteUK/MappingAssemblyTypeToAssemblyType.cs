using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;
using AssemblyType = Xbim.DPoW.AssemblyType;

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    class MappingAssemblyTypeToAssemblyType : MappingDPoWObjectToCOBieObject<AssemblyType, Assembly>
    {
        protected override Assembly Mapping(AssemblyType sObject, Assembly tObject)
        {
            //do basic mapping of the object
            base.Mapping(sObject, tObject);

            //add assembly to asset types (which is a nonsense but it is how it is defined in the COBieLite schema for now)
            if (sObject.AggregationOfAssetTypes != null && sObject.AggregationOfAssetTypes.Any())
            {
                var assetMap = Exchanger.GetOrCreateMappings<MappingAssetTypeToAssetType>();
                tObject.ChildAssetsOrTypes= new List<EntityKey>();
                foreach (var id in sObject.AggregationOfAssetTypes)
                {
                    AssetType tAsset;
                    if (!assetMap.GetTargetObject(id.ToString(), out tAsset)) continue;
                    tObject.ChildAssetsOrTypes.Add(new EntityKey { Name = tAsset.Name , KeyType = EntityType.AssetType});
                }
            }

            return tObject;
        }
    }
}
