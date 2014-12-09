using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Xbim.COBie.Client
{
    public partial class MergeSelect : Form
    {
        private List<string> mergeItemsIn { get; set; }
        public List<string> mergeItemsOut { get; set; }
        public string FileFilter { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="genMergeItems">List of merge file strings</param>
        public MergeSelect(List<string> genMergeItems)
        {
            InitializeComponent();
            FileFilter = "Files|*.xls;*.ifc;*.xBIM|XLS Files|*.xls|IFC Files|*.ifc|Xbim Files|*.xBIM";
            //Set up merge lists
            mergeItemsOut = new List<string>();
            mergeItemsIn = new List<string>();
            //copy passed list  
            mergeItemsIn.AddRange(genMergeItems);

            SetInitialValues();
        }

        /// <summary>
        /// Set up controls to current data
        /// </summary>
        private void SetInitialValues()
        {
            if (mergeItemsIn.Count > 0)
            {
                mainFileTxtBox.Text = mergeItemsIn.First();
                mergeItemsIn.RemoveAt(0);
                if (mergeItemsIn.Count > 0)
                    mergeListBox.Items.AddRange(mergeItemsIn.ToArray());
            }
            
        }

        /// <summary>
        /// Browse button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectBtn_Click(object sender, EventArgs e)
        {
            stripLbl.Text = "";

            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = FileFilter;
            openDlg.Title = "Choose an excel file (Main File)";
            openDlg.CheckFileExists = true;
            // Show open file dialog box 
            openDlg.FileOk += new CancelEventHandler(dlg_FileOk);
            openDlg.ShowDialog();
        }

        /// <summary>
        /// File Open Dialog OK click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ce"></param>
        private void dlg_FileOk(object sender, CancelEventArgs ce)
        {
            OpenFileDialog openDlg = sender as OpenFileDialog;
            if (openDlg != null)
                mainFileTxtBox.Text = openDlg.FileName;
            
        }

        /// <summary>
        /// This forms OK button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okBtn_Click(object sender, EventArgs e)
        {
            stripLbl.Text = "";

            if (string.IsNullOrEmpty(mainFileTxtBox.Text)) 
            {
                stripLbl.Text = "Please select a main file";
                selectBtn.Focus();
                return;
            }
            if (mergeListBox.Items.Count == 0)
            {
                stripLbl.Text = "Please select files to merge";
                addBtn.Focus();
                return;
            }
            mergeItemsOut.Add(mainFileTxtBox.Text);
            mergeItemsOut.AddRange(mergeListBox.Items.Cast<string>().ToList());
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// Add button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBtn_Click(object sender, EventArgs e)
        {
            stripLbl.Text = "";

            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Multiselect = true;
            openDlg.Filter = FileFilter;
            openDlg.Title = "Choose excel files to merge into (Main File)";
            openDlg.CheckFileExists = true;
            // Show open file dialog box 
            openDlg.FileOk += new CancelEventHandler(dlg_MergeFileOk);
            openDlg.ShowDialog();
        }

        /// <summary>
        /// Open dialog OK button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ce"></param>
        private void dlg_MergeFileOk(object sender, CancelEventArgs ce)
        {
            List<string> errorFiles = new List<string>();
            OpenFileDialog openDlg = sender as OpenFileDialog;
            if ((openDlg != null) &&
                (openDlg.FileNames.Count() > 0)
                )
            {
               //mergeListBox.Items.AddRange(openDlg.FileNames);
                foreach (var item in openDlg.FileNames)
                {
                    if (!mergeListBox.Items.Contains(item)) //not in list
                    {
                        if (item != mainFileTxtBox.Text) //not main file
                            mergeListBox.Items.Add(item);
                        else
                            errorFiles.Add(Path.GetFileName(mainFileTxtBox.Text) + "(Main File)"); 
                    }
                    else
                        errorFiles.Add(Path.GetFileName(item));
                }

                if (errorFiles.Count > 0)
                    stripLbl.Text = string.Join(", ", errorFiles) + " already in list";
            }
        }

        /// <summary>
        /// Remove button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeBtn_Click(object sender, EventArgs e)
        {
            stripLbl.Text = "";

            List<string> items = mergeListBox.SelectedItems.Cast<string>().ToList();
            foreach (string item in items)
	        {
                mergeListBox.Items.Remove(item);
	        }    
        }

    }
}
