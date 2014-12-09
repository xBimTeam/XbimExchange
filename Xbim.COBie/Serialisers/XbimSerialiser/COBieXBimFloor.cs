using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.XbimExtensions;
using Xbim.Ifc2x3.Extensions;
using Xbim.XbimExtensions.Transactions;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.IO;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimFloor : COBieXBim
    {
        public COBieXBimFloor(COBieXBimContext xBimContext)
            : base(xBimContext)
        {

        }

        #region Methods
        /// <summary>
        /// Create and setup objects held in the Floor COBieSheet
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieFloorRows to read data from</param>
        public void SerialiseFloor(COBieSheet<COBieFloorRow> cOBieSheet)
        {
            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Floor"))
            {

                try
                {

                    int count = 1;
                    ProgressIndicator.ReportMessage("Starting Floors...");
                    ProgressIndicator.Initialise("Creating Floors", cOBieSheet.RowCount);
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        BumpTransaction(trans, count);
                        count++;
                        ProgressIndicator.IncrementAndUpdate();
                        COBieFloorRow row = cOBieSheet[i]; 
                        AddBuildingStory(row);
                    }

                    ProgressIndicator.Finalise();
                    trans.Commit();

                }
                catch (Exception)
                {
                    //TODO: Catch with logger?
                    throw;
                }

            }
        }

        /// <summary>
        /// Add the data to the BuildingStory object
        /// </summary>
        /// <param name="row">COBieFloorRow holding the data</param>
        private void AddBuildingStory(COBieFloorRow row)
        {
            //we are merging so check for an existing item name, assume the same item as should be the same building, maybe should do a check on that
            if (CheckIfExistOnMerge<IfcBuildingStorey>(row.Name))
            {
                return;//we have it so no need to create
            }

            IfcBuildingStorey ifcBuildingStorey = Model.Instances.New<IfcBuildingStorey>();
            //Set the CompositionType to Element as it is a required field
            ifcBuildingStorey.CompositionType = IfcElementCompositionEnum.ELEMENT;

            //Add Created By, Created On and ExtSystem to Owner History. 
            SetUserHistory(ifcBuildingStorey, row.ExtSystem, row.CreatedBy, row.CreatedOn);
            
            //using statement will set the Model.OwnerHistoryAddObject to ifcBuildingStorey.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
            using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcBuildingStorey.OwnerHistory)) 
            {
                //Add Name
                if (ValidateString(row.Name)) ifcBuildingStorey.Name = row.Name;

                //Add Category
                AddCategory(row.Category, ifcBuildingStorey);

                //Add GlobalId
                AddGlobalId(row.ExtIdentifier, ifcBuildingStorey);


                //Add Elevation
                if (ValidateString(row.Elevation)) ifcBuildingStorey.Elevation = GetDoubleFromString(row.Elevation);

                //Add Floor Height
                AddFloorHeight(row.Height, ifcBuildingStorey);

                
                //add Description
                if (ValidateString(row.Description)) ifcBuildingStorey.Description = row.Description;

                //create relationship between building and building story
                GetBuilding().AddToSpatialDecomposition(ifcBuildingStorey);
            }
            
            
        }

        /// <summary>
        /// Add the Building Story (Floor) height
        /// </summary>
        /// <param name="floorHeight">the floor height</param>
        /// <param name="ifcBuildingStorey">IfcBuildingStorey object to add height property too</param>
        private void AddFloorHeight(string floorHeight, IfcBuildingStorey ifcBuildingStorey)
        {
            if (ValidateString(floorHeight))
            {
                double? height = GetDoubleFromString(floorHeight);
                if (height != null)
                {
                    //IfcPropertySingleValue ifcPropertySingleValue = ifcBuildingStorey.SetPropertySingleValue("Pset_BuildingStory_COBie", "StoreyHeight", new IfcLengthMeasure((double)height));
                    //ifcPropertySingleValue.Description = "Building Story Floor Height";
                    ////set description for the property set, nice to have but optional
                    //SetPropertySetDescription(ifcBuildingStorey, "Pset_BuildingStory_COBie", "Building Story Properties From COBie");
                    AddPropertySingleValue(ifcBuildingStorey, "Pset_BuildingStory_COBie", "Building Story Properties From COBie", "StoreyHeight", "Building Story Floor Height From Datum", new IfcLengthMeasure((double)height));
                }

            }
        }
        #endregion

    }
}
