using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLite;
using Xbim.COBieLite.CollectionTypes;
using Xbim.DPoW;

namespace XbimExchanger.DPoW2ToCOBieLite
{
    class MappingAssetTypeToAssetType: MappingDPoWObjectToCOBieObject<AssetType, AssetTypeInfoType>
    {
        protected override AssetTypeInfoType Mapping(AssetType sObject, AssetTypeInfoType tObject)
        {
            //perform base mapping on the level of objects
            base.Mapping(sObject, tObject);

            //------------------ mappings specific to asset type ----------------------
            if (!String.IsNullOrEmpty(sObject.Variant))
            {
                //set variant of the object as a model number
                tObject.AssetTypeModelNumber = sObject.Variant;
            }

            if (sObject.RequiredLOD != null)
                tObject.AssetTypeShapeDescription = sObject.RequiredLOD.Code;

            //add dummy asset so that COBieLite => IFC will create real dummy geometry objects
            if (tObject.Assets == null) tObject.Assets = new AssetCollectionType();
            tObject.Assets.Add(new AssetInfoType()
            {
                externalID = Guid.NewGuid().ToString(),
                AssetDescription = sObject.Description,
                AssetName = sObject.Name
            });


            return tObject;
        }
    }
}
