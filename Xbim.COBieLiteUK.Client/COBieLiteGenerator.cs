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
using Xbim.COBieLiteUK.Client;
using System.Diagnostics;
using Xbim.IO;
using XbimExchanger.IfcToCOBieLiteUK;

namespace Xbim.Client
{
    public partial class COBieLiteGenerator : Form
    {
        BackgroundWorker _worker;
        

        public COBieLiteGenerator()
        {
            InitializeComponent();
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
            AppendLog("Selected Roles: " + roles.ToString("F"));
            return roles;
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
                        String indent = "";
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
                        string errMsg = args.Result as String;
                        if (!string.IsNullOrEmpty(errMsg))
                            AppendLog(errMsg);

                    }
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                     String indent = "";
                        
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
                

            };
        }

        private void AppendLog(string text)
        {
            txtOutput.AppendText(text + Environment.NewLine);
            txtOutput.ScrollToCaret();
        }

        

        private void COBieLiteGenerator_Load(object sender, EventArgs e)
        {
            //set roles
            var roleList = Enum.GetNames(typeof(RoleFilter));
            checkedListRoles.Items.AddRange(roleList.Where(r => r != RoleFilter.Unknown.ToString()).ToArray());
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

            tooltip.SetToolTip(chkBoxFlipFilter, "Export all excludes to excel file, Note PropertySet Excludes are not flipped");
            tooltip.SetToolTip(chkBoxOpenFile, "Open in excel once file is created");
            tooltip.SetToolTip(checkedListRoles, "Select roles for export filtering");
            tooltip.SetToolTip(btnGenerate, "Generate COBie excel workbook");
            tooltip.SetToolTip(btnBrowse, "Select file to extract COBie from");
            tooltip.SetToolTip(btnBrowseTemplate, "Select template excel file");
            tooltip.SetToolTip(cmboxFiletype, "Select excel file extension to generate");
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

            btnGenerate.Enabled = false;
            CreateWorker();
            _worker.DoWork += CobieLiteUKWorker;
            //get Excel File Type
            ExcelTypeEnum excelType = GetExcelType();
            //set parameters
            var args = new Params { ModelFile = txtPath.Text, TemplateFile = txtTemplate.Text, Roles = SetRoles(), ExcelType = excelType, FlipFilter = chkBoxFlipFilter.Checked, OpenExcel = chkBoxOpenFile.Checked };
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
                _worker.ReportProgress(0, String.Format("That file doesn't exist: {0}.", parameters.ModelFile));
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
            var facilities = GenerateFacility(parameters);
            //COBieBuilder builder = GenerateCOBieWorkBook(parameters);
            timer.Stop();
            _worker.ReportProgress(0, String.Format("Time to generate COBieLite data = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
            timer.Reset();
            timer.Start();
            int index = 1;
            foreach (var facilityType in facilities)
            {
                string fileName = Path.GetFileNameWithoutExtension(parameters.ModelFile) + ((facilities.Count == 1) ? "" : index.ToString());
                string path = Path.GetDirectoryName(parameters.ModelFile);
                string outputFile = Path.Combine(path, Path.ChangeExtension(fileName, parameters.ExcelType == ExcelTypeEnum.XLS ? ".xls" : ".xlsx"));
                string logFile = Path.ChangeExtension(outputFile, ".log");
                _worker.ReportProgress(0, string.Format("Creating file: {0}", logFile));
                using (var log = new LogOutput(txtOutput, ref _worker))//File.CreateText(logFile)
                { 
                    facilityType.ValidateUK2012(log, true);
                    //_worker.ReportProgress(0, string.Format("Validate file log: {0}", log.ToString())); 
                }
                _worker.ReportProgress(0, string.Format("Creating file: {0}", outputFile));
                string msg;
                //set attribute name filters
                OutPutFilters assetfilters = new OutPutFilters();
                assetfilters.ApplyRoleFilters(parameters.Roles);
                assetfilters.FlipResult = parameters.FlipFilter;
                using (var file = File.Create(outputFile))
                {
                    facilityType.WriteCobie(file, parameters.ExcelType, out msg, assetfilters, parameters.TemplateFile, true);
                }
                _worker.ReportProgress(0, msg);
                if (parameters.OpenExcel)
                {
                    Process.Start(outputFile); 
                }
                index++;
            }
            timer.Stop();
            _worker.ReportProgress(0, String.Format("Time to generate COBieLite Excel = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));

            _worker.ReportProgress(0, "Finished COBie Generation");
        }

        private List<Facility> GenerateFacility(Params parameters)
        {
            var facilities = new List<Facility>();
            using (var model = new XbimModel())
            {

                var xbimFile = Path.ChangeExtension(parameters.ModelFile, "xbim");
                model.CreateFrom(parameters.ModelFile, xbimFile, null, true, true);
                
                var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(model, facilities);
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

        

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All XBim Files|*.ifc;*.ifcxml;*.ifczip;*.xbim;*.xbimf|IFC Files|*.ifc;*.ifcxml;*.ifczip|Xbim Files|*.xbim|Xbim Federated Files|*.xbimf|XLS Files|*.xls";
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

    private class Params
    {
        public string ModelFile { get; set; }
        public string TemplateFile { get; set; }
        public ExcelTypeEnum ExcelType { get; set; }
        public bool FlipFilter { get; set; }
        public bool OpenExcel { get; set; }
        public RoleFilter Roles { get; set; }
    }

    private class LogOutput : TextWriter
    {
        RichTextBox _txtBox = null;
        BackgroundWorker _worker = null;
        public LogOutput(RichTextBox txtBox, ref BackgroundWorker worker)
        {
            _txtBox = txtBox;
            _worker = worker;
        }
        public override void Write(char value)
        {
            base.Write(value);
            _worker.ReportProgress(0, value.ToString()); // When character data is written, append it to the text box.
        }
        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }

    

    }

    

}
