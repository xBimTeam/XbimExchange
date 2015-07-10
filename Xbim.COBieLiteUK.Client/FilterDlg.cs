using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xbim.FilterHelper;

namespace Xbim.Client
{
    public partial class FilterDlg : Form
    {
        /// <summary>
        /// RolesFilters.RolesFilterHolder used to hold the filters per role assigned by this dialog
        /// </summary>
        public OutPutFilters RolesFilters { get; set; }
        public bool SeeMergeFilter { get; set; }
        public RoleFilter Roles { get; set; }

        public FilterDlg(OutPutFilters assetfilters, bool seeMergeFilterOnRole = false, RoleFilter roles = RoleFilter.Unknown)
        {
            InitializeComponent();
            SeeMergeFilter = seeMergeFilterOnRole;
            Roles = roles;
            RolesFilters = assetfilters;
            SetFilter();
        }


        public void SetFilter()
        {
            if (SeeMergeFilter)
            {
                TabPage page = new System.Windows.Forms.TabPage(Roles.ToString("F"));
                page.Controls.Add(new FilterTab(RolesFilters, true));
                tabControl.TabPages.Add(page);
                btnOK.Enabled = false;
            }
            else
            {
                foreach (RoleFilter role in Enum.GetValues(typeof(RoleFilter)))
                {
                    TabPage page = new System.Windows.Forms.TabPage(role.ToString());
                    page.Name = role.ToString();
                    page.Controls.Add(new FilterTab(RolesFilters.GetRoleFilter(role)));
                    tabControl.TabPages.Add(page);

                }
                btnOK.Enabled = true;
            }
            

        }

        /// <summary>
        /// OK Button Action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (TabPage item in tabControl.TabPages)
            {
                var tabName = item.Name;
                var filterTabCtr = item.Controls[0];
                if (filterTabCtr is FilterTab)
                {
                    RoleFilter role;
                    if (Enum.TryParse(tabName, out role))
                    {
                        var filterObj = ((FilterTab)filterTabCtr).Save();
                        RolesFilters.AddRoleFilterHolderItem(role, filterObj);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }


        
    }
}
