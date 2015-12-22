using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.ConstructionMgmtDomain;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimSpare : COBieXBim
    {

        #region Properties
        public IEnumerable<IfcTypeObject> IfcTypeObjects { get; private set; }
        #endregion

        public COBieXBimSpare(COBieXBimContext xBimContext)
            : base(xBimContext)
        {

        }

        /// <summary>
        /// Add the IfcConstructionProductResource to the Model object
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieSpareRow to read data from</param>
        public void SerialiseSpare(COBieSheet<COBieSpareRow> cOBieSheet)
        {

            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Spare"))
            {
                try
                {
                    int count = 1;
                    IfcTypeObjects = Model.Instances.OfType<IfcTypeObject>();

                    ProgressIndicator.ReportMessage("Starting Spares...");
                    ProgressIndicator.Initialise("Creating Spares", cOBieSheet.RowCount);
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        BumpTransaction(trans, count);
                        count++;
                        ProgressIndicator.IncrementAndUpdate();
                        COBieSpareRow row = cOBieSheet[i];
                        AddSpare(row);
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
        /// Add the data to the IfcConstructionProductResource object
        /// </summary>
        /// <param name="row">COBieSpareRow holding the data</param>
        private void AddSpare(COBieSpareRow row)
        {
            //we are merging so check for an existing item name, assume the same item as should be the same building
            if (CheckIfExistOnMerge<IfcConstructionProductResource>(row.Name))
            {
                return;//we have it so no need to create
            }

            IfcConstructionProductResource ifcConstructionProductResource = Model.Instances.New<IfcConstructionProductResource>();
            
            //Add Created By, Created On and ExtSystem to Owner History. 
            SetUserHistory(ifcConstructionProductResource, row.ExtSystem, row.CreatedBy, row.CreatedOn);
            
            //using statement will set the Model.OwnerHistoryAddObject to IfcConstructionProductResource.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
            using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcConstructionProductResource.OwnerHistory)) 
            {
                //Add Name
                if (ValidateString(row.Name)) ifcConstructionProductResource.Name = row.Name;

                //Add Category
                AddCategory(row.Category, ifcConstructionProductResource);

                //Add Type Relationship
                if (ValidateString(row.TypeName))
                {
                    List<string> typeNames = SplitString(row.TypeName, ':');
                    IEnumerable<IfcTypeObject> ifcTypeObjects = IfcTypeObjects.Where(to => typeNames.Contains(to.Name.ToString().Trim()));
                    SetRelAssignsToResource(ifcConstructionProductResource, ifcTypeObjects);
                }
                //Add GlobalId
                AddGlobalId(row.ExtIdentifier, ifcConstructionProductResource);

                //add Description
                if (ValidateString(row.Description)) ifcConstructionProductResource.Description = row.Description;

                if (ValidateString(row.Suppliers))
                    AddPropertySingleValue(ifcConstructionProductResource, "Pset_Spare_COBie", "Spare Properties From COBie", "Suppliers", "Suppliers Contact Details", new IfcLabel(row.Suppliers));

                if (ValidateString(row.SetNumber))
                    AddPropertySingleValue(ifcConstructionProductResource, "Pset_Spare_COBie", null, "SetNumber", "Set Number", new IfcLabel(row.SetNumber));

                if (ValidateString(row.PartNumber))
                    AddPropertySingleValue(ifcConstructionProductResource, "Pset_Spare_COBie", null, "PartNumber", "Part Number", new IfcLabel(row.PartNumber));
                
            }
        }

        /// <summary>
        /// Create the relationships between the Resource and the types it relates too
        /// </summary>
        /// <param name="processObj">IfcResource Object</param>
        /// <param name="typeObjs">IEnumerable of IfcTypeObject, list of IfcTypeObjects </param>
        public void SetRelAssignsToResource(IfcResource resourceObj, IEnumerable<IfcTypeObject> typeObjs)
        {
            //find any existing relationships to this type
            IfcRelAssignsToResource processRel = Model.Instances.Where<IfcRelAssignsToResource>(rd => rd.RelatingResource == resourceObj).FirstOrDefault();
            if (processRel == null) //none defined create the relationship
            {
                processRel = Model.Instances.New<IfcRelAssignsToResource>();
                processRel.RelatingResource = resourceObj;
                
                
            }
            //add the type objects
            foreach (IfcTypeObject type in typeObjs)
            {
                processRel.RelatedObjects.Add(type);
            }
        }

    }


}
