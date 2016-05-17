namespace Xbim.Client
{
    partial class PropertyMapTab
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyMapTab));
            this.listBoxFieldNames = new System.Windows.Forms.ListBox();
            this.listViewMaps = new Xbim.Client.EditableList();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxFieldNames
            // 
            this.listBoxFieldNames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxFieldNames.FormattingEnabled = true;
            this.listBoxFieldNames.ItemHeight = 16;
            this.listBoxFieldNames.Location = new System.Drawing.Point(0, 0);
            this.listBoxFieldNames.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBoxFieldNames.Name = "listBoxFieldNames";
            this.listBoxFieldNames.Size = new System.Drawing.Size(393, 242);
            this.listBoxFieldNames.TabIndex = 0;
            this.listBoxFieldNames.SelectedIndexChanged += new System.EventHandler(this.listBoxFieldNames_SelectedIndexChanged);
            // 
            // listViewMaps
            // 
            this.listViewMaps.AllUpper = true;
            this.listViewMaps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewMaps.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("listViewMaps.Items")));
            this.listViewMaps.Key = null;
            this.listViewMaps.Location = new System.Drawing.Point(0, 0);
            this.listViewMaps.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.listViewMaps.Name = "listViewMaps";
            this.listViewMaps.Size = new System.Drawing.Size(393, 239);
            this.listViewMaps.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBoxFieldNames);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listViewMaps);
            this.splitContainer1.Size = new System.Drawing.Size(393, 485);
            this.splitContainer1.SplitterDistance = 242;
            this.splitContainer1.TabIndex = 2;
            // 
            // PropertyMapTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PropertyMapTab";
            this.Size = new System.Drawing.Size(393, 485);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxFieldNames;
        private EditableList listViewMaps;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
