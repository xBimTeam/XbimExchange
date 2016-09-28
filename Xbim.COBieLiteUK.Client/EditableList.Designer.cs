namespace Xbim.CobieLiteUk.Client
{
    partial class EditableList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.add = new global::System.Windows.Forms.Button();
            this.remove = new global::System.Windows.Forms.Button();
            this.editlistView = new global::System.Windows.Forms.ListView();
            this.columnHeader1 = ((global::System.Windows.Forms.ColumnHeader)(new global::System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // add
            // 
            this.add.Anchor = ((global::System.Windows.Forms.AnchorStyles)((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right)));
            this.add.Location = new global::System.Drawing.Point(234, 0);
            this.add.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
            this.add.Name = "add";
            this.add.Size = new global::System.Drawing.Size(31, 28);
            this.add.TabIndex = 18;
            this.add.Text = "+";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new global::System.EventHandler(this.add_Click);
            // 
            // remove
            // 
            this.remove.Anchor = ((global::System.Windows.Forms.AnchorStyles)((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right)));
            this.remove.Location = new global::System.Drawing.Point(234, 36);
            this.remove.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
            this.remove.Name = "remove";
            this.remove.Size = new global::System.Drawing.Size(31, 28);
            this.remove.TabIndex = 19;
            this.remove.Text = "-";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new global::System.EventHandler(this.remove_Click);
            // 
            // editlistView
            // 
            this.editlistView.Anchor = ((global::System.Windows.Forms.AnchorStyles)((((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom) 
            | global::System.Windows.Forms.AnchorStyles.Left) 
            | global::System.Windows.Forms.AnchorStyles.Right)));
            this.editlistView.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.editlistView.HeaderStyle = global::System.Windows.Forms.ColumnHeaderStyle.None;
            this.editlistView.LabelEdit = true;
            this.editlistView.Location = new global::System.Drawing.Point(0, 0);
            this.editlistView.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editlistView.MultiSelect = false;
            this.editlistView.Name = "editlistView";
            this.editlistView.RightToLeft = global::System.Windows.Forms.RightToLeft.No;
            this.editlistView.Size = new global::System.Drawing.Size(226, 86);
            this.editlistView.TabIndex = 17;
            this.editlistView.UseCompatibleStateImageBehavior = false;
            this.editlistView.View = global::System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 200;
            // 
            // EditableList
            // 
            this.AutoScaleDimensions = new global::System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.remove);
            this.Controls.Add(this.add);
            this.Controls.Add(this.editlistView);
            this.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "EditableList";
            this.Size = new global::System.Drawing.Size(269, 86);
            this.SizeChanged += new global::System.EventHandler(this.EditableList_SizeChanged);
            this.Leave += new global::System.EventHandler(this.EditableList_Leave);
            this.ResumeLayout(false);

        }

        #endregion

        private global::System.Windows.Forms.ColumnHeader columnHeader1;
        private global::System.Windows.Forms.Button add;
        private global::System.Windows.Forms.Button remove;
        private global::System.Windows.Forms.ListView editlistView;
    }
}
