namespace Xbim.COBieLiteUK.Client
{
    partial class PropertyMapDlg
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
            this.tabControl = new global::System.Windows.Forms.TabControl();
            this.btnSave = new global::System.Windows.Forms.Button();
            this.btnCancel = new global::System.Windows.Forms.Button();
            this.btnDefaults = new global::System.Windows.Forms.Button();
            this.panel1 = new global::System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((global::System.Windows.Forms.AnchorStyles)((((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom) 
            | global::System.Windows.Forms.AnchorStyles.Left) 
            | global::System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Location = new global::System.Drawing.Point(3, 0);
            this.tabControl.Margin = new global::System.Windows.Forms.Padding(4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new global::System.Drawing.Size(545, 510);
            this.tabControl.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = global::System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new global::System.Drawing.Point(56, 9);
            this.btnSave.Margin = new global::System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new global::System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new global::System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new global::System.Drawing.Point(164, 9);
            this.btnCancel.Margin = new global::System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new global::System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnDefaults
            // 
            this.btnDefaults.Location = new global::System.Drawing.Point(272, 9);
            this.btnDefaults.Margin = new global::System.Windows.Forms.Padding(4);
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.Size = new global::System.Drawing.Size(100, 28);
            this.btnDefaults.TabIndex = 3;
            this.btnDefaults.Text = "Defaults";
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new global::System.EventHandler(this.btnDefaults_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnDefaults);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Location = new global::System.Drawing.Point(64, 517);
            this.panel1.Name = "panel1";
            this.panel1.Size = new global::System.Drawing.Size(424, 55);
            this.panel1.TabIndex = 4;
            // 
            // PropertyMapDlg
            // 
            this.AutoScaleDimensions = new global::System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new global::System.Drawing.Size(546, 566);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl);
            this.Margin = new global::System.Windows.Forms.Padding(4);
            this.Name = "PropertyMapDlg";
            this.Text = "Property Maps";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private global::System.Windows.Forms.TabControl tabControl;
        private global::System.Windows.Forms.Button btnSave;
        private global::System.Windows.Forms.Button btnCancel;
        private global::System.Windows.Forms.Button btnDefaults;
        private global::System.Windows.Forms.Panel panel1;
    }
}