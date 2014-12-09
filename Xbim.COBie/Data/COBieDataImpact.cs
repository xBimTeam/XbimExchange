using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.Kernel;
using Xbim.XbimExtensions;
using Xbim.Ifc2x3.PropertyResource;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Impact tab.
    /// </summary>
    public class COBieDataImpact : COBieData<COBieImpactRow>
    {

        /// <summary>
        /// Data Impact constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataImpact(COBieContext context) : base(context)
        { }

        #region Methods

        /// <summary>
        /// Fill sheet rows for Impact sheet
        /// </summary>
        /// <returns>COBieSheet<COBieImpactRow></returns>
        public override COBieSheet<COBieImpactRow> Fill()
        {
            ProgressIndicator.ReportMessage("Starting Impacts...");

            //create new sheet
            COBieSheet<COBieImpactRow> impacts = new COBieSheet<COBieImpactRow>(Constants.WORKSHEET_IMPACT);

            // get all IfcPropertySet objects from IFC file

            IEnumerable<IfcPropertySet> ifcProperties = Model.Instances.OfType<IfcPropertySet>().Where(ps => ps.Name.ToString() == "Pset_EnvironmentalImpactValues");

            ProgressIndicator.Initialise("Creating Impacts", ifcProperties.Count());

            foreach (IfcPropertySet propSet in ifcProperties)
            {
                ProgressIndicator.IncrementAndUpdate();

                COBieImpactRow impact = new COBieImpactRow(impacts);
                List<IfcSimpleProperty> propertyList = propSet.HasProperties.OfType<IfcSimpleProperty>().ToList();
                
                Interval propValues = GetPropertyValue(propertyList, "ImpactName");
                impact.Name = (propValues.Value == DEFAULT_STRING) ? propSet.Name.ToString() : propValues.Value.ToString();

                impact.CreatedBy = GetTelecomEmailAddress(propSet.OwnerHistory);
                impact.CreatedOn = GetCreatedOnDateAsFmtString(propSet.OwnerHistory);

                propValues = GetPropertyValue(propertyList, "ImpactType");
                impact.ImpactType = propValues.Value;

                propValues = GetPropertyValue(propertyList, "ImpactStage");
                impact.ImpactStage = propValues.Value;

                IfcRoot ifcRoot = GetAssociatedObject(propSet);
                impact.SheetName = GetSheetByObjectType(ifcRoot.GetType());
                impact.RowName = (!string.IsNullOrEmpty(ifcRoot.Name.ToString())) ? ifcRoot.Name.ToString() : DEFAULT_STRING;

                propValues = GetPropertyValue(propertyList, "Value");
                impact.Value = propValues.Value;
                impact.ImpactUnit = propValues.Unit;

                propValues = GetPropertyValue(propertyList, "LeadInTime");
                impact.LeadInTime = propValues.Value;

                propValues = GetPropertyValue(propertyList, "Duration");
                impact.Duration = propValues.Value;

                propValues = GetPropertyValue(propertyList, "LeadOutTime");
                impact.LeadOutTime = propValues.Value;

                impact.ExtSystem = GetExternalSystem(propSet);
                impact.ExtObject = propSet.GetType().Name;
                impact.ExtIdentifier = propSet.GlobalId;

                impact.Description = (propSet.Description != null) ? propSet.Description.ToString() : DEFAULT_STRING;

                impacts.AddRow(impact);
            }

            impacts.OrderBy(s => s.Name);

            ProgressIndicator.Finalise();

            return impacts;
        }

        

        /// <summary>
        /// Get IfcPropertySet first associated object
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        private IfcRoot GetAssociatedObject (IfcPropertySet ps)
        {
            if ((ps.PropertyDefinitionOf.FirstOrDefault() != null) &&
                (ps.PropertyDefinitionOf.First().RelatedObjects.FirstOrDefault() != null)
                )
            {
                return ps.PropertyDefinitionOf.First().RelatedObjects.First();
            }
            if (ps.DefinesType.FirstOrDefault() != null) 
            {
                return ps.DefinesType.FirstOrDefault();
            }

            return null;
        }
        
        #endregion
    }
}
