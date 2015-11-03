using System;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.ProductExtension;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.COBieLiteUkToIfc
{
    class MappingSpaceToIfcSpace : CoBieLiteUkIfcMappings<string, Space, IfcSpace>
    {
        protected override IfcSpace Mapping(Space spaceType, IfcSpace ifcSpace)
        {
            Exchanger.SetUserHistory(ifcSpace, spaceType.ExternalSystem, (spaceType.CreatedBy == null) ? null : spaceType.CreatedBy.Email, (spaceType.CreatedOn == null) ? DateTime.Now : (DateTime)spaceType.CreatedOn);
            using (OwnerHistoryEditScope ohContext = new OwnerHistoryEditScope(Exchanger.TargetRepository, ifcSpace.OwnerHistory))
            {
                ifcSpace.Name = spaceType.Name;
                ifcSpace.Description = spaceType.Description;
                ifcSpace.CompositionType = IfcElementCompositionEnum.ELEMENT;
                Exchanger.TryCreatePropertySingleValue(ifcSpace, new DecimalAttributeValue { Value = spaceType.GrossArea }, "SpaceGrossAreaValue", Exchanger.DefaultAreaUnit);
                Exchanger.TryCreatePropertySingleValue(ifcSpace, new DecimalAttributeValue { Value = spaceType.NetArea }, "SpaceNetAreaValue", Exchanger.DefaultAreaUnit);
                Exchanger.TryCreatePropertySingleValue(ifcSpace, spaceType.RoomTag, "SpaceSignageName");
                Exchanger.TryCreatePropertySingleValue(ifcSpace, new DecimalAttributeValue { Value = spaceType.UsableHeight }, "SpaceUsableHeightValue", Exchanger.DefaultAreaUnit);

                #region Attributes
                if (spaceType.Attributes != null)
                {
                    foreach (var attribute in spaceType.Attributes)
                        Exchanger.ConvertAttributeTypeToIfcObjectProperty(ifcSpace, attribute);
                }
                #endregion

                #region Documents
                if (spaceType.Documents != null && spaceType.Documents.Any())
                {
                    Exchanger.ConvertDocumentsToDocumentSelect(ifcSpace, spaceType.Documents);
                }
                #endregion
            }
            return ifcSpace;
        }
    }
}
