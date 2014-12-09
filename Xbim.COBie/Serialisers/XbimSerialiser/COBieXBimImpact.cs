using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.XbimExtensions.Transactions;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc.SelectTypes;
using Xbim.XbimExtensions.SelectTypes;
using Xbim.IO;
using Xbim.Ifc2x3.Extensions;



namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimImpact : COBieXBim
    {
        #region Properties
        public IEnumerable<IfcTypeObject> IfcTypeObjects { get; private set; }
        public IEnumerable<IfcProduct> IfcProducts { get; private set; }
        #endregion

        public COBieXBimImpact(COBieXBimContext xBimContext)
            : base(xBimContext)
        {

        }

        #region Methods
        /// <summary>
        /// Create and setup objects held in the Component COBieSheet
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieImpactRow to read data from</param>
        public void SerialiseImpact(COBieSheet<COBieImpactRow> cOBieSheet)
        {
            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Impact"))
            {
                try
                {
                    int count = 1;
                    ProgressIndicator.ReportMessage("Starting Impacts...");
                    ProgressIndicator.Initialise("Creating Impacts", cOBieSheet.RowCount);
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        BumpTransaction(trans, count);
                        count++;
                        ProgressIndicator.IncrementAndUpdate();
                        COBieImpactRow row = cOBieSheet[i];
                        AddImpact(row);
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
        /// Add the Impact and fill with data from COBieComponentRow
        /// </summary>
        /// <param name="row">COBieImpactRow holding the data</param>
        private void AddImpact(COBieImpactRow row)
        {
            string pSetName = "Pset_EnvironmentalImpactValues";
            string description = Constants.DEFAULT_STRING;
            if (ValidateString(row.Description))
                description = row.Description;

            IfcPropertySet ifcPropertySet = null;
            if (row.SheetName.ToLower().Trim() == "type")
            {
                if (IfcTypeObjects == null)
                            IfcTypeObjects = Model.Instances.OfType<IfcTypeObject>();
                IfcTypeObject ifcTypeObject = IfcTypeObjects.Where(to => to.Name.ToString().ToLower() == row.RowName.ToLower()).FirstOrDefault();
                if (ifcTypeObject != null)
                {
                    if (XBimContext.IsMerge)
                    {
                        ifcPropertySet = ifcTypeObject.GetPropertySet(pSetName);
                        if (ifcPropertySet != null) //Property set Pset_EnvironmentalImpactValues already set so assume exists so skip
                        {
#if DEBUG
                            Console.WriteLine("{0} Pset_EnvironmentalImpactValues Property set so skip on merge", ifcTypeObject.GetType().Name);
#endif
                            return;
                        }
                    }
                    
                    ifcPropertySet = AddPropertySet(ifcTypeObject, pSetName, description);
                }
            }
            else
            {
                if (IfcProducts == null)
                    IfcProducts = Model.Instances.OfType<IfcProduct>();
                IfcProduct ifcProduct = IfcProducts.Where(to => to.Name.ToString().ToLower() == row.RowName.ToLower()).FirstOrDefault();
                if (ifcProduct != null)
                {
                    if (XBimContext.IsMerge)
                    {
                        ifcPropertySet = ifcProduct.GetPropertySet(pSetName);
                        if (ifcPropertySet != null)//Property set Pset_EnvironmentalImpactValues already set so assume exists so skip
                        {
#if DEBUG
                            Console.WriteLine("{0} Pset_EnvironmentalImpactValues Property set so skip on merge", ifcProduct.GetType().Name);
#endif
                            return;
                        }
                           
                    }
                    
                    ifcPropertySet = AddPropertySet(ifcProduct, pSetName, description);
                }
            }

            //check we have a property set from the found SheetName/RowName object
            if (ifcPropertySet != null)
            {
                //Add Created By, Created On and ExtSystem to Owner History. 
                SetUserHistory(ifcPropertySet, row.ExtSystem, row.CreatedBy, row.CreatedOn);
                //using statement will set the Model.OwnerHistoryAddObject to ifcPropertySet.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
                //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
                using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcPropertySet.OwnerHistory))
                {
                    if (ValidateString(row.Name))
                        AddPropertySingleValue(ifcPropertySet, "ImpactName", "Impact Name", new IfcText(row.Name), null);
                    
                    if (ValidateString(row.ImpactType))
                        AddPropertySingleValue(ifcPropertySet, "ImpactType", "Impact Type", new IfcText(row.ImpactType), null);
                    
                    if (ValidateString(row.ImpactStage))
                        AddPropertySingleValue(ifcPropertySet, "ImpactStage", "Impact Stage", new IfcText(row.ImpactStage), null);

                    if (ValidateString(row.Value))
                    {
                        IfcValue ifcValue = SetValue(row.Value);
                        
                        IfcUnit ifcUnit = null;
                        if (ValidateString(row.ImpactUnit))
                        {
                            ifcUnit = GetDurationUnit(row.ImpactUnit); //see if time unit
                            //see if we can convert to a IfcSIUnit
                            if (ifcUnit == null)
                                ifcUnit = GetSIUnit(row.ImpactUnit);
                            //OK set as a user defined
                            if (ifcUnit == null)
                                ifcUnit = SetContextDependentUnit(row.ImpactUnit);
                        }
                        AddPropertySingleValue(ifcPropertySet, "Value", "Value", ifcValue, ifcUnit);
                    }

                    if (ValidateString(row.LeadInTime))
                    {
                        IfcValue ifcValue = SetValue(row.LeadInTime);
                        AddPropertySingleValue(ifcPropertySet, "LeadInTime", "Lead In Time", ifcValue, null);
                    }

                    if (ValidateString(row.Duration))
                    {
                        IfcValue ifcValue = SetValue(row.Duration);
                        AddPropertySingleValue(ifcPropertySet, "Duration", "Duration", ifcValue, null);
                    }

                    if (ValidateString(row.LeadOutTime))
                    {
                        IfcValue ifcValue = SetValue(row.LeadOutTime);
                        AddPropertySingleValue(ifcPropertySet, "LeadOutTime", "Lead Out Time", ifcValue, null);
                    }

                    //Add GlobalId
                    AddGlobalId(row.ExtIdentifier, ifcPropertySet);

                    //row.Description done above on property set
                }
                
            }
            
        }

        /// <summary>
        /// set IfcValue to IfcText or IfcReal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private IfcValue SetValue(string value)
        {
            IfcValue ifcValue;
            double test;
            if (double.TryParse(value, out test))
                ifcValue = new IfcReal(test);
            else
                ifcValue = new IfcText(value);
            return ifcValue;
        }

        #endregion
    }
}
