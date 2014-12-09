using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.XbimExtensions;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.ConstructionMgmtDomain;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.COBie.Serialisers.XbimSerialiser;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Spare tab.
    /// </summary>
    public class COBieDataSpare : COBieData<COBieSpareRow>, IAttributeProvider
    {

        /// <summary>
        /// Data Spare constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataSpare(COBieContext context) : base(context)
        { }

        #region Methods

        /// <summary>
        /// Fill sheet rows for Spare sheet
        /// </summary>
        /// <returns>COBieSheet<COBieSpareRow></returns>
        public override COBieSheet<COBieSpareRow> Fill()
        {
            ProgressIndicator.ReportMessage("Starting Spares...");
            //Create new sheet
            COBieSheet<COBieSpareRow> spares = new COBieSheet<COBieSpareRow>(Constants.WORKSHEET_SPARE);
                        // get all IfcBuildingStory objects from IFC file
            IEnumerable<IfcConstructionProductResource> ifcConstructionProductResources = Model.Instances.OfType<IfcConstructionProductResource>();

            COBieDataPropertySetValues allPropertyValues = new COBieDataPropertySetValues(); //properties helper class
            COBieDataAttributeBuilder attributeBuilder = new COBieDataAttributeBuilder(Context, allPropertyValues);
            attributeBuilder.InitialiseAttributes(ref _attributes);
            attributeBuilder.RowParameters["Sheet"] = "Spare";
            //set up filters on COBieDataPropertySetValues
            attributeBuilder.ExcludeAttributePropertyNames.AddRange(Context.Exclude.Spare.AttributesEqualTo);
            attributeBuilder.ExcludeAttributePropertyNamesWildcard.AddRange(Context.Exclude.Spare.AttributesContain);
            

            //IfcTypeObject typeObject = Model.Instances.OfType<IfcTypeObject>().FirstOrDefault();

            ProgressIndicator.Initialise("Creating Spares", ifcConstructionProductResources.Count());

            foreach (IfcConstructionProductResource ifcConstructionProductResource in ifcConstructionProductResources)
            {
                ProgressIndicator.IncrementAndUpdate();

                COBieSpareRow spare = new COBieSpareRow(spares);
                //set allPropertyValues to this element
                allPropertyValues.SetAllPropertyValues(ifcConstructionProductResource); //set the internal filtered IfcPropertySingleValues List in allPropertyValues
                
                spare.Name = (string.IsNullOrEmpty(ifcConstructionProductResource.Name)) ? "" : ifcConstructionProductResource.Name.ToString();

                string createBy = allPropertyValues.GetPropertySingleValueValue("COBieCreatedBy", false); //support for COBie Toolkit for Autodesk Revit
                spare.CreatedBy = ValidateString(createBy) ? createBy : GetTelecomEmailAddress(ifcConstructionProductResource.OwnerHistory);
                string createdOn = allPropertyValues.GetPropertySingleValueValue("COBieCreatedOn", false);//support for COBie Toolkit for Autodesk Revit
                spare.CreatedOn = ValidateString(createdOn) ? createdOn : GetCreatedOnDateAsFmtString(ifcConstructionProductResource.OwnerHistory);

                spare.Category = GetCategory(ifcConstructionProductResource);

                spare.TypeName = GetObjectType(ifcConstructionProductResource);

                string extSystem = allPropertyValues.GetPropertySingleValueValue("COBieExtSystem", false);//support for COBie Toolkit for Autodesk Revit
                spare.ExtSystem = ValidateString(extSystem) ? extSystem : GetExternalSystem(ifcConstructionProductResource);
                spare.ExtObject = ifcConstructionProductResource.GetType().Name;
                spare.ExtIdentifier = ifcConstructionProductResource.GlobalId;
                string description = allPropertyValues.GetPropertySingleValueValue("COBieDescription", false);//support for COBie Toolkit for Autodesk Revit
                if (ValidateString(description))
                    spare.Description = description;
                else
                    spare.Description = (ifcConstructionProductResource == null) ? "" : ifcConstructionProductResource.Description.ToString();

                //get information from Pset_Spare_COBie property set 
                IfcPropertySingleValue ifcPropertySingleValue = null;
                IfcPropertySet ifcPropertySet =  ifcConstructionProductResource.GetPropertySet("Pset_Spare_COBie");
                if (ifcPropertySet != null)
                {
                    ifcPropertySingleValue = ifcPropertySet.HasProperties.Where<IfcPropertySingleValue>(p => p.Name == "Suppliers").FirstOrDefault();
                    spare.Suppliers = ((ifcPropertySingleValue != null) && (!string.IsNullOrEmpty(ifcPropertySingleValue.NominalValue.ToString()))) ? ifcPropertySingleValue.NominalValue.ToString() : DEFAULT_STRING;

                    ifcPropertySingleValue = ifcPropertySet.HasProperties.Where<IfcPropertySingleValue>(p => p.Name == "SetNumber").FirstOrDefault();
                    spare.SetNumber = ((ifcPropertySingleValue != null) && (!string.IsNullOrEmpty(ifcPropertySingleValue.NominalValue.ToString()))) ? ifcPropertySingleValue.NominalValue.ToString() : DEFAULT_STRING; ;

                    ifcPropertySingleValue = ifcPropertySet.HasProperties.Where<IfcPropertySingleValue>(p => p.Name == "PartNumber").FirstOrDefault();
                    spare.PartNumber = ((ifcPropertySingleValue != null) && (!string.IsNullOrEmpty(ifcPropertySingleValue.NominalValue.ToString()))) ? ifcPropertySingleValue.NominalValue.ToString() : DEFAULT_STRING; ;
                }
                else
                {
                    spare.Suppliers = DEFAULT_STRING;
                    spare.SetNumber = DEFAULT_STRING;
                    spare.PartNumber = DEFAULT_STRING;
                }
                if ((spare.Name == DEFAULT_STRING) && (spare.TypeName == DEFAULT_STRING) && (spare.Description == DEFAULT_STRING))
                {
                    continue;
                }
                spares.AddRow(spare);

                //----------fill in the attribute information for spaces-----------

                //fill in the attribute information
                attributeBuilder.RowParameters["Name"] = spare.Name;
                attributeBuilder.RowParameters["CreatedBy"] = spare.CreatedBy;
                attributeBuilder.RowParameters["CreatedOn"] = spare.CreatedOn;
                attributeBuilder.RowParameters["ExtSystem"] = spare.ExtSystem;
                attributeBuilder.PopulateAttributesRows(ifcConstructionProductResource); //fill attribute sheet rows//pass data from this sheet info as Dictionary
                
            }

            spares.OrderBy(s => s.Name);

            ProgressIndicator.Finalise();
            return spares;
        }

        /// <summary>
        /// Get the object IfcTypeObject name from the IfcConstructionProductResource object
        /// </summary>
        /// <param name="ifcConstructionProductResource">IfcConstructionProductResource object</param>
        /// <returns>string holding IfcTypeObject name</returns>
        private string GetObjectType(IfcConstructionProductResource ifcConstructionProductResource)
        {
            //first try on ResourceOf.RelatedObjects
            IEnumerable<IfcTypeObject> ifcTypeObjects = ifcConstructionProductResource.ResourceOf.SelectMany(ro => ro.RelatedObjects).OfType<IfcTypeObject>();
            
            //second try on IsDefinedBy.OfType<IfcRelDefinesByType>
            if ((ifcTypeObjects == null) || (ifcTypeObjects.Count() == 0))
                ifcTypeObjects  = ifcConstructionProductResource.IsDefinedBy.OfType<IfcRelDefinesByType>().Select(idb => (idb as IfcRelDefinesByType).RelatingType);
            
            //third try on IsDefinedBy.OfType<IfcRelDefinesByProperties> for DefinesType
            if ((ifcTypeObjects == null) || (ifcTypeObjects.Count() == 0))
                ifcTypeObjects = ifcConstructionProductResource.IsDefinedBy.OfType<IfcRelDefinesByProperties>().SelectMany(idb => (idb as IfcRelDefinesByProperties).RelatingPropertyDefinition.DefinesType);
            
            //convert to string and return if all ok
            if ((ifcTypeObjects != null) || (ifcTypeObjects.Count() > 0))
            {
                List<string> strList = new List<string>();
                foreach (IfcTypeObject ifcTypeItem in ifcTypeObjects)
                {
                    if ((ifcTypeItem != null) && (!string.IsNullOrEmpty(ifcTypeItem.Name.ToString())))
                    {
                        if (!strList.Contains(ifcTypeItem.Name.ToString()))
                            strList.Add(ifcTypeItem.Name.ToString());
                    }
                }
                return (strList.Count > 0) ? COBieXBim.JoinStrings(':', strList) : DEFAULT_STRING;
            }


            //last try on IsDefinedBy.OfType<IfcRelDefinesByProperties> for IfcObject
            if ((ifcTypeObjects == null) || (ifcTypeObjects.Count() == 0))
            {
                IEnumerable<IfcObject> ifcObjects = ifcConstructionProductResource.IsDefinedBy.OfType<IfcRelDefinesByProperties>().SelectMany(idb => idb.RelatedObjects);
                List<string> strList = new List<string>();
                foreach (IfcObject ifcObject in ifcObjects)
                {
                    IEnumerable<IfcRelDefinesByType> ifcRelDefinesByTypes = ifcObject.IsDefinedBy.OfType<IfcRelDefinesByType>();
                    foreach (IfcRelDefinesByType ifcRelDefinesByType in ifcRelDefinesByTypes)
                    {
                        if ((ifcRelDefinesByType != null) &&
                            (ifcRelDefinesByType.RelatingType != null) &&
                            (!string.IsNullOrEmpty(ifcRelDefinesByType.RelatingType.Name.ToString()))
                            )
                        {
                            if (!strList.Contains(ifcRelDefinesByType.RelatingType.Name.ToString()))
                                strList.Add(ifcRelDefinesByType.RelatingType.Name.ToString());
                        }
                    }
                    return (strList.Count > 0) ? COBieXBim.JoinStrings(':', strList) : DEFAULT_STRING;
                }
                return (strList.Count > 0) ? COBieXBim.JoinStrings(':', strList) : DEFAULT_STRING;
            }

            return DEFAULT_STRING; //fail to get any types
        }
        #endregion

        COBieSheet<COBieAttributeRow> _attributes;

        public void InitialiseAttributes(ref COBieSheet<COBieAttributeRow> attributeSheet)
        {
            _attributes = attributeSheet;
        }
    }
}
