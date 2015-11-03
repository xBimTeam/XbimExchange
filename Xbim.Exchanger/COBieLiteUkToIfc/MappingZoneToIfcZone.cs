using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.COBieLiteUkToIfc
{
    class MappingZoneToIfcZone : CoBieLiteUkIfcMappings<string, Zone, IfcZone>
    {
        protected override IfcZone Mapping(Zone zone, IfcZone ifcZone)
        {
            Exchanger.SetUserHistory(ifcZone, zone.ExternalSystem, (zone.CreatedBy == null) ? null : zone.CreatedBy.Email, (zone.CreatedOn == null) ? DateTime.Now : (DateTime)zone.CreatedOn);
            using (OwnerHistoryEditScope ohContext = new OwnerHistoryEditScope(Exchanger.TargetRepository, ifcZone.OwnerHistory))
            {
                ifcZone.Name = zone.Name;
                ifcZone.Description = zone.Description;

                #region Properties



                #endregion

                #region Categories

                if (zone.Categories != null)
                    foreach (var category in zone.Categories)
                    {
                        Exchanger.ConvertCategoryToClassification(category, ifcZone);
                    }

                #endregion

                #region Attributes

                if (zone.Attributes != null)
                {

                    foreach (var attribute in zone.Attributes)
                    {
                        Exchanger.ConvertAttributeTypeToIfcObjectProperty(ifcZone, attribute);
                    }
                }
                #endregion

                #region Spaces

                if (zone.Spaces != null)
                    foreach (var spaceKey in zone.Spaces)
                    {
                        Exchanger.AddSpaceToZone(spaceKey, ifcZone);
                    }

                #endregion

                #region Documents
                if (zone.Documents != null && zone.Documents.Any())
                {
                    Exchanger.ConvertDocumentsToDocumentSelect(ifcZone, zone.Documents);
                }
                #endregion
            }
            return ifcZone;

        }
    }
}
