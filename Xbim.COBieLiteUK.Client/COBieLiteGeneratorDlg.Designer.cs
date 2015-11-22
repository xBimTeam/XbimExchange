
namespace Xbim.Client
{
    partial class COBieLiteGeneratorDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnFederate = new System.Windows.Forms.Button();
            this.btnBrowseTemplate = new System.Windows.Forms.Button();
            this.txtTemplate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmboxFiletype = new System.Windows.Forms.ComboBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.StatusMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.btnClassFilter = new System.Windows.Forms.Button();
            this.chkBoxFlipFilter = new System.Windows.Forms.CheckBox();
            this.chkBoxOpenFile = new System.Windows.Forms.CheckBox();
            this.btnMergeFilter = new System.Windows.Forms.Button();
            this.chkBoxNoFilter = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPropMaps = new System.Windows.Forms.Button();
            this.chkBoxIds = new System.Windows.Forms.CheckBox();
            this.checkedListSys = new System.Windows.Forms.CheckedListBox();
            this.chkBoxLog = new System.Windows.Forms.CheckBox();
            this.rolesList = new Xbim.Client.RolesList();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtPath);
            this.groupBox1.Controls.Add(this.btnFederate);
            this.groupBox1.Controls.Add(this.btnBrowseTemplate);
            this.groupBox1.Controls.Add(this.txtTemplate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(211, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(615, 102);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Location";
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(99, 23);
            this.txtPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(328, 22);
            this.txtPath.TabIndex = 6;
            // 
            // btnFederate
            // 
            this.btnFederate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFederate.Location = new System.Drawing.Point(516, 25);
            this.btnFederate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFederate.Name = "btnFederate";
            this.btnFederate.Size = new System.Drawing.Size(83, 28);
            this.btnFederate.TabIndex = 5;
            this.btnFederate.Text = "Federate";
            this.btnFederate.UseVisualStyleBackColor = true;
            this.btnFederate.Click += new System.EventHandler(this.btnFederate_Click);
            // 
            // btnBrowseTemplate
            // 
            this.btnBrowseTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseTemplate.Enabled = false;
            this.btnBrowseTemplate.Location = new System.Drawing.Point(433, 54);
            this.btnBrowseTemplate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseTemplate.Name = "btnBrowseTemplate";
            this.btnBrowseTemplate.Size = new System.Drawing.Size(165, 28);
            this.btnBrowseTemplate.TabIndex = 4;
            this.btnBrowseTemplate.Text = "&Browse...";
            this.btnBrowseTemplate.UseVisualStyleBackColor = true;
            this.btnBrowseTemplate.Click += new System.EventHandler(this.btnBrowseTemplate_Click);
            // 
            // txtTemplate
            // 
            this.txtTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtTemplate.FormattingEnabled = true;
            this.txtTemplate.Location = new System.Drawing.Point(99, 57);
            this.txtTemplate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.Size = new System.Drawing.Size(325, 24);
            this.txtTemplate.TabIndex = 3;
            this.txtTemplate.SelectedIndexChanged += new System.EventHandler(this.txtTemplate_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Template:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(433, 25);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(84, 28);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "&Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select file:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(687, 122);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 17);
            this.label3.TabIndex = 30;
            this.label3.Text = "System Mode";
            // 
            // cmboxFiletype
            // 
            this.cmboxFiletype.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmboxFiletype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboxFiletype.FormattingEnabled = true;
            this.cmboxFiletype.Items.AddRange(new object[] {
            "XLS",
            "XLSX",
            "JSON",
            "XML",
            "IFC"});
            this.cmboxFiletype.Location = new System.Drawing.Point(703, 523);
            this.cmboxFiletype.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmboxFiletype.Name = "cmboxFiletype";
            this.cmboxFiletype.Size = new System.Drawing.Size(99, 24);
            this.cmboxFiletype.TabIndex = 18;
            this.cmboxFiletype.SelectedIndexChanged += new System.EventHandler(this.cmboxFiletype_SelectedIndexChanged);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(703, 452);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 28);
            this.btnClear.TabIndex = 17;
            this.btnClear.Text = "&Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(703, 487);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(100, 28);
            this.btnGenerate.TabIndex = 16;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgressBar,
            this.StatusMsg,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 588);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(836, 22);
            this.statusStrip1.TabIndex = 19;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ProgressBar
            // 
            this.ProgressBar.AutoSize = false;
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(200, 20);
            this.ProgressBar.Visible = false;
            // 
            // StatusMsg
            // 
            this.StatusMsg.Name = "StatusMsg";
            this.StatusMsg.Size = new System.Drawing.Size(816, 17);
            this.StatusMsg.Spring = true;
            this.StatusMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(20, 119);
            this.txtOutput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(664, 459);
            this.txtOutput.TabIndex = 20;
            this.txtOutput.Text = "";
            // 
            // btnClassFilter
            // 
            this.btnClassFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClassFilter.Location = new System.Drawing.Point(8, 76);
            this.btnClassFilter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClassFilter.Name = "btnClassFilter";
            this.btnClassFilter.Size = new System.Drawing.Size(100, 32);
            this.btnClassFilter.TabIndex = 21;
            this.btnClassFilter.Text = "Set Filters";
            this.btnClassFilter.UseVisualStyleBackColor = true;
            this.btnClassFilter.Click += new System.EventHandler(this.btnClassFilter_Click);
            // 
            // chkBoxFlipFilter
            // 
            this.chkBoxFlipFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxFlipFilter.AutoSize = true;
            this.chkBoxFlipFilter.Location = new System.Drawing.Point(10, 48);
            this.chkBoxFlipFilter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkBoxFlipFilter.Name = "chkBoxFlipFilter";
            this.chkBoxFlipFilter.Size = new System.Drawing.Size(87, 21);
            this.chkBoxFlipFilter.TabIndex = 22;
            this.chkBoxFlipFilter.Text = "Flip Filter";
            this.chkBoxFlipFilter.UseVisualStyleBackColor = true;
            this.chkBoxFlipFilter.CheckedChanged += new System.EventHandler(this.chkBoxFlipFilter_CheckedChanged);
            // 
            // chkBoxOpenFile
            // 
            this.chkBoxOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxOpenFile.AutoSize = true;
            this.chkBoxOpenFile.Checked = true;
            this.chkBoxOpenFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxOpenFile.Location = new System.Drawing.Point(709, 556);
            this.chkBoxOpenFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkBoxOpenFile.Name = "chkBoxOpenFile";
            this.chkBoxOpenFile.Size = new System.Drawing.Size(102, 21);
            this.chkBoxOpenFile.TabIndex = 23;
            this.chkBoxOpenFile.Text = "Open Excel";
            this.chkBoxOpenFile.UseVisualStyleBackColor = true;
            // 
            // btnMergeFilter
            // 
            this.btnMergeFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMergeFilter.Location = new System.Drawing.Point(9, 113);
            this.btnMergeFilter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMergeFilter.Name = "btnMergeFilter";
            this.btnMergeFilter.Size = new System.Drawing.Size(100, 28);
            this.btnMergeFilter.TabIndex = 24;
            this.btnMergeFilter.Text = "Applied Filter";
            this.btnMergeFilter.UseVisualStyleBackColor = true;
            this.btnMergeFilter.Click += new System.EventHandler(this.btnMergeFilter_Click);
            // 
            // chkBoxNoFilter
            // 
            this.chkBoxNoFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxNoFilter.AutoSize = true;
            this.chkBoxNoFilter.Location = new System.Drawing.Point(11, 22);
            this.chkBoxNoFilter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkBoxNoFilter.Name = "chkBoxNoFilter";
            this.chkBoxNoFilter.Size = new System.Drawing.Size(90, 21);
            this.chkBoxNoFilter.TabIndex = 25;
            this.chkBoxNoFilter.Text = "No Filters";
            this.chkBoxNoFilter.UseVisualStyleBackColor = true;
            this.chkBoxNoFilter.CheckedChanged += new System.EventHandler(this.chkBoxNoFilter_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chkBoxNoFilter);
            this.groupBox2.Controls.Add(this.btnClassFilter);
            this.groupBox2.Controls.Add(this.btnMergeFilter);
            this.groupBox2.Controls.Add(this.chkBoxFlipFilter);
            this.groupBox2.Location = new System.Drawing.Point(691, 209);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(117, 151);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filters";
            // 
            // btnPropMaps
            // 
            this.btnPropMaps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPropMaps.Location = new System.Drawing.Point(699, 367);
            this.btnPropMaps.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPropMaps.Name = "btnPropMaps";
            this.btnPropMaps.Size = new System.Drawing.Size(100, 28);
            this.btnPropMaps.TabIndex = 27;
            this.btnPropMaps.Text = "Mappings";
            this.btnPropMaps.UseVisualStyleBackColor = true;
            this.btnPropMaps.Click += new System.EventHandler(this.btnPropMaps_Click);
            // 
            // chkBoxIds
            // 
            this.chkBoxIds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxIds.AutoSize = true;
            this.chkBoxIds.Location = new System.Drawing.Point(703, 399);
            this.chkBoxIds.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkBoxIds.Name = "chkBoxIds";
            this.chkBoxIds.Size = new System.Drawing.Size(125, 21);
            this.chkBoxIds.TabIndex = 28;
            this.chkBoxIds.Text = "ExId as EntityId";
            this.chkBoxIds.UseVisualStyleBackColor = true;
            // 
            // checkedListSys
            // 
            this.checkedListSys.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListSys.CheckOnClick = true;
            this.checkedListSys.FormattingEnabled = true;
            this.checkedListSys.Location = new System.Drawing.Point(691, 142);
            this.checkedListSys.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkedListSys.Name = "checkedListSys";
            this.checkedListSys.Size = new System.Drawing.Size(119, 55);
            this.checkedListSys.TabIndex = 29;
            // 
            // chkBoxLog
            // 
            this.chkBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxLog.AutoSize = true;
            this.chkBoxLog.Location = new System.Drawing.Point(703, 420);
            this.chkBoxLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkBoxLog.Name = "chkBoxLog";
            this.chkBoxLog.Size = new System.Drawing.Size(108, 21);
            this.chkBoxLog.TabIndex = 32;
            this.chkBoxLog.Text = "Log (debug)";
            this.chkBoxLog.UseVisualStyleBackColor = true;
            // 
            // rolesList
            // 
            this.rolesList.Location = new System.Drawing.Point(20, 10);
            this.rolesList.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.rolesList.Name = "rolesList";
            this.rolesList.Roles = Xbim.FilterHelper.RoleFilter.Unknown;
            this.rolesList.Size = new System.Drawing.Size(183, 102);
            this.rolesList.TabIndex = 31;
            // 
            // COBieLiteGeneratorDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 610);
            this.Controls.Add(this.chkBoxLog);
            this.Controls.Add(this.rolesList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkedListSys);
            this.Controls.Add(this.chkBoxIds);
            this.Controls.Add(this.btnPropMaps);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chkBoxOpenFile);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cmboxFiletype);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "COBieLiteGeneratorDlg";
            this.Text = "XBim COBieLiteUK Test Harness";
            this.Load += new System.EventHandler(this.COBieLiteGenerator_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBrowseTemplate;
        private System.Windows.Forms.ComboBox txtTemplate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmboxFiletype;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel StatusMsg;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.Button btnClassFilter;
        private System.Windows.Forms.CheckBox chkBoxFlipFilter;
        private System.Windows.Forms.CheckBox chkBoxOpenFile;
        private System.Windows.Forms.Button btnMergeFilter;
        private System.Windows.Forms.CheckBox chkBoxNoFilter;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnPropMaps;
        private System.Windows.Forms.CheckBox chkBoxIds;
        private System.Windows.Forms.CheckedListBox checkedListSys;
        private System.Windows.Forms.Label label3;
        private RolesList rolesList;
        private System.Windows.Forms.Button btnFederate;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.CheckBox chkBoxLog;
    }
}

