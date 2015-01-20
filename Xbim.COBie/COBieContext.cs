using System;
using System.Collections.Generic;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.XbimExtensions.Interfaces;
using Xbim.IO;
using Xbim.COBie.Data;
using Xbim.Ifc2x3.ActorResource;

namespace Xbim.COBie
{
	/// <summary>
	/// Context for generating COBie data from one or more IFC models
	/// </summary>
    public class COBieContext : ICOBieContext 
	{

        //Worksheet Global
        public Dictionary<long, string> EMails { get; private set; } 
        public string TemplateFileName { get; set; } //template used by the workbook
        public string RunDate { get; set; } //Date the Workbook was created 
        public string RunDateTime { get; set; } //Date the Workbook was created 
        public bool ExcludeFromPickList { get; set; }

        /// <summary>
        /// Map models to roles for federated models
        /// </summary>
        public Dictionary<XbimModel, COBieMergeRoles> MapMergeRoles { get; private set; } 
 
        private  GlobalUnits _workBookUnits;
        /// <summary>
        /// Global Units for the workbook
        /// </summary>
        public GlobalUnits WorkBookUnits
        {
            get
            {
                if (Model == null)
                {
                    throw new ArgumentException("COBieContext must contain a model before calling WorkBookUnits."); 
                }
                if (_workBookUnits == null) //set up global units
                {
                    _workBookUnits = new GlobalUnits();
                    COBieData<COBieRow>.GetGlobalUnits(Model, _workBookUnits);
                }
                return _workBookUnits;
            }
           
        }
        /// <summary>
        /// if set to true and no IfcZones or no IfcSpace property names of "ZoneName", we will list 
        /// any IfcSpace property names "Department" in the Zone sheet
        /// </summary>
        public bool DepartmentsUsedAsZones { get; set; } //indicate if we have taken departments as Zones

        /// <summary>
        /// filter values for attribute extraction in sheets
        /// </summary>
        public FilterValues Exclude { get;  set; } 

        /// <summary>
        /// set the error reporting to be either one (first row is labelled one) or 
        /// two based (first row is labelled two) on the rows of the tables/excel sheet
        /// </summary>
        public ErrorRowIndexBase ErrorRowStartIndex { get; set; } 

        public COBieContext()
        {
            RunDate = DateTime.Now.ToString(Constants.DATE_FORMAT);
            RunDateTime = DateTime.Now.ToString(Constants.DATETIME_FORMAT);
            EMails = new Dictionary<long, string>();
            Model = null;
            //if no IfcZones or no IfcSpace property names of "ZoneName" then if DepartmentsUsedAsZones is true we will list 
            //any IfcSpace property names "Department" in the Zone sheet and remove the "Department" property from the attribute sheet
            DepartmentsUsedAsZones = false;

            Exclude = new FilterValues();

            ExcludeFromPickList = false;

            //set the row index to report error rows on
            ErrorRowStartIndex = ErrorRowIndexBase.RowTwo; //default for excel sheet
            MapMergeRoles = new Dictionary<XbimModel, COBieMergeRoles>();

        }

        /// <summary>
        /// Get merge roles for federated models, used to work out Model Merge Precedence Rules
        /// </summary>
        private Dictionary<XbimModel, COBieMergeRoles> LinkRoleToModel()
        {
            Dictionary<IfcRole, COBieMergeRoles> mapMergeRoles = MapRolesForMerge();//assign merge role to a IfcRoleEnum value
            Dictionary<XbimModel, COBieMergeRoles> mapModelToMergeRoles = new Dictionary<XbimModel, COBieMergeRoles>();
            
            //mapModelToMergeRoles.Add(Model, COBieMergeRoles.Unknown); //assume that it is just the holder model(xBIMf) (as xbim is creating holding file .xbimf) for and all the models are in the Model.RefencedModels property
            
            //now get the referenced models
            foreach (var refModel in Model.ReferencedModels)
            {
                IfcDocumentInformation doc = refModel.DocumentInformation;
                IfcOrganization owner = doc.DocumentOwner as IfcOrganization;
                if ((owner != null) && (owner.Roles != null))
                {
                    COBieMergeRoles mergeRoles = COBieMergeRoles.Unknown;
                    
                    foreach (var role in owner.Roles)
                    {
                        IfcRole roleitem = role.Role;

                        if (mapMergeRoles[roleitem] != COBieMergeRoles.Unknown)
                        {
                            mergeRoles = mergeRoles | mapMergeRoles[roleitem]; //use in if's ((mergeRoles & COBieMergeRoles.Architectural) == COBieMergeRoles.Architectural)
                            //remove the unknown as we now have at least one value
                            if ((mergeRoles & COBieMergeRoles.Unknown) == COBieMergeRoles.Unknown)
                                mergeRoles = mergeRoles ^ COBieMergeRoles.Unknown;
                        }
                    }
                    mapModelToMergeRoles.Add(refModel.Model, mergeRoles);
                }
            }
            return mapModelToMergeRoles;
        }
        
       
        /// <summary>
        /// Map ifcRols to the MergeRole for COBie
        /// </summary>
        /// <returns></returns>
        private Dictionary<IfcRole, COBieMergeRoles> MapRolesForMerge()
        {
            Dictionary<IfcRole, COBieMergeRoles> mapRoles = new Dictionary<IfcRole,COBieMergeRoles>();
            foreach (var item in Enum.GetValues(typeof(IfcRole)))
            {
                IfcRole role = (IfcRole)item;
                switch (role)
                {
                    case IfcRole.Supplier:
                    case IfcRole.Manufacturer:
                    case IfcRole.Contractor:
                    case IfcRole.Subcontractor:
                    case IfcRole.StructuralEngineer:
                    case IfcRole.CostEngineer:
                    case IfcRole.Client:
                    case IfcRole.BuildingOwner:
                    case IfcRole.BuildingOperator:
                    case IfcRole.ProjectManager:
                    case IfcRole.FacilitiesManager:
                    case IfcRole.CivilEngineer:
                    case IfcRole.ComissioningEngineer:
                    case IfcRole.Engineer:
                    case IfcRole.Consultant:
                    case IfcRole.ConstructionManager:
                    case IfcRole.FieldConstructionManager:
                    case IfcRole.Owner:
                    case IfcRole.Reseller:
                    case IfcRole.UserDefined:
                        mapRoles.Add(role, COBieMergeRoles.Unknown);
                        break;
                    case IfcRole.Architect:
                        mapRoles.Add(role, COBieMergeRoles.Architectural);
                        break;
                    case IfcRole.MechanicalEngineer:
                        mapRoles.Add(role, COBieMergeRoles.Mechanical);
                        break;
                    case IfcRole.ElectricalEngineer:
                        mapRoles.Add(role, COBieMergeRoles.Electrical);
                        break;
                    default:
                        mapRoles.Add(role, COBieMergeRoles.Unknown);
                        break;
                }
            }  

            return mapRoles;
        }

        public COBieContext(ReportProgressDelegate progressHandler = null) : this() 
		{
            if (progressHandler != null)
            {
                _progress = progressHandler;
                this.ProgressStatus += progressHandler;
            }
		}

		/// /// <summary>
        /// Gets the model defined in this context to generate COBie data from
        /// </summary>
        
        private XbimModel _model;

        public XbimModel Model
        {
            get { return _model; }
            set { 
                _model = value;
                //set the merge role relationships
                if ((_model != null) && (IsFederation))
                {
                    //get merge roles for federated models, used to work out Model Merge Precedence Rules
                    MapMergeRoles = LinkRoleToModel();
                }
            }
        }

        /// <summary>
        /// Model is federated true/false
        /// </summary>
        public bool IsFederation
        {
            get { return (_model != null) ? _model.IsFederation : false;  }
        }

        private ReportProgressDelegate _progress = null;

        public event ReportProgressDelegate ProgressStatus;

        /// <summary>
        /// Updates the delegates with the current percentage complete
        /// </summary>
        /// <param name="message"></param>
        /// <param name="total"></param>
        /// <param name="current"></param>
        public void UpdateStatus(string message, int total = 0, int current = 0)
        {
            decimal percent = 0;
            if (total != 0 && current > 0)
            {
                message = string.Format("{0} [{1}/{2}]", message, current, total);
                percent = (decimal)current / total * 100;
                if ((percent > 0) && (percent < 1))
                {
                    percent = 1; //stops display of status bar in text list
                }
            }
            if (ProgressStatus != null)
                ProgressStatus((int)percent, message);
           
        }

        public void Dispose()
        {
            if (_progress != null)
            {
                ProgressStatus -= _progress;
                _progress = null;
            }
        }
    }

    /// <summary>
    /// Index for the rows the errors are reported on, either one based (first row is labelled one) (Data Table etc...) 
    /// or two based (first row is labelled two) (Excel sheets)
    /// </summary>
    public enum ErrorRowIndexBase
    {
        RowOne,
        RowTwo
    }

    /// <summary>
    /// Global units
    /// </summary>
    public class GlobalUnits 
    {
        public string LengthUnit { get; set; }
        public string AreaUnit { get; set; }
        public string VolumeUnit { get; set; }
        public string MoneyUnit { get; set; }
    }

    public interface ICOBieContext : IDisposable
    {
        void UpdateStatus(string message, int total = 0, int current = 0);
    }
}
