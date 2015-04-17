using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.Ifc2x3.SharedFacilitiesElements;
using Xbim.Ifc2x3.UtilityResource;
using Xbim.Ifc2x3.MaterialResource;
using System.Diagnostics;
using System;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Type tab.
    /// </summary>
    public class COBieDataType : COBieData<COBieTypeRow>, IAttributeProvider
    {

        
        
        /// <summary>
        /// Data Type constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataType(COBieContext context) : base(context)
        {
            RowHashs = new Dictionary<string, bool>();
        }

        private Dictionary<string, bool> RowHashs { get; set; }

        #region Methods

        /// <summary>
        /// Fill sheet rows for Type sheet
        /// </summary>
        /// <returns>COBieSheet</returns>
        public override COBieSheet<COBieTypeRow> Fill()
        {
#if DEBUG
            Stopwatch timer = new Stopwatch();
            timer.Start();
#endif            
            ProgressIndicator.ReportMessage("Starting Types...");

            // Create new Sheet
            COBieSheet<COBieTypeRow> types = new COBieSheet<COBieTypeRow>(Constants.WORKSHEET_TYPE);
            
            //group the types by name as we need to filter duplicate items in for each loop
            IEnumerable<IfcTypeObject> ifcTypeObjects = Model.Instances.OfType<IfcTypeObject>()
                .Select(type => type)
                .Where(type => !Context.Exclude.ObjectType.Types.Contains(type.GetType()))
                .GroupBy(type => type.Name).SelectMany(g => g);//.Distinct()

            
            
            //set up property set helper class
            COBieDataPropertySetValues allPropertyValues = new COBieDataPropertySetValues(); //properties helper class
            COBieDataAttributeBuilder attributeBuilder = new COBieDataAttributeBuilder(Context, allPropertyValues);
            attributeBuilder.InitialiseAttributes(ref _attributes);
            attributeBuilder.ExcludeAttributePropertyNames.AddRange(Context.Exclude.Types.AttributesEqualTo);//we do not want for the attribute sheet so filter them out
            attributeBuilder.ExcludeAttributePropertyNamesWildcard.AddRange(Context.Exclude.Types.AttributesContain);//we do not want for the attribute sheet so filter them out
            attributeBuilder.ExcludeAttributePropertySetNames.AddRange(Context.Exclude.Types.PropertySetsEqualTo); //exclude the property set from selection of values
            attributeBuilder.RowParameters["Sheet"] = "Type";

            ProgressIndicator.Initialise("Creating Types", ifcTypeObjects.Count());
            //COBieTypeRow lastRow = null;
            foreach (IfcTypeObject type in ifcTypeObjects)
            {
                ProgressIndicator.IncrementAndUpdate();
                
                
                COBieTypeRow typeRow = new COBieTypeRow(types);
                
                // TODO: Investigate centralising this common code.
                string name = type.Name;
                if (string.IsNullOrEmpty(type.Name))
                {
                    name = "Name Unknown " + UnknownCount.ToString();
                    UnknownCount++;
                }

                //set allPropertyValues to this element
                allPropertyValues.SetAllPropertyValues(type); //set the internal filtered IfcPropertySingleValues List in allPropertyValues
                
                typeRow.Name = name;
                string create_By = allPropertyValues.GetPropertySingleValueValue("COBieTypeCreatedBy", false); //support for COBie Toolkit for Autodesk Revit
                typeRow.CreatedBy = ValidateString(create_By) ? create_By : GetTelecomEmailAddress(type.OwnerHistory);
                string created_On = allPropertyValues.GetPropertySingleValueValue("COBieTypeCreatedOn", false);//support for COBie Toolkit for Autodesk Revit
                typeRow.CreatedOn = ValidateString(created_On) ? created_On : GetCreatedOnDateAsFmtString(type.OwnerHistory);
                typeRow.Category = GetCategory(allPropertyValues);
                string description = allPropertyValues.GetPropertySingleValueValue("COBieDescription", false);//support for COBie Toolkit for Autodesk Revit
                typeRow.Description = ValidateString(description) ? description : GetTypeObjDescription(type);

                string ext_System = allPropertyValues.GetPropertySingleValueValue("COBieTypeExtSystem", false);//support for COBie Toolkit for Autodesk Revit
                typeRow.ExtSystem = ValidateString(ext_System) ? ext_System : GetExternalSystem(type);
                typeRow.ExtObject = type.GetType().Name;
                typeRow.ExtIdentifier = type.GlobalId;

                
            
                FillPropertySetsValues(allPropertyValues, type, typeRow);
                //not duplicate so add to sheet
                //if (CheckForDuplicateRow(lastRow, typeRow)) 
                //{
                string rowhash = typeRow.RowHashValue;
                if (RowHashs.ContainsKey(rowhash))
                {
                    continue;
                }
                else
                {
                    types.AddRow(typeRow);
                    RowHashs.Add(rowhash, true);
                }
                    
                    //lastRow = typeRow; //save this row to test on next loop
                //}
                // Provide Attribute sheet with our context
                //fill in the attribute information
                attributeBuilder.RowParameters["Name"] = typeRow.Name;
                attributeBuilder.RowParameters["CreatedBy"] = typeRow.CreatedBy;
                attributeBuilder.RowParameters["CreatedOn"] = typeRow.CreatedOn;
                attributeBuilder.RowParameters["ExtSystem"] = typeRow.ExtSystem;
                attributeBuilder.PopulateAttributesRows(type); //fill attribute sheet rows
                
            }
            ProgressIndicator.Finalise();
            //--------------Loop all IfcMaterialLayerSet-----------------------------
            ProgressIndicator.ReportMessage("Starting MaterialLayerSets...");
            IEnumerable<IfcMaterialLayerSet> ifcMaterialLayerSets = Model.Instances.OfType<IfcMaterialLayerSet>();
            ChildNamesList rowHolderChildNames = new ChildNamesList();
            ChildNamesList rowHolderLayerChildNames = new ChildNamesList();
            
            string createdBy = DEFAULT_STRING, createdOn = DEFAULT_STRING, extSystem = DEFAULT_STRING;
            ProgressIndicator.Initialise("Creating MaterialLayerSets", ifcMaterialLayerSets.Count());

            foreach (IfcMaterialLayerSet ifcMaterialLayerSet in ifcMaterialLayerSets)
            {
                ProgressIndicator.IncrementAndUpdate();
                //Material layer has no owner history, so lets take the owner history from IfcRelAssociatesMaterial.RelatingMaterial -> (IfcMaterialLayerSetUsage.ForLayerSet -> IfcMaterialLayerSet) || IfcMaterialLayerSet || IfcMaterialLayer as it is a IfcMaterialSelect
                IfcOwnerHistory ifcOwnerHistory = GetMaterialOwnerHistory(ifcMaterialLayerSet);
                if (ifcOwnerHistory != null)
                {
                    createdBy = GetTelecomEmailAddress(ifcOwnerHistory);
                    createdOn = GetCreatedOnDateAsFmtString(ifcOwnerHistory);
                    extSystem = GetExternalSystem(ifcOwnerHistory);
                }
                else //default to the project as we failed to find a IfcRoot object to extract it from
                {
                    createdBy = GetTelecomEmailAddress(Model.IfcProject.OwnerHistory);
                    createdOn = GetCreatedOnDateAsFmtString(Model.IfcProject.OwnerHistory);
                    extSystem = GetExternalSystem(Model.IfcProject.OwnerHistory);
                }
                //add materialLayerSet name to rows
                COBieTypeRow matSetRow = new COBieTypeRow(types);
                matSetRow.Name = (string.IsNullOrEmpty(ifcMaterialLayerSet.Name)) ? DEFAULT_STRING : ifcMaterialLayerSet.Name;
                matSetRow.CreatedBy = createdBy;
                matSetRow.CreatedOn = createdOn;
                matSetRow.ExtSystem = extSystem;
                matSetRow.ExtObject = ifcMaterialLayerSet.GetType().Name;
                matSetRow.AssetType = "Fixed";
                types.AddRow(matSetRow);

                //loop the materials within the material layer set
                foreach (IfcMaterialLayer ifcMaterialLayer in ifcMaterialLayerSet.MaterialLayers)
                {
                    if ((ifcMaterialLayer.Material != null) && 
                        (!string.IsNullOrEmpty(ifcMaterialLayer.Material.Name))
                        )
                    {
                        string name = ifcMaterialLayer.Material.Name.ToString().Trim();
                        double thickness = ifcMaterialLayer.LayerThickness;
                        string keyName = name + " (" + thickness.ToString() + ")";
                        if (!rowHolderLayerChildNames.Contains(keyName.ToLower())) //check we do not already have it
                        {
                            COBieTypeRow matRow = new COBieTypeRow(types);

                            matRow.Name = keyName;
                            matRow.CreatedBy = createdBy; 
                            matRow.CreatedOn = createdOn;
                            matRow.ExtSystem = extSystem;
                            matRow.ExtObject = ifcMaterialLayer.GetType().Name;
                            matRow.AssetType = "Fixed";
                            matRow.NominalWidth = thickness.ToString();

                            rowHolderLayerChildNames.Add(keyName.ToLower());

                            //we also don't want to repeat on the IfcMaterial loop below
                            if (!rowHolderChildNames.Contains(name.ToLower()))
                                rowHolderChildNames.Add(name.ToLower());

                            types.AddRow(matRow);
                        }

                       
                    }
                }
            }
            ProgressIndicator.Finalise();
            //--------Loop Materials in case they are not in a layer Set-----
            ProgressIndicator.ReportMessage("Starting Materials...");
            
            IEnumerable<IfcMaterial> ifcMaterials = Model.Instances.OfType<IfcMaterial>();
            ProgressIndicator.Initialise("Creating Materials", ifcMaterials.Count());
            foreach (IfcMaterial ifcMaterial in ifcMaterials)
            {
                ProgressIndicator.IncrementAndUpdate();
                string name = ifcMaterial.Name.ToString().Trim();
                if (!string.IsNullOrEmpty(ifcMaterial.Name))
                {
                    if (!rowHolderChildNames.Contains(name.ToLower())) //check we do not already have it
                    {
                        COBieTypeRow matRow = new COBieTypeRow(types);
                        
                        matRow.Name = name;
                        matRow.CreatedBy = createdBy; //no way of extraction on material, if no material layer set, so use last found in Layer Set loop
                        matRow.CreatedOn = createdOn; //ditto
                        matRow.ExtSystem = extSystem; //ditto
                        matRow.ExtObject = ifcMaterial.GetType().Name;
                        matRow.AssetType = "Fixed";

                        types.AddRow(matRow);
                    }

                    rowHolderChildNames.Add(name.ToLower());
                }
            }

            types.OrderBy(s => s.Name);

            ProgressIndicator.Finalise();

#if DEBUG
            timer.Stop();
            Console.WriteLine(String.Format("Time to generate Type data = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
#endif
            return types;
        }

        private static bool CheckForDuplicateRow(COBieTypeRow lastRow, COBieTypeRow typeRow)
        {
            bool AddRecord = true;
            //test to see if we have a duplicate
            if (lastRow != null) //filter out first loop
            {
                if (string.Equals(lastRow.Name, typeRow.Name)) //only test sizing if names the same
                {
                    if ((!string.Equals(lastRow.NominalLength, typeRow.NominalLength)) || //use or's to skip further tests on a true not equal
                        (!string.Equals(lastRow.NominalWidth, typeRow.NominalWidth)) ||
                        (!string.Equals(lastRow.NominalHeight, typeRow.NominalHeight)) ||
                        (!string.Equals(lastRow.ModelNumber, typeRow.ModelNumber)) ||
                        (!string.Equals(lastRow.ModelReference, typeRow.ModelReference)) ||
                        (!string.Equals(lastRow.Size, typeRow.Size)) ||
                        (!string.Equals(lastRow.Manufacturer, typeRow.Manufacturer))
                         )
                        AddRecord = true; //one of the values do not match so record OK
                    else
                        AddRecord = false;//skip this record
                }
            }
            return AddRecord;
        }

        private void FillPropertySetsValues(COBieDataPropertySetValues allPropertyValues, IfcTypeObject type, COBieTypeRow typeRow)
        {
               
            //get related object properties to extract from if main way fails
            allPropertyValues.SetAllPropertyValues(type, "Pset_Asset");
            typeRow.AssetType =     GetAssetType(type, allPropertyValues); 
            allPropertyValues.SetAllPropertyValues(type, "Pset_ManufacturersTypeInformation");
            string manufacturer =   allPropertyValues.GetPropertySingleValueValue("Manufacturer", false);
            typeRow.Manufacturer =  ((manufacturer == DEFAULT_STRING) || (!IsEmailAddress(manufacturer))) ? Constants.DEFAULT_EMAIL : manufacturer;

            typeRow.ModelNumber =   GetModelNumber(type, allPropertyValues);


            allPropertyValues.SetAllPropertyValues(type, new List<string>(new string[] { "COBie_Warranty", "Pset_Warranty" })); //reset property set name from "Pset_Warranty" to "COBie_Warranty"
            
            string warrantyDurationPart =       allPropertyValues.GetPropertySingleValueValue("WarrantyDurationParts", false);
            typeRow.WarrantyDurationParts =     ((warrantyDurationPart == DEFAULT_STRING) || (!IsNumeric(warrantyDurationPart)) ) ? DEFAULT_NUMERIC : warrantyDurationPart;
            Interval warrantyDuration =         GetDurationUnitAndValue(allPropertyValues.GetPropertySingleValue("WarrantyDurationLabor")); 
            typeRow.WarrantyDurationLabor =     (!IsNumeric(warrantyDuration.Value)) ? DEFAULT_NUMERIC : warrantyDuration.Value;
            typeRow.WarrantyDurationUnit =      (string.IsNullOrEmpty(warrantyDuration.Unit)) ? "Year" : warrantyDuration.Unit; //redundant column via matrix sheet states set as year

            typeRow.ReplacementCost =           GetReplacementCost(type, allPropertyValues); 
            typeRow.WarrantyGuarantorParts =    GetWarrantyGuarantorParts(type, allPropertyValues);
            typeRow.WarrantyGuarantorLabor =    GetWarrantyGuarantorLabor(type, allPropertyValues);
            typeRow.WarrantyDescription =       GetWarrantyDescription(type, allPropertyValues);
            

            allPropertyValues.SetAllPropertyValues(type, "Pset_ServiceLife");
            
            Interval serviceDuration =  GetDurationUnitAndValue(allPropertyValues.GetPropertySingleValue("ServiceLifeDuration"));
            typeRow.ExpectedLife =      GetExpectedLife(type, serviceDuration, allPropertyValues);
            typeRow.DurationUnit =      serviceDuration.Unit;

            allPropertyValues.SetAllPropertyValues(type, new List<string>(new string[] { "COBie_Specification", "Pset_Specification" }));//changed from "Pset_Specification" via v16 matrix sheet
            
            typeRow.Shape = allPropertyValues.GetPropertySingleValueValue("Shape", false);
            typeRow.Size =                          allPropertyValues.GetPropertySingleValueValue("Size", false);
            typeRow.Finish =                        allPropertyValues.GetPropertySingleValueValue("Finish", false);
            typeRow.Grade =                         allPropertyValues.GetPropertySingleValueValue("Grade", false);
            typeRow.Material =                      allPropertyValues.GetPropertySingleValueValue("Material", false);
            typeRow.Features =                      allPropertyValues.GetPropertySingleValueValue("Features", false);

            typeRow.NominalLength =                 GetNominalLength(type, allPropertyValues);
            typeRow.NominalWidth =                  GetNominalWidth(type, allPropertyValues);
            typeRow.NominalHeight =                 GetNominalHeight(type, allPropertyValues);
            typeRow.ModelReference =                GetModelReference(type, allPropertyValues);
            typeRow.Color =                         GetColour(type, allPropertyValues);
            typeRow.Constituents =                  GetConstituents(type, allPropertyValues);
            typeRow.AccessibilityPerformance =      GetAccessibilityPerformance(type, allPropertyValues);
            typeRow.CodePerformance =               GetCodePerformance(type, allPropertyValues);
            typeRow.SustainabilityPerformance =     GetSustainabilityPerformance(type, allPropertyValues); 
            
        }

        /// <summary>
        /// Get the Asset Type from the property set property if nothing found then default to Moveable/Fixed decided on object type
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject Object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holding the property sets</param>
        /// <returns>String holding Asset Type</returns>
        private string GetAssetType(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            string value = allPropertyValues.GetPropertySingleValueValue("AssetAccountingType", false);
            if (value == DEFAULT_STRING)
	        {
                if (ifcTypeObject is IfcFurnitureType)
                    value = "Moveable";                   
                else
                    value = "Fixed";
	        }
            return value;
        }

        /// <summary>
        /// Get the Sustainability Performance for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetSustainabilityPerformance(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            string value = allPropertyValues.GetPropertySingleValueValue("SustainabilityPerformance", false);
            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            if (value == DEFAULT_STRING)
            {
                allPropertyValues.SetAllPropertyValues(ifcTypeObject);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("SustainabilityPerformance", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("Environmental", true);
            }
            return (string.IsNullOrEmpty(value)) ? DEFAULT_STRING : value;
        }

        /// <summary>
        /// Get the Code Performance for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetCodePerformance(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            string value = allPropertyValues.GetPropertySingleValueValue("CodePerformance", false);
            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            if (value == DEFAULT_STRING)
            {
                allPropertyValues.SetAllPropertyValues(ifcTypeObject);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("CodePerformance", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("Regulation", true);
            }
            return (string.IsNullOrEmpty(value)) ? DEFAULT_STRING : value;
        }


        /// <summary>
        /// Get the Accessibility Performance for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetAccessibilityPerformance(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            string value = allPropertyValues.GetPropertySingleValueValue("AccessibilityPerformance", false);
            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            if (value == DEFAULT_STRING)
            {
                allPropertyValues.SetAllPropertyValues(ifcTypeObject);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("AccessibilityPerformance", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("Access", true);
            }
            return (string.IsNullOrEmpty(value)) ? DEFAULT_STRING : value;
        }

        /// <summary>
        /// Get the Constituents for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetConstituents(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            string value = allPropertyValues.GetPropertySingleValueValue("Constituents", false);
            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            if (value == DEFAULT_STRING)
            {
                allPropertyValues.SetAllPropertyValues(ifcTypeObject);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("constituents", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("parts", true);
            }
            return (string.IsNullOrEmpty(value)) ? DEFAULT_STRING : value;
        }

        /// <summary>
        /// Get the Colour for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetColour(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            string value = allPropertyValues.GetPropertySingleValueValue("Colour", false);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("Color", false);
            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            if (value == DEFAULT_STRING)
            {
                allPropertyValues.SetAllPropertyValues(ifcTypeObject);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("Colour", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("Color", true);
            }
            return (string.IsNullOrEmpty(value)) ? DEFAULT_STRING : value;
        }

        /// <summary>
        /// Get the Model Reference for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetModelReference(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            allPropertyValues.SetAllPropertyValues(ifcTypeObject, "Pset_ManufacturersTypeInformation");
            string value = allPropertyValues.GetPropertySingleValueValue("ModelReference", false);
            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            if (value == DEFAULT_STRING)
            {
                allPropertyValues.SetAllPropertyValues(ifcTypeObject);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("ModelReference", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("Reference", true);
            }
            return (string.IsNullOrEmpty(value)) ? DEFAULT_STRING : value;
        }

        /// <summary>
        /// Get the Nominal Height for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetNominalHeight(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            string value = allPropertyValues.GetPropertySingleValueValue("NominalHeight", false);
            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            if (value == DEFAULT_STRING)
            {
                allPropertyValues.SetAllPropertyValues(ifcTypeObject);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("NominalHeight", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("OverallHeight", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("Height", true);

                
            }
            return ConvertNumberOrDefault(value);
        }


        /// <summary>
        /// Get the Nominal Width for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetNominalWidth(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            string value = allPropertyValues.GetPropertySingleValueValue("NominalWidth", false);
            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            if (value == DEFAULT_STRING)
            {
                allPropertyValues.SetAllPropertyValues(ifcTypeObject);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("NominalWidth", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("OverallWidth", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("Width", true);
                
            }
            return ConvertNumberOrDefault(value);
        }

        /// <summary>
        /// Get the Nominal Length for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetNominalLength(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            string value = allPropertyValues.GetPropertySingleValueValue("NominalLength", false);
            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            if (value == DEFAULT_STRING)
            {
                allPropertyValues.SetAllPropertyValues(ifcTypeObject);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("NominalLength", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("OverallLength", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("Length", true);
            }
            return ConvertNumberOrDefault(value);
        }



        /// <summary>
        /// Get the Expected Life for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetExpectedLife(IfcTypeObject ifcTypeObject, Interval serviceDuration, COBieDataPropertySetValues allPropertyValues)
        {
            string value = serviceDuration.Value;

            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            allPropertyValues.SetAllPropertyValues(ifcTypeObject);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("ServiceLifeDuration", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue(" Expected", true);
            return ((string.IsNullOrEmpty(value)) || (value == DEFAULT_STRING) || (!IsNumeric(value))) ? DEFAULT_NUMERIC : value;
        }


        /// <summary>
        /// Get the Replacement Cost for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetReplacementCost(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            allPropertyValues.SetAllPropertyValues(ifcTypeObject, "COBie_EconomicImpactValues"); //changed from "Pset_EconomicImpactValues" on v16 of matrix
            string value = allPropertyValues.GetPropertySingleValueValue("ReplacementCost", false);

            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            if (value == DEFAULT_STRING)
            {
                allPropertyValues.SetAllPropertyValues(ifcTypeObject);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("ReplacementCost", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("Replacement Cost", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("Cost", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("Replacement", true);
            }
            return ((string.IsNullOrEmpty(value)) || (value == DEFAULT_STRING) || (!IsNumeric(value))) ? DEFAULT_NUMERIC : value;

        }

        /// <summary>
        /// Get the Warranty Description for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetWarrantyDescription(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            string value = allPropertyValues.GetPropertySingleValueValue("WarrantyDescription", false);

            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            if (value == DEFAULT_STRING)
            {
                allPropertyValues.SetAllPropertyValues(ifcTypeObject);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("WarrantyDescription", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("WarrantyIdentifier", true);

                
            }
            return (string.IsNullOrEmpty(value)) ? DEFAULT_STRING : value;
        }

        /// <summary>
        /// Get the Warranty Guarantor Labour for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetWarrantyGuarantorLabor(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            string value = allPropertyValues.GetPropertySingleValueValue("WarrantyGuarantorLabor", false);
            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            if (value == DEFAULT_STRING)
            {
                allPropertyValues.SetAllPropertyValues(ifcTypeObject);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("WarrantyGuarantorParts", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("PointOfContact", true);

                
            }
            return (((string.IsNullOrEmpty(value)) || (value == DEFAULT_STRING)) || (!IsEmailAddress(value))) ? Constants.DEFAULT_EMAIL : value;
        }

        /// <summary>
        /// Get the Warranty Guarantor Parts for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetWarrantyGuarantorParts(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            string value = allPropertyValues.GetPropertySingleValueValue("WarrantyGuarantorParts", false);
            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            if (value == DEFAULT_STRING)
            {
                allPropertyValues.SetAllPropertyValues(ifcTypeObject);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("WarrantyGuarantorParts", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("PointOfContact", true);
            }
            return (((string.IsNullOrEmpty(value)) || (value == DEFAULT_STRING)) || (!IsEmailAddress(value))) ? Constants.DEFAULT_EMAIL : value;
        }

        /// <summary>
        /// Get the Model Number for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetModelNumber(IfcTypeObject ifcTypeObject, COBieDataPropertySetValues allPropertyValues)
        {
            string value = allPropertyValues.GetPropertySingleValueValue("ModelLabel", false);
            //Fall back to wild card properties
            //get the property single values for this ifcTypeObject
            if (value == DEFAULT_STRING)
            {
                allPropertyValues.SetAllPropertyValues(ifcTypeObject);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("ArticleNumber", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("ModelLabel", true);
            }
            return (string.IsNullOrEmpty(value)) ? DEFAULT_STRING : value;
        }

        /// <summary>
        /// Return the Description for the passed IfcTypeObject object
        /// </summary>
        /// <param name="type">IfcTypeObject</param>
        /// <returns>Description for Type Object</returns>
        private string GetTypeObjDescription(IfcTypeObject type)
        {
            if (type != null)
            {
                if (!string.IsNullOrEmpty(type.Description)) return type.Description;
                else if (!string.IsNullOrEmpty(type.Name)) return type.Name;
                
                //if supports PredefinedType and no description or name then use the predefined type or ElementType if they exist
                IEnumerable<PropertyInfo> pInfo = type.GetType().GetProperties(); //get properties
                var predefinedType =  pInfo.Where(p => p.Name == "PredefinedType").FirstOrDefault();
                if (predefinedType != null)
                {
                    string temp = predefinedType.GetValue(type, null).ToString(); //get predefindtype as description

                    if (!string.IsNullOrEmpty(temp))
                    {
                        if (temp == "USERDEFINED")
                        {
                            //if used defined then the type description should be in ElementType, so see if property exists
                            var elementType = pInfo.Where(p => p.Name == "ElementType").FirstOrDefault();
                            if (elementType != null)
                            {
                                temp = elementType.GetValue(type, null).ToString(); //get ElementType
                                if (!string.IsNullOrEmpty(temp)) return temp;
                            }
                        }
                        if (temp == "NOTDEFINED") //if not defined then give up and return default
                        {
                            return DEFAULT_STRING;
                        }

                        return temp;
                    }
                }
                
            }
            return DEFAULT_STRING;
        }

        /// <summary>
        /// Get the list or properties matching the passed in list of attribute names 
        /// </summary>
        /// <param name="typeObj">IfcTypeObject </param>
        /// <param name="attNames">list of attribute names</param>
        /// <returns>List of IfcPropertySingleValue which are contained in AttNames</returns>
        private IEnumerable<IfcPropertySingleValue> GetTypeObjRelAttributes(IfcTypeObject typeObj, List<string> attNames)
        {
            IEnumerable<IfcPropertySingleValue> properties = Enumerable.Empty<IfcPropertySingleValue>();
            // can hold zero or 1 ObjectTypeOf (IfcRelDefinesByType- holds list of objects of this type in RelatedObjects property) so test return
            var typeInstanceRel = typeObj.ObjectTypeOf.FirstOrDefault(); 
            if (typeInstanceRel != null)
            {
                // TODO: Check usage of GetAllProperties - duplicates Properties from Type?
                foreach (IfcPropertySet pset in typeInstanceRel.RelatedObjects.First().GetAllPropertySets()) 
                {
                    //has to have 1 or more object that are of this type, so get first and see what we get
                    properties = properties.Concat(pset.HasProperties.Where<IfcPropertySingleValue>(p => attNames.Contains(p.Name.ToString())));
                }
            }


            return properties;
        }

        

        /// <summary>
        /// Get the Time unit and value for the passed in property
        /// </summary>
        /// <param name="typeObject">IfcTypeObject </param>
        /// <param name="psetName">Property Set Name to retrieve IfcPropertySet Object</param>
        /// <param name="propertyName">Property Name held in IfcPropertySingleValue object</param>
        /// <param name="psetValues">List of IfcPropertySingleValue filtered to attribute names we require</param>
        /// <returns>Dictionary holding unit and value e.g. Year, 2.0</returns>
        private Interval GetDurationUnitAndValue( IfcPropertySingleValue typeValue)
        {
            const string DefaultUnit = "Year"; // n/a is not acceptable, so create a valid default

            Interval result = new Interval() { Value = DEFAULT_NUMERIC, Unit = DefaultUnit };
            // try to get the property from the Type first
            //IfcPropertySingleValue typeValue = typeObject.GetPropertySingleValue(psetName, propertyName);

            //// TODO: Check this logic
            //// if null then try and get from first instance of this type
            //if (typeValue == null) 
            //    typeValue = psetValues.Where(p => p.Name == propertyName).FirstOrDefault();

            if (typeValue == null)
                return result;

            //Get the unit type
            if (typeValue.Unit != null)
                result.Unit = GetUnitName(typeValue.Unit);            
            
            //Get the time period value
            if ((typeValue.NominalValue != null) && (typeValue.NominalValue is IfcReal)) //if a number then we can calculate
            {
                double val = (double)typeValue.NominalValue.Value; 
                result.Value = val.ToString();
            }
            else if (typeValue.NominalValue != null) //no number value so just show value for passed in propName
                result.Value = typeValue.NominalValue.Value.ToString();

            return result;
        }

        

        /// <summary>
        /// Get the Category for the IfcTypeObject
        /// </summary>
        /// <param name="type">IfcTypeObject</param>
        /// <returns>string of the category</returns>
        public string GetCategory(COBieDataPropertySetValues allPropertyValues)
        {
            string categoryRef = GetCategoryClassification(allPropertyValues.CurrentObject);
            if (!string.IsNullOrEmpty(categoryRef))
            {
                return categoryRef;
            }
  
            //Try by PropertySet as fallback
            //filter list for front end category
            List<string> categoriesCode = new List<string>() { "OmniClass Table 13 Category",  "OmniClass Number", "OmniClass_Number", "Assembly_Code",  "Assembly Code", 
                                                             "Uniclass Code", "Uniclass_Code",  "Category_Code" ,"Category Code",  "Classification Code", "Classification_Code" };
            //filter list for back end category
            List<string> categoriesDesc = new List<string>() { "OmniClass Title", "OmniClass_Title","Assembly_Description","Assembly Description","UniclassDescription", 
                                                             "Uniclass_Description","Category Description", "Category_Description", "Classification Description", "Classification_Description" };
            List<string> categoriesTest = new List<string>();
            categoriesCode.AddRange(categoriesDesc);

            IEnumerable<IfcPropertySingleValue> properties = allPropertyValues.ObjProperties.OfType<IfcPropertySingleValue>();

            if (properties.Any())
            {
                properties = from psetval in properties
                             where categoriesTest.Contains(psetval.Name.ToString())
                             select psetval;
            }
            //second fall back on objects defined by this type, see if they hold a category on the first related object to this type
            if (!properties.Any())
            {
                Dictionary<IfcPropertySet, IEnumerable<IfcSimpleProperty>> propertysets = allPropertyValues.GetRelatedProperties(allPropertyValues.CurrentObject as IfcTypeObject);
            
                if (propertysets != null)
                {
                    properties = (from dic in propertysets
                                  from psetval in dic.Value
                                  where categoriesTest.Contains(psetval.Name.ToString())
                                  select psetval).OfType<IfcPropertySingleValue>();
                }
            }
            string value = "";
            if (properties.Any())
            {
                string conCatChar = " : ";

                string code = properties.Where(p => p.NominalValue != null && categoriesCode.Contains(p.Name)).Select(p => p.NominalValue.ToString()).FirstOrDefault();
                string description = properties.Where(p => p.NominalValue != null && categoriesDesc.Contains(p.Name)).Select(p => p.NominalValue.ToString()).FirstOrDefault();
                if (!string.IsNullOrEmpty(code)) value += code;
                if (!string.IsNullOrEmpty(description)) value += conCatChar + description;
            }

            if (string.IsNullOrEmpty(value))
                return Constants.DEFAULT_STRING;
            else
                return value;
        }
        #endregion

        COBieSheet<COBieAttributeRow> _attributes;

        public void InitialiseAttributes(ref COBieSheet<COBieAttributeRow> attributeSheet)
        {
            _attributes = attributeSheet;
        }
    }

    public struct Interval
    {
        public string Value { get; set; }
        public string Unit { get; set; }
    }
}
