using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.QuantityResource;
using Xbim.XbimExtensions;
using System.Collections;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ExternalReferenceResource;
using System.Diagnostics;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Space tab.
    /// </summary>
    public class COBieDataSpace : COBieData<COBieSpaceRow>, IAttributeProvider
    {
        /// <summary>
        /// Data Space constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataSpace(COBieContext context) : base(context)
        { }

        #region Methods

        /// <summary>
        /// Fill sheet rows for Space sheet
        /// </summary>
        /// <returns>COBieSheet</returns>
        public override COBieSheet<COBieSpaceRow> Fill()
        {
#if DEBUG
            Stopwatch timer = new Stopwatch();
            timer.Start();
#endif
            ProgressIndicator.ReportMessage("Starting Spaces...");

            //create new sheet 
            COBieSheet<COBieSpaceRow> spaces = new COBieSheet<COBieSpaceRow>(Constants.WORKSHEET_SPACE);
            
            // get all IfcBuildingStory objects from IFC file
            List<IfcSpace> ifcSpaces = Model.Instances.OfType<IfcSpace>().OrderBy(ifcSpace => ifcSpace.Name, new CompareIfcLabel()).ToList();
            
            COBieDataPropertySetValues allPropertyValues = new COBieDataPropertySetValues(); //properties helper class
            COBieDataAttributeBuilder attributeBuilder = new COBieDataAttributeBuilder(Context, allPropertyValues);
            attributeBuilder.InitialiseAttributes(ref _attributes);

            if (Context.DepartmentsUsedAsZones)
                attributeBuilder.ExcludeAttributePropertyNames.Add("Department"); //remove the department property from selection
            
            //set up filters on COBieDataPropertySetValues
            attributeBuilder.ExcludeAttributePropertyNames.AddRange(Context.Exclude.Space.AttributesEqualTo);
            attributeBuilder.ExcludeAttributePropertyNamesWildcard.AddRange(Context.Exclude.Space.AttributesContain);
            attributeBuilder.ExcludeAttributePropertySetNames.AddRange(Context.Exclude.Space.PropertySetsEqualTo);
            attributeBuilder.RowParameters["Sheet"] = "Space";

            ProgressIndicator.Initialise("Creating Spaces", ifcSpaces.Count());

            foreach (IfcSpace ifcSpace in ifcSpaces)
            {
                ProgressIndicator.IncrementAndUpdate();

                COBieSpaceRow space = new COBieSpaceRow(spaces);
                //set allPropertyValues to this element
                allPropertyValues.SetAllPropertyValues(ifcSpace); //set the internal filtered IfcPropertySingleValues List in allPropertyValues
                
                space.Name = ifcSpace.Name;

                string createBy = allPropertyValues.GetPropertySingleValueValue("COBieCreatedBy", false); //support for COBie Toolkit for Autodesk Revit
                space.CreatedBy = ValidateString(createBy) ? createBy : GetTelecomEmailAddress(ifcSpace.OwnerHistory);
                string createdOn = allPropertyValues.GetPropertySingleValueValue("COBieCreatedOn", false);//support for COBie Toolkit for Autodesk Revit
                space.CreatedOn = ValidateString(createdOn) ? createdOn : GetCreatedOnDateAsFmtString(ifcSpace.OwnerHistory);

                space.Category = GetCategory(ifcSpace);

                space.FloorName = ((ifcSpace.SpatialStructuralElementParent != null) && (!string.IsNullOrEmpty(ifcSpace.SpatialStructuralElementParent.Name))) ? ifcSpace.SpatialStructuralElementParent.Name.ToString() : DEFAULT_STRING;
                string description = allPropertyValues.GetPropertySingleValueValue("COBieDescription", false);//support for COBie Toolkit for Autodesk Revit
                space.Description = ValidateString(description) ? description : GetSpaceDescription(ifcSpace);
                string extSystem = allPropertyValues.GetPropertySingleValueValue("COBieExtSystem", false);//support for COBie Toolkit for Autodesk Revit
                space.ExtSystem = ValidateString(extSystem) ? extSystem : GetExternalSystem(ifcSpace);
                space.ExtObject = ifcSpace.GetType().Name;
                space.ExtIdentifier = ifcSpace.GlobalId;
                space.RoomTag = GetRoomTag(ifcSpace, allPropertyValues);
                
                //Do Unit Values
                space.UsableHeight = GetUsableHeight(ifcSpace, allPropertyValues);
                space.GrossArea = GetGrossFloorArea(ifcSpace, allPropertyValues);
                space.NetArea = GetNetArea(ifcSpace, allPropertyValues);

                spaces.AddRow(space);
                
                //----------fill in the attribute information for spaces-----------

                //fill in the attribute information
                attributeBuilder.RowParameters["Name"] = space.Name;
                attributeBuilder.RowParameters["CreatedBy"] = space.CreatedBy;
                attributeBuilder.RowParameters["CreatedOn"] = space.CreatedOn;
                attributeBuilder.RowParameters["ExtSystem"] = space.ExtSystem;
                attributeBuilder.PopulateAttributesRows(ifcSpace); //fill attribute sheet rows//pass data from this sheet info as Dictionary
                
            }

            spaces.OrderBy(s => s.Name);

            ProgressIndicator.Finalise();
#if DEBUG
            timer.Stop();
            Console.WriteLine(String.Format("Time to generate Spaces data = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
#endif
            return spaces;
        }

        /// <summary>
        /// Get Net Area value
        /// </summary>
        /// <param name="ifcSpace">IfcSpace object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetNetArea(IfcSpace ifcSpace, COBieDataPropertySetValues allPropertyValues)
        {
            string areaUnit = null;
            double areavalue = 0.0;

            if (!string.IsNullOrEmpty(Context.WorkBookUnits.AreaUnit))
                areaUnit = Context.WorkBookUnits.AreaUnit;//see what the global area unit is
          
            IfcAreaMeasure netAreaValue = ifcSpace.GetNetFloorArea();  //this extension has the GSA built in so no need to get again
            if (netAreaValue != null)
            {
                areavalue = ((double)netAreaValue);
                if (areavalue > 0.0)
                {
                    //if ((!string.IsNullOrEmpty(areaUnit)) && (areaUnit.ToLower().Contains("milli")) && (areavalue > 250000.0)) //we are using millimetres, and areavalue is lightly to be in mmsq if over 250000(0.5msq)
                    //    areavalue = areavalue / 1000000.0;

                    return areavalue.ToString();
                }
            }

            //Fall back to properties
            //get the property single values for this ifcSpace
            if (allPropertyValues.CurrentObject != ifcSpace)
            {
                allPropertyValues.SetAllPropertyValues(ifcSpace);
            }
            

            //try and find it in the attached properties of the ifcSpace
            string value = allPropertyValues.GetPropertySingleValueValue("NetFloorArea", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("GSA", true);

            if (value == DEFAULT_STRING)
                return DEFAULT_NUMERIC;
            else
            {
                if (double.TryParse(value, out areavalue))
                {
                    //if ((!string.IsNullOrEmpty(areaUnit)) && (areaUnit.ToLower().Contains("milli")) && (areavalue > 250000.0))//we are using millimetres, and areavalue is lightly to be in mmsq if over 250000(0.5msq)
                    //    areavalue = areavalue / 1000000.0;
                    return areavalue.ToString();
                }
                return value;
            }
        }
        /// <summary>
        /// Get space gross floor area
        /// </summary>
        /// <param name="ifcSpace">IfcSpace object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetGrossFloorArea(IfcSpace ifcSpace, COBieDataPropertySetValues allPropertyValues)
        {
            string areaUnit = null;
            double areavalue = 0.0;

            if (!string.IsNullOrEmpty(Context.WorkBookUnits.AreaUnit))
                areaUnit = Context.WorkBookUnits.AreaUnit;//see what the global area unit is
            
            
            //Do Gross Areas 
            IfcAreaMeasure grossAreaValue = ifcSpace.GetGrossFloorArea();
            if (grossAreaValue != null)
                areavalue = ((double)grossAreaValue);
            else//if we fail on IfcAreaMeasure try GSA keys
            {
                IfcQuantityArea spArea = ifcSpace.GetQuantity<IfcQuantityArea>("GSA Space Areas", "GSA BIM Area");
                if ((spArea is IfcQuantityArea) && (spArea.AreaValue != null))
                    areavalue = ((double)spArea.AreaValue);
            }
            if (areavalue > 0.0)
	        {
                //if ((!string.IsNullOrEmpty(areaUnit)) && (areaUnit.ToLower().Contains("milli")) && (areavalue > 250000.0)) //we are using millimetres, and areavalue is lightly to be in mmsq if over 250000(0.5msq)
                //    areavalue = areavalue / 1000000.0;
                
		         return areavalue.ToString();
	        }
            
            //Fall back to properties
            //get the property single values for this ifcSpace
            if (allPropertyValues.CurrentObject != ifcSpace)
            {
                allPropertyValues.SetAllPropertyValues(ifcSpace);
            }

            //try and find it in the attached properties of the ifcSpace
            string value = allPropertyValues.GetPropertySingleValueValue("GrossFloorArea", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("GSA", true);

            if (value == DEFAULT_STRING)
                return DEFAULT_NUMERIC;
            else
            {
                if (double.TryParse(value, out areavalue))
                {
                    //if ((!string.IsNullOrEmpty(areaUnit)) && (areaUnit.ToLower().Contains("milli")) && (areavalue > 250000.0))//we are using millimetres, and areavalue is lightly to be in mmsq if over 250000(0.5msq)
                    //    areavalue = areavalue / 1000000.0;
                    return areavalue.ToString();
                }
                return value; 
            }
                
            
        }
        /// <summary>
        /// Get space usable height
        /// </summary>
        /// <param name="ifcSpace">IfcSpace object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetUsableHeight(IfcSpace ifcSpace, COBieDataPropertySetValues allPropertyValues)
        {
            IfcLengthMeasure usableHt = ifcSpace.GetHeight();
            if (usableHt != null)
            return ((double)usableHt).ToString();
            
            //Fall back to properties
            //get the property single values for this ifcSpace
            if (allPropertyValues.CurrentObject != ifcSpace)
            {
                allPropertyValues.SetAllPropertyValues(ifcSpace);
            }

            //try and find it in the attached properties of the ifcSpace
            string value = allPropertyValues.GetPropertySingleValueValue("UsableHeight", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("FinishCeiling", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("FinishCeilingHeight", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("Height", true);
            
            if (value == DEFAULT_STRING)
                return DEFAULT_NUMERIC;
            else
                return value; 
        }
        /// <summary>
        /// Get space description 
        /// </summary>
        /// <param name="ifcSpace">IfcSpace object</param>
        /// <returns>property value as string or default value</returns>
        private string GetSpaceDescription(IfcSpace ifcSpace)
        {
            if (ifcSpace != null)
            {
                if (!string.IsNullOrEmpty(ifcSpace.LongName)) return ifcSpace.LongName;
                else if (!string.IsNullOrEmpty(ifcSpace.Description)) return ifcSpace.Description;
                else if (!string.IsNullOrEmpty(ifcSpace.Name)) return ifcSpace.Name;
                
                
            }
            return DEFAULT_STRING;
        }
        /// <summary>
        /// Get space room tag 
        /// </summary>
        /// <param name="ifcSpace">IfcSpace object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetRoomTag(IfcSpace ifcSpace, COBieDataPropertySetValues allPropertyValues)
        {
            //if (!string.IsNullOrEmpty(ifcSpace.Name)) return ifcSpace.Name;

            string value = ""; // GetSpaceDescription(ifcSpace);
            if (allPropertyValues.CurrentObject != ifcSpace)
            {
                allPropertyValues.SetAllPropertyValues(ifcSpace);
            }
            //try and find it in the attached properties of the ifcSpace
            value = allPropertyValues.GetPropertySingleValueValue("RoomTag", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("Tag", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("Room_Tag", true);
            return value;
        }
        #endregion

        COBieSheet<COBieAttributeRow> _attributes;

        public void InitialiseAttributes(ref COBieSheet<COBieAttributeRow> attributeSheet)
        {
            _attributes = attributeSheet;
        }
    }
}
