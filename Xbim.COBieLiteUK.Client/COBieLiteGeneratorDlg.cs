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
using Xbim.Common.Exceptions;

namespace Xbim.Client
{
    public partial class COBieLiteGeneratorDlg : Form
    {
        /// <summary>
        /// Worker
        /// </summary>
        private COBieLiteWorker _cobieWorker;

        /// <summary>
        /// Filters for Extracting COBie, Role Filtering
        /// </summary>
        private OutPutFilters _assetfilters = new OutPutFilters(); //empty role filter

        /// <summary>
        /// Refrence models to OutPutFilters mappings
        /// </summary>
        public Dictionary<FileInfo, RoleFilter> MapRefModelsRoles {get; private set; }

        

        /// <summary>
        /// Config File holding mappings for ifc property extractions
        /// </summary>
        public FileInfo ConfigFile { get; private set; }

        /// <summary>
        /// Mappings for COBie proerties from IFC
        /// </summary>
        private COBiePropertyMapping PropertyMaps { get;  set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public COBieLiteGeneratorDlg()
        {
            InitializeComponent();
            //set default role filters held in FillRolesFilterHolder property list
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            ConfigFile = new FileInfo(Path.Combine(dir.FullName, "COBieAttributesCustom.config"));
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
            MapRefModelsRoles = new Dictionary<FileInfo, RoleFilter>();
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
        /// On Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void COBieLiteGenerator_Load(object sender, EventArgs e)
        {
            //set roles
            //var roleList = Enum.GetNames(typeof(RoleFilter));
            //checkedListRoles.Items.AddRange(roleList.Where(r => r != RoleFilter.Unknown.ToString()).ToArray());
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
            tooltip.SetToolTip(rolesList, "Select roles for export filtering");
            tooltip.SetToolTip(btnGenerate, "Generate COBie excel workbook");
            tooltip.SetToolTip(btnBrowse, "Select file to extract COBie from");
            tooltip.SetToolTip(btnFederate, "Create federated file to extract COBie from");
            tooltip.SetToolTip(btnBrowseTemplate, "Select template excel file");
            tooltip.SetToolTip(cmboxFiletype, "Select excel file extension to generate");
            tooltip.SetToolTip(btnClassFilter, "Setup Filter");
            tooltip.SetToolTip(btnMergeFilter, "Filter To Apply, based on selected roles");
            tooltip.SetToolTip(btnPropMaps, "Set PropertySet.PropertyName mappings to COBie Fileds");
            tooltip.SetToolTip(chkBoxIds, "Tick to change GUID IDs to Entity IDs");
            tooltip.SetToolTip(checkedListSys, "Tick to include additional Systems (ifcSystems always included)\nPropertyMaps = PropertySet Mappings, System Tab in \"Property Maps\" dialog\nTypes = Object Types listing associated components (not assigned to system)");

        }

        /// <summary>
        /// Set roles for this run
        /// </summary>
        private RoleFilter SetRoles()
        {
            var roles = rolesList.Roles;
            if (!chkBoxNoFilter.Checked)
            {
                if (MapRefModelsRoles.Count > 0)
                {
                    foreach (var item in MapRefModelsRoles)
                    {
                        AppendLog(string.Format("{0} has Roles: {1}",item.Key.Name, item.Value.ToString("F")));
                    }
                }
                else
                {
                    AppendLog("Selected Roles: " + roles.ToString("F"));
                }
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
            if (_cobieWorker == null)
            {
                _cobieWorker = new COBieLiteWorker();
                _cobieWorker.Worker.ProgressChanged += WorkerProgressChanged;
                _cobieWorker.Worker.RunWorkerCompleted += WorkerCompleted;
            }
            //get Excel File Type
            ExcelTypeEnum excelType = GetExcelType();
            //set filters
            RoleFilter filterRoles = SetRoles();
            if (!chkBoxNoFilter.Checked)
            {
                _assetfilters.ApplyRoleFilters(filterRoles);
                _assetfilters.FlipResult = chkBoxFlipFilter.Checked;
            }

            //set parameters
            var args = new Params { ModelFile = txtPath.Text,
                TemplateFile = txtTemplate.Text,
                Roles = filterRoles,
                ExcelType = excelType,
                FlipFilter = chkBoxFlipFilter.Checked,
                OpenExcel = chkBoxOpenFile.Checked,
                FilterOff = chkBoxNoFilter.Checked,
                ExtId = chkBoxIds.Checked ? EntityIdentifierMode.IfcEntityLabels : EntityIdentifierMode.GloballyUniqueIds,
                SysMode = SetSystemMode(),
                Filter = chkBoxNoFilter.Checked ? new OutPutFilters() : _assetfilters,
                ConfigFile = ConfigFile.FullName,
                Log = chkBoxLog.Checked,
            };
            //run worker
           _cobieWorker.Run(args);

        }

        #region Worker Methods
        /// <summary>
        /// Worker Complete method
        /// </summary>
        /// <param name="s"></param>
        /// <param name="args"></param>
        public void WorkerCompleted(object s, RunWorkerCompletedEventArgs args)
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
            if (chkBoxOpenFile.Checked && args.Result != null && !string.IsNullOrEmpty(args.Result.ToString()))
            {
                Process.Start(args.Result.ToString());
            }
            ProgressBar.Visible = false;
        }

        /// <summary>
        /// Worker Progress Changed
        /// </summary>
        /// <param name="s"></param>
        /// <param name="args"></param>
        public void WorkerProgressChanged(object s, ProgressChangedEventArgs args)
        {
            
            //Show message in Text List Box
            if (args.ProgressPercentage == 0)
            {
                StatusMsg.Text = string.Empty;
                ProgressBar.Visible = false;
                AppendLog(args.UserState.ToString());
            }
            else //show message on status bar and update progress bar
            {
                if (ProgressBar.Visible == false)
                {
                    ProgressBar.Visible = true;
                }
                StatusMsg.Text = args.UserState.ToString();
                ProgressBar.Value = args.ProgressPercentage;
            }
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
        #endregion


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
                string fileExt = Path.GetExtension(txtPath.Text);
                MapRefModelsRoles.Clear();
                if (fileExt.Equals(".xbimf", StringComparison.OrdinalIgnoreCase))
                {
                    using (var fedModel = new FederatedModel(new FileInfo(txtPath.Text)))
                    {
                        if (fedModel.Model.IsFederation)
                        {
                            MapRefModelsRoles = fedModel.RefModelRoles.ToDictionary(m => new FileInfo(m.Key.DatabaseName), m => m.Value);
                        }
                        else
                        {
                            throw new XbimException(string.Format("Model is not Federated: {0}", fedModel.FileNameXbimf));
                        }
                    }
                   
                }
                rolesList.Enabled = !fileExt.Equals(".xbimf", StringComparison.OrdinalIgnoreCase);
            }
            
        }

        /// <summary>
        /// On Click federated button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFederate_Click(object sender, EventArgs e)
        {
            Federate FedDlg = new Federate(txtPath.Text);
            var result = FedDlg.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtPath.Text = FedDlg.FileName;
                MapRefModelsRoles = FedDlg.FileRoles;
            }
            rolesList.Enabled = (Path.GetExtension(txtPath.Text).ToUpper() != ".XBIMF");
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
            //set the defaults if not already set
            if (_assetfilters.DefaultsNotSet)
            {
                DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                _assetfilters.FillRolesFilterHolderFromDir(dir);
            }
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FilterDlg filterDlg = null;
                string fileExt = Path.GetExtension(txtPath.Text);
                if (fileExt.Equals(".xbimf", StringComparison.OrdinalIgnoreCase))
                {
                    Dictionary<FileInfo, OutPutFilters> FedFilters = _assetfilters.SetFedModelFilter<FileInfo>(MapRefModelsRoles);
                    filterDlg = new FilterDlg(_assetfilters, true, FedFilters);
                }
                else
                {
                    MapRefModelsRoles.Clear();
                    RoleFilter roles = SetRoles();
                    _assetfilters.ApplyRoleFilters(roles);
                    filterDlg = new FilterDlg(_assetfilters, true);
                }

                //read only
                if (filterDlg != null)
                {
                    filterDlg.ShowDialog();
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            
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
            rolesList.Enabled = !chkBoxNoFilter.Checked;
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

        

        private void checkedListSys_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void chkBoxIds_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkBoxLog_CheckedChanged(object sender, EventArgs e)
        {

        }
    }


}
