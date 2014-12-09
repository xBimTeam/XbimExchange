using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.XbimExtensions.Transactions;
using Xbim.Ifc2x3.ProcessExtensions;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.Ifc2x3.ConstructionMgmtDomain;
using Xbim.IO;


namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimJob : COBieXBim
    {
        #region Properties
        public IEnumerable<IfcTypeObject> IfcTypeObjects { get; private set; }
        public IEnumerable<IfcTask> IfcTasks { get; private set; }
        public IEnumerable<IfcConstructionEquipmentResource> IfcConstructionEquipmentResources { get; private set; }
        
        #endregion

        public COBieXBimJob(COBieXBimContext xBimContext)
            : base(xBimContext)
        {
        }

        /// <summary>
        /// Add the IfcTask to the Model object
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieResourceRow to read data from</param>
        public void SerialiseJob(COBieSheet<COBieJobRow> cOBieSheet)
        {

            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Job"))
            {
                try
                {
                    int count = 1;
                    IfcTypeObjects = Model.Instances.OfType<IfcTypeObject>();
                    IfcConstructionEquipmentResources = Model.Instances.OfType<IfcConstructionEquipmentResource>();

                    ProgressIndicator.ReportMessage("Starting Jobs...");
                    ProgressIndicator.Initialise("Creating Jobs", (cOBieSheet.RowCount * 2));
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        BumpTransaction(trans, count);
                        count++;
                        ProgressIndicator.IncrementAndUpdate();
                        COBieJobRow row = cOBieSheet[i];
                        AddJob(row);
                        
                    }
                    //we need to assign IfcRelSequence relationships, but we need all tasks implemented, so loop rows again
                    IfcTasks = Model.Instances.OfType<IfcTask>(); //get new tasks
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        BumpTransaction(trans, count);
                        ProgressIndicator.IncrementAndUpdate();
                        COBieJobRow row = cOBieSheet[i];
                        SetPriors(row);
                        count++;
                    }
                    ProgressIndicator.Finalise();
                    
                    trans.Commit();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        

        

        /// <summary>
        /// Add the data to the IfcTask object
        /// </summary>
        /// <param name="row">COBieJobRow holding the data</param>
        private void AddJob(COBieJobRow row)
        {
            IEnumerable<IfcTypeObject> ifcTypeObjects = Enumerable.Empty<IfcTypeObject>();
            IfcTask ifcTask = null;
            
            //get the objects in the typeName cell
            if (ValidateString(row.TypeName))
            {
                List<string> typeNames = SplitString(row.TypeName, ':');
                ifcTypeObjects = IfcTypeObjects.Where(to => typeNames.Contains(to.Name.ToString().Trim()));
            }

            //if merging check for existing task
            if (XBimContext.IsMerge)
            {
                string taskNo = string.Empty;
                //get the task ID
                if (ValidateString(row.TaskNumber)) 
                    taskNo = row.TaskNumber;
                //see if task matches name and task number
                ifcTask = CheckIfObjExistOnMerge<IfcTask>(row.Name).Where(task => task.TaskId == taskNo).FirstOrDefault();
                if (ifcTask != null)
                {
                    IfcRelAssignsToProcess processRel = Model.Instances.Where<IfcRelAssignsToProcess>(rd => rd.RelatingProcess == ifcTask).FirstOrDefault();
                    int matchCount = ifcTypeObjects.Count(to => processRel.RelatedObjects.Contains(to));
                    if (matchCount == ifcTypeObjects.Count()) //task IfcRelAssignsToProcess object hold the correct number of ifcTypeObjects objects so consider a match
                        return; //consider a match so return
                    
                }
            }

            //no match on task    
            ifcTask = Model.Instances.New<IfcTask>();
            
            //Add Created By, Created On and ExtSystem to Owner History. 
            SetUserHistory(ifcTask, row.ExtSystem, row.CreatedBy, row.CreatedOn);
            
            //using statement will set the Model.OwnerHistoryAddObject to ifcConstructionEquipmentResource.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
            using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcTask.OwnerHistory))
            {
                //Add Name
                if (ValidateString(row.Name)) ifcTask.Name = row.Name;

                //Add Category
                if (ValidateString(row.Category)) ifcTask.ObjectType = row.Category;

                //AddStatus
                if (ValidateString(row.Status)) ifcTask.Status = row.Status;

                //Add Type Relationship
                if (ifcTypeObjects.Any())
                {
                    SetRelAssignsToProcess(ifcTask, ifcTypeObjects);
                }
                //Add GlobalId
                AddGlobalId(row.ExtIdentifier, ifcTask);

                //add Description
                if (ValidateString(row.Description)) ifcTask.Description = row.Description;

               
                //Add Duration and duration Unit
                if (ValidateString(row.Duration))
                {
                    IfcPropertySingleValue ifcPropertySingleValue = AddPropertySingleValue(ifcTask, "Pset_Job_COBie", "Job Properties From COBie", "TaskDuration", "Task Duration", new IfcReal(row.Duration));
                    //DurationUnit
                    if (ValidateString(row.DurationUnit))
                        ifcPropertySingleValue.Unit = GetDurationUnit(row.DurationUnit);
                }

                //Add start time and start unit
                if (ValidateString(row.Start))
                {
                    IfcPropertySingleValue ifcPropertySingleValue = AddPropertySingleValue(ifcTask, "Pset_Job_COBie", null, "TaskStartDate", "Task Start Date", new IfcText(row.Start));
                    //TaskStartUnit
                    if (ValidateString(row.TaskStartUnit))
                        ifcPropertySingleValue.Unit = GetDurationUnit(row.TaskStartUnit);
                }

                //Add frequency and frequency unit
                if (ValidateString(row.Frequency))
                {
                    IfcPropertySingleValue ifcPropertySingleValue = AddPropertySingleValue(ifcTask, "Pset_Job_COBie", null, "TaskInterval", "Task Interval", new IfcReal(row.Frequency));
                    //TaskStartUnit
                    if (ValidateString(row.FrequencyUnit))
                        ifcPropertySingleValue.Unit = GetDurationUnit(row.FrequencyUnit);
                } 
                
                //Add Task ID
                if (ValidateString(row.TaskNumber)) ifcTask.TaskId = row.TaskNumber;

                //Add Priors, done in another loop see above

                //Add Resource names
                if (ValidateString(row.ResourceNames))
                {
                    List<string> Resources = row.ResourceNames.Split(',').ToList<string>(); //did dangerous using , as ',' as user can easily place out of sequence.
                    for (int i = 0; i < Resources.Count; i++)
                    {
                        Resources[i] = Resources[i].ToLower().Trim().Replace(".", string.Empty); //remove full stop
                    }
                    IEnumerable<IfcConstructionEquipmentResource> ifcConstructionEquipmentResource = IfcConstructionEquipmentResources.Where(cer => Resources.Contains(cer.Name.ToString().ToLower().Trim().Replace(".", string.Empty)));
                    if (ifcConstructionEquipmentResource != null) 
                        SetRelAssignsToProcess(ifcTask, ifcConstructionEquipmentResource);
                   
                }

            }
        }

        

        /// <summary>
        /// Create the relationships between the Process and the types it relates too
        /// </summary>
        /// <param name="processObj">IfcProcess Object</param>
        /// <param name="typeObjs">IEnumerable of IfcTypeObject, list of IfcTypeObjects </param>
        public void SetRelAssignsToProcess(IfcProcess processObj, IEnumerable<IfcObjectDefinition> typeObjs)
        {
            //find any existing relationships to this type
            IfcRelAssignsToProcess processRel = Model.Instances.Where<IfcRelAssignsToProcess>(rd => rd.RelatingProcess == processObj).FirstOrDefault();
            if (processRel == null) //none defined create the relationship
            {
                processRel = Model.Instances.New<IfcRelAssignsToProcess>();
                processRel.RelatingProcess = processObj;
            }
            //add the type objects
            foreach (IfcObjectDefinition type in typeObjs)
            {
                if (!processRel.RelatedObjects.Contains(type))
                {
                    processRel.RelatedObjects.Add(type);
                }
            }
        }

        /// <summary>
        /// set up IfcRelSequence for the task
        /// </summary>
        /// <param name="row">COBieJobRow holding the data</param>
        private void SetPriors(COBieJobRow row)
        {
            IEnumerable<IfcTask> ifcTaskFound = IfcTasks.Where(task => task.Name == row.Name && task.TaskId == row.TaskNumber);
            if (ifcTaskFound.Count() == 1) //should equal one
            {
                IfcTask ifcTask = ifcTaskFound.First();
                if (ValidateString(row.Priors))
                {
                    string priors = row.Priors.ToString();
                    char splitKey = GetSplitChar(priors);
                    string[] priorsArray = row.Priors.ToString().Split(splitKey);
                    foreach (string prior in priorsArray)
                    {
                        string name = row.Name.ToLower().Trim();
                        string testName = prior.ToLower().Trim();
                        IEnumerable<IfcTask> ifcTaskRelating = IfcTasks.Where(task => (ifcTask.EntityLabel != task.EntityLabel) && (task.TaskId.ToString().ToLower().Trim() == testName) && (task.Name.ToString().ToLower().Trim() == name));
                        List<IfcTask> ifcTaskRelatingTasks = ifcTaskRelating.ToList(); //avoids crash of foreach loop, Steve to fix then this can be removed
                        foreach (IfcTask ifcTaskitem in ifcTaskRelatingTasks)
                        {
                            IfcRelSequence relSequence = Model.Instances.New<IfcRelSequence>();
                            relSequence.RelatedProcess = ifcTask;
                            relSequence.RelatingProcess = ifcTaskitem;
                        }
                    }
                }
            }
            //throw new Exception("COBieXBimJob.SetPriors(): did not find a single task matching name and task number");
        }
        
    }
}
