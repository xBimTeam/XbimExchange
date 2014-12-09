using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xbim.Ifc2x3.ProductExtension;
using Xbim.COBie.Rows;
using Xbim.XbimExtensions.Transactions;
using Xbim.Ifc2x3.Extensions;
using Xbim.IO;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimZone : COBieXBim
    {
        #region Properties
        public Dictionary<string, IfcSpace> Spaces { get; set; }
        #endregion

        public COBieXBimZone(COBieXBimContext xBimContext)
            : base(xBimContext)
        {
            Spaces = new Dictionary<string, IfcSpace>();
        }
        
        #region Methods
        // <summary>
        /// Create and setup objects held in the Zone COBieSheet
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieZoneRow to read data from</param>
        public void SerialiseZone(COBieSheet<COBieZoneRow> cOBieSheet)
        {
            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Zone"))
            {
                try
                {

                    int count = 1;
                    ProgressIndicator.ReportMessage("Starting Zones...");
                    ProgressIndicator.Initialise("Creating Zones", cOBieSheet.RowCount);
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        BumpTransaction(trans, count);
                        count++;
                        ProgressIndicator.IncrementAndUpdate();
                        COBieZoneRow row = cOBieSheet[i];
                        AddZone(row);
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
        /// Add the data to the Zone object
        /// </summary>
        /// <param name="row">COBieZoneRow holding the data</param>
        private void AddZone(COBieZoneRow row)
        {
            //we are merging so check for an existing item name, assume the same item as should be the same building
            if (CheckIfExistOnMerge<IfcZone>(row.Name))
            {
                return;//we have it so no need to create
            }
            IfcZone ifcZone = Model.Instances.New<IfcZone>();
            
            //Add Created By, Created On and ExtSystem to Owner History. 
            SetUserHistory(ifcZone, row.ExtSystem, row.CreatedBy, row.CreatedOn);
            
            //using statement will set the Model.OwnerHistoryAddObject to ifcZone.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
            using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcZone.OwnerHistory))
            {
                //Add Name
                if (ValidateString(row.Name)) ifcZone.Name = row.Name;

                //Add Category
                AddCategory(row.Category, ifcZone);

                //add space to the zone group
                string spaceNames = row.SpaceNames;
                if (ValidateString(spaceNames))
                {
                    char splitKey = GetSplitChar(spaceNames);
                    //COBieCell spaceCell = row["SpaceNames"];
                    //List<string> spaceArray = new List<string>();
                    //if (spaceCell.COBieColumn.AllowsMultipleValues)
                    //{
                    //    spaceArray = spaceCell.CellValues;
                    //}
                    List<string> spaceArray = SplitString(spaceNames, splitKey); //uses escaped characters
                    foreach (string spaceName in spaceArray)
                    {
                        AddSpaceToZone(spaceName, ifcZone);
                    }
                }
                

                //Add GlobalId
                AddGlobalId(row.ExtIdentifier, ifcZone);

                //Add Description
                if (ValidateString(row.Description)) ifcZone.Description = row.Description;

            }
        }

        /// <summary>
        /// Add space to the building story(Floor)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="ifcSpace"></param>
        private void AddSpaceToZone(string spaceName, IfcZone ifcZone)
        {
            if (ValidateString(spaceName))
            {
                spaceName = spaceName.Trim().ToLower();
                IfcSpace space = null;
                if (Spaces.ContainsKey(spaceName))
                {
                    space = Spaces[spaceName];
                }
                else
                {
                    space = Model.Instances.OfType<IfcSpace>().Where(sp => sp.Name.ToString().ToLower() == spaceName).FirstOrDefault();
                    if (space != null)
                        Spaces.Add(spaceName, space);
                }
                if (space != null)
                    ifcZone.AddObjectToGroup(space);
                    
            }
        }

        #endregion
    }
}
