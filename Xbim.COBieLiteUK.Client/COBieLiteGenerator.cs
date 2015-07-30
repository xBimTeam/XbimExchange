using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xbim.FilterHelper;
using Xbim.COBieLiteUK;
using System.Diagnostics;
using Xbim.IO;
using XbimExchanger.IfcToCOBieLiteUK;

namespace Xbim.Client
{
    public partial class COBieLiteGenerator : Form
    {
        private BackgroundWorker _worker;
        private OutPutFilters _assetfilters = new OutPutFilters(); //empty role filter
        private string FileName { get;  set; }
        public FileInfo ConfigFile { get; private set; }
        private COBiePropertyMapping PropertyMaps { get;  set; }

        public COBieLiteGenerator()
        {
            InitializeComponent();
            //set default role filters held in FillRolesFilterHolder property list
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            ConfigFile = new FileInfo(Path.Combine(dir.FullName, "COBieAttributes.config"));
            if (!ConfigFile.Exists)
            {
                AppendLog("Creating Config File");
                CreateDefaultAppConfig(ConfigFile);
                ConfigFile.Refresh();
            }
            if (_assetfilters.DefaultsNotSet)
            {
                _assetfilters.FillRolesFilterHolderFromDir(dir);
            }

            PropertyMaps = new COBiePropertyMapping(ConfigFile);
        }
        
        /// <summary>
        /// Copy resource help config file to working directory
        /// </summary>
        /// <param name="fileName"></param>
        public void CreateDefaultAppConfig(FileInfo configFile)
        {
            var asss = System.Reflection.Assembly.GetAssembly(typeof(IfcToCOBieLiteUkExchanger));

            using (var input = asss.GetManifestResourceStream("XbimExchanger.IfcToCOBieLiteUK.COBieAttributes.config"))
            using (var output = configFile.Create())
            {
                if (input != null) input.CopyTo(output);
            }
            configFile.Refresh();
        }
        
        /// <summary>
        /// Set roles for this run
        /// </summary>
        private RoleFilter SetRoles()
        {
            RoleFilter roles = RoleFilter.Unknown; //reset to unknown
            var checkedRoles = checkedListRoles.CheckedItems;
            //set selected roles
            foreach (var item in checkedRoles)
            {
                try
                {
                    RoleFilter role = (RoleFilter)Enum.Parse(typeof(RoleFilter), (string)item);
                    roles |= role;
                }
                catch (Exception)
                {
                    AppendLog("Error: Failed to get requested role");
                }
            }
            //we have selected roles so remove unknown
            if (checkedRoles.Count > 0)
            {
                roles &= ~RoleFilter.Unknown;//remove unknown
            }
            if (!chkBoxNoFilter.Checked)
            {
                AppendLog("Selected Roles: " + roles.ToString("F"));
            }
            else
            {
                AppendLog("Selected Roles: Disabled by no filter flag ");
            }
            
            return roles;
        }

        /// <summary>
        /// Set the System extraction Methods
        /// </summary>
        /// <returns></returns>
        private SystemExtractionMode SetSystemMode()
        {
            SystemExtractionMode sysMode = SystemExtractionMode.System;
            var checkedSysMode = checkedListSys.CheckedItems;
            //add the checked system modes
            foreach (var item in checkedSysMode)
            {
                try
                {
                    SystemExtractionMode mode = (SystemExtractionMode)Enum.Parse(typeof(SystemExtractionMode), (string)item);
                    sysMode |= mode;
                }
                catch (Exception)
                {
                    AppendLog("Error: Failed to get requested system extraction mode");
                }
            }

            return sysMode;
        }


        private void CreateWorker()
        {
            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = false;
            _worker.ProgressChanged += (object s, ProgressChangedEventArgs args) =>
            {
                StatusMsg.Text = (string)args.UserState;
                if (args.ProgressPercentage == 0)
                {
                    AppendLog(args.UserState.ToString());
                }
                else
                {
                    ProgressBar.Value = args.ProgressPercentage;
                }
            };

            _worker.RunWorkerCompleted += (object s, RunWorkerCompletedEventArgs args) =>
            {
                
                try
                {
                    if (args.Result is Exception)
                    {

                        StringBuilder sb = new StringBuilder();
                        Exception ex = args.Result as Exception;
                        string indent = "";
                        while (ex != null)
                        {
                            sb.AppendFormat("{0}{1}\n", indent, ex.Message);
                            ex = ex.InnerException;
                            indent += "\t";
                        }
                        AppendLog(sb.ToString());
                    }
                    else
                    {
                        string errMsg = args.Result as string;
                        if (!string.IsNullOrEmpty(errMsg))
                            AppendLog(errMsg);

                    }
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    string indent = "";
                        
                    while (ex != null)
                    {
                        sb.AppendFormat("{0}{1}\n", indent, ex.Message);
                        ex = ex.InnerException;
                        indent += "\t";
                    }
                    AppendLog(sb.ToString());
                }
                finally
                {
                   btnGenerate.Enabled = true;
                }
                //open file if ticked to open excel
                if (chkBoxOpenFile.Checked && !string.IsNullOrEmpty(FileName))
                {
                    Process.Start(FileName);
                }
            };
        }

        /// <summary>
        /// Add string to Text Output List Box 
        /// </summary>
        /// <param name="text"></param>
        private void AppendLog(string text)
        {
            txtOutput.AppendText(text + Environment.NewLine);
            txtOutput.ScrollToCaret();
        }

        
        /// <summary>
        /// On Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void COBieLiteGenerator_Load(object sender, EventArgs e)
        {
            //set roles
            var roleList = Enum.GetNames(typeof(RoleFilter));
            checkedListRoles.Items.AddRange(roleList.Where(r => r != RoleFilter.Unknown.ToString()).ToArray());
            var sysList = Enum.GetNames(typeof(SystemExtractionMode));
            checkedListSys.Items.AddRange(sysList.Where(r => r != SystemExtractionMode.System.ToString()).ToArray());
            for (int i = 0; i < checkedListSys.Items.Count; i++) //set all to ticked
            {
                checkedListSys.SetItemChecked(i, true);
            }
            //set template from resources
            var templates = typeof(Xbim.COBieLiteUK.Facility).Assembly.GetManifestResourceNames();
            templates = templates.Where(x => x.Contains(".Templates.")).ToArray();
            templates = templates.Select(x => x.Split(new char[] { '.' })[3]).Distinct().ToArray();
            txtTemplate.Items.AddRange(templates);
            txtTemplate.SelectedIndex = 0;
            cmboxFiletype.SelectedIndex = 1;

            //set up tooltips
            ToolTip tooltip = new ToolTip();
            tooltip.AutoPopDelay = 8000;
            tooltip.InitialDelay = 1000;
            tooltip.ReshowDelay = 500;
            tooltip.ShowAlways = true;
            tooltip.IsBalloon = true;
            tooltip.SetToolTip(chkBoxFlipFilter, "Export all excludes to excel file, Note PropertySet Excludes are not flipped");
            tooltip.SetToolTip(chkBoxOpenFile, "Open in excel once file is created");
            tooltip.SetToolTip(checkedListRoles, "Select roles for export filtering");
            tooltip.SetToolTip(btnGenerate, "Generate COBie excel workbook");
            tooltip.SetToolTip(btnBrowse, "Select file to extract COBie from");
            tooltip.SetToolTip(btnBrowseTemplate, "Select template excel file");
            tooltip.SetToolTip(cmboxFiletype, "Select excel file extension to generate");
            tooltip.SetToolTip(btnClassFilter, "Setup Filter");
            tooltip.SetToolTip(btnMergeFilter, "Filter To Apply, based on selected roles");
            tooltip.SetToolTip(btnPropMaps, "Set PropertySet.PropertyName mappings to COBie Fileds");
            tooltip.SetToolTip(chkBoxIds, "Tick to change GUID IDs to Entity IDs");
            tooltip.SetToolTip(checkedListSys, "Tick to include additional Systems (ifcSystems always included)\nPropertyMaps = PropertySet Mappings, System Tab in \"Property Maps\" dialog\nTypes = Object Types listing associated components (not assigned to system)");
            
        }

        /// <summary>
        /// On Click Generate Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (chkBoxFlipFilter.Checked)
            {
                var result = MessageBox.Show("Flip Filter is ticked, this will show only excluded items, Do you want to continue", "Warning", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            btnGenerate.Enabled = false;
            CreateWorker();
            _worker.DoWork += CobieLiteUKWorker;
            //get Excel File Type
            ExcelTypeEnum excelType = GetExcelType();
            //set parameters
            var args = new Params { ModelFile = txtPath.Text,
                                    TemplateFile = txtTemplate.Text,
                                    Roles = SetRoles(),
                                    ExcelType = excelType,
                                    FlipFilter = chkBoxFlipFilter.Checked,
                                    OpenExcel = chkBoxOpenFile.Checked,
                                    FilterOff = chkBoxNoFilter.Checked,
                                    ExtId = chkBoxIds.Checked ? EntityIdentifierMode.IfcEntityLabels : EntityIdentifierMode.GloballyUniqueIds,
                                    SysMode = SetSystemMode()
            };
            //run worker
            _worker.RunWorkerAsync(args);

        }
        
        /// <summary>
        /// DOWork function for worker, generate excel COBie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CobieLiteUKWorker(object sender, DoWorkEventArgs e)
        {
             Params parameters = e.Argument as Params;
            if ((string.IsNullOrEmpty(parameters.ModelFile)) || (!File.Exists(parameters.ModelFile)))
            {
                _worker.ReportProgress(0, string.Format("That file doesn't exist: {0}.", parameters.ModelFile));
                return;
            }
            GenerateCOBieFile(parameters);

        }

        /// <summary>
        /// Create XLS file from ifc/xbim files
        /// </summary>
        /// <param name="parameters">Params</param>
        private void GenerateCOBieFile(Params parameters)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //set attribute name filters
            if (parameters.FilterOff)
            {
                _assetfilters = new OutPutFilters();
            }
            else
            {
                _assetfilters.ApplyRoleFilters(parameters.Roles);
                _assetfilters.FlipResult = parameters.FlipFilter;
            }
            

            var facilities = GenerateFacility(parameters);
            timer.Stop();
            _worker.ReportProgress(0, string.Format("Time to generate COBieLite data = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
            timer.Reset();
            timer.Start();
            int index = 1;
            foreach (var facilityType in facilities)
            {
                string fileName = Path.GetFileNameWithoutExtension(parameters.ModelFile) + ((facilities.Count == 1) ? "" : index.ToString());
                string path = Path.GetDirectoryName(parameters.ModelFile);
                FileName = Path.Combine(path, Path.ChangeExtension(fileName, parameters.ExcelType == ExcelTypeEnum.XLS ? ".xls" : ".xlsx"));
                string logFile = Path.ChangeExtension(FileName, ".log");
                _worker.ReportProgress(0, string.Format("Creating validation log file: {0}", logFile));
                using (var log = File.CreateText(logFile))
                { 
                    facilityType.ValidateUK2012(log, false);
                }
                _worker.ReportProgress(0, string.Format("Creating file: {0}", FileName));
                
                string msg;
                
                using (var file = File.Create(FileName))
                {
                    facilityType.WriteCobie(file, parameters.ExcelType, out msg, _assetfilters, parameters.TemplateFile, true);
                }
                _worker.ReportProgress(0, msg);
                
                index++;
            }
            timer.Stop();
            _worker.ReportProgress(0, string.Format("Time to generate COBieLite Excel = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));

            _worker.ReportProgress(0, "Finished COBie Generation");
        }

        /// <summary>
        /// Genertate the Facilities held within the Model
        /// </summary>
        /// <param name="parameters">Params</param>
        /// <returns>List of Facilities</returns>
        private List<Facility> GenerateFacility(Params parameters)
        {
            string fileExt = Path.GetExtension(parameters.ModelFile);
            var facilities = new List<Facility>();
            using (var model = new XbimModel())
            {
                if ((fileExt.Equals(".xbim", StringComparison.OrdinalIgnoreCase)) ||
                    (fileExt.Equals(".xbimf", StringComparison.OrdinalIgnoreCase))
                   )
                {
                    model.Open(parameters.ModelFile, XbimExtensions.XbimDBAccess.Read, _worker.ReportProgress);
                }
                else
                {
                    var xbimFile = Path.ChangeExtension(parameters.ModelFile, "xbim");
                    model.CreateFrom(parameters.ModelFile, xbimFile, _worker.ReportProgress, true, true);

                }


                var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(model, facilities, _assetfilters, ConfigFile.FullName, parameters.ExtId, parameters.SysMode);
                facilities = ifcToCoBieLiteUkExchanger.Convert();
            }
            return facilities;
        }

        /// <summary>
        /// Get Excel Type From Combo
        /// </summary>
        /// <returns>ExcelTypeEnum</returns>
        private ExcelTypeEnum GetExcelType()
        {
            ExcelTypeEnum excelType;
            try
            {
                excelType = (ExcelTypeEnum)Enum.Parse(typeof(ExcelTypeEnum), cmboxFiletype.Text);
            }
            catch (Exception)
            {
                excelType = ExcelTypeEnum.XLSX;
            }
            return excelType;
        }


        /// <summary>
        /// On Click Browse Model File Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All XBim Files|*.ifc;*.ifcxml;*.ifczip;*.xbim;*.xbimf|IFC Files|*.ifc;*.ifcxml;*.ifczip|Xbim Files|*.xbim|Xbim Federated Files|*.xbimf"; //|XLS Files|*.xls
            dlg.Title = "Choose a source model file";

            dlg.CheckFileExists = true;
            // Show open file dialog box 
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPath.Text = dlg.FileName;
            }
        }

        /// <summary>
        /// Select template from file system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseTemplate_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Choose a COBie template file";
            dlg.Filter = "Excel Files|*.xls;*.xlsx";
            
            dlg.CheckFileExists = true;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtTemplate.Text = dlg.FileName;
                SetExcelType();
                cmboxFiletype.Enabled = false;
            }
        }

        /// <summary>
        /// set the Excel type combo box from template extension
        /// </summary>
        private void SetExcelType()
        {
            var ext = Path.GetExtension(txtTemplate.Text);
            if (!string.IsNullOrEmpty(ext))
            {
                ext = ext.Substring(1).ToUpper();
            }
            cmboxFiletype.SelectedIndex = cmboxFiletype.Items.IndexOf(ext);
        }

        /// <summary>
        /// Change on templates combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmboxFiletype.Enabled = true; //enable excel file type combo
        }
        /// <summary>
        /// Clear Text Box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
        }

        /// <summary>
        /// Internal Params Class, holds parameters for to worker to access
        /// </summary>
        private class Params
        {
            public string ModelFile { get; set; }
            public string TemplateFile { get; set; }
            public ExcelTypeEnum ExcelType { get; set; }
            public bool FlipFilter { get; set; }
            public bool OpenExcel { get; set; }
            public RoleFilter Roles { get; set; }
            public bool FilterOff { get; set; }
            public EntityIdentifierMode ExtId { get; set; }
            public SystemExtractionMode SysMode { get; set; }
        }

        /// <summary>
        /// Display notes on filter flip option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBoxFlipFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxFlipFilter.Checked)
            {
                txtOutput.AppendText("Test purposes Only: Will export filtered items to excel workbook, ignoring property set filters to ensure property names are shown" + Environment.NewLine);
            }
            else
            {
                txtOutput.AppendText("Filter Flip off" + Environment.NewLine);
            }
        }

        /// <summary>
        /// On Click Class filter Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClassFilter_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            //set the defaults if not already set
            if (_assetfilters.DefaultsNotSet)
            {
                
                _assetfilters.FillRolesFilterHolderFromDir(dir);
            }
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FilterDlg filterDlg = new FilterDlg(_assetfilters);
                if (filterDlg.ShowDialog() == DialogResult.OK)
                {
                    _assetfilters = filterDlg.RolesFilters;
                    _assetfilters.WriteXMLRolesFilterHolderToDir(dir);
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            
        }

        /// <summary>
        /// On Click Merge Filter Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMergeFilter_Click(object sender, EventArgs e)
        {
            RoleFilter roles = SetRoles();
            _assetfilters.ApplyRoleFilters(roles);
            FilterDlg filterDlg = new FilterDlg(_assetfilters, true, roles);
            //read only
            filterDlg.ShowDialog();
        }

        /// <summary>
        /// Check Box Change on No Filter Tick Box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBoxNoFilter_CheckedChanged(object sender, EventArgs e)
        {
            chkBoxFlipFilter.Enabled = !chkBoxNoFilter.Checked;
            btnClassFilter.Enabled = !chkBoxNoFilter.Checked;
            btnMergeFilter.Enabled = !chkBoxNoFilter.Checked;
            checkedListRoles.Enabled = !chkBoxNoFilter.Checked;
            if (chkBoxNoFilter.Checked)
            {
                txtOutput.AppendText("Filter Off" + Environment.NewLine);
            }
            else
            {
                txtOutput.AppendText("Filter On" + Environment.NewLine);
            }
        }

        /// <summary>
        /// Show the Property Fields Map Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPropMaps_Click(object sender, EventArgs e)
        {
            PropertyMapDlg PropMapDlg = new PropertyMapDlg(PropertyMaps);
            var result = PropMapDlg.ShowDialog(this);
            if (result == DialogResult.OK)
            {
               
                PropertyMaps = PropMapDlg.PropertyMaps;
                PropertyMaps.Save();
            }
        }
    

    }

    

}
