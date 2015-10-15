using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.ConstructionMgmtDomain;

namespace XbimExchanger.COBieLiteUkToIfc
{
    public class MappingSpareToIfcConstructionProductResource : CoBieLiteUkIfcMappings<string, Spare, IfcConstructionProductResource>
    {
        protected override IfcConstructionProductResource Mapping(Spare spare, IfcConstructionProductResource ifcConstructionProductResource)
        {
            ifcConstructionProductResource.Name = spare.Name;

            Exchanger.SetUserHistory(ifcConstructionProductResource, spare.ExternalSystem, (spare.CreatedBy == null) ? null : spare.CreatedBy.Email, (spare.CreatedOn == null) ? DateTime.Now : (DateTime)spare.CreatedOn);

            //using statement will set the Model.OwnerHistoryAddObject to IfcConstructionProductResource.OwnerHistory as OwnerHistoryAddObject is used to reset user history on any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement to keep user history set in line above SetUserHistory
            using (OwnerHistoryEditScope context = new OwnerHistoryEditScope(Exchanger.TargetRepository, ifcConstructionProductResource.OwnerHistory))
            {
                #region Categories
                if (spare.Categories != null)
                {
                    foreach (var category in spare.Categories)
                    {
                        Exchanger.ConvertCategoryToClassification(category, ifcConstructionProductResource);
                    }
                }
                #endregion
                #region suppliers
                if (spare.Suppliers != null)
                {
                    var emails = string.Join(":", spare.Suppliers.Select(s => s.Email));
                    if (!string.IsNullOrEmpty(emails))
                    {
                        Exchanger.TryCreatePropertySingleValue(ifcConstructionProductResource, emails, "SpareSuppliers");
                    }
                }
                #endregion
                //description
                ifcConstructionProductResource.Description = spare.Description;
                //SetNumber
                Exchanger.TryCreatePropertySingleValue(ifcConstructionProductResource, spare.SetNumber, "SpareSetNumber");
                //PartNumber
                Exchanger.TryCreatePropertySingleValue(ifcConstructionProductResource, spare.PartNumber, "SparePartNumber");
            }
             return ifcConstructionProductResource;
        }
    }
}
