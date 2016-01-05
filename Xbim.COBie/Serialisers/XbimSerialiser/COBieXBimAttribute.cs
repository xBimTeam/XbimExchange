using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.ConstructionMgmtDomain;
using Xbim.IO.Esent;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimAttribute : COBieXBim
    {

        #region Properties
        public IEnumerable<IfcBuildingStorey> IfcBuildingStoreys { get; private set; }
        public IEnumerable<IfcSpace> IfcSpaces { get; private set; }
        public IEnumerable<IfcTypeObject> IfcTypeObjects { get; private set; }
        public IEnumerable<IfcElement> IfcElements { get; private set; }
        public IEnumerable<IfcZone> IfcZones { get; private set; }
        public IEnumerable<IfcBuilding> IfcBuildings { get; private set; }
        public IEnumerable<IfcConstructionProductResource> IfcConstructionProductResources { get; private set; }
        
        private IfcObjectDefinition CurrentObject { get; set; }
        #endregion

        public COBieXBimAttribute(COBieXBimContext xBimContext)
            : base(xBimContext)
        {
            CurrentObject = null;
            
        }

        #region Methods

        /// <summary>
        /// Add Properties back to component and type objects
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieAttributeRow to read data from</param>
        public void SerialiseAttribute(COBieSheet<COBieAttributeRow> cOBieSheet)
        {
            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Attribute"))
            {
                try
                {
                    int count = 1;
                    var sortedRows = cOBieSheet.Rows.OrderBy(r => r.SheetName).ThenBy(r => r.RowName);
                    ProgressIndicator.ReportMessage("Starting Attributes...");
                    ProgressIndicator.Initialise("Creating Attributes", cOBieSheet.RowCount);
                    foreach (COBieAttributeRow row in sortedRows)
                    {
                        BumpTransaction(trans, count);
                        count++;
                        ProgressIndicator.IncrementAndUpdate();
                        AddAttribute(row);
                        
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
        /// Add the properties to the row object
        /// </summary>
        /// <param name="row">COBieAttributeRow holding the data</param>
        private void AddAttribute(COBieAttributeRow row)
        {
            //need a sheet and a row to be able to attach property to an object
            if ((ValidateString(row.RowName)) && (ValidateString(row.SheetName)))
            {
                switch (row.SheetName.ToLower())
                {
                    case "facility":
                        //set list if first time
                        if (IfcBuildings == null) IfcBuildings = Model.Instances.OfType<IfcBuilding>();
                        if (!((CurrentObject is IfcBuilding) && (CurrentObject.Name == row.RowName)))
                            CurrentObject = IfcBuildings.Where(b => b.Name.ToString().ToLower() == row.RowName.ToLower()).FirstOrDefault();
                        break;
                    case "floor":
                        if (IfcBuildingStoreys == null) IfcBuildingStoreys = Model.Instances.OfType<IfcBuildingStorey>();
                        if (!((CurrentObject is IfcBuildingStorey) && (CurrentObject.Name == row.RowName)))
                            CurrentObject = IfcBuildingStoreys.Where(b => b.Name.ToString().ToLower() == row.RowName.ToLower()).FirstOrDefault();
                        break;
                    case "space":
                        if (IfcSpaces == null) IfcSpaces = Model.Instances.OfType<IfcSpace>();
                        if (!((CurrentObject is IfcSpace) && (CurrentObject.Name == row.RowName)))
                            CurrentObject = IfcSpaces.Where(b => b.Name.ToString().ToLower() == row.RowName.ToLower()).FirstOrDefault();
                        break;
                    case "type":
                        if (IfcTypeObjects == null) IfcTypeObjects = Model.Instances.OfType<IfcTypeObject>();
                        if (!((CurrentObject is IfcTypeObject) && (CurrentObject.Name == row.RowName)))
                            CurrentObject = IfcTypeObjects.Where(b => b.Name.ToString().ToLower() == row.RowName.ToLower()).FirstOrDefault();
                        break;
                    case "spare":
                        if (IfcConstructionProductResources == null) IfcConstructionProductResources = Model.Instances.OfType<IfcConstructionProductResource>();
                        if (!((CurrentObject is IfcConstructionProductResource) && (CurrentObject.Name == row.RowName)))
                            CurrentObject = IfcConstructionProductResources.Where(b => b.Name.ToString().ToLower() == row.RowName.ToLower()).FirstOrDefault();
                        break;
                    case "component":
                        if (IfcElements == null) IfcElements = Model.Instances.OfType<IfcElement>();
                        if (!((CurrentObject is IfcElement) && (CurrentObject.Name == row.RowName)))
                            CurrentObject = IfcElements.Where(b => b.Name.ToString().ToLower() == row.RowName.ToLower()).FirstOrDefault();
                        break;
                    case "zone":
                        if (IfcZones == null) IfcZones = Model.Instances.OfType<IfcZone>();
                        if (!((CurrentObject is IfcZone) && (CurrentObject.Name == row.RowName)))
                            CurrentObject = IfcZones.Where(b => b.Name.ToString().ToLower() == row.RowName.ToLower()).FirstOrDefault();
                        break;
                    default:
                        CurrentObject = null;
                        break;
                }
               
                if (CurrentObject != null)
                {
                    if (ValidateString(row.Name)) 
                    {
                        IfcPropertySet ifcPropertySet = CheckIfExistOnMerge(row.ExtObject, row.ExtIdentifier);

                        if (ifcPropertySet == null)
                        {
                            return;
                        }

                        //Set Description
                        string description = "";
                        if (ValidateString(row.Description))
                            description = row.Description;


                        if ((ValidateString(row.Value)) &&
                            row.Value.Contains(":") &&
                            row.Value.Contains("(") &&
                            row.Value.Contains(")") 
                            )//only if we have a IfcPropertyTableValue defined by COBieDataAttributeBuilder
                        {
                            AddPropertyTableValue(ifcPropertySet , row.Name, description, row.Value, row.AllowedValues, row.Unit);
                        }
                        else if ((ValidateString(row.AllowedValues)) &&
                                //row.Value.Contains(":") && can be single value
                                (row.AllowedValues.Contains(":") ||
                                row.AllowedValues.Contains(",")
                                )
                                )//have a IfcPropertyEnumeratedValue
                        {
                            IfcValue[] ifcValues = GetValueArray(row.Value);
                            IfcValue[] ifcValueEnums = GetValueArray(row.AllowedValues);
                            IfcUnit ifcUnit = GetIfcUnit(row.Unit);
                            AddPropertyEnumeratedValue(ifcPropertySet, row.Name, description, ifcValues, ifcValueEnums, ifcUnit);
                        }
                        else
                        {
                            IfcValue ifcValue;
                            double number;
                            if (double.TryParse(row.Value, out number))
                                ifcValue = new IfcReal((double)number);
                            else if (ValidateString(row.Value))
                                ifcValue = new IfcLabel(row.Value);
                            else
                                ifcValue = new IfcLabel("");
                            IfcUnit ifcUnit = GetIfcUnit(row.Unit);
                            AddPropertySingleValue(ifcPropertySet, row.Name, description, ifcValue, ifcUnit);
                        }

                        //Add Category****
                        if (ValidateString(row.Category))
                        {
                            SetCategory(ifcPropertySet, row.Category);
                        }

                        //****************Note need this as last call Add OwnerHistory*************
                        if (ifcPropertySet != null)
                        {
                            //Add Created By, Created On and ExtSystem to Owner History. 
                            SetUserHistory(ifcPropertySet, row.ExtSystem, row.CreatedBy, row.CreatedOn);
                        }
                        //****************Note need SetOwnerHistory above to be last call, as XBim changes to default on any property set or changed, cannot use edit context as property set used more than once per row******
                    }
                    else
                    {
 #if DEBUG
                        Console.WriteLine("Failed to create attribute. No name : {0} value {1}", row.Name, row.ExtObject);
#endif
                    }
                    
                }
                else
                {
#if DEBUG
                    Console.WriteLine("Failed to create attribute. No object found to add too {0} value {1}", row.Name, row.ExtObject);
#endif
                }
                
            }
            else
            {
#if DEBUG
                Console.WriteLine("Failed to create attribute. No sheet or row name {0} value {1}", row.Name, row.ExtObject);
#endif
            }
            
        }

        

        /// <summary>
        /// Check if property set exists on object when in merge, if not merge just create property set
        /// </summary>
        /// <param name="pSetName">Property set name</param>
        /// <returns>IfcPropertySet</returns>
        private IfcPropertySet CheckIfExistOnMerge(string extObject, string extIdentifier )
        {
            string pSetName = string.Empty;
            IfcPropertySet ifcPropertySet = null;

            //check that the GlobalId is not holding the property set name
            if ((ValidateString(extIdentifier)) &&
                (!ValidGlobalId(extIdentifier))
                )
            {
                pSetName = extIdentifier;
                extIdentifier = null; //force new Guid
            } 
            else if (ValidateString(extObject)) //check we have a valid string
                pSetName = extObject;
            else
                pSetName = null;
                        
            if (CurrentObject is IfcObject)
            {
                ifcPropertySet = (CurrentObject as IfcObject).GetPropertySet(pSetName);
                if (ifcPropertySet == null)
                {
                    ifcPropertySet = AddPropertySet((IfcObject)CurrentObject, pSetName, "");
                }
            }
            else if (CurrentObject is IfcTypeObject)
            {
                ifcPropertySet = (CurrentObject as IfcTypeObject).GetPropertySet(pSetName);
                if (ifcPropertySet == null)
                {
                    ifcPropertySet = AddPropertySet((IfcTypeObject)CurrentObject, pSetName, "");
                }
            }
            //Add GlobalId

            AddGlobalId(extIdentifier, ifcPropertySet);
            return ifcPropertySet;
        }

        

        /// <summary>
        /// Set Category to the property set
        /// </summary>
        /// <param name="ifcRoot">IfcRoot Object (IfcPropertySet)</param>
        /// <param name="category">string, category Name</param>
        private void SetCategory(IfcRoot ifcRoot, string category)
        {
            IfcRelAssociatesClassification ifcRelAssociatesClassification = Model.Instances.Where<IfcRelAssociatesClassification>(r => (r.RelatingClassification is IfcClassificationReference) && ((IfcClassificationReference)r.RelatingClassification).Name.ToString().ToLower() == category.ToLower()).FirstOrDefault();
            //create if none found
            if (ifcRelAssociatesClassification == null)
            {
                ifcRelAssociatesClassification = Model.Instances.New<IfcRelAssociatesClassification>();
                IfcClassificationReference ifcClassificationReference = Model.Instances.New<IfcClassificationReference>();
                ifcClassificationReference.Name = category;
                ifcRelAssociatesClassification.RelatingClassification = ifcClassificationReference;
            }
            //add this IfcRoot object if not already associated
            if (!ifcRelAssociatesClassification.RelatedObjects.Contains(ifcRoot))
            {
                ifcRelAssociatesClassification.RelatedObjects.Add(ifcRoot);
            }
            
        }
        #endregion
    }
}
