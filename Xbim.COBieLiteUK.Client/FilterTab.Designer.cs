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
            this.label3 = new System.Windows.Forms.Label();
            this.chkListBoxAss = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkListBoxType = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkListBoxComp = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPropertyCtr = new System.Windows.Forms.TabControl();
            this.tabCommon = new System.Windows.Forms.TabPage();
            this.listViewEqTo = new Xbim.Client.EditableList();
            this.tabZone = new System.Windows.Forms.TabPage();
            this.listViewStartWith = new Xbim.Client.EditableList();
            this.tabType = new System.Windows.Forms.TabPage();
            this.listViewContains = new Xbim.Client.EditableList();
            this.tabSpace = new System.Windows.Forms.TabPage();
            this.listViewPSet = new Xbim.Client.EditableList();
            this.listViewDefinedTypes = new Xbim.Client.EditableList();
            this.label5 = new System.Windows.Forms.Label();
            this.editableList1 = new Xbim.Client.EditableList();
            this.editableList2 = new Xbim.Client.EditableList();
            this.editableList3 = new Xbim.Client.EditableList();
            this.tabFloor = new System.Windows.Forms.TabPage();
            this.tabFacility = new System.Windows.Forms.TabPage();
            this.tabSpare = new System.Windows.Forms.TabPage();
            this.tabComponent = new System.Windows.Forms.TabPage();
            this.tabPropertyCtr.SuspendLayout();
            this.tabCommon.SuspendLayout();
            this.tabZone.SuspendLayout();
            this.tabType.SuspendLayout();
            this.tabSpace.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(617, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Assembly Sheet Excludes";
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
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Type Sheet Excludes";
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
            this.label1.Size = new System.Drawing.Size(162, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Component Sheet Excludes";
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
            this.tabPropertyCtr.Size = new System.Drawing.Size(801, 229);
            this.tabPropertyCtr.TabIndex = 17;
            // 
            // tabCommon
            // 
            this.tabCommon.Controls.Add(this.editableList3);
            this.tabCommon.Controls.Add(this.editableList2);
            this.tabCommon.Controls.Add(this.editableList1);
            this.tabCommon.Controls.Add(this.listViewEqTo);
            this.tabCommon.Location = new System.Drawing.Point(4, 22);
            this.tabCommon.Name = "tabCommon";
            this.tabCommon.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommon.Size = new System.Drawing.Size(793, 203);
            this.tabCommon.TabIndex = 0;
            this.tabCommon.Text = "Common";
            this.tabCommon.UseVisualStyleBackColor = true;
            // 
            // listViewEqTo
            // 
            this.listViewEqTo.Location = new System.Drawing.Point(3, 22);
            this.listViewEqTo.Name = "listViewEqTo";
            this.listViewEqTo.Size = new System.Drawing.Size(190, 175);
            this.listViewEqTo.TabIndex = 0;
            // 
            // tabZone
            // 
            this.tabZone.Controls.Add(this.listViewStartWith);
            this.tabZone.Location = new System.Drawing.Point(4, 22);
            this.tabZone.Name = "tabZone";
            this.tabZone.Padding = new System.Windows.Forms.Padding(3);
            this.tabZone.Size = new System.Drawing.Size(793, 203);
            this.tabZone.TabIndex = 1;
            this.tabZone.Text = "Zone";
            this.tabZone.UseVisualStyleBackColor = true;
            // 
            // listViewStartWith
            // 
            this.listViewStartWith.Location = new System.Drawing.Point(3, 6);
            this.listViewStartWith.Name = "listViewStartWith";
            this.listViewStartWith.Size = new System.Drawing.Size(289, 191);
            this.listViewStartWith.TabIndex = 0;
            // 
            // tabType
            // 
            this.tabType.Controls.Add(this.listViewContains);
            this.tabType.Location = new System.Drawing.Point(4, 22);
            this.tabType.Name = "tabType";
            this.tabType.Size = new System.Drawing.Size(793, 203);
            this.tabType.TabIndex = 2;
            this.tabType.Text = "Type";
            this.tabType.UseVisualStyleBackColor = true;
            // 
            // listViewContains
            // 
            this.listViewContains.Location = new System.Drawing.Point(3, 6);
            this.listViewContains.Name = "listViewContains";
            this.listViewContains.Size = new System.Drawing.Size(289, 191);
            this.listViewContains.TabIndex = 0;
            // 
            // tabSpace
            // 
            this.tabSpace.Controls.Add(this.listViewPSet);
            this.tabSpace.Location = new System.Drawing.Point(4, 22);
            this.tabSpace.Name = "tabSpace";
            this.tabSpace.Size = new System.Drawing.Size(793, 203);
            this.tabSpace.TabIndex = 3;
            this.tabSpace.Text = "Space";
            this.tabSpace.UseVisualStyleBackColor = true;
            // 
            // listViewPSet
            // 
            this.listViewPSet.Location = new System.Drawing.Point(3, 6);
            this.listViewPSet.Name = "listViewPSet";
            this.listViewPSet.Size = new System.Drawing.Size(289, 191);
            this.listViewPSet.TabIndex = 0;
            // 
            // listViewDefinedTypes
            // 
            this.listViewDefinedTypes.Location = new System.Drawing.Point(276, 185);
            this.listViewDefinedTypes.Name = "listViewDefinedTypes";
            this.listViewDefinedTypes.Size = new System.Drawing.Size(259, 66);
            this.listViewDefinedTypes.TabIndex = 16;
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
            // editableList1
            // 
            this.editableList1.Location = new System.Drawing.Point(198, 22);
            this.editableList1.Name = "editableList1";
            this.editableList1.Size = new System.Drawing.Size(190, 175);
            this.editableList1.TabIndex = 1;
            // 
            // editableList2
            // 
            this.editableList2.Location = new System.Drawing.Point(395, 22);
            this.editableList2.Name = "editableList2";
            this.editableList2.Size = new System.Drawing.Size(190, 175);
            this.editableList2.TabIndex = 2;
            // 
            // editableList3
            // 
            this.editableList3.Location = new System.Drawing.Point(592, 22);
            this.editableList3.Name = "editableList3";
            this.editableList3.Size = new System.Drawing.Size(190, 175);
            this.editableList3.TabIndex = 3;
            // 
            // tabFloor
            // 
            this.tabFloor.Location = new System.Drawing.Point(4, 22);
            this.tabFloor.Name = "tabFloor";
            this.tabFloor.Size = new System.Drawing.Size(793, 203);
            this.tabFloor.TabIndex = 4;
            this.tabFloor.Text = "Floor";
            this.tabFloor.UseVisualStyleBackColor = true;
            // 
            // tabFacility
            // 
            this.tabFacility.Location = new System.Drawing.Point(4, 22);
            this.tabFacility.Name = "tabFacility";
            this.tabFacility.Size = new System.Drawing.Size(793, 203);
            this.tabFacility.TabIndex = 5;
            this.tabFacility.Text = "Facility";
            this.tabFacility.UseVisualStyleBackColor = true;
            // 
            // tabSpare
            // 
            this.tabSpare.Location = new System.Drawing.Point(4, 22);
            this.tabSpare.Name = "tabSpare";
            this.tabSpare.Size = new System.Drawing.Size(793, 203);
            this.tabSpare.TabIndex = 6;
            this.tabSpare.Text = "Spare";
            this.tabSpare.UseVisualStyleBackColor = true;
            // 
            // tabComponent
            // 
            this.tabComponent.Location = new System.Drawing.Point(4, 22);
            this.tabComponent.Name = "tabComponent";
            this.tabComponent.Size = new System.Drawing.Size(793, 203);
            this.tabComponent.TabIndex = 7;
            this.tabComponent.Text = "Component";
            this.tabComponent.UseVisualStyleBackColor = true;
            // 
            // FilterTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
        private EditableList listViewEqTo;
        private EditableList listViewStartWith;
        private EditableList listViewContains;
        private EditableList listViewPSet;
        private System.Windows.Forms.Label label5;
        private EditableList editableList3;
        private EditableList editableList2;
        private EditableList editableList1;
        private System.Windows.Forms.TabPage tabFloor;
        private System.Windows.Forms.TabPage tabFacility;
        private System.Windows.Forms.TabPage tabSpare;
        private System.Windows.Forms.TabPage tabComponent;
    }
}
