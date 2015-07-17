
namespace Xbim.Client
{
    partial class COBieLiteGenerator
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
            this.checkedListRoles = new System.Windows.Forms.CheckedListBox();
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
            this.btmPropMaps = new System.Windows.Forms.Button();
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
            this.groupBox1.Location = new System.Drawing.Point(158, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(361, 83);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Location";
            // 
            // btnBrowseTemplate
            // 
            this.btnBrowseTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseTemplate.Enabled = false;
            this.btnBrowseTemplate.Location = new System.Drawing.Point(280, 44);
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
            this.txtTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtTemplate.FormattingEnabled = true;
            this.txtTemplate.Location = new System.Drawing.Point(74, 46);
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.Size = new System.Drawing.Size(189, 21);
            this.txtTemplate.TabIndex = 3;
            this.txtTemplate.SelectedIndexChanged += new System.EventHandler(this.txtTemplate_SelectedIndexChanged);
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
            this.txtPath.Location = new System.Drawing.Point(74, 20);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(189, 21);
            this.txtPath.TabIndex = 0;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(280, 18);
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
            // checkedListRoles
            // 
            this.checkedListRoles.CheckOnClick = true;
            this.checkedListRoles.FormattingEnabled = true;
            this.checkedListRoles.Location = new System.Drawing.Point(12, 12);
            this.checkedListRoles.Name = "checkedListRoles";
            this.checkedListRoles.Size = new System.Drawing.Size(140, 79);
            this.checkedListRoles.TabIndex = 13;
            // 
            // cmboxFiletype
            // 
            this.cmboxFiletype.FormattingEnabled = true;
            this.cmboxFiletype.Items.AddRange(new object[] {
            "XLS",
            "XLSX"});
            this.cmboxFiletype.Location = new System.Drawing.Point(438, 307);
            this.cmboxFiletype.Name = "cmboxFiletype";
            this.cmboxFiletype.Size = new System.Drawing.Size(75, 21);
            this.cmboxFiletype.TabIndex = 18;
            this.cmboxFiletype.Text = "XLS";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(438, 249);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 17;
            this.btnClear.Text = "&Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(438, 278);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 16;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgressBar,
            this.StatusMsg,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 354);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(527, 22);
            this.statusStrip1.TabIndex = 19;
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
            this.StatusMsg.Size = new System.Drawing.Size(360, 17);
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
            this.txtOutput.Location = new System.Drawing.Point(15, 97);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(406, 254);
            this.txtOutput.TabIndex = 20;
            this.txtOutput.Text = "";
            // 
            // btnClassFilter
            // 
            this.btnClassFilter.Location = new System.Drawing.Point(6, 63);
            this.btnClassFilter.Name = "btnClassFilter";
            this.btnClassFilter.Size = new System.Drawing.Size(75, 26);
            this.btnClassFilter.TabIndex = 21;
            this.btnClassFilter.Text = "Set Filters";
            this.btnClassFilter.UseVisualStyleBackColor = true;
            this.btnClassFilter.Click += new System.EventHandler(this.btnClassFilter_Click);
            // 
            // chkBoxFlipFilter
            // 
            this.chkBoxFlipFilter.AutoSize = true;
            this.chkBoxFlipFilter.Location = new System.Drawing.Point(6, 40);
            this.chkBoxFlipFilter.Name = "chkBoxFlipFilter";
            this.chkBoxFlipFilter.Size = new System.Drawing.Size(67, 17);
            this.chkBoxFlipFilter.TabIndex = 22;
            this.chkBoxFlipFilter.Text = "Flip Filter";
            this.chkBoxFlipFilter.UseVisualStyleBackColor = true;
            this.chkBoxFlipFilter.CheckedChanged += new System.EventHandler(this.chkBoxFlipFilter_CheckedChanged);
            // 
            // chkBoxOpenFile
            // 
            this.chkBoxOpenFile.AutoSize = true;
            this.chkBoxOpenFile.Location = new System.Drawing.Point(438, 334);
            this.chkBoxOpenFile.Name = "chkBoxOpenFile";
            this.chkBoxOpenFile.Size = new System.Drawing.Size(81, 17);
            this.chkBoxOpenFile.TabIndex = 23;
            this.chkBoxOpenFile.Text = "Open Excel";
            this.chkBoxOpenFile.UseVisualStyleBackColor = true;
            // 
            // btnMergeFilter
            // 
            this.btnMergeFilter.Location = new System.Drawing.Point(6, 95);
            this.btnMergeFilter.Name = "btnMergeFilter";
            this.btnMergeFilter.Size = new System.Drawing.Size(75, 23);
            this.btnMergeFilter.TabIndex = 24;
            this.btnMergeFilter.Text = "Applied Filter";
            this.btnMergeFilter.UseVisualStyleBackColor = true;
            this.btnMergeFilter.Click += new System.EventHandler(this.btnMergeFilter_Click);
            // 
            // chkBoxNoFilter
            // 
            this.chkBoxNoFilter.AutoSize = true;
            this.chkBoxNoFilter.Location = new System.Drawing.Point(6, 19);
            this.chkBoxNoFilter.Name = "chkBoxNoFilter";
            this.chkBoxNoFilter.Size = new System.Drawing.Size(70, 17);
            this.chkBoxNoFilter.TabIndex = 25;
            this.chkBoxNoFilter.Text = "No Filters";
            this.chkBoxNoFilter.UseVisualStyleBackColor = true;
            this.chkBoxNoFilter.CheckedChanged += new System.EventHandler(this.chkBoxNoFilter_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkBoxNoFilter);
            this.groupBox2.Controls.Add(this.btnClassFilter);
            this.groupBox2.Controls.Add(this.btnMergeFilter);
            this.groupBox2.Controls.Add(this.chkBoxFlipFilter);
            this.groupBox2.Location = new System.Drawing.Point(431, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(88, 123);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filters";
            // 
            // btmPropMaps
            // 
            this.btmPropMaps.Location = new System.Drawing.Point(437, 220);
            this.btmPropMaps.Name = "btmPropMaps";
            this.btmPropMaps.Size = new System.Drawing.Size(75, 23);
            this.btmPropMaps.TabIndex = 27;
            this.btmPropMaps.Text = "Mappings";
            this.btmPropMaps.UseVisualStyleBackColor = true;
            this.btmPropMaps.Click += new System.EventHandler(this.btmPropMaps_Click);
            // 
            // COBieLiteGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 376);
            this.Controls.Add(this.btmPropMaps);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chkBoxOpenFile);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cmboxFiletype);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.checkedListRoles);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "COBieLiteGenerator";
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
        private System.Windows.Forms.ComboBox txtPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox checkedListRoles;
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
        private System.Windows.Forms.Button btmPropMaps;
    }
}

