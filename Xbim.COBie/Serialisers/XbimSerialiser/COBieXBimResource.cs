using System;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.ConstructionMgmtDomain;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimResource : COBieXBim
    {
        public COBieXBimResource(COBieXBimContext xBimContext)
            : base(xBimContext)
        {
        }
        
        /// <summary>
        /// Add the IfcConstructionEquipmentResource to the Model object
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieResourceRow to read data from</param>
        public void SerialiseResource(COBieSheet<COBieResourceRow> cOBieSheet)
        {

            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Resource"))
            {
                try
                {
                    int count = 1;
                    ProgressIndicator.ReportMessage("Starting Resources...");
                    ProgressIndicator.Initialise("Creating Resources", cOBieSheet.RowCount);
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        BumpTransaction(trans, count);
                        count++;
                        ProgressIndicator.IncrementAndUpdate();
                        COBieResourceRow row = cOBieSheet[i];
                        AddResource(row);
                    }
                    ProgressIndicator.Finalise();
                    trans.Commit();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Add the data to the IfcConstructionEquipmentResource object
        /// </summary>
        /// <param name="row">COBieResourceRow holding the data</param>
        private void AddResource(COBieResourceRow row)
        {
            //we are merging so check for an existing item name, assume the same item as should be the same building
            if (CheckIfExistOnMerge<IfcConstructionEquipmentResource>(row.Name))
            {
                return;//we have it so no need to create
            }
            IfcConstructionEquipmentResource ifcConstructionEquipmentResource = Model.Instances.New<IfcConstructionEquipmentResource>();
            
            //Add Created By, Created On and ExtSystem to Owner History. 
            SetUserHistory(ifcConstructionEquipmentResource, row.ExtSystem, row.CreatedBy, row.CreatedOn);
            
            //using statement will set the Model.OwnerHistoryAddObject to ifcConstructionEquipmentResource.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
            using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcConstructionEquipmentResource.OwnerHistory))
            {
                //Add Name
                if (ValidateString(row.Name)) ifcConstructionEquipmentResource.Name = row.Name;

                //Add Category
                if (ValidateString(row.Category)) ifcConstructionEquipmentResource.ObjectType = row.Category;

                //Add GlobalId
                AddGlobalId(row.ExtIdentifier, ifcConstructionEquipmentResource);

                //add Description
                if (ValidateString(row.Description)) ifcConstructionEquipmentResource.Description = row.Description;
            }
        }
        
    }
}
