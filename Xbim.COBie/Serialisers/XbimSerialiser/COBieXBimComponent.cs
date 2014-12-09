using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.XbimExtensions.Transactions;
using Xbim.Ifc2x3.Kernel;
using Xbim.XbimExtensions;
using System.Reflection;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.Extensions;
using Xbim.XbimExtensions.Interfaces;
using Xbim.IO;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimComponent : COBieXBim
    {

        #region Properties
        public  IEnumerable<IfcTypeObject> IfcTypeObjects { get; private set; }
        public  IEnumerable<IfcSpace> IfcSpaces { get; private set; }
        public IEnumerable<IfcBuildingStorey> IfcBuildingStoreys { get; private set; }
        
        #endregion

        public COBieXBimComponent(COBieXBimContext xBimContext)
            : base(xBimContext)
        {
            
        }

        #region Methods
        /// <summary>
        /// Create and setup objects held in the Component COBieSheet
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieComponentRow to read data from</param>
        public void SerialiseComponent(COBieSheet<COBieComponentRow> cOBieSheet)
        {
            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Component"))
            {

                try
                {
                    int count = 1;
                    IfcTypeObjects = Model.Instances.OfType<IfcTypeObject>();
                    IfcSpaces = Model.Instances.OfType<IfcSpace>();
                    IfcBuildingStoreys = Model.Instances.OfType<IfcBuildingStorey>();
                    ProgressIndicator.ReportMessage("Starting Components...");
                    ProgressIndicator.Initialise("Creating Components", cOBieSheet.RowCount);
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        BumpTransaction(trans, count);
                        count++;
                        ProgressIndicator.IncrementAndUpdate();
                        COBieComponentRow row = cOBieSheet[i]; 
                        AddComponent(row);
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
        /// Add the components and fill with data from COBieComponentRow
        /// </summary>
        /// <param name="row">COBieComponentRow holding the data</param>
        private void AddComponent(COBieComponentRow row)
        {

            //we are merging so check for an existing item name, assume the same item as should be the same building
            if (CheckIfExistOnMerge<IfcElement>(row.Name))
            {
                return;//we have it so no need to create
            }
            //we need the ExtObject to exist to create the object
            //Create object using reflection
            IfcElement ifcElement = GetElementInstance(row.ExtObject, Model);
                    
            if(ifcElement != null)
            {
                //Add Created By, Created On and ExtSystem to Owner History. 
                SetUserHistory(ifcElement, row.ExtSystem, row.CreatedBy, row.CreatedOn);
                //using statement will set the Model.OwnerHistoryAddObject to ifcElement.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
                //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
                using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcElement.OwnerHistory))
                {
                    //Add Name
                    string name = row.Name;
                    if (ValidateString(name)) ifcElement.Name = name;

                    //Add description
                    if (ValidateString(row.Description)) ifcElement.Description = row.Description;

                    //Add GlobalId
                    AddGlobalId(row.ExtIdentifier, ifcElement);

                    //Add Property Set Properties
                    if (ValidateString(row.SerialNumber))
                        AddPropertySingleValue(ifcElement, "Pset_Component", "Component Properties From COBie", "SerialNumber", "Serial Number for " + name, new IfcLabel(row.SerialNumber));
                    if (ValidateString(row.InstallationDate))
                        AddPropertySingleValue(ifcElement, "Pset_Component", null, "InstallationDate", "Installation Date for " + name, new IfcLabel(row.InstallationDate));
                    if (ValidateString(row.WarrantyStartDate))
                        AddPropertySingleValue(ifcElement, "Pset_Component", null, "WarrantyStartDate", "Warranty Start Date for " + name, new IfcLabel(row.WarrantyStartDate));
                    if (ValidateString(row.TagNumber))
                        AddPropertySingleValue(ifcElement, "Pset_Component", null, "TagNumber", "Tag Number for " + name, new IfcLabel(row.TagNumber));
                    if (ValidateString(row.BarCode))
                        AddPropertySingleValue(ifcElement, "Pset_Component", null, "BarCode", "Bar Code for " + name, new IfcLabel(row.BarCode));
                    if (ValidateString(row.AssetIdentifier))
                        AddPropertySingleValue(ifcElement, "Pset_Component", null, "AssetIdentifier", "Asset Identifier for " + name, new IfcLabel(row.AssetIdentifier));
                    //set up relationship of the component to the type the component is
                    if (ValidateString(row.TypeName))
                    {
                        IfcTypeObject ifcTypeObject = IfcTypeObjects.Where(to => to.Name.ToString().ToLower() == row.TypeName.ToLower()).FirstOrDefault();
                        if (ifcTypeObject != null)
                            ifcElement.SetDefiningType(ifcTypeObject, Model);
                        else
                            ifcElement.ObjectType = row.TypeName; //no type so save type name in IfcLable property of IfcObject
                    }
                    //set up relationship of the component to the space
                    if (ValidateString(row.Space))
                    {
                        AddElementRelationship(ifcElement, row.Space);
                    }
                    else
                    {
                        GetBuilding().AddElement(ifcElement); //default to building, probably give incorrect bounding box as we do not know what the element parent was
                    }
                }
            }
            else
            {
#if DEBUG
                Console.WriteLine("Failed to create component {0} of {1}", row.Name, row.ExtObject);
#endif
            }
        }

        /// <summary>
        /// Add element relationship to a parent object
        /// </summary>
        /// <param name="ifcElement">IfcElement object</param>
        /// <param name="spaceNames">Name used as key to find parent object</param>
        private void AddElementRelationship(IfcElement ifcElement, string spaceNames)
        {
            spaceNames = spaceNames.ToLower().Trim();
            IfcSpace ifcSpace = null;
            IfcBuildingStorey ifcBuildingStorey = null;
            //see if the full name is in spaces
            ifcSpace = IfcSpaces.Where(space => space.Name.ToString().ToLower().Trim() == spaceNames).FirstOrDefault();
            if (ifcSpace != null)
                ifcSpace.AddElement(ifcElement);
            else //not in spaces so try Floors
            {
                ifcBuildingStorey = IfcBuildingStoreys.Where(bs => bs.Name.ToString().ToLower().Trim() == spaceNames).FirstOrDefault();
                if (ifcBuildingStorey != null)
                    ifcBuildingStorey.AddElement(ifcElement);
                else //not in floors so see if the space names is a delimited list
                {
                    char splitKey = GetSplitChar(spaceNames);
                    string[] spaceArray = spaceNames.Split(splitKey);
                    if (spaceArray.Count() > 1) //if one we have already tried above, if more than one then try each item 
                    {
                        foreach (string spaceitem in spaceArray)
                        {
                            string spaceName = spaceitem.Trim();
                            ifcSpace = IfcSpaces.Where(space => space.Name.ToString().ToLower().Trim() == spaceName).FirstOrDefault();
                            if (ifcSpace != null)
                                ifcSpace.AddElement(ifcElement);
                            else
                            {
                                ifcBuildingStorey = IfcBuildingStoreys.Where(bs => bs.Name.ToString().ToLower().Trim() == spaceName).FirstOrDefault();
                                if (ifcBuildingStorey != null)
                                    ifcBuildingStorey.AddElement(ifcElement);
                                else
                                    GetBuilding().AddElement(ifcElement); //default to building, probably give incorrect bounding box as we do not know what the element parent was
                            }
                        }
                    }
                    else
                    {
                        GetBuilding().AddElement(ifcElement); //default to building, probably give incorrect bounding box as we do not know what the element parent was
                    }

                }
            }
        }

        /// <summary>
        /// Create an instance of an object via a string name
        /// </summary>
        /// <param name="elementTypeName">String holding object type name we eant to create</param>
        /// <param name="model">Model object</param>
        /// <returns></returns>
        public static IfcElement GetElementInstance(string elementTypeName, IModel model)
        {
            elementTypeName = elementTypeName.Trim().ToUpper();
            IfcType ifcType;
            IfcElement ifcElement = null;
            if (IfcMetaData.TryGetIfcType(elementTypeName, out ifcType))
            {
                MethodInfo method = typeof(IXbimInstanceCollection).GetMethod("New", Type.EmptyTypes);
                MethodInfo generic = method.MakeGenericMethod(ifcType.Type);
                var eleObj = generic.Invoke(model.Instances, null);
                if (eleObj is IfcElement)
                    ifcElement = (IfcElement)eleObj;
            }


            if (ifcElement == null)
                ifcElement = model.Instances.New<IfcFurnishingElement>(); //was IfcVirtualElement, but this is excluded in the import filters, so just using 
            return ifcElement;
        }
        #endregion
    }
}
