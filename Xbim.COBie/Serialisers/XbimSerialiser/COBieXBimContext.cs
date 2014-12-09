using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.XbimExtensions;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.GeometryResource;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.Kernel;
using Xbim.IO;
using Xbim.XbimExtensions.Interfaces;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimContext : ICOBieContext
    {

        #region Fields
        private IfcSIUnit _secondUnit;
        private IfcDimensionalExponents _dimensionalExponents;

        private ReportProgressDelegate _progress = null;
        public event ReportProgressDelegate ProgressStatus;
        
        #endregion

        #region Properties
        // <summary>
        /// Model to add COBie data too
        /// </summary>
        public XbimModel Model { get; set; }

        /// <summary>
        /// Flag for merging
        /// </summary>
        public bool IsMerge { get; set; }
        
        /// <summary>
        /// WorkBook holding the COBie data
        /// </summary>
        public COBieWorkbook WorkBook { get;  set; }

        /// <summary>
        /// Contacts dictionary keyed on email address
        /// </summary>
        public Dictionary<string, IfcPersonAndOrganization> Contacts { get; private set; }
        
        private IfcConversionBasedUnit _ifcConversionBasedUnitYear;
        /// <summary>
        /// Year Unit
        /// </summary>
        public IfcConversionBasedUnit IfcConversionBasedUnitYear
        {
            get
            {
                if (_ifcConversionBasedUnitYear == null)
                    SetYearDurationUnit();
                return _ifcConversionBasedUnitYear;
            }          
            
        }

        public IfcDimensionalExponents DimensionalExponentSingleUnit
        {
            get
            {
                if (_dimensionalExponents == null)
                    SetSecondUnit();
                return _dimensionalExponents;
            }
        }

        private IfcConversionBasedUnit _ifcConversionBasedUnitMonth;
        /// <summary>
        /// Month Unit
        /// </summary>
        public IfcConversionBasedUnit IfcConversionBasedUnitMonth
        {
            get
            {
                if (_ifcConversionBasedUnitMonth == null)
                    SetMonthDurationUnit();
                return _ifcConversionBasedUnitMonth;
            }

        }

        private IfcConversionBasedUnit _ifcConversionBasedUnitWeek;
        /// <summary>
        /// Month Unit
        /// </summary>
        public IfcConversionBasedUnit IfcConversionBasedUnitWeek
        {
            get
            {
                if (_ifcConversionBasedUnitWeek == null)
                    SetWeekDurationUnit();
                return _ifcConversionBasedUnitWeek;
            }

        }

        private IfcConversionBasedUnit _ifcConversionBasedUnitMinute;
        /// <summary>
        /// Month Unit
        /// </summary>
        public IfcConversionBasedUnit IfcConversionBasedUnitMinute
        {
            get
            {
                if (_ifcConversionBasedUnitMinute == null)
                    SetMinuteDurationUnit();
                return _ifcConversionBasedUnitMinute;
            }

        }
        
        /// <summary>
        /// World Coordinates System for the Model
        /// </summary>
        public IfcAxis2Placement3D WCS { get;  set; }


        #endregion
        
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Model to add COBie data too</param>
        /// <param name="workBook">Work Book With COBie data </param>
        public COBieXBimContext (XbimModel model)
        {
            Model = model;
            Contacts = new Dictionary<string, IfcPersonAndOrganization>();
            
        }

        public COBieXBimContext(XbimModel model, ReportProgressDelegate progressHandler = null)
            : this(model) 
        {
            if (progressHandler != null)
            {
                _progress = progressHandler;
                this.ProgressStatus += progressHandler;
            }
        }


        public void Reset()
        {
            Contacts.Clear();
            _ifcConversionBasedUnitYear = null;
            _ifcConversionBasedUnitMonth = null;
            _ifcConversionBasedUnitWeek = null;
            _secondUnit = null;
            _dimensionalExponents = null;
            WorkBook = null;
        }

        /// <summary>
        /// Set the Year Duration Unit
        /// </summary>
        private void SetYearDurationUnit()
        {
            SetSecondUnit();
            
            IfcMeasureWithUnit yearMeasure = Model.Instances.New<IfcMeasureWithUnit>(mwu =>
            {
                mwu.ValueComponent = new IfcReal(3.1536E7);
                mwu.UnitComponent = _secondUnit;
            });
            IfcConversionBasedUnit yearConBaseUnit = Model.Instances.New<IfcConversionBasedUnit>(s =>
            {
                s.UnitType = IfcUnitEnum.TIMEUNIT;
                s.Name = "Year";
                s.Dimensions = _dimensionalExponents;
                s.ConversionFactor = yearMeasure;
            });
            //set context units
            _ifcConversionBasedUnitYear = yearConBaseUnit;
        }

        /// <summary>
        /// Set the month duration unit
        /// </summary>
        private void SetMonthDurationUnit()
        {
            SetSecondUnit();

            IfcMeasureWithUnit monthMeasure = Model.Instances.New<IfcMeasureWithUnit>(mwu =>
            {
                mwu.ValueComponent = new IfcReal(3.1536E7 / 12); //not accurate month but take average
                mwu.UnitComponent = _secondUnit;
            });
            IfcConversionBasedUnit monthConBaseUnit = Model.Instances.New<IfcConversionBasedUnit>(s =>
            {
                s.UnitType = IfcUnitEnum.TIMEUNIT;
                s.Name = "Month";
                s.Dimensions = _dimensionalExponents;
                s.ConversionFactor = monthMeasure;
            });

            
            //set context units
            _ifcConversionBasedUnitMonth = monthConBaseUnit;
        }

        /// <summary>
        /// Set the week duration unit
        /// </summary>
        private void SetWeekDurationUnit()
        {
            SetSecondUnit();

            IfcMeasureWithUnit weekMeasure = Model.Instances.New<IfcMeasureWithUnit>(mwu =>
            {
                mwu.ValueComponent = new IfcReal(3.1536E7 / 52); //not accurate week but take average
                mwu.UnitComponent = _secondUnit;
            });
            IfcConversionBasedUnit weekConBaseUnit = Model.Instances.New<IfcConversionBasedUnit>(s =>
            {
                s.UnitType = IfcUnitEnum.TIMEUNIT;
                s.Name = "Month";
                s.Dimensions = _dimensionalExponents;
                s.ConversionFactor = weekMeasure;
            });

            //set context units
            _ifcConversionBasedUnitWeek = weekConBaseUnit;
        }

        /// <summary>
        /// Set the week duration unit
        /// </summary>
        private void SetMinuteDurationUnit()
        {
            SetSecondUnit();

            IfcMeasureWithUnit minuteMeasure = Model.Instances.New<IfcMeasureWithUnit>(mwu =>
            {
                mwu.ValueComponent = new IfcReal(60); //not accurate week but take average
                mwu.UnitComponent = _secondUnit;
            });
            IfcConversionBasedUnit minuteConBaseUnit = Model.Instances.New<IfcConversionBasedUnit>(s =>
            {
                s.UnitType = IfcUnitEnum.TIMEUNIT;
                s.Name = "Minute";
                s.Dimensions = _dimensionalExponents;
                s.ConversionFactor = minuteMeasure;
            });

            //set context units
            _ifcConversionBasedUnitMinute = minuteConBaseUnit;
        }

        /// <summary>
        /// set the second unit, used for time duration units
        /// </summary>
        private void SetSecondUnit()
        {
            if (_secondUnit == null)
            {
                _secondUnit = Model.Instances.New<IfcSIUnit>(si =>
                {
                    si.UnitType = IfcUnitEnum.TIMEUNIT;
                    si.Prefix = null;
                    si.Name = IfcSIUnitName.SECOND;
                });
            }

            if (_dimensionalExponents == null)
            {
                _dimensionalExponents = Model.Instances.New<IfcDimensionalExponents>(de =>
                {
                    de.LengthExponent = 1;
                    de.MassExponent = 1;
                    de.TimeExponent = 1;
                    de.ElectricCurrentExponent = 1;
                    de.ThermodynamicTemperatureExponent = 1;
                    de.AmountOfSubstanceExponent = 1;
                    de.LuminousIntensityExponent = 1;
                });
            }
        }


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
            }
            if (ProgressStatus != null)
                ProgressStatus((int)percent, message);
        }

        /// <summary>
        /// Dispose function
        /// </summary>
        public void Dispose()
        {
            if (_progress != null)
            {
                ProgressStatus -= _progress;
                _progress = null;
            }
        }

    }
}
