namespace Xbim.COBieLiteUK.Client
{
    partial class Federate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private global::System.ComponentModel.IContainer components = null;

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
            this.listBox = new global::System.Windows.Forms.ListBox();
            this.btnAdd = new global::System.Windows.Forms.Button();
            this.btnRemove = new global::System.Windows.Forms.Button();
            this.btnSave = new global::System.Windows.Forms.Button();
            this.btnCancel = new global::System.Windows.Forms.Button();
            this.txtAuthor = new global::System.Windows.Forms.TextBox();
            this.txtOrg = new global::System.Windows.Forms.TextBox();
            this.label1 = new global::System.Windows.Forms.Label();
            this.label2 = new global::System.Windows.Forms.Label();
            this.statusStrip = new global::System.Windows.Forms.StatusStrip();
            this.ProgressBar = new global::System.Windows.Forms.ToolStripProgressBar();
            this.toolStripLabel = new global::System.Windows.Forms.ToolStripStatusLabel();
            this.labelPath = new global::System.Windows.Forms.Label();
            this.txtPrj = new global::System.Windows.Forms.TextBox();
            this.label3 = new global::System.Windows.Forms.Label();
            this.rolesList = new RolesList();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.Anchor = ((global::System.Windows.Forms.AnchorStyles)((((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom) 
            | global::System.Windows.Forms.AnchorStyles.Left) 
            | global::System.Windows.Forms.AnchorStyles.Right)));
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new global::System.Drawing.Point(12, 85);
            this.listBox.Name = "listBox";
            this.listBox.Size = new global::System.Drawing.Size(524, 251);
            this.listBox.TabIndex = 1;
            this.listBox.SelectedIndexChanged += new global::System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((global::System.Windows.Forms.AnchorStyles)((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new global::System.Drawing.Point(543, 89);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new global::System.Drawing.Size(91, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new global::System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((global::System.Windows.Forms.AnchorStyles)((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new global::System.Drawing.Point(543, 118);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new global::System.Drawing.Size(91, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new global::System.EventHandler(this.btnRemove_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((global::System.Windows.Forms.AnchorStyles)((global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new global::System.Drawing.Point(543, 284);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new global::System.Drawing.Size(91, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new global::System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((global::System.Windows.Forms.AnchorStyles)((global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new global::System.Drawing.Point(543, 313);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new global::System.Drawing.Size(91, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtAuthor
            // 
            this.txtAuthor.Anchor = ((global::System.Windows.Forms.AnchorStyles)(((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left) 
            | global::System.Windows.Forms.AnchorStyles.Right)));
            this.txtAuthor.Location = new global::System.Drawing.Point(116, 6);
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.Size = new global::System.Drawing.Size(420, 20);
            this.txtAuthor.TabIndex = 7;
            // 
            // txtOrg
            // 
            this.txtOrg.Anchor = ((global::System.Windows.Forms.AnchorStyles)(((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left) 
            | global::System.Windows.Forms.AnchorStyles.Right)));
            this.txtOrg.Location = new global::System.Drawing.Point(116, 32);
            this.txtOrg.Name = "txtOrg";
            this.txtOrg.Size = new global::System.Drawing.Size(420, 20);
            this.txtOrg.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new global::System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new global::System.Drawing.Size(69, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Author Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new global::System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new global::System.Drawing.Size(97, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Organisation Name";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] {
            this.ProgressBar,
            this.toolStripLabel});
            this.statusStrip.Location = new global::System.Drawing.Point(0, 365);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new global::System.Drawing.Size(646, 22);
            this.statusStrip.TabIndex = 11;
            this.statusStrip.Text = "statusStrip1";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new global::System.Drawing.Size(100, 16);
            this.ProgressBar.Visible = false;
            // 
            // toolStripLabel
            // 
            this.toolStripLabel.Name = "toolStripLabel";
            this.toolStripLabel.Overflow = global::System.Windows.Forms.ToolStripItemOverflow.Always;
            this.toolStripLabel.Size = new global::System.Drawing.Size(529, 17);
            this.toolStripLabel.Spring = true;
            this.toolStripLabel.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPath
            // 
            this.labelPath.Anchor = ((global::System.Windows.Forms.AnchorStyles)(((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left) 
            | global::System.Windows.Forms.AnchorStyles.Right)));
            this.labelPath.AutoEllipsis = true;
            this.labelPath.Location = new global::System.Drawing.Point(12, 344);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new global::System.Drawing.Size(622, 13);
            this.labelPath.TabIndex = 12;
            // 
            // txtPrj
            // 
            this.txtPrj.Anchor = ((global::System.Windows.Forms.AnchorStyles)(((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left) 
            | global::System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrj.Location = new global::System.Drawing.Point(116, 58);
            this.txtPrj.Name = "txtPrj";
            this.txtPrj.Size = new global::System.Drawing.Size(421, 20);
            this.txtPrj.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new global::System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new global::System.Drawing.Size(71, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Project Name";
            // 
            // rolesList
            // 
            this.rolesList.Anchor = ((global::System.Windows.Forms.AnchorStyles)((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right)));
            this.rolesList.Location = new global::System.Drawing.Point(543, 6);
            this.rolesList.Name = "rolesList";
            this.rolesList.Roles = Xbim.FilterHelper.RoleFilter.Unknown;
            this.rolesList.Size = new global::System.Drawing.Size(91, 88);
            this.rolesList.TabIndex = 0;
            this.rolesList.Leave += new global::System.EventHandler(this.rolesList_Leave);
            // 
            // Federate
            // 
            this.AutoScaleDimensions = new global::System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new global::System.Drawing.Size(646, 387);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPrj);
            this.Controls.Add(this.labelPath);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOrg);
            this.Controls.Add(this.txtAuthor);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.rolesList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Federate";
            this.Text = "Federate";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RolesList rolesList;
        private global::System.Windows.Forms.ListBox listBox;
        private global::System.Windows.Forms.Button btnAdd;
        private global::System.Windows.Forms.Button btnRemove;
        private global::System.Windows.Forms.Button btnSave;
        private global::System.Windows.Forms.Button btnCancel;
        private global::System.Windows.Forms.TextBox txtAuthor;
        private global::System.Windows.Forms.TextBox txtOrg;
        private global::System.Windows.Forms.Label label1;
        private global::System.Windows.Forms.Label label2;
        private global::System.Windows.Forms.StatusStrip statusStrip;
        private global::System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private global::System.Windows.Forms.ToolStripStatusLabel toolStripLabel;
        private global::System.Windows.Forms.Label labelPath;
        private global::System.Windows.Forms.TextBox txtPrj;
        private global::System.Windows.Forms.Label label3;
    }
}