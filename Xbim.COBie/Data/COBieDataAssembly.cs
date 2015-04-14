using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.XbimExtensions;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.MaterialResource;
using Xbim.Ifc2x3.UtilityResource;
using Xbim.COBie.Serialisers.XbimSerialiser;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Assembly tab.
    /// </summary>
    public class COBieDataAssembly : COBieData<COBieAssemblyRow>
    {
        /// <summary>
        /// Data Assembly constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataAssembly(COBieContext context) : base(context)
        { }

        #region Methods

        /// <summary>
        /// Fill sheet rows for Assembly sheet
        /// </summary>
        /// <returns>COBieSheet</returns>
        public override COBieSheet<COBieAssemblyRow> Fill()
        {
            ProgressIndicator.ReportMessage("Starting Assemblies...");
            //Create new sheet
            COBieSheet<COBieAssemblyRow> assemblies = new COBieSheet<COBieAssemblyRow>(Constants.WORKSHEET_ASSEMBLY);

            // get ifcRelAggregates objects from IFC file what are not in the excludedTypes type list
            IEnumerable<IfcRelAggregates> ifcRelAggregates = Model.Instances.OfType<IfcRelAggregates>();
            IEnumerable<IfcRelNests> ifcRelNests = Model.Instances.OfType<IfcRelNests>(); 
            

            IEnumerable<IfcRelDecomposes> relAll = (from ra in ifcRelAggregates
                                                    where ((ra.RelatingObject is IfcProduct) || (ra.RelatingObject is IfcTypeObject)) && !Context.Exclude.ObjectType.Assembly.Contains(ra.RelatingObject.GetType())
                                                      select ra as IfcRelDecomposes).Union
                                                      (from rn in ifcRelNests
                                                       where ((rn.RelatingObject is IfcProduct) || (rn.RelatingObject is IfcTypeObject)) && !Context.Exclude.ObjectType.Assembly.Contains(rn.RelatingObject.GetType())
                                                      select rn as IfcRelDecomposes);
            
            ProgressIndicator.Initialise("Creating Assemblies", relAll.Count());
            
            int childColumnLength = 255; //default vale, reassigned below

            foreach (IfcRelDecomposes ra in relAll)
            {
                ProgressIndicator.IncrementAndUpdate();
                COBieAssemblyRow assembly = new COBieAssemblyRow(assemblies);
                if (string.IsNullOrEmpty(ra.Name))
                {
                    if (!string.IsNullOrEmpty(ra.RelatingObject.Name))
                        assembly.Name = (string.IsNullOrEmpty(ra.RelatingObject.Name)) ? DEFAULT_STRING : ra.RelatingObject.Name.ToString();
                    else
                        assembly.Name = DEFAULT_STRING;
                }
                else 
                    assembly.Name = ra.Name.ToString();
                
                assembly.CreatedBy = GetTelecomEmailAddress(ra.OwnerHistory);
                assembly.CreatedOn = GetCreatedOnDateAsFmtString(ra.OwnerHistory);
                assembly.SheetName = GetSheetByObjectType(ra.RelatingObject.GetType());
                assembly.ParentName = ra.RelatingObject.Name;
                
                assembly.AssemblyType = "Fixed"; //as Responsibility matrix instruction
                assembly.ExtSystem = GetExternalSystem(ra);
                assembly.ExtObject = ra.GetType().Name;
                if (!string.IsNullOrEmpty(ra.GlobalId)) 
                {
                    assembly.ExtIdentifier = ra.GlobalId.ToString();
                }
                
                assembly.Description = GetAssemblyDescription(ra);

                //get the assembly child names of objects that make up assembly
                ChildNamesList childNamesUnique = ExtractChildNames(ra);
                if (childColumnLength == 0)  childColumnLength = assembly["ChildNames"].COBieColumn.ColumnLength;
                ChildNamesList childNames = ConCatChildNamesList(childNamesUnique, childColumnLength);
                if (childNames.Count > 0)
                    AddChildRows(assemblies, assembly, childNames);
            }

            //--------------Loop all IfcMaterialLayerSet-----------------------------
            IEnumerable<IfcMaterialLayerSet> ifcMaterialLayerSets = Model.Instances.OfType<IfcMaterialLayerSet>();
            char setNamePostFix = 'A';       
            foreach (IfcMaterialLayerSet ifcMaterialLayerSet in ifcMaterialLayerSets)
            {
                COBieAssemblyRow assembly = new COBieAssemblyRow(assemblies);
                if (string.IsNullOrEmpty(ifcMaterialLayerSet.Name))
                {
                    assembly.Name = "Material Layer Set " + setNamePostFix;
                    setNamePostFix++;
                }
                else
                    assembly.Name = ifcMaterialLayerSet.LayerSetName.ToString();

                //Material layer has no owner history, so lets take the owner history from IfcRelAssociatesMaterial.RelatingMaterial -> (IfcMaterialLayerSetUsage.ForLayerSet -> IfcMaterialLayerSet) || IfcMaterialLayerSet || IfcMaterialLayer as it is a IfcMaterialSelect
                IfcOwnerHistory ifcOwnerHistory = GetMaterialOwnerHistory(ifcMaterialLayerSet);

                if (ifcOwnerHistory != null)
                {
                    assembly.CreatedBy = GetTelecomEmailAddress(ifcOwnerHistory);
                    assembly.CreatedOn = GetCreatedOnDateAsFmtString(ifcOwnerHistory);
                    assembly.ExtSystem = GetExternalSystem(ifcOwnerHistory);
                }
                else //default to the project as we failed to find a IfcRoot object to extract it from
                {
                    assembly.CreatedBy = GetTelecomEmailAddress(Model.IfcProject.OwnerHistory);
                    assembly.CreatedOn = GetCreatedOnDateAsFmtString(Model.IfcProject.OwnerHistory);
                    assembly.ExtSystem = GetExternalSystem(Model.IfcProject.OwnerHistory);
                }

                assembly.SheetName = Constants.WORKSHEET_TYPE; //any material objects should be in the TYPE sheet
                assembly.Description = GetMaterialSetDescription(ifcMaterialLayerSet.MaterialLayers.ToList());
                assembly.ParentName = (!string.IsNullOrEmpty(ifcMaterialLayerSet.Name)) ? ifcMaterialLayerSet.Name : DEFAULT_STRING;
                assembly.AssemblyType = "Layer";
                assembly.ExtObject = ifcMaterialLayerSet.GetType().Name;              

                //Loop Material names
                ChildNamesList childNamesUnique = ExtractChildNames(ifcMaterialLayerSet.MaterialLayers.ToList());
                ChildNamesList childNames = ConCatChildNamesList(childNamesUnique, childColumnLength); //childColumnLength is max number of chars for the ChildNames cell
                if (childNames.Count > 0)
                    AddChildRows(assemblies, assembly, childNames);
            }

            assemblies.OrderBy(s => s.Name);
            
            ProgressIndicator.Finalise();

            return assemblies;
        }

        /// <summary>
        /// Add the child row and overflow rows
        /// </summary>
        /// <param name="assemblies">COBieSheet to add rows too</param>
        /// <param name="assembly">COBieAssemblyRow object to copy data from</param>
        /// <param name="childNames">ChildNamesList object holing the names to add to the rows</param>
        private void AddChildRows(COBieSheet<COBieAssemblyRow> assemblies, COBieAssemblyRow assembly, ChildNamesList childNames)
        {
            COBieAssemblyRow assemblyCont = null;
            int index = 0;
            foreach (string childStr in childNames)
            {
                if (index == 0)
                {
                    assembly.ChildNames = childStr;
                    assemblies.AddRow(assembly);
                }
                else
                {
                    assemblyCont = new COBieAssemblyRow(assemblies);
                    assemblyCont.Name = assembly.Name + " : continued " + index.ToString();
                    assemblyCont.CreatedBy = assembly.CreatedBy;
                    assemblyCont.CreatedOn = assembly.CreatedOn;
                    assemblyCont.SheetName = assembly.SheetName;
                    assemblyCont.ParentName = assembly.ParentName;
                    assemblyCont.AssemblyType = assembly.AssemblyType;
                    assemblyCont.ExtSystem = assembly.ExtSystem;
                    assemblyCont.ExtObject = assembly.ExtObject;
                    assemblyCont.ExtIdentifier = assembly.ExtIdentifier;
                    assemblyCont.Description = assembly.Description;
                    assemblyCont.ChildNames = childStr;
                    assemblies.AddRow(assemblyCont);
                }
                index = ++index;
            }
        }

        

        /// <summary>
        /// Get Description 
        /// </summary>
        /// <param name="ra">IfcRelDecomposes object</param>
        /// <returns>string holding description if found</returns>
        private string GetAssemblyDescription(IfcRelDecomposes ra)
        {
            if (ra != null)
            {
                if (!string.IsNullOrEmpty(ra.Description)) return ra.Description;
                else if (!string.IsNullOrEmpty(ra.Name)) return ra.Name;
                else if (!string.IsNullOrEmpty(ra.RelatingObject.Name)) return ra.RelatingObject.Name;
            }
            return Constants.DEFAULT_STRING;
        }

        /// <summary>
        /// Build Material Layer Set description form the materials and thickness
        /// </summary>
        /// <param name="ifcMaterialLayers">List of IfcMaterialLayer</param>
        /// <returns>string holding description if found</returns>
        private string GetMaterialSetDescription(List<IfcMaterialLayer> ifcMaterialLayers)
        {
            StringBuilder strBuilder = new StringBuilder();
            int i = 1;
            foreach (IfcMaterialLayer ifcMaterialLayer in ifcMaterialLayers)
            {
                if ((ifcMaterialLayer.Material != null) &&
                    (!string.IsNullOrEmpty(ifcMaterialLayer.Material.Name))
                    )
                {
                    strBuilder.Append(ifcMaterialLayer.Material.Name);
                    double? thick = ifcMaterialLayer.LayerThickness;

                    if (thick != null)
                    {
                        
                        strBuilder.Append(" (");
                        strBuilder.Append(((double)thick).ToString());
                        strBuilder.Append(")");
                    }
                    if (ifcMaterialLayers.Count > i)
                        strBuilder.Append(" : ");
                }
                i++;
            }

            if (strBuilder.Length > 0) 
                return strBuilder.ToString();
            else
                return DEFAULT_STRING;
            
        }

        /// <summary>
        /// get all names from the IfcRelDecomposes RelatedObjects
        /// </summary>
        /// <param name="ifcMaterialLayers">IfcRelDecomposes Object</param>
        /// <returns>list of strings as ChildNamesList class</returns>
        private ChildNamesList ExtractChildNames(List<IfcMaterialLayer> ifcMaterialLayers)
        {
            ChildNamesList childNamesFilter = new ChildNamesList();
            foreach (IfcMaterialLayer ifcMaterialLayer in ifcMaterialLayers)
            {
                if ((ifcMaterialLayer.Material != null) &&
                    (!string.IsNullOrEmpty(ifcMaterialLayer.Material.Name))
                    )
                {
                    string name = ifcMaterialLayer.Material.Name;
                    double? thick = ifcMaterialLayer.LayerThickness;
                    if (thick != null)
                    {
                        name += " (" + ((double)thick).ToString() + ")";
                    }
                    childNamesFilter.Add(name);
                }
            }
            return childNamesFilter;
        }

        /// <summary>
        /// get all names from the IfcRelDecomposes RelatedObjects
        /// </summary>
        /// <param name="ra">IfcRelDecomposes Object</param>
        /// <returns>list of strings as ChildNamesList class</returns>
        private ChildNamesList ExtractChildNames(IfcRelDecomposes ra)
        {
            ChildNamesList childNamesFilter = new ChildNamesList();
            foreach (IfcObjectDefinition obj in ra.RelatedObjects)
            {
                //filter on type filters used for component and type sheet
                if (Context.Exclude.ObjectType.Component.Contains(obj.GetType()))
                    break;
                if (Context.Exclude.ObjectType.Types.Contains(obj.GetType()))
                    break;

                if (!string.IsNullOrEmpty(obj.Name))
                {
                    //if (!childNamesFilter.Contains(obj.Name))//removed the filter as we should recode all elements of the assembly
                        childNamesFilter.Add(obj.Name);
                }
            }
            return childNamesFilter;
        }

        /// <summary>
        /// Get list of child object names from relatedObjects property of a ifcProduct asset and join with a " : " delimiter
        /// </summary>
        /// <param name="ra">IfcRelDecomposes relationship object</param>
        /// <returns>List of strings fixed to a string limit per string entry</returns>
        private ChildNamesList ConCatChildNamesList(ChildNamesList childNamesUnique, int fieldLength)
        {
            ChildNamesList childNames = new ChildNamesList();
            int strCount = 0;
            List<string> fieldValueList = new List<string>();
            //build field length strings
            foreach (string str in childNamesUnique)
            {
                if (fieldValueList.Count == 0)
                        strCount += str.Length;
                    else
                        strCount += str.Length + 3; //add 3 fro the " : "

                    if (strCount <= fieldLength)
                    {
                        fieldValueList.Add(str);
                    }
                    else
                    {
                        childNames.Add(COBieXBim.JoinStrings(':', fieldValueList));
                        strCount = str.Length; //reset strCount to the current value length
                        fieldValueList.Clear();
                        fieldValueList.Add(str);
                    }
            }
            if (fieldValueList.Count > 0)
            {
                childNames.Add(COBieXBim.JoinStrings(':', fieldValueList));
            }
            return childNames;
        }

        #endregion
    }

    public class ChildNamesList : List<string>{}
}
