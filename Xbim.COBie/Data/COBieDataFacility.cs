using System.Collections.Generic;
using System.Linq;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.QuantityResource;


namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Facility tab.
    /// </summary>
    public class COBieDataFacility : COBieData<COBieFacilityRow>, IAttributeProvider
    {
        /// <summary>
        /// Data Facility constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataFacility(COBieContext context) : base(context)
        { }

      
        #region Methods

        /// <summary>
        /// Fill sheet rows for Facility sheet
        /// </summary>
        /// <returns>COBieSheet</returns>
        public override COBieSheet<COBieFacilityRow> Fill()
        {
            ProgressIndicator.ReportMessage("Starting Facilities...");

            //Create new sheet
            COBieSheet<COBieFacilityRow> facilities = new COBieSheet<COBieFacilityRow>(Constants.WORKSHEET_FACILITY);

            IfcProject ifcProject = Model.IfcProject as IfcProject;
            IfcSite ifcSite = Model.Instances.OfType<IfcSite>().FirstOrDefault();
            IfcBuilding ifcBuilding = Model.Instances.OfType<IfcBuilding>().FirstOrDefault();

            //get Element Quantity holding area values as used for AreaMeasurement below
            IfcElementQuantity ifcElementQuantityAreas = Model.Instances.OfType<IfcElementQuantity>().Where(eq => eq.Quantities.OfType<IfcQuantityArea>().Count() > 0).FirstOrDefault();
           
            List<IfcObject> ifcObjectList = new List<IfcObject>();
            if (ifcProject != null) ifcObjectList.Add(ifcProject);
            if (ifcSite != null) ifcObjectList.Add(ifcSite);
            if (ifcBuilding != null)  ifcObjectList.Add(ifcBuilding);

            IEnumerable<IfcObject> ifcObjects = ifcObjectList.AsEnumerable();
            if (ifcObjects.Any())
            {
                COBieDataPropertySetValues allPropertyValues = new COBieDataPropertySetValues(); //properties helper class
                COBieDataAttributeBuilder attributeBuilder = new COBieDataAttributeBuilder(Context, allPropertyValues);
                attributeBuilder.InitialiseAttributes(ref _attributes);

                //list of attributes to exclude form attribute sheet
                //set up filters on COBieDataPropertySetValues for the SetAttributes only
                attributeBuilder.ExcludeAttributePropertyNames.AddRange(Context.Exclude.Facility.AttributesEqualTo);
                attributeBuilder.ExcludeAttributePropertyNamesWildcard.AddRange(Context.Exclude.Facility.AttributesContain);
                attributeBuilder.RowParameters["Sheet"] = "Facility";

                COBieFacilityRow facility = new COBieFacilityRow(facilities);

                string name = "";
                if ((ifcBuilding != null) && (!string.IsNullOrEmpty(ifcBuilding.Name)))
                    name = ifcBuilding.Name;
                else if ((ifcSite != null) && (!string.IsNullOrEmpty(ifcSite.Name)))
                    name = ifcSite.Name;
                else if ((ifcProject != null) && (!string.IsNullOrEmpty(ifcProject.Name)))
                    name = ifcProject.Name;
                else
                    name = DEFAULT_STRING;

                facility.Name = (string.IsNullOrEmpty(name)) ? "The Facility Name Here" : name;

                IfcValue createBy = ifcBuilding.GetPropertySingleNominalValue("Other", "COBieCreatedBy");//support for COBie Toolkit for Autodesk Revit
                facility.CreatedBy = ((createBy != null) && ValidateString(createBy.ToString())) ? createBy.ToString() : GetTelecomEmailAddress(ifcBuilding.OwnerHistory);
                IfcValue createdOn = ifcBuilding.GetPropertySingleNominalValue("Other", "COBieCreatedOn");//support for COBie Toolkit for Autodesk Revit
                facility.CreatedOn = ((createdOn != null) && ValidateString(createdOn.ToString())) ? createdOn.ToString() : GetCreatedOnDateAsFmtString(ifcBuilding.OwnerHistory);

                facility.Category = GetCategory(ifcBuilding);

                facility.ProjectName = GetFacilityProjectName(ifcProject);
                facility.SiteName = GetFacilitySiteName(ifcSite);

                facility.LinearUnits = Context.WorkBookUnits.LengthUnit;
                facility.AreaUnits = Context.WorkBookUnits.AreaUnit;
                facility.VolumeUnits = Context.WorkBookUnits.VolumeUnit;
                facility.CurrencyUnit = Context.WorkBookUnits.MoneyUnit;

                string AreaMeasurement = (ifcElementQuantityAreas == null) ? DEFAULT_STRING : ifcElementQuantityAreas.MethodOfMeasurement.ToString();

                facility.AreaMeasurement = ((AreaMeasurement == DEFAULT_STRING) || (AreaMeasurement.ToLower().Contains("bim area"))) ? AreaMeasurement : AreaMeasurement + " BIM Area";
                facility.ExternalSystem = GetExternalSystem(ifcBuilding);

                facility.ExternalProjectObject = "IfcProject";
                facility.ExternalProjectIdentifier = ifcProject.GlobalId;

                facility.ExternalSiteObject = "IfcSite";
                facility.ExternalSiteIdentifier = (ifcSite != null) ? ifcSite.GlobalId.ToString() : DEFAULT_STRING;

                facility.ExternalFacilityObject = "IfcBuilding";
                facility.ExternalFacilityIdentifier = ifcBuilding.GlobalId;

                facility.Description = GetFacilityDescription(ifcBuilding);
                facility.ProjectDescription = GetFacilityProjectDescription(ifcProject);
                facility.SiteDescription = GetFacilitySiteDescription(ifcSite);
                facility.Phase = (string.IsNullOrEmpty(Model.IfcProject.Phase.ToString())) ? DEFAULT_STRING : Model.IfcProject.Phase.ToString();

                facilities.AddRow(facility);


                //fill in the attribute information
                foreach (IfcObject ifcObject in ifcObjects)
                {
                    attributeBuilder.RowParameters["Name"] = facility.Name;
                    attributeBuilder.RowParameters["CreatedBy"] = facility.CreatedBy;
                    attributeBuilder.RowParameters["CreatedOn"] = facility.CreatedOn;
                    attributeBuilder.RowParameters["ExtSystem"] = facility.ExternalSystem;
                    attributeBuilder.PopulateAttributesRows(ifcObject); //fill attribute sheet rows//pass data from this sheet info as Dictionary
                }
            }

            facilities.OrderBy(s => s.Name);

            ProgressIndicator.Finalise();
            return facilities;
        }

       

        private string GetFacilityDescription(IfcBuilding ifcBuilding)
        {
            if (ifcBuilding != null)
            {
                if (!string.IsNullOrEmpty(ifcBuilding.LongName)) return ifcBuilding.LongName;
                else if (!string.IsNullOrEmpty(ifcBuilding.Description)) return ifcBuilding.Description;
                else if (!string.IsNullOrEmpty(ifcBuilding.Name)) return ifcBuilding.Name;
            }
            return Constants.DEFAULT_STRING;
        }

        private string GetFacilityProjectDescription(IfcProject ifcProject)
        {
            if (ifcProject != null)
            {
                if (!string.IsNullOrEmpty(ifcProject.LongName)) return ifcProject.LongName;
                else if (!string.IsNullOrEmpty(ifcProject.Description)) return ifcProject.Description;
                else if (!string.IsNullOrEmpty(ifcProject.Name)) return ifcProject.Name;
            }
            return "Project Description";
        }

        private string GetFacilitySiteDescription(IfcSite ifcSite)
        {
            if (ifcSite != null)
            {
                if (!string.IsNullOrEmpty(ifcSite.LongName)) return ifcSite.LongName;
                else if (!string.IsNullOrEmpty(ifcSite.Description)) return ifcSite.Description;
                else if (!string.IsNullOrEmpty(ifcSite.Name)) return ifcSite.Name;
            }
            return "Site Description";
        }

        private string GetFacilitySiteName(IfcSite ifcSite)
        {
            if (ifcSite != null)
            {
                if (!string.IsNullOrEmpty(ifcSite.Name)) return ifcSite.Name;
                else if (!string.IsNullOrEmpty(ifcSite.LongName)) return ifcSite.LongName;
                else if (!string.IsNullOrEmpty(ifcSite.GlobalId)) return ifcSite.GlobalId;
            }
            return "Site Name";
        }

        private string GetFacilityProjectName(IfcProject ifcProject)
        {
            if (ifcProject != null)
            {
                if (!string.IsNullOrEmpty(ifcProject.Name)) return ifcProject.Name;
                else if (!string.IsNullOrEmpty(ifcProject.LongName)) return ifcProject.LongName;
                else if (!string.IsNullOrEmpty(ifcProject.GlobalId)) return ifcProject.GlobalId;
            }
            return "Site Name";
        }

        

        

        

        
        
        #endregion

        COBieSheet<COBieAttributeRow> _attributes;

        public void InitialiseAttributes(ref COBieSheet<COBieAttributeRow> attributeSheet)
        {
            _attributes = attributeSheet;
        }
    }
}
