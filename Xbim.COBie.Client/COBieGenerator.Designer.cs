namespace Xbim.COBie.Client
{
    partial class COBieGenerator
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
            this.btnBrowseTemplate = new System.Windows.Forms.Button();
            this.txtTemplate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.ComboBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.mergeBtn = new System.Windows.Forms.Button();
            this.GeoOnlyChkBox = new System.Windows.Forms.CheckBox();
            this.MergeChkBox = new System.Windows.Forms.CheckBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.StatusMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ValidateChkBox = new System.Windows.Forms.CheckBox();
            this.SkipGeoChkBox = new System.Windows.Forms.CheckBox();
            this.checkedListRoles = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClassFilter = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnBrowseTemplate);
            this.groupBox1.Controls.Add(this.txtTemplate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPath);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(139, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(372, 83);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IFC File Location";
            // 
            // btnBrowseTemplate
            // 
            this.btnBrowseTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseTemplate.Location = new System.Drawing.Point(291, 44);
            this.btnBrowseTemplate.Name = "btnBrowseTemplate";
            this.btnBrowseTemplate.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseTemplate.TabIndex = 4;
            this.btnBrowseTemplate.Text = "&Browse...";
            this.btnBrowseTemplate.UseVisualStyleBackColor = true;
            this.btnBrowseTemplate.Click += new System.EventHandler(this.btnBrowseTemplate_Click);
            // 
            // txtTemplate
            // 
            this.txtTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTemplate.FormattingEnabled = true;
            this.txtTemplate.Items.AddRange(new object[] {
            "COBie-UK-2012-template.xls",
            "COBie-US-2_4-template.xls"});
            this.txtTemplate.Location = new System.Drawing.Point(74, 46);
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.Size = new System.Drawing.Size(200, 21);
            this.txtTemplate.TabIndex = 3;
            this.txtTemplate.Text = "COBie-UK-2012-template.xls";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Template:";
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.FormattingEnabled = true;
            this.txtPath.Items.AddRange(new object[] {
            "2012-03-23-Duplex-Design.ifc",
            "2012-03-23-Duplex-Design.xbim",
            "Clinic_A_20110906.ifc",
            "Clinic_A_20110906.xbim",
            "2012-09-03-Clinic-Handover.ifc",
            "2012-09-03-Clinic-Handover.xbim",
            "BCU-XX-XX-A-VCUK-M3-00-0001.Xbim"});
            this.txtPath.Location = new System.Drawing.Point(74, 20);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(200, 21);
            this.txtPath.TabIndex = 0;
            this.txtPath.Text = "2012-03-23-Duplex-Design.ifc";
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(291, 18);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "&Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select file:";
            // 
            // mergeBtn
            // 
            this.mergeBtn.Enabled = false;
            this.mergeBtn.Location = new System.Drawing.Point(234, 15);
            this.mergeBtn.Name = "mergeBtn";
            this.mergeBtn.Size = new System.Drawing.Size(130, 23);
            this.mergeBtn.TabIndex = 7;
            this.mergeBtn.Text = "Select &Merge Files...";
            this.mergeBtn.UseVisualStyleBackColor = true;
            this.mergeBtn.Click += new System.EventHandler(this.mergeBtn_Click);
            // 
            // GeoOnlyChkBox
            // 
            this.GeoOnlyChkBox.AutoSize = true;
            this.GeoOnlyChkBox.Checked = true;
            this.GeoOnlyChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GeoOnlyChkBox.Enabled = false;
            this.GeoOnlyChkBox.Location = new System.Drawing.Point(6, 38);
            this.GeoOnlyChkBox.Name = "GeoOnlyChkBox";
            this.GeoOnlyChkBox.Size = new System.Drawing.Size(148, 17);
            this.GeoOnlyChkBox.TabIndex = 6;
            this.GeoOnlyChkBox.Text = "Merge geometry data only";
            this.GeoOnlyChkBox.UseVisualStyleBackColor = true;
            // 
            // MergeChkBox
            // 
            this.MergeChkBox.AutoSize = true;
            this.MergeChkBox.Location = new System.Drawing.Point(6, 19);
            this.MergeChkBox.Name = "MergeChkBox";
            this.MergeChkBox.Size = new System.Drawing.Size(210, 17);
            this.MergeChkBox.TabIndex = 5;
            this.MergeChkBox.Text = "Merge files COBie data into one IFC file";
            this.MergeChkBox.UseVisualStyleBackColor = true;
            this.MergeChkBox.CheckedChanged += new System.EventHandler(this.MergeChkBox_CheckedChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(430, 418);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(430, 388);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "&Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(13, 176);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(400, 265);
            this.txtOutput.TabIndex = 5;
            this.txtOutput.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgressBar,
            this.StatusMsg,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 450);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(523, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ProgressBar
            // 
            this.ProgressBar.AutoSize = false;
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(150, 16);
            // 
            // StatusMsg
            // 
            this.StatusMsg.Name = "StatusMsg";
            this.StatusMsg.Size = new System.Drawing.Size(356, 17);
            this.StatusMsg.Spring = true;
            this.StatusMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // ValidateChkBox
            // 
            this.ValidateChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ValidateChkBox.AutoSize = true;
            this.ValidateChkBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ValidateChkBox.Enabled = false;
            this.ValidateChkBox.Location = new System.Drawing.Point(417, 117);
            this.ValidateChkBox.Name = "ValidateChkBox";
            this.ValidateChkBox.Size = new System.Drawing.Size(95, 17);
            this.ValidateChkBox.TabIndex = 10;
            this.ValidateChkBox.Text = "Validate xls file";
            this.ValidateChkBox.UseVisualStyleBackColor = true;
            // 
            // SkipGeoChkBox
            // 
            this.SkipGeoChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SkipGeoChkBox.AutoSize = true;
            this.SkipGeoChkBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SkipGeoChkBox.Location = new System.Drawing.Point(419, 140);
            this.SkipGeoChkBox.Name = "SkipGeoChkBox";
            this.SkipGeoChkBox.Size = new System.Drawing.Size(93, 17);
            this.SkipGeoChkBox.TabIndex = 11;
            this.SkipGeoChkBox.Text = "Skip geometry";
            this.SkipGeoChkBox.UseVisualStyleBackColor = true;
            // 
            // checkedListRoles
            // 
            this.checkedListRoles.CheckOnClick = true;
            this.checkedListRoles.FormattingEnabled = true;
            this.checkedListRoles.Location = new System.Drawing.Point(13, 31);
            this.checkedListRoles.Name = "checkedListRoles";
            this.checkedListRoles.Size = new System.Drawing.Size(120, 64);
            this.checkedListRoles.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Roles";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.MergeChkBox);
            this.groupBox2.Controls.Add(this.GeoOnlyChkBox);
            this.groupBox2.Controls.Add(this.mergeBtn);
            this.groupBox2.Location = new System.Drawing.Point(13, 101);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(400, 69);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Merge to IFC";
            // 
            // btnClassFilter
            // 
            this.btnClassFilter.Location = new System.Drawing.Point(430, 176);
            this.btnClassFilter.Name = "btnClassFilter";
            this.btnClassFilter.Size = new System.Drawing.Size(75, 26);
            this.btnClassFilter.TabIndex = 14;
            this.btnClassFilter.Text = "Class filter";
            this.btnClassFilter.UseVisualStyleBackColor = true;
            this.btnClassFilter.Click += new System.EventHandler(this.btnClassFilter_Click);
            // 
            // COBieGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 472);
            this.Controls.Add(this.btnClassFilter);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkedListRoles);
            this.Controls.Add(this.SkipGeoChkBox);
            this.Controls.Add(this.ValidateChkBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.groupBox1);
            this.Name = "COBieGenerator";
            this.Text = "Xbim COBie Test Harness";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.COBieGenerator_FormClosed);
            this.Load += new System.EventHandler(this.COBieGenerator_Load);
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
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.ComboBox txtPath;
        private System.Windows.Forms.Button btnBrowseTemplate;
        private System.Windows.Forms.ComboBox txtTemplate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusMsg;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.CheckBox MergeChkBox;
        private System.Windows.Forms.CheckBox GeoOnlyChkBox;
        private System.Windows.Forms.Button mergeBtn;
        private System.Windows.Forms.CheckBox ValidateChkBox;
        private System.Windows.Forms.CheckBox SkipGeoChkBox;
        private System.Windows.Forms.CheckedListBox checkedListRoles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnClassFilter;
    }
}

