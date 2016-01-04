using System;
using System.Linq;
using Xbim.COBieLite;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingAssetInfoTypeToIfcElement<TToObject> : CoBieLiteIfcMappings<string, AssetInfoType, TToObject> where TToObject : IfcElement
    {

        protected override TToObject Mapping(AssetInfoType assetInfoType, TToObject ifcElement)
        {
            ifcElement.Name = assetInfoType.AssetName;
            ifcElement.Description = assetInfoType.AssetDescription;
            
            #region Attributes

            if (assetInfoType.AssetAttributes != null)
            {

                foreach (var attribute in assetInfoType.AssetAttributes)
                {
                    Exchanger.ConvertAttributeTypeToIfcObjectProperty(ifcElement, attribute);
                }
            }
            #endregion

            #region Space Assignments

            if (assetInfoType.AssetSpaceAssignments != null && assetInfoType.AssetSpaceAssignments.Any())
            {
                foreach (var spaceAssignment in assetInfoType.AssetSpaceAssignments)
                {
                    var ifcSpace = Exchanger.GetIfcSpace(spaceAssignment);
                    if (ifcSpace == null) throw new Exception("Space " + spaceAssignment.FloorName + " - " + spaceAssignment.SpaceName+" cannot be found");

                    ifcSpace.AddElement(ifcElement);
                }
            }
            #endregion

            return ifcElement;
        }
    }

}
