using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.ConstructionMgmtDomain;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProcessExtensions;
using Xbim.XbimExtensions;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.XbimExtensions.SelectTypes;
using Xbim.COBie.Serialisers.XbimSerialiser;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Job tab.
    /// </summary>
    public class COBieDataJob : COBieData<COBieJobRow>
    {
        /// <summary>
        /// Data Job constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataJob(COBieContext context) : base(context)
        { }

        #region Methods

        /// <summary>
        /// Fill sheet rows for Job sheet
        /// </summary>
        /// <returns>COBieSheet<COBieJobRow></returns>
        public override COBieSheet<COBieJobRow> Fill()
        {
            ProgressIndicator.ReportMessage("Starting Jobs...");

            //create new sheet
            COBieSheet<COBieJobRow> jobs = new COBieSheet<COBieJobRow>(Constants.WORKSHEET_JOB);

            // get all IfcTask objects from IFC file
            IEnumerable<IfcTask> ifcTasks = Model.Instances.OfType<IfcTask>();

            COBieDataPropertySetValues allPropertyValues = new COBieDataPropertySetValues(); //properties helper class
            
            //IfcTypeObject typObj = Model.Instances.OfType<IfcTypeObject>().FirstOrDefault();
            IfcConstructionEquipmentResource cer = Model.Instances.OfType<IfcConstructionEquipmentResource>().FirstOrDefault();

            ProgressIndicator.Initialise("Creating Jobs", ifcTasks.Count());

            foreach (IfcTask ifcTask in ifcTasks)
            {
                ProgressIndicator.IncrementAndUpdate();

                if (ifcTask == null) continue;

                COBieJobRow job = new COBieJobRow(jobs);

                job.Name =  (string.IsNullOrEmpty(ifcTask.Name.ToString())) ? DEFAULT_STRING : ifcTask.Name.ToString();
                job.CreatedBy = GetTelecomEmailAddress(ifcTask.OwnerHistory);
                job.CreatedOn = GetCreatedOnDateAsFmtString(ifcTask.OwnerHistory);
                job.Category =  ifcTask.ObjectType.ToString(); 
                job.Status = (string.IsNullOrEmpty(ifcTask.Status.ToString())) ? DEFAULT_STRING : ifcTask.Status.ToString();

                job.TypeName = GetObjectType(ifcTask);
                job.Description = (string.IsNullOrEmpty(ifcTask.Description.ToString())) ? DEFAULT_STRING : ifcTask.Description.ToString();

                allPropertyValues.SetAllPropertyValues(ifcTask); //set properties values to this task
                IfcPropertySingleValue ifcPropertySingleValue = allPropertyValues.GetPropertySingleValue("TaskDuration");
                job.Duration = ((ifcPropertySingleValue != null) && (ifcPropertySingleValue.NominalValue != null)) ? ConvertNumberOrDefault(ifcPropertySingleValue.NominalValue.ToString()) : DEFAULT_NUMERIC;
                string unitName = ((ifcPropertySingleValue != null) && (ifcPropertySingleValue.Unit != null)) ? GetUnitName(ifcPropertySingleValue.Unit) : null; 
                job.DurationUnit = (string.IsNullOrEmpty(unitName)) ?  DEFAULT_STRING : unitName;

                ifcPropertySingleValue = allPropertyValues.GetPropertySingleValue("TaskStartDate");
                job.Start = GetStartTime(ifcPropertySingleValue);
                unitName = ((ifcPropertySingleValue != null) && (ifcPropertySingleValue.Unit != null)) ? GetUnitName(ifcPropertySingleValue.Unit) : null; 
                job.TaskStartUnit = (string.IsNullOrEmpty(unitName)) ? DEFAULT_STRING : unitName;

                ifcPropertySingleValue = allPropertyValues.GetPropertySingleValue("TaskInterval");
                job.Frequency = ((ifcPropertySingleValue != null) && (ifcPropertySingleValue.NominalValue != null)) ? ConvertNumberOrDefault(ifcPropertySingleValue.NominalValue.ToString()) : DEFAULT_NUMERIC;
                unitName = ((ifcPropertySingleValue != null) && (ifcPropertySingleValue.Unit != null)) ? GetUnitName(ifcPropertySingleValue.Unit) : null;
                job.FrequencyUnit = (string.IsNullOrEmpty(unitName)) ? DEFAULT_STRING : unitName;

                job.ExtSystem = GetExternalSystem(ifcTask);
                job.ExtObject = ifcTask.GetType().Name;
                job.ExtIdentifier = ifcTask.GlobalId;

                job.TaskNumber = (string.IsNullOrEmpty(ifcTask.TaskId.ToString())) ? DEFAULT_STRING : ifcTask.TaskId.ToString();
                job.Priors =  GetPriors(ifcTask);
                job.ResourceNames = GetResources(ifcTask);

                jobs.AddRow(job);
            }

            jobs.OrderBy(s => s.Name);

            ProgressIndicator.Finalise();
            return jobs;
        }

        /// <summary>
        /// Get Formatted Start Date
        /// </summary>
        /// <param name="allPropertyValues"></param>
        /// <returns></returns>
        private string GetStartTime(IfcPropertySingleValue ifcPropertySingleValue)
        {
            string startData = "";
            if ((ifcPropertySingleValue != null) && (ifcPropertySingleValue.NominalValue != null))
                startData = ifcPropertySingleValue.NominalValue.ToString(); 
            
            DateTime frmDate;
            if (DateTime.TryParse(startData, out frmDate))
                startData = frmDate.ToString(Constants.DATE_FORMAT);
            else
                startData = Constants.DEFAULT_STRING; //Context.RunDate;//default is Now
            return startData;
        }

        /// <summary>
        /// Get the number of tasks before this task
        /// </summary>
        /// <param name="ifcTask">IfcTask object</param>
        /// <returns>string holding predecessor name of last task(s)</returns>
        private string GetPriors(IfcTask ifcTask)
        {
            IEnumerable<IfcRelSequence> isSuccessorFrom = ifcTask.IsSuccessorFrom;
            List<string> relatingTasks = new List<string>();
            foreach (IfcRelSequence ifcRelSequence in isSuccessorFrom)
            {
                IfcTask relatingIfcTask = ifcRelSequence.RelatingProcess as IfcTask;
                if (relatingIfcTask != null)
                    relatingTasks.Add(relatingIfcTask.TaskId.ToString().Trim());
            }

            if (relatingTasks.Count > 0)
                return string.Join(":", relatingTasks);
            else
                return ifcTask.TaskId.ToString().Trim(); //if no priors, reference itself
           
        }

        /// <summary>
        /// Get required resources for the task
        /// </summary>
        /// <param name="ifcTask">IfcTask object to get resources for</param>
        /// <returns>delimited string of the resources</returns>
        private string GetResources(IfcTask ifcTask)
        {
            IEnumerable<IfcConstructionEquipmentResource> ifcConstructionEquipmentResources = ifcTask.OperatesOn.SelectMany(ratp => ratp.RelatedObjects.OfType<IfcConstructionEquipmentResource>());
            List<string> strList = new List<string>();
            foreach (IfcConstructionEquipmentResource ifcConstructionEquipmentResource in ifcConstructionEquipmentResources)
            {
                if ((ifcConstructionEquipmentResource != null) && (!string.IsNullOrEmpty(ifcConstructionEquipmentResource.Name.ToString())))
                {
                    if (!strList.Contains(ifcConstructionEquipmentResource.Name.ToString()))
                        strList.Add(ifcConstructionEquipmentResource.Name.ToString());
                }
            }
            return (strList.Count > 0) ? (string.Join(", ", strList) + "." ) : DEFAULT_STRING;
        }
        /// <summary>
        /// Get the object IfcTypeObject name from the IfcTask object
        /// </summary>
        /// <param name="ifcTask">IfcTask object</param>
        /// <returns>string holding IfcTypeObject name</returns>
        private string GetObjectType(IfcTask ifcTask)
        {
            //first try
            IEnumerable<IfcTypeObject> ifcTypeObjects = ifcTask.OperatesOn.SelectMany(ratp => ratp.RelatedObjects.OfType<IfcTypeObject>());
            //second try on IsDefinedBy.OfType<IfcRelDefinesByType>
            if ((ifcTypeObjects == null) || (ifcTypeObjects.Count() == 0)) 
                ifcTypeObjects = ifcTask.IsDefinedBy.OfType<IfcRelDefinesByType>().Select(idb => (idb as IfcRelDefinesByType).RelatingType);

            //third try on IsDefinedBy.OfType<IfcRelDefinesByProperties> for DefinesType
            if ((ifcTypeObjects == null) || (ifcTypeObjects.Count() == 0))
                ifcTypeObjects = ifcTask.IsDefinedBy.OfType<IfcRelDefinesByProperties>().SelectMany(idb => (idb as IfcRelDefinesByProperties).RelatingPropertyDefinition.DefinesType);

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
                IEnumerable<IfcObject> ifcObjects = ifcTask.IsDefinedBy.OfType<IfcRelDefinesByProperties>().SelectMany(idb => idb.RelatedObjects);
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
    }
}
