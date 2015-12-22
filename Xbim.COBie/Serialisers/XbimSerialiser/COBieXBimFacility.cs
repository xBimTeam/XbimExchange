using System;
using System.Globalization;
using System.Linq;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.GeometricConstraintResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.QuantityResource;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimFacility : COBieXBim
    {
        public COBieXBimFacility(COBieXBimContext xBimContext)
            : base(xBimContext)
        {

        }

        #region Methods
        /// <summary>
        /// Create and setup objects held in the Facility COBieSheet
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieFacilityRows to read data from</param>
        public void SerialiseFacility(COBieSheet<COBieFacilityRow> cOBieSheet)
        {
            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Facility"))
            {
                try
                {
                    ProgressIndicator.ReportMessage("Starting Facility...");
                    ProgressIndicator.Initialise("Creating Facility", 3);
                    
                    //Assume we have only one site and one building to a project
                    COBieFacilityRow row = cOBieSheet[0];
                    ProgressIndicator.IncrementAndUpdate();
                    SetUpProject(row);

                    ProgressIndicator.IncrementAndUpdate();
                    CreateSite(row);

                    ProgressIndicator.IncrementAndUpdate();
                    CreateBuilding(row);

                    //set up relationships
                    GetSite().AddBuilding(GetBuilding());
                    Model.IfcProject.AddSite(GetSite());

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
        /// Create and setup IfcSite object
        /// </summary>
        /// <param name="row">COBieFacilityRow object to read data from</param>
        /// <returns>IfcSite object</returns>
        private void CreateSite(COBieFacilityRow row)
        {
            IfcSite ifcSite = Model.Instances.New<IfcSite>();
            
            //set owner history
            SetNewOwnerHistory(ifcSite, row.ExternalSystem, Model.DefaultOwningUser, row.CreatedOn);
            //using statement will set the Model.OwnerHistoryAddObject to ifcSite.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
            using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcSite.OwnerHistory))
            {
                AddGlobalId(row.ExternalSiteIdentifier, ifcSite);
                if (ValidateString(row.SiteName))
                    ifcSite.Name = row.SiteName;
                ifcSite.CompositionType = IfcElementCompositionEnum.ELEMENT;
                IfcLocalPlacement lp = Model.Instances.New<IfcLocalPlacement>();
                lp.RelativePlacement = WCS;
                ifcSite.ObjectPlacement = lp;

                if (ValidateString(row.SiteDescription))
                    ifcSite.Description = row.SiteDescription;

            }
        }


        /// <summary>
        /// Create and setup the IfcBuilding building object
        /// </summary>
        /// <param name="row">COBieFacilityRow object to read data from</param>
        private void CreateBuilding(COBieFacilityRow row)
        {
            IfcBuilding ifcBuilding = Model.Instances.New<IfcBuilding>();
            SetNewOwnerHistory(ifcBuilding, row.ExternalSystem, Model.DefaultOwningUser, row.CreatedOn);

            //using statement will set the Model.OwnerHistoryAddObject to ifcBuilding.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
            using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcBuilding.OwnerHistory))
            {
                AddGlobalId(row.ExternalFacilityIdentifier, ifcBuilding);
                if (ValidateString(row.Name))
                    ifcBuilding.Name = row.Name;
                //add category
                AddCategory(row.Category, ifcBuilding);

                if (ValidateString(row.Description))
                    ifcBuilding.Description = row.Description;
                if (ValidateString(row.AreaMeasurement))
                {
                    SetAreaMeasure(ifcBuilding, row);
                }

                ifcBuilding.CompositionType = IfcElementCompositionEnum.ELEMENT;
                IfcLocalPlacement lp = Model.Instances.New<IfcLocalPlacement>();
                lp.RelativePlacement = WCS;
                lp.PlacementRelTo = GetSite().ObjectPlacement;
                ifcBuilding.ObjectPlacement = lp;
               

            }
            
        }

        /// <summary>
        /// Set the area measure to the building
        /// </summary>
        /// <param name="ifcBuilding">Building object</param>
        /// <param name="row">COBieFacilityRow object holding data</param>
        private void SetAreaMeasure(IfcBuilding ifcBuilding, COBieFacilityRow row)
        {
            IfcSIUnit ifcSIUnitArea = null;
           if (ValidateString(row.AreaUnits))
            {
                ifcSIUnitArea = GetSIUnit(row.AreaUnits);
            }
           string areaMeasure = string.Empty;
           if (ValidateString(row.AreaMeasurement))
           {
               areaMeasure = row.AreaMeasurement;
           }
            
           IfcQuantityArea IfcQuantityArea = Model.Instances.New<IfcQuantityArea>(qa => 
                                                { 
                                                    qa.Unit = ifcSIUnitArea;
                                                    qa.Name = "AreaMeasure";
                                                    qa.Description = "Created to maintain COBie information";
                                               });
           IfcElementQuantity ifcElementQuantity = Model.Instances.New<IfcElementQuantity>(eq =>
                                                        {
                                                            eq.Quantities.Add(IfcQuantityArea);
                                                            eq.MethodOfMeasurement = areaMeasure;
                                                            eq.Description = "Created to maintain COBie information";
 
                                                        });
           IfcRelDefinesByProperties ifcRelDefinesByProperties = Model.Instances.New<IfcRelDefinesByProperties>(rdbp =>
                                                               {
                                                                   rdbp.RelatedObjects.Add(ifcBuilding);
                                                                   rdbp.RelatingPropertyDefinition = ifcElementQuantity;
                                                                   rdbp.Description = "Created to maintain COBie information";
                                                               });
        }

        /// <summary>
        /// SetUp the Model Project Object
        /// </summary>
        /// <param name="row">COBieFacilityRow object to read data from</param>
        private void SetUpProject(COBieFacilityRow row)
        {
            IfcProject ifcProject = Model.IfcProject;
            ifcProject.Initialize(ProjectUnits.SIUnitsUK);
            SetOwnerHistory(ifcProject, row.ExternalSystem, Model.DefaultOwningUser, row.CreatedOn);
            //using statement will set the Model.OwnerHistoryAddObject to ifcProject.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
            using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcProject.OwnerHistory))
            {
                if (ValidateString(row.ProjectName))
                    ifcProject.Name = row.ProjectName;

                if (ValidateString(row.ProjectDescription))
                    ifcProject.Description = row.ProjectDescription;
                if (ValidateString(row.Phase))
                    ifcProject.Phase = row.Phase;
                if (ValidateString(row.LinearUnits))
                {
                    SetUnitToProject(IfcUnitEnum.LENGTHUNIT, row.LinearUnits);

                }

                AddGlobalId(row.ExternalProjectIdentifier, ifcProject);


                if (ValidateString(row.AreaUnits))
                {
                    SetUnitToProject(IfcUnitEnum.AREAUNIT, row.AreaUnits);

                }

                if (ValidateString(row.VolumeUnits))
                {
                    SetUnitToProject(IfcUnitEnum.VOLUMEUNIT, row.VolumeUnits);

                }

                if (ValidateString(row.CurrencyUnit))
                {
                    SetMonetaryUnit(row.CurrencyUnit);
                }
            }
            
        }

        /// <summary>
        /// Convert string back into IfcSIUnitName and IfcSIPrefix enumerations
        /// </summary>
        /// <param name="unitType">IfcUnitEnum unit type</param>
        /// <param name="value">string representing the unit type</param>
        private void SetUnitToProject(IfcUnitEnum unitType, string value)
        {
            IfcSIUnitName? returnUnit;
            IfcSIPrefix? returnPrefix;

            IfcUnitAssignment ifcUnitAssignment = GetProjectUnitAssignment();
            if (GetUnitEnumerations(value, out returnUnit, out returnPrefix))
            {
                ifcUnitAssignment.SetOrChangeSIUnit(unitType, (IfcSIUnitName)returnUnit, returnPrefix);
            }
            else
            {
                ConversionBasedUnit conversionBasedUnit;
                value = value.Trim().Replace(" ", "_").ToUpper();
                //see if the passed value contains a ConversionBasedUnit value, i.e INCHS would match ConversionBasedUnit.INCH
                string[] cBasedUnits = Enum.GetNames(typeof(ConversionBasedUnit));
                if (!cBasedUnits.Contains(value))
                {
                    foreach (string str in cBasedUnits)
                    {
                        if (str == value)
                        {
                            break;
                        }
                        string test = str.Split('_').First();
                        //try both ways
                        if ((value.Contains(test)) ||
                            (test.Contains(value))
                            )
                        {
                            value = str;
                            break;
                        }

                    }
                }
                
                if (Enum.TryParse(value, out conversionBasedUnit))
                {
                    ifcUnitAssignment.SetOrChangeConversionUnit(unitType, conversionBasedUnit);
                }
            }
        }

        

        /// <summary>
        /// Get Monetary Unit
        /// </summary>
        ///<param name="value">string representing the currency type</param>
        private void SetMonetaryUnit(string value)
        {

            string code = "";
            //TODO: Convert currency to match pick list
            //convert to pick list hard coded for now
            if (!string.IsNullOrEmpty(value))
            {
                if (value == "Dollars")
                    code = "USD";
                else if (value == "Euros")
                    code = "EUR";
                else if (value == "Pounds")
                    code = "GBP";
            }
            if (string.IsNullOrEmpty(code))
            {
                code = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                                 .Where(c => new RegionInfo(c.LCID).CurrencyEnglishName == value)
                                 .Select(c => new RegionInfo(c.LCID).ISOCurrencySymbol)
                                 .FirstOrDefault();
            }
            IfcCurrencyEnum enumCode;
            if (Enum.TryParse(code, out enumCode))
            {
                IfcUnitAssignment ifcUnitAssignment = GetProjectUnitAssignment();
                IfcMonetaryUnit mu = ifcUnitAssignment.Units.OfType<IfcMonetaryUnit>().FirstOrDefault();
                if (mu != null)
                {
                    mu.Currency = enumCode;
                }
                else
                {
                    ifcUnitAssignment.Units.Add(Model.Instances.New<IfcMonetaryUnit>(ifcmu =>
                    {
                        ifcmu.Currency = enumCode;
                    }));
                }
            }
        }

        /// <summary>
        /// Set the global units in the UnitsInContext property
        /// </summary>
        private IfcUnitAssignment GetProjectUnitAssignment()
        {
            IfcProject ifcProject = Model.IfcProject;
            if (ifcProject.UnitsInContext == null)
            {
                ifcProject.UnitsInContext = Model.Instances.New<IfcUnitAssignment>();
            }

            return ifcProject.UnitsInContext;
        }
        #endregion
        
    }
}
