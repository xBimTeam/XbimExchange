using System;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.COBieLiteUkToIfc
{
    class MappingAssetToIfcElement<TToObject> : CoBieLiteUkIfcMappings<string, Asset, TToObject> where TToObject : IfcElement, new()
    {

        protected override TToObject Mapping(Asset assetInfoType, TToObject ifcElement)
        {
            ifcElement.Name = assetInfoType.Name;
            ifcElement.Description = assetInfoType.Description;
            
            #region Attributes

            if (assetInfoType.Attributes != null)
            {

                foreach (var attribute in assetInfoType.Attributes)
                {
                    Exchanger.ConvertAttributeTypeToIfcObjectProperty(ifcElement, attribute);
                }
            }
            #endregion

            #region Space Assignments

            if (assetInfoType.Spaces != null && assetInfoType.Spaces.Any())
            {
                foreach (var spaceAssignment in assetInfoType.Spaces)
                {
                    var ifcSpace = Exchanger.GetIfcSpace(spaceAssignment);
                    if (ifcSpace == null) throw new Exception("Space " + spaceAssignment.Name + " - " + spaceAssignment.Name+" cannot be found");

                    ifcSpace.AddElement(ifcElement);
                }
            }
            #endregion

            return ifcElement;
        }
    }

}
