namespace Xbim.CobieLiteUk.Client
{
    partial class FilterTab
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
            this.components = new global::System.ComponentModel.Container();
            global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(FilterTab));
            this.label3 = new global::System.Windows.Forms.Label();
            this.chkListBoxAss = new global::System.Windows.Forms.CheckedListBox();
            this.label2 = new global::System.Windows.Forms.Label();
            this.chkListBoxType = new global::System.Windows.Forms.CheckedListBox();
            this.label1 = new global::System.Windows.Forms.Label();
            this.chkListBoxComp = new global::System.Windows.Forms.CheckedListBox();
            this.label4 = new global::System.Windows.Forms.Label();
            this.tabPropertyCtr = new global::System.Windows.Forms.TabControl();
            this.tabCommon = new global::System.Windows.Forms.TabPage();
            this.tabZone = new global::System.Windows.Forms.TabPage();
            this.tabType = new global::System.Windows.Forms.TabPage();
            this.tabSpace = new global::System.Windows.Forms.TabPage();
            this.tabFloor = new global::System.Windows.Forms.TabPage();
            this.tabFacility = new global::System.Windows.Forms.TabPage();
            this.tabSpare = new global::System.Windows.Forms.TabPage();
            this.tabComponent = new global::System.Windows.Forms.TabPage();
            this.label5 = new global::System.Windows.Forms.Label();
            this.btnSetDefaults = new global::System.Windows.Forms.Button();
            this.menuClick = new global::System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
            this.fuillToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
            this.pSetFiltersCommon = new PropertySetFilters();
            this.pSetFiltersZone = new PropertySetFilters();
            this.pSetFiltersType = new PropertySetFilters();
            this.pSetFiltersSpace = new PropertySetFilters();
            this.pSetFiltersFloor = new PropertySetFilters();
            this.pSetFiltersFacility = new PropertySetFilters();
            this.pSetFiltersSpare = new PropertySetFilters();
            this.pSetFiltersComponent = new PropertySetFilters();
            this.listViewDefinedTypes = new EditableList();
            this.tabPropertyCtr.SuspendLayout();
            this.tabCommon.SuspendLayout();
            this.tabZone.SuspendLayout();
            this.tabType.SuspendLayout();
            this.tabSpace.SuspendLayout();
            this.tabFloor.SuspendLayout();
            this.tabFacility.SuspendLayout();
            this.tabSpare.SuspendLayout();
            this.tabComponent.SuspendLayout();
            this.menuClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new global::System.Drawing.Point(543, 6);
            this.label3.Name = "label3";
            this.label3.Size = new global::System.Drawing.Size(96, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Assembly Sheet";
            // 
            // chkListBoxAss
            // 
            this.chkListBoxAss.CheckOnClick = true;
            this.chkListBoxAss.ContextMenuStrip = this.menuClick;
            this.chkListBoxAss.FormattingEnabled = true;
            this.chkListBoxAss.Location = new global::System.Drawing.Point(546, 22);
            this.chkListBoxAss.Name = "chkListBoxAss";
            this.chkListBoxAss.Size = new global::System.Drawing.Size(259, 229);
            this.chkListBoxAss.Sorted = true;
            this.chkListBoxAss.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new global::System.Drawing.Point(273, 6);
            this.label2.Name = "label2";
            this.label2.Size = new global::System.Drawing.Size(72, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Type Sheet";
            // 
            // chkListBoxType
            // 
            this.chkListBoxType.ContextMenuStrip = this.menuClick;
            this.chkListBoxType.FormattingEnabled = true;
            this.chkListBoxType.Location = new global::System.Drawing.Point(276, 22);
            this.chkListBoxType.Name = "chkListBoxType";
            this.chkListBoxType.Size = new global::System.Drawing.Size(259, 139);
            this.chkListBoxType.Sorted = true;
            this.chkListBoxType.TabIndex = 10;
            this.chkListBoxType.SelectedIndexChanged += new global::System.EventHandler(this.chkListBoxType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new global::System.Drawing.Point(8, 6);
            this.label1.Name = "label1";
            this.label1.Size = new global::System.Drawing.Size(107, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Component Sheet";
            // 
            // chkListBoxComp
            // 
            this.chkListBoxComp.CheckOnClick = true;
            this.chkListBoxComp.ContextMenuStrip = this.menuClick;
            this.chkListBoxComp.FormattingEnabled = true;
            this.chkListBoxComp.Location = new global::System.Drawing.Point(8, 22);
            this.chkListBoxComp.Name = "chkListBoxComp";
            this.chkListBoxComp.Size = new global::System.Drawing.Size(259, 229);
            this.chkListBoxComp.Sorted = true;
            this.chkListBoxComp.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new global::System.Drawing.Point(273, 164);
            this.label4.Name = "label4";
            this.label4.Size = new global::System.Drawing.Size(89, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Defined Types";
            // 
            // tabPropertyCtr
            // 
            this.tabPropertyCtr.Controls.Add(this.tabCommon);
            this.tabPropertyCtr.Controls.Add(this.tabZone);
            this.tabPropertyCtr.Controls.Add(this.tabType);
            this.tabPropertyCtr.Controls.Add(this.tabSpace);
            this.tabPropertyCtr.Controls.Add(this.tabFloor);
            this.tabPropertyCtr.Controls.Add(this.tabFacility);
            this.tabPropertyCtr.Controls.Add(this.tabSpare);
            this.tabPropertyCtr.Controls.Add(this.tabComponent);
            this.tabPropertyCtr.Location = new global::System.Drawing.Point(8, 287);
            this.tabPropertyCtr.Name = "tabPropertyCtr";
            this.tabPropertyCtr.SelectedIndex = 0;
            this.tabPropertyCtr.Size = new global::System.Drawing.Size(801, 241);
            this.tabPropertyCtr.TabIndex = 17;
            // 
            // tabCommon
            // 
            this.tabCommon.Controls.Add(this.pSetFiltersCommon);
            this.tabCommon.Location = new global::System.Drawing.Point(4, 22);
            this.tabCommon.Name = "tabCommon";
            this.tabCommon.Padding = new global::System.Windows.Forms.Padding(3);
            this.tabCommon.Size = new global::System.Drawing.Size(793, 215);
            this.tabCommon.TabIndex = 0;
            this.tabCommon.Text = "Common";
            this.tabCommon.UseVisualStyleBackColor = true;
            // 
            // tabZone
            // 
            this.tabZone.Controls.Add(this.pSetFiltersZone);
            this.tabZone.Location = new global::System.Drawing.Point(4, 22);
            this.tabZone.Name = "tabZone";
            this.tabZone.Padding = new global::System.Windows.Forms.Padding(3);
            this.tabZone.Size = new global::System.Drawing.Size(793, 215);
            this.tabZone.TabIndex = 1;
            this.tabZone.Text = "Zone";
            this.tabZone.UseVisualStyleBackColor = true;
            // 
            // tabType
            // 
            this.tabType.Controls.Add(this.pSetFiltersType);
            this.tabType.Location = new global::System.Drawing.Point(4, 22);
            this.tabType.Name = "tabType";
            this.tabType.Size = new global::System.Drawing.Size(793, 215);
            this.tabType.TabIndex = 2;
            this.tabType.Text = "Type";
            this.tabType.UseVisualStyleBackColor = true;
            // 
            // tabSpace
            // 
            this.tabSpace.Controls.Add(this.pSetFiltersSpace);
            this.tabSpace.Location = new global::System.Drawing.Point(4, 22);
            this.tabSpace.Name = "tabSpace";
            this.tabSpace.Size = new global::System.Drawing.Size(793, 215);
            this.tabSpace.TabIndex = 3;
            this.tabSpace.Text = "Space";
            this.tabSpace.UseVisualStyleBackColor = true;
            // 
            // tabFloor
            // 
            this.tabFloor.Controls.Add(this.pSetFiltersFloor);
            this.tabFloor.Location = new global::System.Drawing.Point(4, 22);
            this.tabFloor.Name = "tabFloor";
            this.tabFloor.Size = new global::System.Drawing.Size(793, 215);
            this.tabFloor.TabIndex = 4;
            this.tabFloor.Text = "Floor";
            this.tabFloor.UseVisualStyleBackColor = true;
            // 
            // tabFacility
            // 
            this.tabFacility.Controls.Add(this.pSetFiltersFacility);
            this.tabFacility.Location = new global::System.Drawing.Point(4, 22);
            this.tabFacility.Name = "tabFacility";
            this.tabFacility.Size = new global::System.Drawing.Size(793, 215);
            this.tabFacility.TabIndex = 5;
            this.tabFacility.Text = "Facility";
            this.tabFacility.UseVisualStyleBackColor = true;
            // 
            // tabSpare
            // 
            this.tabSpare.Controls.Add(this.pSetFiltersSpare);
            this.tabSpare.Location = new global::System.Drawing.Point(4, 22);
            this.tabSpare.Name = "tabSpare";
            this.tabSpare.Size = new global::System.Drawing.Size(793, 215);
            this.tabSpare.TabIndex = 6;
            this.tabSpare.Text = "Spare";
            this.tabSpare.UseVisualStyleBackColor = true;
            // 
            // tabComponent
            // 
            this.tabComponent.Controls.Add(this.pSetFiltersComponent);
            this.tabComponent.Location = new global::System.Drawing.Point(4, 22);
            this.tabComponent.Name = "tabComponent";
            this.tabComponent.Size = new global::System.Drawing.Size(793, 215);
            this.tabComponent.TabIndex = 7;
            this.tabComponent.Text = "Component";
            this.tabComponent.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new global::System.Drawing.Point(8, 271);
            this.label5.Name = "label5";
            this.label5.Size = new global::System.Drawing.Size(109, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Property Excludes";
            // 
            // btnSetDefaults
            // 
            this.btnSetDefaults.Location = new global::System.Drawing.Point(727, 266);
            this.btnSetDefaults.Name = "btnSetDefaults";
            this.btnSetDefaults.Size = new global::System.Drawing.Size(75, 23);
            this.btnSetDefaults.TabIndex = 1;
            this.btnSetDefaults.Text = "Set Defaults";
            this.btnSetDefaults.UseVisualStyleBackColor = true;
            this.btnSetDefaults.Click += new global::System.EventHandler(this.btnSetDefaults_Click);
            // 
            // menuClick
            // 
            this.menuClick.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.fuillToolStripMenuItem});
            this.menuClick.Name = "menuClick";
            this.menuClick.Size = new global::System.Drawing.Size(153, 70);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new global::System.Drawing.Size(118, 22);
            this.clearToolStripMenuItem.Text = "Clear All";
            this.clearToolStripMenuItem.Click += new global::System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // fuillToolStripMenuItem
            // 
            this.fuillToolStripMenuItem.Name = "fuillToolStripMenuItem";
            this.fuillToolStripMenuItem.Size = new global::System.Drawing.Size(152, 22);
            this.fuillToolStripMenuItem.Text = "Tick All";
            this.fuillToolStripMenuItem.Click += new global::System.EventHandler(this.fuillToolStripMenuItem_Click);
            // 
            // pSetFiltersCommon
            // 
            this.pSetFiltersCommon.Location = new global::System.Drawing.Point(4, 4);
            this.pSetFiltersCommon.Name = "pSetFiltersCommon";
            this.pSetFiltersCommon.Size = new global::System.Drawing.Size(786, 209);
            this.pSetFiltersCommon.TabIndex = 0;
            // 
            // pSetFiltersZone
            // 
            this.pSetFiltersZone.Location = new global::System.Drawing.Point(4, 4);
            this.pSetFiltersZone.Name = "pSetFiltersZone";
            this.pSetFiltersZone.Size = new global::System.Drawing.Size(786, 208);
            this.pSetFiltersZone.TabIndex = 0;
            // 
            // pSetFiltersType
            // 
            this.pSetFiltersType.Location = new global::System.Drawing.Point(4, 4);
            this.pSetFiltersType.Name = "pSetFiltersType";
            this.pSetFiltersType.Size = new global::System.Drawing.Size(786, 208);
            this.pSetFiltersType.TabIndex = 0;
            // 
            // pSetFiltersSpace
            // 
            this.pSetFiltersSpace.Location = new global::System.Drawing.Point(4, 4);
            this.pSetFiltersSpace.Name = "pSetFiltersSpace";
            this.pSetFiltersSpace.Size = new global::System.Drawing.Size(786, 208);
            this.pSetFiltersSpace.TabIndex = 0;
            // 
            // pSetFiltersFloor
            // 
            this.pSetFiltersFloor.Location = new global::System.Drawing.Point(4, 4);
            this.pSetFiltersFloor.Name = "pSetFiltersFloor";
            this.pSetFiltersFloor.Size = new global::System.Drawing.Size(786, 208);
            this.pSetFiltersFloor.TabIndex = 0;
            // 
            // pSetFiltersFacility
            // 
            this.pSetFiltersFacility.Location = new global::System.Drawing.Point(4, 4);
            this.pSetFiltersFacility.Name = "pSetFiltersFacility";
            this.pSetFiltersFacility.Size = new global::System.Drawing.Size(786, 208);
            this.pSetFiltersFacility.TabIndex = 0;
            // 
            // pSetFiltersSpare
            // 
            this.pSetFiltersSpare.Location = new global::System.Drawing.Point(4, 4);
            this.pSetFiltersSpare.Name = "pSetFiltersSpare";
            this.pSetFiltersSpare.Size = new global::System.Drawing.Size(786, 208);
            this.pSetFiltersSpare.TabIndex = 0;
            // 
            // pSetFiltersComponent
            // 
            this.pSetFiltersComponent.Location = new global::System.Drawing.Point(4, 4);
            this.pSetFiltersComponent.Name = "pSetFiltersComponent";
            this.pSetFiltersComponent.Size = new global::System.Drawing.Size(786, 208);
            this.pSetFiltersComponent.TabIndex = 0;
            // 
            // listViewDefinedTypes
            // 
            this.listViewDefinedTypes.AllUpper = true;
            this.listViewDefinedTypes.Items = ((global::System.Collections.Generic.List<string>)(resources.GetObject("listViewDefinedTypes.Items")));
            this.listViewDefinedTypes.Key = null;
            this.listViewDefinedTypes.Location = new global::System.Drawing.Point(276, 185);
            this.listViewDefinedTypes.Name = "listViewDefinedTypes";
            this.listViewDefinedTypes.Size = new global::System.Drawing.Size(259, 66);
            this.listViewDefinedTypes.TabIndex = 16;
            // 
            // FilterTab
            // 
            this.AutoScaleDimensions = new global::System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSetDefaults);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tabPropertyCtr);
            this.Controls.Add(this.listViewDefinedTypes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkListBoxAss);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkListBoxType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkListBoxComp);
            this.Name = "FilterTab";
            this.Size = new global::System.Drawing.Size(827, 528);
            this.tabPropertyCtr.ResumeLayout(false);
            this.tabCommon.ResumeLayout(false);
            this.tabZone.ResumeLayout(false);
            this.tabType.ResumeLayout(false);
            this.tabSpace.ResumeLayout(false);
            this.tabFloor.ResumeLayout(false);
            this.tabFacility.ResumeLayout(false);
            this.tabSpare.ResumeLayout(false);
            this.tabComponent.ResumeLayout(false);
            this.menuClick.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private global::System.Windows.Forms.Label label3;
        private global::System.Windows.Forms.CheckedListBox chkListBoxAss;
        private global::System.Windows.Forms.Label label2;
        private global::System.Windows.Forms.CheckedListBox chkListBoxType;
        private global::System.Windows.Forms.Label label1;
        private global::System.Windows.Forms.CheckedListBox chkListBoxComp;
        private global::System.Windows.Forms.Label label4;
        private EditableList listViewDefinedTypes;
        private global::System.Windows.Forms.TabControl tabPropertyCtr;
        private global::System.Windows.Forms.TabPage tabCommon;
        private global::System.Windows.Forms.TabPage tabZone;
        private global::System.Windows.Forms.TabPage tabType;
        private global::System.Windows.Forms.TabPage tabSpace;
        private global::System.Windows.Forms.Label label5;
        private global::System.Windows.Forms.TabPage tabFloor;
        private global::System.Windows.Forms.TabPage tabFacility;
        private global::System.Windows.Forms.TabPage tabSpare;
        private global::System.Windows.Forms.TabPage tabComponent;
        private PropertySetFilters pSetFiltersCommon;
        private PropertySetFilters pSetFiltersZone;
        private PropertySetFilters pSetFiltersType;
        private PropertySetFilters pSetFiltersSpace;
        private PropertySetFilters pSetFiltersFloor;
        private PropertySetFilters pSetFiltersFacility;
        private PropertySetFilters pSetFiltersSpare;
        private PropertySetFilters pSetFiltersComponent;
        private global::System.Windows.Forms.Button btnSetDefaults;
        private global::System.Windows.Forms.ContextMenuStrip menuClick;
        private global::System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem fuillToolStripMenuItem;
    }
}
