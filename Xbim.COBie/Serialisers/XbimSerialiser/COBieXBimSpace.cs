using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.XbimExtensions.Transactions;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.QuantityResource;
using Xbim.IO;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimSpace : COBieXBim
    {
        #region Properties
        public Dictionary<string, IfcBuildingStorey> Floors { get; set; }
        #endregion
        
        public COBieXBimSpace(COBieXBimContext xBimContext)
            : base(xBimContext)
        {
            Floors = new Dictionary<string, IfcBuildingStorey>();
        }
        
        #region Methods
        // <summary>
        /// Create and setup objects held in the Space COBieSheet
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieSpaceRows to read data from</param>
        public void SerialiseSpace(COBieSheet<COBieSpaceRow> cOBieSheet)
        {
            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Space"))
            {
                try
                {

                    int count = 1;
                    ProgressIndicator.ReportMessage("Starting Spaces...");
                    ProgressIndicator.Initialise("Creating Spaces", cOBieSheet.RowCount);
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        BumpTransaction(trans, count);
                        count++;
                        ProgressIndicator.IncrementAndUpdate();
                        COBieSpaceRow row = cOBieSheet[i]; 
                        AddSpace(row);
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
        /// Add the data to the Space object
        /// </summary>
        /// <param name="row">COBieSpaceRow holding the data</param>
        private void AddSpace(COBieSpaceRow row)
        {
            //we are merging so check for an existing item name, assume the same item as should be the same building
            if (CheckIfExistOnMerge<IfcSpace>(row.Name))
            {
                return;//we have it so no need to create
            }
            
            IfcSpace ifcSpace = Model.Instances.New<IfcSpace>();
            //Set the CompositionType to Element as it is a required field
            ifcSpace.CompositionType = IfcElementCompositionEnum.ELEMENT;

            //Add Created By, Created On and ExtSystem to Owner History. 
            SetUserHistory(ifcSpace, row.ExtSystem, row.CreatedBy, row.CreatedOn);
            
            //using statement will set the Model.OwnerHistoryAddObject to ifcSpace.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
            using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcSpace.OwnerHistory))
            {
                //Add Name
                if (ValidateString(row.Name)) ifcSpace.Name = row.Name;

                //Add Category
                AddCategory(row.Category, ifcSpace);

                //Add Space to the Building Story(Floor)
                AddSpaceToBuildingStory(row.FloorName, ifcSpace);

                //Add Description
                if (ValidateString(row.Description)) ifcSpace.Description = row.Description;

                //Add GlobalId
                AddGlobalId(row.ExtIdentifier, ifcSpace);

                //Add Room Tag
                if (ValidateString(row.RoomTag)) AddPropertySingleValue(ifcSpace, "Pset_SpaceCommon", "Space Properties From COBie", "RoomTag", "Room Id", new IfcLabel(row.RoomTag));

                //Add Usable Height
                AddUsableHeight(ifcSpace, row.UsableHeight);

                //Add gross Floor Area
                AddGrossArea(ifcSpace, row.GrossArea);

                //Add Net Floor Area
                AddNetArea(ifcSpace, row.NetArea);
            }
            

        }

        /// <summary>
        /// Add Net Floor Area
        /// </summary>
        /// <param name="ifcSpace">IfcSpace Object</param>
        /// <param name="areaValue">Area value as string</param>
        private void AddNetArea(IfcSpace ifcSpace, string areaValue)
        {
            if (ValidateString(areaValue)) //ifcSpace.AddQuantity()
            {
                double? area = GetDoubleFromString(areaValue);
                if (area != null)
                {
                    IfcQuantityArea ifcQuantityArea = Model.Instances.New<IfcQuantityArea>(qa => { qa.Name = "NetFloorArea"; qa.Description = "Net Floor Area"; qa.AreaValue = new IfcAreaMeasure((double)area); });
                    string appname = "";
                    if (ifcSpace.OwnerHistory.OwningApplication != null)
                        appname = ifcSpace.OwnerHistory.OwningApplication.ApplicationFullName.ToString();

                    ifcSpace.AddQuantity("BaseQuantities", ifcQuantityArea, appname);
                }
            }
        }

        /// <summary>
        /// Add Gross Floor Area
        /// </summary>
        /// <param name="ifcSpace">IfcSpace Object</param>
        /// <param name="areaValue">Area value as string</param>
        private void AddGrossArea(IfcSpace ifcSpace, string areaValue)
        {
            if (ValidateString(areaValue)) //ifcSpace.AddQuantity()
            {
                double? area = GetDoubleFromString(areaValue);
                if (area != null)
                {
                    IfcQuantityArea ifcQuantityArea = Model.Instances.New<IfcQuantityArea>(qa => { qa.Name = "GrossFloorArea"; qa.Description = "Gross Floor Area"; qa.AreaValue = new IfcAreaMeasure((double)area); });
                    string appname = "";
                    if (ifcSpace.OwnerHistory.OwningApplication != null)
                        appname = ifcSpace.OwnerHistory.OwningApplication.ApplicationFullName.ToString();
                    

                    ifcSpace.AddQuantity("BaseQuantities", ifcQuantityArea, appname);
                }
            }
        }

        /// <summary>
        /// Add Usable Height to the IfcSpace
        /// </summary>
        /// <param name="ifcSpace">IfcSpace Object</param>
        /// <param name="usableHeight">Height value as string</param>
        private void AddUsableHeight(IfcSpace ifcSpace, string usableHeight)
        {
            if (ValidateString(usableHeight))
            {
                double? height = GetDoubleFromString(usableHeight);
                if (height != null)
                {
                    AddPropertySingleValue(ifcSpace, "Pset_SpaceCommon", null, "UsableHeight", "Space Usable Height", new IfcLengthMeasure((double)height));
                }

            }
        }

        /// <summary>
        /// Add space to the building story(Floor)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="ifcSpace"></param>
        private void AddSpaceToBuildingStory(string floorName, IfcSpace ifcSpace)
        {
            if (ValidateString(floorName))
            {
                IfcBuildingStorey spaceBuildingStory = null;
                if (Floors.ContainsKey(floorName))
                {
                    spaceBuildingStory = Floors[floorName];
                }
                else
                {
                    spaceBuildingStory = Model.Instances.OfType<IfcBuildingStorey>().Where(bs => bs.Name.ToString().ToLower().Trim() == floorName.ToLower().Trim()).FirstOrDefault();
                    if (spaceBuildingStory != null)
                        Floors.Add(floorName, spaceBuildingStory);
                }
                if (spaceBuildingStory != null)
                    spaceBuildingStory.AddToSpatialDecomposition(ifcSpace);
            }
        }

        
        #endregion
    }
}
