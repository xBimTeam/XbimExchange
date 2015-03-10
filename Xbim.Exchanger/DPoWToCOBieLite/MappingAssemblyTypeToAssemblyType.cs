using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLite;
using Xbim.COBieLite.CollectionTypes;
using Assembly = Xbim.COBieLite.AssemblyType;
using AssemblyType = Xbim.DPoW.AssemblyType;

namespace XbimExchanger.DPoWToCOBieLite
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

                foreach (var id in sObject.AggregationOfAssetTypes)
                {
                    AssetTypeInfoType tAsset;
                    if (!assetMap.GetTargetObject(id.ToString(), out tAsset)) continue;
                    if (tAsset.AssemblyAssignments == null)
                        tAsset.AssemblyAssignments = new AssemblyAssignmentCollectionType();
                    tAsset.AssemblyAssignments.Add(tObject);
                }
            }

            //do specific mappings
            return tObject;
        }
    }
}
