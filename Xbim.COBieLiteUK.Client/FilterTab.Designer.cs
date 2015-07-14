namespace Xbim.Client
{
    partial class FilterTab
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterTab));
            this.label3 = new System.Windows.Forms.Label();
            this.chkListBoxAss = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkListBoxType = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkListBoxComp = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPropertyCtr = new System.Windows.Forms.TabControl();
            this.tabCommon = new System.Windows.Forms.TabPage();
            this.pSetFiltersCommon = new Xbim.Client.PropertySetFilters();
            this.tabZone = new System.Windows.Forms.TabPage();
            this.pSetFiltersZone = new Xbim.Client.PropertySetFilters();
            this.tabType = new System.Windows.Forms.TabPage();
            this.pSetFiltersType = new Xbim.Client.PropertySetFilters();
            this.tabSpace = new System.Windows.Forms.TabPage();
            this.pSetFiltersSpace = new Xbim.Client.PropertySetFilters();
            this.tabFloor = new System.Windows.Forms.TabPage();
            this.pSetFiltersFloor = new Xbim.Client.PropertySetFilters();
            this.tabFacility = new System.Windows.Forms.TabPage();
            this.pSetFiltersFacility = new Xbim.Client.PropertySetFilters();
            this.tabSpare = new System.Windows.Forms.TabPage();
            this.pSetFiltersSpare = new Xbim.Client.PropertySetFilters();
            this.tabComponent = new System.Windows.Forms.TabPage();
            this.pSetFiltersComponent = new Xbim.Client.PropertySetFilters();
            this.label5 = new System.Windows.Forms.Label();
            this.listViewDefinedTypes = new Xbim.Client.EditableList();
            this.btnSetDefaults = new System.Windows.Forms.Button();
            this.tabPropertyCtr.SuspendLayout();
            this.tabCommon.SuspendLayout();
            this.tabZone.SuspendLayout();
            this.tabType.SuspendLayout();
            this.tabSpace.SuspendLayout();
            this.tabFloor.SuspendLayout();
            this.tabFacility.SuspendLayout();
            this.tabSpare.SuspendLayout();
            this.tabComponent.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(543, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Assembly Sheet";
            // 
            // chkListBoxAss
            // 
            this.chkListBoxAss.CheckOnClick = true;
            this.chkListBoxAss.FormattingEnabled = true;
            this.chkListBoxAss.Location = new System.Drawing.Point(546, 22);
            this.chkListBoxAss.Name = "chkListBoxAss";
            this.chkListBoxAss.Size = new System.Drawing.Size(259, 229);
            this.chkListBoxAss.Sorted = true;
            this.chkListBoxAss.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(273, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Type Sheet";
            // 
            // chkListBoxType
            // 
            this.chkListBoxType.FormattingEnabled = true;
            this.chkListBoxType.Location = new System.Drawing.Point(276, 22);
            this.chkListBoxType.Name = "chkListBoxType";
            this.chkListBoxType.Size = new System.Drawing.Size(259, 139);
            this.chkListBoxType.Sorted = true;
            this.chkListBoxType.TabIndex = 10;
            this.chkListBoxType.SelectedIndexChanged += new System.EventHandler(this.chkListBoxType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Component Sheet";
            // 
            // chkListBoxComp
            // 
            this.chkListBoxComp.CheckOnClick = true;
            this.chkListBoxComp.FormattingEnabled = true;
            this.chkListBoxComp.Location = new System.Drawing.Point(8, 22);
            this.chkListBoxComp.Name = "chkListBoxComp";
            this.chkListBoxComp.Size = new System.Drawing.Size(259, 229);
            this.chkListBoxComp.Sorted = true;
            this.chkListBoxComp.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(273, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
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
            this.tabPropertyCtr.Location = new System.Drawing.Point(8, 287);
            this.tabPropertyCtr.Name = "tabPropertyCtr";
            this.tabPropertyCtr.SelectedIndex = 0;
            this.tabPropertyCtr.Size = new System.Drawing.Size(801, 241);
            this.tabPropertyCtr.TabIndex = 17;
            // 
            // tabCommon
            // 
            this.tabCommon.Controls.Add(this.pSetFiltersCommon);
            this.tabCommon.Location = new System.Drawing.Point(4, 22);
            this.tabCommon.Name = "tabCommon";
            this.tabCommon.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommon.Size = new System.Drawing.Size(793, 215);
            this.tabCommon.TabIndex = 0;
            this.tabCommon.Text = "Common";
            this.tabCommon.UseVisualStyleBackColor = true;
            // 
            // pSetFiltersCommon
            // 
            this.pSetFiltersCommon.Location = new System.Drawing.Point(4, 4);
            this.pSetFiltersCommon.Name = "pSetFiltersCommon";
            this.pSetFiltersCommon.Size = new System.Drawing.Size(786, 209);
            this.pSetFiltersCommon.TabIndex = 0;
            // 
            // tabZone
            // 
            this.tabZone.Controls.Add(this.pSetFiltersZone);
            this.tabZone.Location = new System.Drawing.Point(4, 22);
            this.tabZone.Name = "tabZone";
            this.tabZone.Padding = new System.Windows.Forms.Padding(3);
            this.tabZone.Size = new System.Drawing.Size(793, 215);
            this.tabZone.TabIndex = 1;
            this.tabZone.Text = "Zone";
            this.tabZone.UseVisualStyleBackColor = true;
            // 
            // pSetFiltersZone
            // 
            this.pSetFiltersZone.Location = new System.Drawing.Point(4, 4);
            this.pSetFiltersZone.Name = "pSetFiltersZone";
            this.pSetFiltersZone.Size = new System.Drawing.Size(786, 208);
            this.pSetFiltersZone.TabIndex = 0;
            // 
            // tabType
            // 
            this.tabType.Controls.Add(this.pSetFiltersType);
            this.tabType.Location = new System.Drawing.Point(4, 22);
            this.tabType.Name = "tabType";
            this.tabType.Size = new System.Drawing.Size(793, 215);
            this.tabType.TabIndex = 2;
            this.tabType.Text = "Type";
            this.tabType.UseVisualStyleBackColor = true;
            // 
            // pSetFiltersType
            // 
            this.pSetFiltersType.Location = new System.Drawing.Point(4, 4);
            this.pSetFiltersType.Name = "pSetFiltersType";
            this.pSetFiltersType.Size = new System.Drawing.Size(786, 208);
            this.pSetFiltersType.TabIndex = 0;
            // 
            // tabSpace
            // 
            this.tabSpace.Controls.Add(this.pSetFiltersSpace);
            this.tabSpace.Location = new System.Drawing.Point(4, 22);
            this.tabSpace.Name = "tabSpace";
            this.tabSpace.Size = new System.Drawing.Size(793, 215);
            this.tabSpace.TabIndex = 3;
            this.tabSpace.Text = "Space";
            this.tabSpace.UseVisualStyleBackColor = true;
            // 
            // pSetFiltersSpace
            // 
            this.pSetFiltersSpace.Location = new System.Drawing.Point(4, 4);
            this.pSetFiltersSpace.Name = "pSetFiltersSpace";
            this.pSetFiltersSpace.Size = new System.Drawing.Size(786, 208);
            this.pSetFiltersSpace.TabIndex = 0;
            // 
            // tabFloor
            // 
            this.tabFloor.Controls.Add(this.pSetFiltersFloor);
            this.tabFloor.Location = new System.Drawing.Point(4, 22);
            this.tabFloor.Name = "tabFloor";
            this.tabFloor.Size = new System.Drawing.Size(793, 215);
            this.tabFloor.TabIndex = 4;
            this.tabFloor.Text = "Floor";
            this.tabFloor.UseVisualStyleBackColor = true;
            // 
            // pSetFiltersFloor
            // 
            this.pSetFiltersFloor.Location = new System.Drawing.Point(4, 4);
            this.pSetFiltersFloor.Name = "pSetFiltersFloor";
            this.pSetFiltersFloor.Size = new System.Drawing.Size(786, 208);
            this.pSetFiltersFloor.TabIndex = 0;
            // 
            // tabFacility
            // 
            this.tabFacility.Controls.Add(this.pSetFiltersFacility);
            this.tabFacility.Location = new System.Drawing.Point(4, 22);
            this.tabFacility.Name = "tabFacility";
            this.tabFacility.Size = new System.Drawing.Size(793, 215);
            this.tabFacility.TabIndex = 5;
            this.tabFacility.Text = "Facility";
            this.tabFacility.UseVisualStyleBackColor = true;
            // 
            // pSetFiltersFacility
            // 
            this.pSetFiltersFacility.Location = new System.Drawing.Point(4, 4);
            this.pSetFiltersFacility.Name = "pSetFiltersFacility";
            this.pSetFiltersFacility.Size = new System.Drawing.Size(786, 208);
            this.pSetFiltersFacility.TabIndex = 0;
            // 
            // tabSpare
            // 
            this.tabSpare.Controls.Add(this.pSetFiltersSpare);
            this.tabSpare.Location = new System.Drawing.Point(4, 22);
            this.tabSpare.Name = "tabSpare";
            this.tabSpare.Size = new System.Drawing.Size(793, 215);
            this.tabSpare.TabIndex = 6;
            this.tabSpare.Text = "Spare";
            this.tabSpare.UseVisualStyleBackColor = true;
            // 
            // pSetFiltersSpare
            // 
            this.pSetFiltersSpare.Location = new System.Drawing.Point(4, 4);
            this.pSetFiltersSpare.Name = "pSetFiltersSpare";
            this.pSetFiltersSpare.Size = new System.Drawing.Size(786, 208);
            this.pSetFiltersSpare.TabIndex = 0;
            // 
            // tabComponent
            // 
            this.tabComponent.Controls.Add(this.pSetFiltersComponent);
            this.tabComponent.Location = new System.Drawing.Point(4, 22);
            this.tabComponent.Name = "tabComponent";
            this.tabComponent.Size = new System.Drawing.Size(793, 215);
            this.tabComponent.TabIndex = 7;
            this.tabComponent.Text = "Component";
            this.tabComponent.UseVisualStyleBackColor = true;
            // 
            // pSetFiltersComponent
            // 
            this.pSetFiltersComponent.Location = new System.Drawing.Point(4, 4);
            this.pSetFiltersComponent.Name = "pSetFiltersComponent";
            this.pSetFiltersComponent.Size = new System.Drawing.Size(786, 208);
            this.pSetFiltersComponent.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 271);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Property Excludes";
            // 
            // listViewDefinedTypes
            // 
            this.listViewDefinedTypes.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("listViewDefinedTypes.Items")));
            this.listViewDefinedTypes.Key = null;
            this.listViewDefinedTypes.Location = new System.Drawing.Point(276, 185);
            this.listViewDefinedTypes.Name = "listViewDefinedTypes";
            this.listViewDefinedTypes.Size = new System.Drawing.Size(259, 66);
            this.listViewDefinedTypes.TabIndex = 16;
            // 
            // btnSetDefaults
            // 
            this.btnSetDefaults.Location = new System.Drawing.Point(727, 266);
            this.btnSetDefaults.Name = "btnSetDefaults";
            this.btnSetDefaults.Size = new System.Drawing.Size(75, 23);
            this.btnSetDefaults.TabIndex = 1;
            this.btnSetDefaults.Text = "Set Defaults";
            this.btnSetDefaults.UseVisualStyleBackColor = true;
            this.btnSetDefaults.Click += new System.EventHandler(this.btnSetDefaults_Click);
            // 
            // FilterTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Size = new System.Drawing.Size(827, 528);
            this.tabPropertyCtr.ResumeLayout(false);
            this.tabCommon.ResumeLayout(false);
            this.tabZone.ResumeLayout(false);
            this.tabType.ResumeLayout(false);
            this.tabSpace.ResumeLayout(false);
            this.tabFloor.ResumeLayout(false);
            this.tabFacility.ResumeLayout(false);
            this.tabSpare.ResumeLayout(false);
            this.tabComponent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox chkListBoxAss;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox chkListBoxType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox chkListBoxComp;
        private System.Windows.Forms.Label label4;
        private EditableList listViewDefinedTypes;
        private System.Windows.Forms.TabControl tabPropertyCtr;
        private System.Windows.Forms.TabPage tabCommon;
        private System.Windows.Forms.TabPage tabZone;
        private System.Windows.Forms.TabPage tabType;
        private System.Windows.Forms.TabPage tabSpace;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabFloor;
        private System.Windows.Forms.TabPage tabFacility;
        private System.Windows.Forms.TabPage tabSpare;
        private System.Windows.Forms.TabPage tabComponent;
        private PropertySetFilters pSetFiltersCommon;
        private PropertySetFilters pSetFiltersZone;
        private PropertySetFilters pSetFiltersType;
        private PropertySetFilters pSetFiltersSpace;
        private PropertySetFilters pSetFiltersFloor;
        private PropertySetFilters pSetFiltersFacility;
        private PropertySetFilters pSetFiltersSpare;
        private PropertySetFilters pSetFiltersComponent;
        private System.Windows.Forms.Button btnSetDefaults;
    }
}
