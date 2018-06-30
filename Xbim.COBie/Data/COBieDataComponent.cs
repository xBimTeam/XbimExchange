using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBie.Rows;
using Xbim.Common.Geometry;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.PropertyResource;

#if DEBUG
using System.Diagnostics;
#endif

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Holds Geometry information for the space objects
    /// </summary>
    public struct SpaceInfo
    {
        public string Name { get; set; }
        public XbimRect3D Rectangle { get; set; }
        public XbimMatrix3D Matrix { get; set; }
    }
    
    /// <summary>
    /// Class to input data into excel worksheets for the the Component tab.
    /// </summary>
    public class COBieDataComponent : COBieData<COBieComponentRow>, IAttributeProvider
    {
        /// <summary>
        /// Data Component constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataComponent(COBieContext context)
            : base(context)
        {
            SpaceBoundingBoxInfo = new List<SpaceInfo>();
        }

        public List<SpaceInfo> SpaceBoundingBoxInfo { get; set; }

        #region Methods

        /// <summary>
        /// Fill sheet rows for Component sheet
        /// </summary>
        /// <returns>COBieSheet</returns>
        public override COBieSheet<COBieComponentRow> Fill()
        {
#if DEBUG
            Stopwatch timer = new Stopwatch();
            timer.Start();
#endif
            ProgressIndicator.ReportMessage("Starting Components...");
            //Create new sheet
            COBieSheet<COBieComponentRow> components = new COBieSheet<COBieComponentRow>(Constants.WORKSHEET_COMPONENT);
         
            

            IEnumerable<IfcRelAggregates> relAggregates = Model.FederatedInstances.OfType<IfcRelAggregates>();
            IEnumerable<IfcRelContainedInSpatialStructure> relSpatial = Model.FederatedInstances.OfType<IfcRelContainedInSpatialStructure>();

            IEnumerable<IfcObject> ifcElements = ((from x in relAggregates
                                            from y in x.RelatedObjects
                                                   where !Context.Exclude.ObjectType.Component.Contains(y.GetType())
                                            select y).Union(from x in relSpatial
                                                            from y in x.RelatedElements
                                                            where !Context.Exclude.ObjectType.Component.Contains(y.GetType())
                                                            select y)).OfType<IfcObject>(); //.GroupBy(el => el.Name).Select(g => g.First())//.Distinct().ToList();
            
            COBieDataPropertySetValues allPropertyValues = new COBieDataPropertySetValues(); //properties helper class
            COBieDataAttributeBuilder attributeBuilder = new COBieDataAttributeBuilder(Context, allPropertyValues);
            attributeBuilder.InitialiseAttributes(ref _attributes);
            //set up filters on COBieDataPropertySetValues for the SetAttributes only
            attributeBuilder.ExcludeAttributePropertyNames.AddRange(Context.Exclude.Component.AttributesEqualTo); //we do not want listed properties for the attribute sheet so filter them out
            attributeBuilder.ExcludeAttributePropertyNamesWildcard.AddRange(Context.Exclude.Component.AttributesContain);//we do not want listed properties for the attribute sheet so filter them out
            attributeBuilder.RowParameters["Sheet"] = "Component";


            ProgressIndicator.Initialise("Creating Components", ifcElements.Count());

            foreach (var obj in ifcElements)
            {
                ProgressIndicator.IncrementAndUpdate();
                
                COBieComponentRow component = new COBieComponentRow(components);
                
                IfcElement el = obj as IfcElement;
                if (el == null)
                    continue;
                string name = el.Name.ToString();
                if (string.IsNullOrEmpty(name))
                {
                    name = "Name Unknown " + UnknownCount.ToString();
                    UnknownCount++;
                }
                //set allPropertyValues to this element
                allPropertyValues.SetAllPropertyValues(el); //set the internal filtered IfcPropertySingleValues List in allPropertyValues
                component.Name = name;

                string createBy = allPropertyValues.GetPropertySingleValueValue("COBieCreatedBy", false); //support for COBie Toolkit for Autodesk Revit
                component.CreatedBy = ValidateString(createBy) ? createBy : GetTelecomEmailAddress(el.OwnerHistory);
                string createdOn = allPropertyValues.GetPropertySingleValueValue("COBieCreatedOn", false);//support for COBie Toolkit for Autodesk Revit
                component.CreatedOn = ValidateString(createdOn) ?  createdOn : GetCreatedOnDateAsFmtString(el.OwnerHistory);
                
                component.TypeName = GetTypeName(el);
                component.Space = COBieHelpers.GetComponentRelatedSpace(el, Model, SpaceBoundingBoxInfo, Context);
                string description = allPropertyValues.GetPropertySingleValueValue("COBieDescription", false);//support for COBie Toolkit for Autodesk Revit
                component.Description = ValidateString(description) ? description : GetComponentDescription(el);
                string extSystem = allPropertyValues.GetPropertySingleValueValue("COBieExtSystem", false);//support for COBie Toolkit for Autodesk Revit
                component.ExtSystem = ValidateString(extSystem) ? extSystem : GetExternalSystem(el);
                component.ExtObject = el.GetType().Name;
                component.ExtIdentifier = el.GlobalId;

                //set from PropertySingleValues filtered via candidateProperties
                //set the internal filtered IfcPropertySingleValues List in allPropertyValues to this element set above
                component.SerialNumber = allPropertyValues.GetPropertySingleValueValue("SerialNumber", false);
                component.InstallationDate = GetDateFromProperty(allPropertyValues, "InstallationDate");
                component.WarrantyStartDate = GetDateFromProperty(allPropertyValues, "WarrantyStartDate");
                component.TagNumber = allPropertyValues.GetPropertySingleValueValue("TagNumber", false);
                component.BarCode = allPropertyValues.GetPropertySingleValueValue("BarCode", false);
                component.AssetIdentifier = allPropertyValues.GetPropertySingleValueValue("AssetIdentifier", false);
                
                components.AddRow(component);

                //fill in the attribute information
                attributeBuilder.RowParameters["Name"] = component.Name;
                attributeBuilder.RowParameters["CreatedBy"] = component.CreatedBy;
                attributeBuilder.RowParameters["CreatedOn"] = component.CreatedOn;
                attributeBuilder.RowParameters["ExtSystem"] = component.ExtSystem;
                attributeBuilder.PopulateAttributesRows(el); //fill attribute sheet rows
            }

            components.OrderBy(s=>s.Name);

            ProgressIndicator.Finalise();
#if DEBUG
            timer.Stop();
            Console.WriteLine("Time to generate Component data = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3"));
#endif
           
            
            return components;
        }



        /// <summary>
        /// Get Formatted Start Date
        /// </summary>
        /// <param name="allPropertyValues"></param>
        /// <returns></returns>
        private string GetDateFromProperty (COBieDataPropertySetValues allPropertyValues, string propertyName)
        {
            string startData = "";
            IfcPropertySingleValue ifcPropertySingleValue = allPropertyValues.GetPropertySingleValue (propertyName);
            if (ifcPropertySingleValue != null)
            {
                if (ifcPropertySingleValue.NominalValue != null)
                {
                    startData = ifcPropertySingleValue.NominalValue.ToString ();
                }
            }
            else
            {
                startData = allPropertyValues.GetPropertyValue (propertyName, false);
            }

            DateTime frmDate;
            if (DateTime.TryParse(startData, out frmDate))
                startData = frmDate.ToString(Constants.DATE_FORMAT);
            else if (string.IsNullOrEmpty(startData))
                startData = Constants.DEFAULT_STRING;//Context.RunDate;
            
            return startData;
        }

        /// <summary>
        /// Get Description for passed in IfcElement
        /// </summary>
        /// <param name="el">Element holding description</param>
        /// <returns>string</returns>
        internal string GetComponentDescription(IfcElement el)
        {
            if (el != null)
            {
                if (!string.IsNullOrEmpty(el.Description)) return el.Description;
                else if (!string.IsNullOrEmpty(el.Name)) return el.Name;
            }
            return DEFAULT_STRING;
        }
        #endregion

        COBieSheet<COBieAttributeRow> _attributes;

        public void InitialiseAttributes(ref COBieSheet<COBieAttributeRow> attributeSheet)
        {
            _attributes = attributeSheet;
        }
    }
}
