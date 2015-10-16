namespace Xbim.Client
{
    partial class PropertySetFilters
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
            this.listViewPSetEqTo = new Xbim.Client.EditableList();
            this.listViewContains = new Xbim.Client.EditableList();
            this.listViewStartWith = new Xbim.Client.EditableList();
            this.listViewEqTo = new Xbim.Client.EditableList();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewPSetEqTo
            // 
            this.listViewPSetEqTo.Location = new System.Drawing.Point(593, 21);
            this.listViewPSetEqTo.Name = "listViewPSetEqTo";
            this.listViewPSetEqTo.Size = new System.Drawing.Size(190, 175);
            this.listViewPSetEqTo.TabIndex = 7;
            // 
            // listViewContains
            // 
            this.listViewContains.Location = new System.Drawing.Point(396, 21);
            this.listViewContains.Name = "listViewContains";
            this.listViewContains.Size = new System.Drawing.Size(190, 175);
            this.listViewContains.TabIndex = 6;
            // 
            // listViewStartWith
            // 
            this.listViewStartWith.Location = new System.Drawing.Point(199, 21);
            this.listViewStartWith.Name = "listViewStartWith";
            this.listViewStartWith.Size = new System.Drawing.Size(190, 175);
            this.listViewStartWith.TabIndex = 5;
            // 
            // listViewEqTo
            // 
            this.listViewEqTo.Location = new System.Drawing.Point(4, 21);
            this.listViewEqTo.Name = "listViewEqTo";
            this.listViewEqTo.Size = new System.Drawing.Size(190, 175);
            this.listViewEqTo.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Property Name Equal To";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(196, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Property Name Start With";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(393, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Property Name Contains";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(590, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(170, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "PropertySets Name Equal To";
            // 
            // PropertySetFilters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listViewPSetEqTo);
            this.Controls.Add(this.listViewContains);
            this.Controls.Add(this.listViewStartWith);
            this.Controls.Add(this.listViewEqTo);
            this.Name = "PropertySetFilters";
            this.Size = new System.Drawing.Size(786, 208);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private EditableList listViewPSetEqTo;
        private EditableList listViewContains;
        private EditableList listViewStartWith;
        private EditableList listViewEqTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}
