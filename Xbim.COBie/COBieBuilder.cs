using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Xbim.COBie.Rows;
using Xbim.XbimExtensions;
using Xbim.Ifc2x3.Extensions;
using System.Linq;
using System.Reflection;
using Xbim.COBie.Contracts;
using Xbim.COBie.Serialisers;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.Kernel;
using Xbim.IO;



namespace Xbim.COBie
{ 
	/// <summary>
	/// Interrogates IFC models and builds COBie-format objects from the models
	/// </summary>
    public class COBieBuilder
    {

		private COBieBuilder()
		{
			ResetWorksheets();
		}

		/// <summary>
		/// Constructor which also sets the Context
		/// </summary>
		/// <param name="context"></param>
		public COBieBuilder(COBieContext context) : this()
		{
            Context = context;
            GenerateCOBieData();
		}

        /// <summary>
        /// The context in which this COBie data is being built
        /// </summary>
        /// <remarks>Contains the source models, templates, environmental data and other parameters</remarks>
        public COBieContext Context { get; private set; }

        /// <summary>
        /// The set of COBie worksheets
        /// </summary>
        public COBieWorkbook Workbook { get; private set; }

		private void ResetWorksheets()
		{
            Workbook = new COBieWorkbook();
		}

        
		private void Initialise()
        {
			if (Context == null) { throw new InvalidOperationException("COBieReader can't initialise without a valid Context."); }
			if (Context.Model == null) { throw new ArgumentException("COBieReader context must contain a models."); }

            //set default date for this run
            Context.RunDate = DateTime.Now.ToString(Constants.DATE_FORMAT);

            // set all the properties
            COBieQueries cq = new COBieQueries(Context);

            //create pick list from the template sheet
            COBieSheet<COBiePickListsRow> CobiePickLists = null;
            if ((!string.IsNullOrEmpty(Context.TemplateFileName)) &&
                File.Exists(Context.TemplateFileName)
                )
            {
                COBieXLSDeserialiser deSerialiser = new COBieXLSDeserialiser(Context.TemplateFileName, Constants.WORKSHEET_PICKLISTS);
                COBieWorkbook wbook = deSerialiser.Deserialise();
                if (wbook.Count > 0) CobiePickLists = (COBieSheet<COBiePickListsRow>)wbook.FirstOrDefault();

                
            }

            
            //fall back to xml file if not in template
            string pickListFileName = "PickLists.xml";
            if ((CobiePickLists == null) &&
                File.Exists(pickListFileName)
                )
                CobiePickLists = cq.GetCOBiePickListsSheet(pickListFileName);// create pick lists from xml
            if (Context.ExcludeFromPickList)
            {
                SetExcludeComponentTypes(CobiePickLists);
                SetExcludeObjTypeTypes(CobiePickLists);
            }
            //start the Cache
            Context.Model.CacheStart();
                
            //contact sheet first as it will fill contact information lookups for other sheets
            Workbook.Add(cq.GetCOBieContactSheet());
            Workbook.Add(cq.GetCOBieFacilitySheet()); 
            Workbook.Add(cq.GetCOBieFloorSheet());
            Workbook.Add(cq.GetCOBieSpaceSheet());
            Workbook.Add(cq.GetCOBieZoneSheet()); 
            Workbook.Add(cq.GetCOBieTypeSheet());
            Workbook.Add(cq.GetCOBieComponentSheet());
            Workbook.Add(cq.GetCOBieSystemSheet(Workbook[Constants.WORKSHEET_COMPONENT].Indices)); //pass component names 
            Workbook.Add(cq.GetCOBieAssemblySheet());
            Workbook.Add(cq.GetCOBieConnectionSheet());
            Workbook.Add(cq.GetCOBieSpareSheet());
            Workbook.Add(cq.GetCOBieResourceSheet());
            Workbook.Add(cq.GetCOBieJobSheet());
            Workbook.Add(cq.GetCOBieImpactSheet());
            Workbook.Add(cq.GetCOBieDocumentSheet());
            Workbook.Add(cq.GetCOBieAttributeSheet());//we need to fill attributes here as it is populated by Components, Type, Space, Zone, Floors, Facility etc
            //#if GEOMETRY_IMPLEMENTED
            Workbook.Add(cq.GetCOBieCoordinateSheet());
            //#endif
            Workbook.Add(cq.GetCOBieIssueSheet());
            if (CobiePickLists != null)
                Workbook.Add(CobiePickLists);
            else
                Workbook.Add(new COBieSheet<COBiePickListsRow>(Constants.WORKSHEET_PICKLISTS)); //add empty pick list
           
            //clear sheet session values from context
            Context.EMails.Clear();
            

        }

        /// <summary>
        /// Set the exclude list using the pick list sheet as the source of allowed Object Types
        /// </summary>
        /// <param name="CobiePickLists">COBieSheet of COBiePickListsRow</param>
        private void SetExcludeObjTypeTypes(COBieSheet<COBiePickListsRow> CobiePickLists)
        {
            List<Type> objTypes = GetExcludedTypes(CobiePickLists, typeof(IfcTypeObject), 37);
            Context.Exclude.ObjectType.Types.Clear();
            Context.Exclude.ObjectType.Types.AddRange(objTypes);
        }

        /// <summary>
        /// Set the exclude list using the pick list sheet as the source of allowed Element types
        /// </summary>
        /// <param name="CobiePickLists">COBieSheet of COBiePickListsRow</param>
        private void SetExcludeComponentTypes(COBieSheet<COBiePickListsRow> CobiePickLists)
        {
            List<Type> eleTypes = GetExcludedTypes(CobiePickLists, typeof(IfcElement), 22);
            Context.Exclude.ObjectType.Component.Clear();
            Context.Exclude.ObjectType.Component.AddRange(eleTypes);
        }

        /// <summary>
        /// Returns a list of class types to use as exclusions
        /// </summary>
        /// <param name="CobiePickLists">COBieSheet of COBiePickListsRow</param>
        /// <param name="reqType">Type object to filter selection on</param>
        /// <param name="colIndex">column index to get required classes from</param>
        /// <returns></returns>
        private List<Type> GetExcludedTypes(COBieSheet<COBiePickListsRow> CobiePickLists, Type reqType, int colIndex)
        {
            List<Type> classTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.IsSubclassOf(reqType)).ToList();
            
            if ((CobiePickLists.RowCount > 0) && 
                (classTypes.Count > 0)
                )
            {
                for (int i = 0; i < CobiePickLists.RowCount; i++)
                {
                    COBiePickListsRow row = CobiePickLists[i];
                    var colvalue = row[colIndex];
                    if ((colvalue != null) && 
                        (!string.IsNullOrEmpty(colvalue.CellValue))
                        )
                    {
                        IfcType ifcType;
                        if (IfcMetaData.TryGetIfcType(colvalue.CellValue.Trim().ToUpper(), out ifcType))
                            classTypes.Remove(ifcType.Type);
                    }
                }
            }
            return classTypes;
        }

        private void PopulateErrors(ICOBieValidationTemplate ValidationTemplate = null)
        {
            try
            {
                COBieProgress progress = new COBieProgress(Context);
                progress.Initialise("Validating Workbooks", Workbook.Count, 0);
                progress.ReportMessage("Building Indices...");
                Workbook.CreateIndices();
                progress.ReportMessage("Building Indices...Finished");
                
                // Validate the workbook
                progress.ReportMessage("Starting Validation...");
                Workbook.Validate(Context.ErrorRowStartIndex, ValidationTemplate, (lastProcessedSheetIndex) =>
                {
                    // When each sheet has been processed, increment the progress bar
                    progress.IncrementAndUpdate();
                } );
                progress.ReportMessage("Finished Validation");

                progress.Finalise();

            }
            catch (Exception)
            {
                // TODO: Handle
                throw;
            }
        }

        private int GetCOBieSheetIndexBySheetName(string sheetName)
        {
            for (int i = 0; i < Workbook.Count; i++)
            {
                if (sheetName == Workbook[i].SheetName)
                    return i;
            }
            return -1;
        }

        
        private void GenerateCOBieData()
        {
            Initialise();
            Workbook.SetInitialHashCode();//set the initial row hash value to compare against for row changes
           
            PopulateErrors();

            //Role validation
            COBieProgress progress = new COBieProgress(Context);
            //check we have values in MapMergeRoles, only on federated or via test harness
            if ((Context.MapMergeRoles.Count > 0) &&
                (Context.MapMergeRoles.ContainsKey(Context.Model))
                )
            {
                progress.ReportMessage(string.Format("Starting Merge Validation for {0}...", Context.MapMergeRoles[Context.Model]));
                Workbook.ValidateRoles(Context.Model, Context.MapMergeRoles[Context.Model]);
                progress.ReportMessage("Finished Merge Validation...");
            }
        }

		/// <summary>
        /// Passes this instance of the COBieReader into the provided ICOBieSerialiser
		/// </summary>
        /// <param name="serialiser">The object implementing the ICOBieSerialiser interface.</param>
        public void Export(ICOBieSerialiser serialiser, ICOBieValidationTemplate ValidationTemplate = null)
		{
            if (serialiser == null) { throw new ArgumentNullException("formatter", "Parameter passed to COBieReader.Export(ICOBieSerialiser) must not be null."); }


			serialiser.Serialise(Workbook, ValidationTemplate);
		}


        
    }
}
