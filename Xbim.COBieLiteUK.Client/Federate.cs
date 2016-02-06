using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xbim.COBieLiteUK.Client;
using Xbim.FilterHelper;
using Xbim.Ifc;
using Xbim.IO;

namespace Xbim.Client
{
    public partial class Federate : Form
    {
        #region Properties

        /// <summary>
        /// list of files
        /// </summary>
        public BindingList<FileInfo> RefModels
        { get; set; }

        /// <summary>
        /// List of file roles
        /// </summary>
        public Dictionary<FileInfo, RoleFilter> FileRoles
        { get; set; }

        /// <summary>
        /// Federated file name
        /// </summary>
        public string FileName
        { get; set; }

        /// <summary>
        /// Worker to convert file to xbim
        /// </summary>
        private BackgroundWorker _worker = null;

        /// <summary>
        /// Track files to be processed
        /// </summary>
        private Stack<string> _filesToProcess = new Stack<string>();

        #endregion
        
        /// <summary>
        /// Constructor
        /// </summary>
        public Federate(string file)
        {
            InitializeComponent();

            rolesList.Enabled = false;

            FileName = string.Empty;
            RefModels = new BindingList<FileInfo>();
            FileRoles = new Dictionary<FileInfo, RoleFilter>();
            SetUp(file);

            listBox.DataSource = RefModels;
            listBox.DisplayMember = "Name";
            listBox.ValueMember = "FullName";
        }

        private void SetUp(string filename)
        {
            if (!string.IsNullOrEmpty(filename) &&
                (Path.GetExtension(filename).ToUpper() == ".XBIMF")
                )
            {
                try
                {
                    FileInfo file = new FileInfo(filename);
                    if (file.Exists)
                    {
                        using (FederatedModel fedModel = new FederatedModel(file))
                        {
                            if (fedModel.Model.IsFederation)
                            {
                                txtAuthor.Text = fedModel.Author;
                                txtOrg.Text = fedModel.Organisation;
                                txtPrj.Text = fedModel.ProjectName;
                                FileRoles = fedModel.RefModelRoles.ToDictionary(m => new FileInfo(m.Key.Name), m => m.Value);
                                RefModels = new BindingList<FileInfo>(FileRoles.Keys.ToList());
                                Text += " : " + file.Name;
                                FileName = filename;
                                rolesList.Enabled = true;
                            }
                        }
                    }
                }
                catch (ArgumentException Ex) //bad paths etc..
                {
                    toolStripLabel.Text = Ex.Message;
                }
            }
            
        }

        /// <summary>
        /// Save Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (RefModels.Count > 0)
            {
                string author = txtAuthor.Text;
                string org = txtOrg.Text;
                string prj = txtPrj.Text;

                if (string.IsNullOrEmpty(FileName))
                {
                    SaveFileDialog dlg = new SaveFileDialog();

                    dlg.Filter = "Xbim Federated Files|*.xbimf";
                    dlg.Title = "Save Federated file";

                    dlg.CheckFileExists = false;
                    dlg.CheckPathExists = true;

                    // Show open file dialog box 
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        labelPath.Text = dlg.FileName;
                        FileName = dlg.FileName; //get it to create or save this file below
                    }
                }
                //create or overwrite file
                if (!string.IsNullOrEmpty(FileName))
                {
                    CreateFedFile(author, org, prj);
                    
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            else
            {
                toolStripLabel.Text = "Please add models to save as federated";
            }
        }

        /// <summary>
        /// Create Federation file
        /// </summary>
        /// <param name="file">FileInfo</param>
        /// <param name="author">Authors name</param>
        /// <param name="organisation">Organisation name</param>
        /// <param name="prjName">Project name</param>
        private void CreateFedFile(string author, string organisation, string prjName)
        {
            using (var fedModel = new FederatedModel(author, organisation, prjName))
            {
                foreach (var item in FileRoles)
                {
                    fedModel.AddRefModel(item.Key, organisation, item.Value);
                }
                fedModel.Model.SaveAs(FileName);
            }
            
        }

        /// <summary>
        /// Add Model event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_worker == null || !_worker.IsBusy)
                toolStripLabel.Text = string.Empty;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All XBim Files|*.ifc;*.ifcxml;*.ifczip;*.xbim|IFC Files|*.ifc;*.ifcxml;*.ifczip|Xbim Files|*.xbim"; //|XLS Files|*.xls
            dlg.Title = "Choose a refrence model file to add to federation";

            dlg.CheckFileExists = true;

            // Show open file dialog box 
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var xbimFile = Path.ChangeExtension(dlg.FileName, "xbim");
                FileInfo file = new FileInfo(xbimFile);

                if (!RefModels.Contains(file, new FileInfoComparer()))
                {
                    if (Path.GetExtension(dlg.FileName).ToUpper().Contains(".IFC"))
                    {

                        _filesToProcess.Push(dlg.FileName);
                        if (_worker == null)
                        {
                            _worker = CreateWorker();
                            _worker.DoWork += CreateXBimFile;
                        }
                        if (!_worker.IsBusy)
                        {
                            btnSave.Enabled = false;
                            _worker.RunWorkerAsync(_filesToProcess.Pop());
                        }
                    }


                    rolesList.Enabled = true;

                    RefModels.Add(file);
                    var roles = RoleFilter.Unknown;
                    FileRoles.Add(file, roles);

                    if (_worker == null || !_worker.IsBusy)
                    {
                        labelPath.Text = "Path:" + file.FullName;
                    }
                    //highlight added name
                    listBox.SelectedIndex = RefModels.Count - 1;
                    rolesList.Roles = FileRoles[file];
                }
                else
                {
                    toolStripLabel.Text = "File already added";
                }
            }
        }

        /// <summary>
        /// Remove model event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (_worker == null || !_worker.IsBusy)
                toolStripLabel.Text = string.Empty;

            if (listBox.SelectedIndex >= 0)
            {
                var file = RefModels[listBox.SelectedIndex];
                RefModels.Remove(file);
                FileRoles.Remove(file);
                if (listBox.SelectedIndex >= 0)
                {
                    file = RefModels[listBox.SelectedIndex];
                    toolStripLabel.Text = file.FullName;
                    rolesList.Roles = FileRoles[file];
                }
            }
            else
            {
                toolStripLabel.Text = string.Empty;
            }
        }

        /// <summary>
        /// Chenge selection event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_worker == null || !_worker.IsBusy)
                toolStripLabel.Text = string.Empty;

            if (listBox.SelectedIndex >= 0)
            {
                var file = RefModels[listBox.SelectedIndex];
                if (_worker == null || !_worker.IsBusy)
                {
                    labelPath.Text = "Path: " + file.FullName;
                }

                rolesList.Roles = FileRoles[file];
                rolesList.Enabled = true;
            }
            else
            {
                if (_worker == null || !_worker.IsBusy)
                {
                    labelPath.Text = string.Empty;
                }
                rolesList.Roles = RoleFilter.Unknown;
                rolesList.Enabled = false;
            }
        }

        /// <summary>
        /// Create Background Worker
        /// </summary>
        /// <returns></returns>
        private BackgroundWorker CreateWorker()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = false;

            worker.ProgressChanged += (object s, ProgressChangedEventArgs args) =>
            {
                if (!ProgressBar.Visible)
                {
                    ProgressBar.Visible = true;
                }
                if (args.ProgressPercentage == 0)
                {
                    toolStripLabel.Text = args.UserState.ToString();
                }
                else
                {
                    ProgressBar.Value = args.ProgressPercentage;
                }

            };

            worker.RunWorkerCompleted += (object s, RunWorkerCompletedEventArgs args) =>
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
                    MessageBox.Show(sb.ToString());

                }
                else
                {
                    string errMsg = args.Result as string;
                    if (!string.IsNullOrEmpty(errMsg))
                        toolStripLabel.Text = errMsg;
                }
                //clear status
                toolStripLabel.Text = "";
                ProgressBar.Value = 0;
                ProgressBar.Visible = false;
                //lets go again
                if (_filesToProcess.Count > 0)
                {
                    _worker.RunWorkerAsync(_filesToProcess.Pop());
                }
                else
                {
                    btnSave.Enabled = true;
                }
            };
            return worker;
        }

        /// <summary>
        /// Create xbim file on selection of ifc files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateXBimFile(object sender, DoWorkEventArgs e)
        {
            string filename = e.Argument as string;
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var xbimFile = Path.ChangeExtension(filename, "xbim");
            _worker.ReportProgress(0, string.Format("Creating {0}", Path.GetFileName(xbimFile)));
             var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            using (var model = IfcStore.Open(filename))
            {
                model.SaveAs(xbimFile,IfcStorageType.Xbim );
                model.Close();
            }
        }

        /// <summary>
        /// attach roles to the currently selected file on leave roleslist event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rolesList_Leave(object sender, EventArgs e)
        {
            toolStripLabel.Text = string.Empty;
            if (listBox.SelectedIndex >= 0)
            {
                var file = RefModels[listBox.SelectedIndex];
                FileRoles[file] = rolesList.Roles; 

            }
        }
    }

    /// <summary>
    /// FileInfo compare class
    /// </summary>
    public class FileInfoComparer : IEqualityComparer<FileInfo>
    {
        public bool Equals(FileInfo x, FileInfo y)
        {
            return x.FullName == y.FullName;
        }

        public int GetHashCode(FileInfo obj)
        {
            return obj.FullName.GetHashCode();
        }
    }
}
