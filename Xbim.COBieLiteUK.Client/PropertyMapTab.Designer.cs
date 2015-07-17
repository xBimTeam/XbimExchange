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
            this.SuspendLayout();
            // 
            // listBoxFieldNames
            // 
            this.listBoxFieldNames.FormattingEnabled = true;
            this.listBoxFieldNames.Location = new System.Drawing.Point(3, 3);
            this.listBoxFieldNames.Name = "listBoxFieldNames";
            this.listBoxFieldNames.Size = new System.Drawing.Size(289, 186);
            this.listBoxFieldNames.TabIndex = 0;
            this.listBoxFieldNames.SelectedIndexChanged += new System.EventHandler(this.listBoxFieldNames_SelectedIndexChanged);
            // 
            // listViewMaps
            // 
            this.listViewMaps.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("listViewMaps.Items")));
            this.listViewMaps.Key = null;
            this.listViewMaps.Location = new System.Drawing.Point(3, 195);
            this.listViewMaps.Name = "listViewMaps";
            this.listViewMaps.Size = new System.Drawing.Size(292, 187);
            this.listViewMaps.TabIndex = 1;
            // 
            // PropertyMapTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewMaps);
            this.Controls.Add(this.listBoxFieldNames);
            this.Name = "PropertyMapTab";
            this.Size = new System.Drawing.Size(295, 394);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxFieldNames;
        private EditableList listViewMaps;
    }
}
