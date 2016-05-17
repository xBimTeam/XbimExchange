namespace Xbim.Client
{
    partial class EditableList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.add = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.editlistView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // add
            // 
            this.add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.add.Location = new System.Drawing.Point(234, 0);
            this.add.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(31, 28);
            this.add.TabIndex = 18;
            this.add.Text = "+";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // remove
            // 
            this.remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.remove.Location = new System.Drawing.Point(234, 36);
            this.remove.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(31, 28);
            this.remove.TabIndex = 19;
            this.remove.Text = "-";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // editlistView
            // 
            this.editlistView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editlistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.editlistView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.editlistView.LabelEdit = true;
            this.editlistView.Location = new System.Drawing.Point(0, 0);
            this.editlistView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editlistView.MultiSelect = false;
            this.editlistView.Name = "editlistView";
            this.editlistView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.editlistView.Size = new System.Drawing.Size(226, 86);
            this.editlistView.TabIndex = 17;
            this.editlistView.UseCompatibleStateImageBehavior = false;
            this.editlistView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 200;
            // 
            // EditableList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.remove);
            this.Controls.Add(this.add);
            this.Controls.Add(this.editlistView);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "EditableList";
            this.Size = new System.Drawing.Size(269, 86);
            this.SizeChanged += new System.EventHandler(this.EditableList_SizeChanged);
            this.Leave += new System.EventHandler(this.EditableList_Leave);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.ListView editlistView;
    }
}
