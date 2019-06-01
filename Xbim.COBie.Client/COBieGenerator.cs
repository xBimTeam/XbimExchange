using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xbim.ModelGeometry.Scene;
using System.Diagnostics;
using System.IO;
using Xbim.Common;
using Xbim.Ifc2x3.Kernel;
using Xbim.COBie.Contracts;
using Xbim.COBie.Serialisers;
using Xbim.COBie.Rows;
using Xbim.COBie.Federate;
using Xbim.Ifc;


namespace Xbim.COBie.Client
{
    public partial class COBieGenerator : Form
    {

        public COBieGenerator()
        {
            InitializeComponent();
            MergeItemsList = new List<string>();
            UserFilters = new FilterValues(); //class filters

            // Configure the app to use Esent when appropriate
            IfcStore.ModelProviderFactory.UseHeuristicModelProvider();
        }

        public string ModelFile
        {
            get
            {
                return txtPath.Text;
            }
        }

        public string TemplateFile
        {
            get
            {
                if (File.Exists(txtTemplate.Text))
                {
                    return txtTemplate.Text;
                }
                else
                {
                    return Path.Combine("Templates", txtTemplate.Text);
                }
            }
        }

        public bool IsXLSX
        {
            get
            {
                return (cmboxFiletype.Text == "XLSX") ? true : false;
            } 
 
        }

        public COBieMergeRoles Roles { get; private set; }

        public IfcStore Model { get; set; }

        public FilterValues UserFilters { get; set; }

        public List<string> MergeItemsList { get; set; }
        private bool IsGenerating { get; set; } // stop button clicks when already clicked

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (!IsGenerating)
            {
                IsGenerating = true;
                Roles = COBieMergeRoles.Unknown; //set to unknown, reset below if any, if not will stay as unknown
                var checkedRoles = checkedListRoles.CheckedItems;
                var index = 0;
                foreach (var item in checkedRoles)
                {
                    var rolestr = (string)item;
                    var role = (COBieMergeRoles)Enum.Parse(typeof(COBieMergeRoles), rolestr);
                    if (index == 0)
                    {
                        Roles = role;
                    }
                    else
                    {
                        Roles |= role;
                    }

                    index++;
                }
                Generate();
            }
            else
            {
                MessageBox.Show("Currently in an action");
            }

        }

        private void Generate()
        {
            CreateWorker();
            _worker.DoWork += COBieWorker;

            var cobieParams = BuildParams();
            _worker.RunWorkerAsync(cobieParams);
        }

        private Params BuildParams()
        {
            if (MergeChkBox.Checked)
            {
                return new MergeParams() { MergeItemsList = MergeItemsList, ModelFile = MergeItemsList.FirstOrDefault(), TemplateFile = TemplateFile, IsXLSX = IsXLSX };
            }
            else
            {
                return new Params() { ModelFile = ModelFile, TemplateFile = TemplateFile, IsXLSX = IsXLSX };
            }
        }

        private void COBieWorker(object s, DoWorkEventArgs args)
        {
            try
            {
                var parameters = args.Argument as Params;

                if ((parameters.ModelFile == null) || (!File.Exists(parameters.ModelFile)))
                {
                    LogBackground(String.Format("That file doesn't exist in {0}.", Directory.GetCurrentDirectory()));
                    return;
                }
                if (parameters is MergeParams)
                {
                    MergeCOBieFiles(parameters as MergeParams);
                }
                else if (Path.GetExtension(parameters.ModelFile).ToLower() == ".xls")
                {
                    if (ValidateChkBox.Checked)
                        ValidateXlSfile(parameters);
                    else
                        GenerateIFCFile(parameters);
                }
                else if (Path.GetExtension(parameters.ModelFile).ToLower() == ".xbimf") //federated 
                {
                    GenerateFederatedCOBieFile(parameters);
                }
                else
                {
                    GenerateCOBieFile(parameters);
                }
            }
            catch (Exception ex)
            {
                args.Result = ex;
                return;
            }
            finally { IsGenerating = false; }

        }

        /// <summary>
        /// Create the COBieBuilder, holds COBieWorkBook
        /// </summary>
        /// <param name="parameters">Params</param>
        /// <returns>COBieBuilder</returns>
        private COBieWorkbook GenerateFederatedCOBieWorkBook(Params parameters)
        {
            var xbimFile = parameters.ModelFile;
            COBieBuilder builder = null;
            COBieWorkbook fedWorkBook = null;
            var workbooks = new List<COBieWorkbook>();

            LogBackground(String.Format("Loading federated model {0}...", xbimFile));
            using (var model = IfcStore.Open(xbimFile))
            {
                Model = model; //used to check we close file on Close event

                // Build context on the XBimF model to link roles
                var context = new COBieContext(_worker.ReportProgress)
                {
                    TemplateFileName = parameters.TemplateFile,
                    Model = model,
                    Exclude = UserFilters
                };
                foreach (var modelRoles in context.MapMergeRoles)
                {
                    var refModel = modelRoles.Key;
                    var roles = modelRoles.Value;

                    var refContext = new COBieContext(_worker.ReportProgress)
                    {
                        TemplateFileName = parameters.TemplateFile,
                        Model = (IfcStore)refModel
                    };
                    refContext.MapMergeRoles[refModel] = roles;
                    refContext.Exclude = UserFilters;

                    // Create COBieReader
                    LogBackground("Generating COBie data...");
                    builder = new COBieBuilder(refContext);
                    workbooks.Add(builder.Workbook);
                }
            }
            if (workbooks.Count > 1)
            {
                var fedCOBie = new FederateCOBie(_worker.ReportProgress);
                fedWorkBook = fedCOBie.Merge(workbooks);
            }
            else if (workbooks.Count == 1)
            {
                fedWorkBook = workbooks[0];
            }

            return fedWorkBook;
        }

        /// <summary>
        /// Create XLS file from ifc/xbim files
        /// </summary>
        /// <param name="parameters">Params</param>
        /// <param name="validationTemplate"></param>
        private void GenerateFederatedCOBieFile(Params parameters, ICOBieValidationTemplate validationTemplate = null)
        {
            var outputFile = Path.ChangeExtension(parameters.ModelFile, ".xls");
            var timer = new Stopwatch();
            timer.Start();
            var fedWorkBook = GenerateFederatedCOBieWorkBook(parameters);
            timer.Stop();
            LogBackground(String.Format("Time to generate Federated COBie data = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));

            // Export
            LogBackground(String.Format("Formatting as {1} using {0} template...", Path.GetFileName(parameters.TemplateFile), parameters.IsXLSX ? "XLSX" : "XLS"));
            var serialiser = new COBieXLSXSerialiser(outputFile, parameters.TemplateFile);
            serialiser.Excludes = UserFilters;
            serialiser.IsXlsx = parameters.IsXLSX;
            serialiser.Serialise(fedWorkBook, validationTemplate);

            LogBackground(String.Format("Export Complete: {0}", outputFile));

            Process.Start(serialiser.FileName);

            LogBackground("Finished Federation COBie Generation");
        }

        /// <summary>
        /// Create XLS file from ifc/xbim files
        /// </summary>
        /// <param name="parameters">Params</param>
        private void GenerateCOBieFile(Params parameters)
        {
            var outputFile = Path.ChangeExtension(parameters.ModelFile, ".xls");
            var timer = new Stopwatch();
            timer.Start();
            var builder = GenerateCOBieWorkBook(parameters);
            timer.Stop();
            LogBackground(String.Format("Time to generate COBie data = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));

            // Export
            LogBackground(String.Format("Formatting as {1} using {0} template...", Path.GetFileName(parameters.TemplateFile), parameters.IsXLSX ? "XLSX" : "XLS"));
            var serialiser = new COBieXLSXSerialiser(outputFile, parameters.TemplateFile);
            serialiser.Excludes = UserFilters;
            serialiser.IsXlsx = parameters.IsXLSX;
            builder.Export(serialiser);

            LogBackground(String.Format("Export Complete: {0}", outputFile));

            Process.Start(serialiser.FileName);

            LogBackground("Finished COBie Generation");
        }

        /// <summary>
        /// Create the COBieBuilder, holds COBieWorkBook
        /// </summary>
        /// <param name="parameters">Params</param>
        /// <returns>COBieBuilder</returns>
        private COBieBuilder GenerateCOBieWorkBook(Params parameters)
        {
            COBieBuilder builder = null;
            LogBackground(String.Format("Loading model {0}...", Path.GetFileName(parameters.ModelFile)));
            using (var model = IfcStore.Open(parameters.ModelFile))
            {

                // Build context
                var context = new COBieContext(_worker.ReportProgress);
                context.TemplateFileName = parameters.TemplateFile;
                context.Model = model;
                context.MapMergeRoles[model] = Roles;
                context.Exclude = UserFilters;
                Model = model; //used to check we close file on Close event
                //Create Scene, required for Coordinates sheet
                if (!SkipGeoChkBox.Checked)
                {
                    GenerateGeometry(context);
                }

                // Create COBieReader
                LogBackground("Generating COBie data...");
                builder = new COBieBuilder(context);
            }
            return builder;
        }

        /// <summary>
        /// Validate XLS file for COBie errors, also will swap templates if required
        /// </summary>
        /// <param name="parameters">Params</param>
        /// <param name="validationTemplate"></param>
        /// <returns>Created file name</returns>
        private void ValidateXlSfile(Params parameters, ICOBieValidationTemplate validationTemplate = null)
        {

            //read xls file
            LogBackground(String.Format("Reading {0}....", parameters.ModelFile));
            var deSerialiser = new COBieXLSDeserialiser(parameters.ModelFile);
            var workbook = deSerialiser.Deserialise();

            //extract pick list from the template sheet and swap into workbook (US / UK)
            LogBackground("Swapping PickList from template...");
            COBieSheet<COBiePickListsRow> cobiePickLists = null;
            if ((!string.IsNullOrEmpty(parameters.TemplateFile)) &&
                File.Exists(parameters.TemplateFile)
                )
            {
                //extract the pick list sheet from template
                var deSerialiserPickList = new COBieXLSDeserialiser(parameters.TemplateFile, Constants.WORKSHEET_PICKLISTS);
                var wbookPickList = deSerialiserPickList.Deserialise();
                if (wbookPickList.Count > 0) cobiePickLists = (COBieSheet<COBiePickListsRow>)wbookPickList.FirstOrDefault();
                //check the workbook last sheet is a pick list
                if (workbook.LastOrDefault() is COBieSheet<COBiePickListsRow>)
                {
                    //remove original pick list and replace with templates
                    workbook.RemoveAt(workbook.Count - 1);
                    workbook.Add(cobiePickLists);
                }
                else
                {
                    LogBackground("Failed to Swap PickList from template...");
                }

            }

            var context = new COBieContext(_worker.ReportProgress);
            var progress = new COBieProgress(context);
            context.Exclude = UserFilters;

            //Validate
            progress.Initialise("Validating Workbooks", workbook.Count, 0);
            progress.ReportMessage("Building Indices...");
            foreach (var item in workbook)
            {
                item.BuildIndices();
            }
            progress.ReportMessage("Building Indices...Finished");

            // Validate the workbook
            progress.ReportMessage("Starting Validation...");

            workbook.Validate(ErrorRowIndexBase.RowTwo, null, (lastProcessedSheetIndex) =>
            {
                // When each sheet has been processed, increment the progress bar
                progress.IncrementAndUpdate();
            });
            progress.ReportMessage("Finished Validation");
            progress.Finalise();

            // Export
            LogBackground(String.Format("Formatting as {1} using {0} template...", Path.GetFileName(parameters.TemplateFile), parameters.IsXLSX ? "XLSX" : "XLS"));
            var serialiser = new COBieXLSXSerialiser(parameters.ModelFile, parameters.TemplateFile);
            serialiser.Excludes = UserFilters;
            serialiser.IsXlsx = parameters.IsXLSX;
            serialiser.Serialise(workbook, validationTemplate);

            LogBackground(String.Format("Export Complete: {0}", parameters.ModelFile));

            Process.Start(serialiser.FileName);

            LogBackground("Finished COBie Validation");


        }

        /// <summary>
        /// Create IFC file from XLS file
        /// </summary>
        /// <param name="parameters">Params</param>
        /// <returns>Created file name</returns>
        private string GenerateIFCFile(Params parameters)
        {
            string outputFile;

            LogBackground(String.Format("Reading {0}....", parameters.ModelFile));
            var deSerialiser = new COBieXLSDeserialiser(parameters.ModelFile);
            var newbook = deSerialiser.Deserialise();

            LogBackground("Creating xBim objects...");
            var timer = new Stopwatch();
            timer.Start();

            outputFile = Path.GetFileNameWithoutExtension(parameters.ModelFile) + "-COBieToIFC.ifc";
            outputFile = Path.GetDirectoryName(parameters.ModelFile) + "\\" + outputFile;

            using (var xBimSerialiser = new COBieXBimSerialiser(outputFile, _worker.ReportProgress))
            {
                xBimSerialiser.Serialise(newbook);
                timer.Stop();
                LogBackground(String.Format("Time to generate XBim COBie data = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));

            }
            LogBackground(String.Format("Finished {0} Generation", outputFile));
            return outputFile;
        }

        /// <summary>
        /// Merge COBie data files
        /// </summary>
        /// <param name="parameters">MergeParams</param>
        /// <returns>Created file name</returns>
        private string MergeCOBieFiles(MergeParams parameters)
        {
            var mergeList = new List<string>();
            mergeList.AddRange(parameters.MergeItemsList);
            var outputFile = string.Empty;
            if (mergeList.Count > 0)
            {
                var timer = new Stopwatch();
                timer.Start();

                var mainFile = mergeList.First();
                mergeList.RemoveAt(0);

                if (!File.Exists(mainFile))
                {
                    LogBackground(String.Format("File {0} doesn't exist. cannot merge as main file not found", mainFile));
                    return string.Empty;
                }

                outputFile = Path.GetFileNameWithoutExtension(mainFile) + "-COBieMergeToIFC.ifc";
                outputFile = Path.GetDirectoryName(mainFile) + "\\" + outputFile;
                parameters.ModelFile = mainFile;
                using (var xBimSerialiser = new COBieXBimSerialiser(outputFile, _worker.ReportProgress))
                {

                    LogBackground(String.Format("Reading main file {0}....", Path.GetFileName(mainFile)));
                    var mainWorkBook = GetWorkBook(parameters);

                    LogBackground(String.Format("Writing main file {0} to {1}....", Path.GetFileName(mainFile), Path.GetFileName(outputFile)));
                    xBimSerialiser.Create(mainWorkBook);
                    xBimSerialiser.MergeGeometryOnly = GeoOnlyChkBox.Checked;
                    foreach (var mergeFile in mergeList)
                    {
                        if (File.Exists(mergeFile))
                        {
                            var mergExt = Path.GetExtension(mergeFile).ToLower();
                            LogBackground(String.Format("Reading file to merge {0}....", Path.GetFileName(mergeFile)));
                            parameters.ModelFile = mergeFile;
                            var mergeWorkBook = GetWorkBook(parameters);
                            LogBackground(String.Format("Writing merge file {0} into {1}....", Path.GetFileName(mergeFile), Path.GetFileName(outputFile)));
                            xBimSerialiser.Merge(mergeWorkBook);

                        }
                        else
                        {
                            LogBackground(String.Format("File {0} doesn't exist. skipping merge on this file", mergeFile));
                        }
                    }

                    timer.Stop();
                    LogBackground(String.Format("Time to generate XBim COBie data = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));


                    LogBackground(String.Format("Creating file {0}....", Path.GetFileName(outputFile)));

                    xBimSerialiser.Save();
                }
                LogBackground(String.Format("Finished Generating {0}", outputFile));
            }
            return outputFile;
        }

        /// <summary>
        /// Generate a COBieWorkbook from ifc/xbim/xls files
        /// </summary>
        /// <param name="parameters">MergeParams</param>
        /// <returns>COBieWorkbook</returns>
        private COBieWorkbook GetWorkBook(MergeParams parameters)
        {
            var mainFile = parameters.ModelFile;
            var mainExt = Path.GetExtension(mainFile).ToLower();

            COBieWorkbook workBook = null;
            if (mainExt == ".xls")
            {
                var deSerialiser = new COBieXLSDeserialiser(mainFile);
                workBook = deSerialiser.Deserialise();
            }
            else if ((mainExt == ".ifc") || (mainExt == ".xbim"))
            {
                var builder = GenerateCOBieWorkBook(parameters);
                workBook = builder.Workbook;
            }
            return workBook;
        }




        //Needed Geometry to test, but Steve's comment on "need to resolve generate geometry" may see GenerateGeometry change
        private void GenerateGeometry(COBieContext context)
        {
            //now convert the geometry
            var model = context.Model;
            var total = (int)model.Instances.CountOf<IfcProduct>();
            ReportProgressDelegate progDelegate = delegate(int percentProgress, object userState)
            {
                context.UpdateStatus("Creating Geometry File", total, (total * percentProgress / 100));
            };
            var m3D = new Xbim3DModelContext(model);
            try
            {
                m3D.CreateContext(progDelegate: progDelegate);
            }
            catch (Exception ce)
            {
                context.UpdateStatus("Error compiling geometry\n" + ce.Message, total, 100);
            }
        }




        private void AppendLog(string text)
        {
            txtOutput.AppendText(text + Environment.NewLine);
            txtOutput.ScrollToCaret();
        }

        private void LogBackground(string text)
        {
            _worker.ReportProgress(0, text);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "All XBim Files|*.ifc;*.ifcxml;*.ifczip;*.xbim;*.xbimf|IFC Files|*.ifc;*.ifcxml;*.ifczip|Xbim Files|*.xbim|Xbim Federated Files|*.xbimf|XLS Files|*.xls";
            dlg.Title = "Choose a source model file";

            dlg.CheckFileExists = true;
            // Show open file dialog box 
            dlg.FileOk += new CancelEventHandler(dlg_FileOk);
            dlg.ShowDialog();
        }

        private void dlg_FileOk(object sender, CancelEventArgs ce)
        {
            var dlg = sender as OpenFileDialog;
            if (dlg != null)
            {
                txtPath.Text = dlg.FileName;
                if (Path.GetExtension(dlg.FileName).ToLower() == ".xls")
                    ValidateChkBox.Enabled = true;
                else
                    ValidateChkBox.Enabled = false;

                if ((Path.GetExtension(dlg.FileName).ToLower() == ".ifc") ||
                    (Path.GetExtension(dlg.FileName).ToLower() == ".xbim")
                    )
                    checkedListRoles.Enabled = true;
                else
                    checkedListRoles.Enabled = false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
        }

        private void btnBrowseTemplate_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Title = "Choose a COBie template file";
            dlg.Filter = "XLS Files|*.xls";

            dlg.CheckFileExists = true;
            // Show open file dialog box 
            dlg.FileOk += new CancelEventHandler(dlg_TemplateFileOk);
            dlg.ShowDialog();
        }

        private void dlg_TemplateFileOk(object sender, CancelEventArgs ce)
        {
            var dlg = sender as OpenFileDialog;
            if (dlg != null)
            {
                txtTemplate.Text = dlg.FileName;
            }
        }

        BackgroundWorker _worker;

        private void CreateWorker()
        {
            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = false;
            _worker.ProgressChanged += (s, args) =>
            {
                StatusMsg.Text = (string)args.UserState;
                if (args.ProgressPercentage == 0)
                {
                    AppendLog(args.UserState.ToString());
                }
                else
                {
                    ProgressBar.Value = Math.Max (0, Math.Min (args.ProgressPercentage, 100));
                }
            };

            _worker.RunWorkerCompleted += (object s, RunWorkerCompletedEventArgs args) =>
            {
                var errMsg = args.Result as String;
                if (!string.IsNullOrEmpty(errMsg))
                    AppendLog(errMsg);

                if (args.Result is Exception)
                {

                    var sb = new StringBuilder();
                    var ex = args.Result as Exception;
                    var indent = "";
                    while (ex != null)
                    {
                        sb.AppendFormat("{0}{1}\n", indent, ex.Message);
                        ex = ex.InnerException;
                        indent += "\t";
                    }
                    AppendLog(sb.ToString());
                }

            };
        }

        private class Params
        {
            public string ModelFile { get; set; }
            public string TemplateFile { get; set; }
            public string ValidaitonFile { get; set; }
            public bool IsXLSX { get; set; }
        }

        private class MergeParams : Params
        {
            public List<string> MergeItemsList { get; set; }
            //public string FileToMerge { get; set; }
        }

        private void MergeChkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (MergeChkBox.Checked)
            {
                GeoOnlyChkBox.Enabled = true;
                mergeBtn.Enabled = true;
                groupBox1.Enabled = false;
            }
            else
            {
                GeoOnlyChkBox.Enabled = false;
                mergeBtn.Enabled = false;
                groupBox1.Enabled = true;
            }
        }

        private void mergeBtn_Click(object sender, EventArgs e)
        {
            var mergeSelectDlg = new MergeSelect(MergeItemsList);
            if (mergeSelectDlg.ShowDialog() == DialogResult.OK)
            {
                MergeItemsList = mergeSelectDlg.mergeItemsOut;
                AppendLog("Files to merge");
                foreach (var item in MergeItemsList)
                {
                    AppendLog(Path.GetFileName(item));
                }
            }
        }

        private void COBieGenerator_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Model != null)
            {
                Model.Close();
            }


        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPath.Text))
            {
                if (Path.GetExtension(txtPath.Text).ToLower() == ".xbim")
                {
                    SkipGeoChkBox.Checked = true; //if xbim file assume we have geometry
                    SkipGeoChkBox.Enabled = false;
                }
                else
                    SkipGeoChkBox.Enabled = true;

            }

        }

        private void COBieGenerator_Load(object sender, EventArgs e)
        {
            checkedListRoles.Items.AddRange(Enum.GetNames(typeof(COBieMergeRoles)));
        }

        private void btnClassFilter_Click(object sender, EventArgs e)
        {
            var classFilterDlg = new ClassFilter(UserFilters);
            if (classFilterDlg.ShowDialog() == DialogResult.OK)
            {
                UserFilters = classFilterDlg.UserFilters; //not needed but just to make it clear 
            }
        }
    }

}
