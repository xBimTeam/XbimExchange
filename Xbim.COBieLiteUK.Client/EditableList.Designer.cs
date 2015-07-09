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
            this.editlistView = new Xbim.Client.EditListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(162, 3);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(23, 23);
            this.add.TabIndex = 18;
            this.add.Text = "+";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // remove
            // 
            this.remove.Location = new System.Drawing.Point(162, 27);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(23, 23);
            this.remove.TabIndex = 19;
            this.remove.Text = "-";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // editlistView
            // 
            this.editlistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.editlistView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.editlistView.Key = null;
            this.editlistView.LabelEdit = true;
            this.editlistView.Location = new System.Drawing.Point(0, 0);
            this.editlistView.MultiSelect = false;
            this.editlistView.Name = "editlistView";
            this.editlistView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.editlistView.Size = new System.Drawing.Size(156, 64);
            this.editlistView.TabIndex = 17;
            this.editlistView.UseCompatibleStateImageBehavior = false;
            this.editlistView.View = System.Windows.Forms.View.Details;
            this.editlistView.Leave += new System.EventHandler(this.editlistView_Leave);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 200;
            // 
            // EditableList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.remove);
            this.Controls.Add(this.add);
            this.Controls.Add(this.editlistView);
            this.Name = "EditableList";
            this.Size = new System.Drawing.Size(192, 70);
            this.SizeChanged += new System.EventHandler(this.EditableList_SizeChanged);
            this.Leave += new System.EventHandler(this.EditableList_Leave);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button remove;
        private EditListView editlistView;
    }
}
