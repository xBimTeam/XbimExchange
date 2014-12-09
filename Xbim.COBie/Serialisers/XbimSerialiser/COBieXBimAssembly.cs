using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.XbimExtensions.Transactions;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.MaterialResource;
using Xbim.IO;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimAssembly : COBieXBim
    {
        #region Fields
        private  IEnumerable<IfcElement> IfcElements { get; set; }
        private IEnumerable<IfcTypeObject> IfcTypeObjects { get; set; }
        private IEnumerable<IfcMaterialLayer> IfcMaterialLayers { get; set; }
        private IfcRelDecomposes LastIfcRelDecomposes { get; set; }
        private IfcMaterialLayerSet LastIfcMaterialLayerSet { get; set; }
        private COBieAssemblyRow LastRow { get; set; }
        #endregion
        
        public COBieXBimAssembly(COBieXBimContext xBimContext)
            : base(xBimContext)
        {
            
        }

        /// <summary>
        /// Add the IfcPersonAndOrganizations to the Model object
        /// </summary>
        /// <param name="cOBieSheet"></param>
        public void SerialiseAssembly(COBieSheet<COBieAssemblyRow> cOBieSheet)
        {

            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Assembly"))
            {
                try
                {
                    int count = 1;
                    IfcElements = Model.Instances.OfType<IfcElement>();
                    IfcTypeObjects = Model.Instances.OfType<IfcTypeObject>();
                    IfcMaterialLayers = Model.Instances.OfType<IfcMaterialLayer>();

                    ProgressIndicator.ReportMessage("Starting Assemblies...");
                    ProgressIndicator.Initialise("Creating Assemblies", cOBieSheet.RowCount);
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        BumpTransaction(trans, count);
                        count++;
                        ProgressIndicator.IncrementAndUpdate();
                        COBieAssemblyRow row = cOBieSheet[i];
                        string objType = row.ExtObject.ToLower().Trim();
                        if (objType.Contains("ifcmaterial"))
                            AddMaterial(row);
                        else
                            AddAssembly(row);
                        
                        LastRow = row;
                        

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
        /// Add the data to the IfcMaterialLayerSet object
        /// </summary>
        /// <param name="row">COBieAssemblyRow holding the data</param>
        private void AddMaterial(COBieAssemblyRow row)
        {
            //check we have a chance of creating the IfcMaterialLayerSet object
            if (ValidateString(row.ParentName)) // && (ValidateString(row.ChildNames))
            {
                IfcMaterialLayerSet ifcMaterialLayerSet = null;
                IfcMaterialLayerSetUsage ifcMaterialLayerSetUsage = null;
                IfcRelAssociatesMaterial ifcRelAssociatesMaterial = null;
                IfcBuildingElementProxy ifcBuildingElementProxy = null;

                if ((LastIfcMaterialLayerSet != null) && IsContinuedMaterialRow(row)) //this row line is a continuation of objects from the line above
                    ifcMaterialLayerSet = LastIfcMaterialLayerSet;
                else
                {
                    ifcMaterialLayerSet = Model.Instances.Where<IfcMaterialLayerSet>(mls => mls.LayerSetName == row.ParentName).FirstOrDefault();
                    if (ifcMaterialLayerSet == null)
                        ifcMaterialLayerSet = Model.Instances.New<IfcMaterialLayerSet>(mls => { mls.LayerSetName = row.ParentName; });

                    ifcMaterialLayerSetUsage = Model.Instances.Where<IfcMaterialLayerSetUsage>(mlsu => mlsu.ForLayerSet == ifcMaterialLayerSet).FirstOrDefault();
                    if (ifcMaterialLayerSetUsage == null)
                        ifcMaterialLayerSetUsage = Model.Instances.New<IfcMaterialLayerSetUsage>(mlsu => { mlsu.ForLayerSet = ifcMaterialLayerSet; });
                    
                    string placeholderText = "Place holder for material layer Set " + row.ParentName;
                    ifcBuildingElementProxy = Model.Instances.Where<IfcBuildingElementProxy>(bep =>  bep.Name == placeholderText).FirstOrDefault();
                    if (ifcBuildingElementProxy == null)
                        ifcBuildingElementProxy = Model.Instances.New<IfcBuildingElementProxy>(bep => { bep.Name = placeholderText; });
                    ifcRelAssociatesMaterial = Model.Instances.Where<IfcRelAssociatesMaterial>(ras => ((ras.RelatingMaterial as IfcMaterialLayerSetUsage)  == ifcMaterialLayerSetUsage) ).FirstOrDefault();
                    if (ifcRelAssociatesMaterial == null)
                    {
                        ifcRelAssociatesMaterial = Model.Instances.New<IfcRelAssociatesMaterial>(ras =>
                                                    {
                                                        ras.RelatingMaterial = ifcMaterialLayerSetUsage;
                                                        ras.RelatedObjects.Add(ifcBuildingElementProxy);
                                                    });

                        //Add Created By, Created On and ExtSystem to Owner History. 
                        SetUserHistory(ifcRelAssociatesMaterial, row.ExtSystem, row.CreatedBy, row.CreatedOn);
                    }
                }

                //add the child objects
                AddChildObjects(ifcMaterialLayerSet, row.ChildNames);
                LastIfcMaterialLayerSet = ifcMaterialLayerSet;
            }
        }

        private bool IsContinuedMaterialRow(COBieAssemblyRow row)
        {
            if (ValidateString(row.Name))
            {
                if (row.Name.Contains(" : continued "))//our flag for a continued assembly child list
                    return true;

                if (ValidateString(row.ChildNames))
                {
                    string name = row.Name.ToLower().Trim();
                    string lastname = LastRow.Name.ToLower().Trim();
                    lastname = RemPostFixNumber(lastname);

                    if (name.Contains(lastname))
                    {
                        //test to see if ChildName row as a whole text finds a material, if so then a sing material on the row, so assume the material layer set is listed per row, and not in the ChildName field as a delimited string
                        string test = row.ChildNames.ToLower().Trim();
                        IfcMaterialLayer ifcMaterialLayer = IfcMaterialLayers.Where(ml => (ml.Material.Name != null) && (ml.Material.Name.ToString().ToLower().Trim() == test)).FirstOrDefault();
                        if (ifcMaterialLayer != null)
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Add the data to the IfcRelDecomposes object
        /// </summary>
        /// <param name="row">COBieAssemblyRow holding the data</param>
        private void AddAssembly(COBieAssemblyRow row)
        {
            //check we have a chance of creating the IfcRelDecomposes object
            if ((ValidateString(row.ParentName)) && (ValidateString(row.ChildNames)))
            {
                IfcRelDecomposes ifcRelDecomposes = null;

                if ((LastIfcRelDecomposes != null) && IsContinuedAssemblyRow(row)) //this row line is a continuation of objects from the line above
                {
                    ifcRelDecomposes = LastIfcRelDecomposes;
                }
                else
                {
                    IfcObjectDefinition relatingObject = GetParentObject(row.ParentName);
                    //check on merge we have not already created using name and parent object as check
                    if (ValidateString(row.Name))
                    {
                        string testName = row.Name.ToLower().Trim();
                        ifcRelDecomposes = Model.Instances.Where<IfcRelDecomposes>(rc => (rc.Name.ToString().ToLower().Trim() == testName) && (rc.RelatingObject == relatingObject)).FirstOrDefault();
                    }

                    if ((ifcRelDecomposes == null) && (relatingObject != null))
                    {
                        if (row.ExtObject.ToLower().Trim() == "ifcrelnests")
                            ifcRelDecomposes = Model.Instances.New<IfcRelNests>();
                        else
                            ifcRelDecomposes = Model.Instances.New<IfcRelAggregates>();


                        //Add Created By, Created On and ExtSystem to Owner History. 
                        SetUserHistory(ifcRelDecomposes, row.ExtSystem, row.CreatedBy, row.CreatedOn);
                    }
                    if (relatingObject == null)
                    {
                        Console.WriteLine(string.Format("Failed to find ifcRelDecomposes parent object in AddAssembly() for {0}", row.Name.ToString()));
                        return;
                    }
                }

                

                //using statement will set the Model.OwnerHistoryAddObject to IfcConstructionProductResource.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
                //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
                using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcRelDecomposes.OwnerHistory))
                {
                    if (ValidateString(row.Name)) ifcRelDecomposes.Name = row.Name;
                    if (ValidateString(row.Description)) ifcRelDecomposes.Description = row.Description;

                    //Add GlobalId
                    AddGlobalId(row.ExtIdentifier, ifcRelDecomposes);
                        
                    if (! (AddParentObject(ifcRelDecomposes, row.ParentName) &&
                           AddChildObjects(ifcRelDecomposes, row.SheetName, row.ChildNames)
                           )
                        )
                    {
                        //failed to add parent or child so remove as not a valid IfcRelDecomposes object
                        try
                        {
                            Model.Delete(ifcRelDecomposes);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("Failed to delete ifcRelDecomposes in AddAssembly() - {0}", ex.Message));
                            
                        }
                        ifcRelDecomposes = null;
                    }
                    
                }
                //save for next row, might be a continuation line
                LastIfcRelDecomposes = ifcRelDecomposes;
            }
        }

        private bool IsContinuedAssemblyRow(COBieAssemblyRow row)
        {
            if (ValidateString(row.Name))
            {
		        if (row.Name.Contains(" : continued ") )//our flag for a continued assembly child list
                    return true;

                if (ValidateString(row.ChildNames))
                {
                    string name = row.Name.ToLower().Trim();
                    string lastname = LastRow.Name.ToLower().Trim();
                    lastname = RemPostFixNumber(lastname);

                    if (name.Contains(lastname))
                    {
                        //this is a bit messy but gets over the placement of : on single entries
                        IEnumerable<IfcObjectDefinition> childObjs = GetSheetObjectList(row.SheetName);
                        //check that the name is not a single element, also gets over single entries with : in them
                        string test = row.ChildNames.ToLower().Trim();
                        IfcObjectDefinition RelatedObject = childObjs.Where(obj => obj.Name.ToString().ToLower().Trim() == test).FirstOrDefault();
                        if (RelatedObject != null)
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Remove the post fix numbers on a string
        /// </summary>
        /// <param name="str">string to process</param>
        /// <returns>String with post fix numbers removed</returns>
        private string RemPostFixNumber (string str)
        {
            for (int i = (str.Length - 1); i >= 0 ; i--)
			{
                if (char.IsDigit(str[i]))
                    str = str.Remove(i);
                else
                    break;
            }
            return str;
        }

        

        
        

        /// <summary>
        /// Add the parent objects to the IfcRelDecomposes
        /// </summary>
        /// <param name="ifcRelDecomposes">Either a IfcRelAggregates or IfcRelNests object</param>
        /// <param name="parentName">IfcObjectDefinition.Name value to search for, NOT case sensitive</param>
        /// <returns></returns>
        private bool AddParentObject(IfcRelDecomposes ifcRelDecomposes, string parentName)
        {
            IfcObjectDefinition relatingObject = GetParentObject(parentName);

            if (relatingObject != null)
            {
                ifcRelDecomposes.RelatingObject = relatingObject;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get the parent object for the ifcRelDecomposes object
        /// </summary>
        /// <param name="parentName">Math the Name property with this string</param>
        /// <returns>IfcObjectDefinition </returns>
        private IfcObjectDefinition GetParentObject(string parentName)
        {
            string name = parentName.ToLower().Trim();
            IfcObjectDefinition RelatingObject = IfcElements.Where(obj => obj.Name.ToString().ToLower().Trim() == name).FirstOrDefault();
            if (RelatingObject == null) //try IfcTypeObjects
                RelatingObject = IfcTypeObjects.Where(obj => obj.Name.ToString().ToLower().Trim() == name).FirstOrDefault();
            return RelatingObject;
        }

        /// <summary>
        /// Add the child objects to the IfcRelDecomposes
        /// </summary>
        /// <param name="ifcRelDecomposes">Either a IfcRelAggregates or IfcRelNests object</param>
        /// <param name="sheetName">SheetName the children come from</param>
        /// <param name="childNames">list of child object names separated by " : ", NOT case sensitive</param>
        private bool AddChildObjects(IfcRelDecomposes ifcRelDecomposes, string sheetName, string childNames)
        {
            bool returnValue = false;
            IEnumerable<IfcObjectDefinition> childObjs  = GetSheetObjectList(sheetName);
            //check that the name is not a single element, also gets over single entries with : in them
            string test = childNames.ToLower().Trim();
            IfcObjectDefinition RelatedObject = childObjs.Where(obj => obj.Name.ToString().ToLower().Trim() == test).FirstOrDefault();
            if (RelatedObject != null)
            {
                //check we have not already added as this can be a merge
                if (!ifcRelDecomposes.RelatedObjects.Contains(RelatedObject))
                {
                    ifcRelDecomposes.RelatedObjects.Add(RelatedObject);
                }
                returnValue = true;
            }
            else //ok nothing found for the full string so assume delimited string
            {
                char splitChar = GetSplitChar(childNames);
                List<string> splitChildNames = SplitString(childNames, splitChar);

                foreach (string item in splitChildNames)
                {
                    string name = item.ToLower().Trim();
                    RelatedObject = childObjs.Where(obj => obj.Name.ToString().ToLower().Trim() == name).FirstOrDefault();
                    if (RelatedObject != null)
                    {
                        //check we have not already added as this can be a merge
                        if (!ifcRelDecomposes.RelatedObjects.Contains(RelatedObject))
                        {
                            ifcRelDecomposes.RelatedObjects.Add(RelatedObject);
                        }
                        returnValue = true;
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Get the ObjectDefinitions related to the sheet
        /// </summary>
        /// <param name="sheetName">Sheet name we are wanting the objects for</param>
        /// <returns>IEnumerable of IfcObjectDefinition</returns>
        private IEnumerable<IfcObjectDefinition> GetSheetObjectList(string sheetName)
        {
            IEnumerable<IfcObjectDefinition> childObjs;
            if (sheetName.ToLower() == Constants.WORKSHEET_COMPONENT.ToLower())
                childObjs = IfcElements;
            else //if not components then it should by Type
                childObjs = IfcTypeObjects;
            return childObjs;
        }

        

        /// <summary>
        /// Add the child objects to the IfcMaterialLayerSet
        /// </summary>
        /// <param name="ifcMaterialLayerSet">IfcMaterialLayerSet object</param>
        /// <param name="childNames">list of child object names separated by " : ", NOT case sensitive</param>
        private bool AddChildObjects(IfcMaterialLayerSet ifcMaterialLayerSet, string childNames)
        {
            bool returnValue = false;
            List<string> splitChildNames = SplitString(childNames, ':');

            foreach (string item in splitChildNames)
            {
                string name = item;
                double? thickness = GetLayerThickness(name);
                name = GetMaterialName(name).ToLower().Trim();
                IfcMaterialLayer ifcMaterialLayer = null;
                    
                IEnumerable<IfcMaterialLayer> ifcMaterialLayers = IfcMaterialLayers.Where(ml => (ml.Material.Name != null) && (ml.Material.Name.ToString().ToLower().Trim() == name));
                if ((ifcMaterialLayers.Any()) && 
                    (ifcMaterialLayers.Count() > 1) &&
                    (thickness != null)
                    )
                {
                    ifcMaterialLayer = ifcMaterialLayers.Where(ml => ml.LayerThickness == thickness).FirstOrDefault();
                }
                else
                {
                    ifcMaterialLayer = ifcMaterialLayers.FirstOrDefault();
                }

                if (ifcMaterialLayer != null)
                {
                    if (!ifcMaterialLayerSet.MaterialLayers.Contains(ifcMaterialLayer))
                        ifcMaterialLayerSet.MaterialLayers.Add(ifcMaterialLayer);
                    returnValue = true;
                }
            }
            return returnValue;
        }

        
    }
}
