
namespace Xbim.CobieLiteUk.Client
{
    partial class RolesList
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
            this.chkBoxList = new global::System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // chkBoxList
            // 
            this.chkBoxList.CheckOnClick = true;
            this.chkBoxList.Dock = global::System.Windows.Forms.DockStyle.Fill;
            this.chkBoxList.FormattingEnabled = true;
            this.chkBoxList.Location = new global::System.Drawing.Point(0, 0);
            this.chkBoxList.Name = "chkBoxList";
            this.chkBoxList.Size = new global::System.Drawing.Size(295, 253);
            this.chkBoxList.TabIndex = 0;
            // 
            // RolesList
            // 
            this.AutoScaleDimensions = new global::System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkBoxList);
            this.Name = "RolesList";
            this.Size = new global::System.Drawing.Size(295, 253);
            this.ResumeLayout(false);

        }

        #endregion

        private global::System.Windows.Forms.CheckedListBox chkBoxList;
    }
}
