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

        public FilterDlg(OutPutFilters assetfilters)
        {
            InitializeComponent();
            RolesFilters = assetfilters; 
            SetFilter();
        }


        public void SetFilter()
        {
            foreach (RoleFilter role in Enum.GetValues(typeof(RoleFilter)))
            {
                TabPage page = new System.Windows.Forms.TabPage(role.ToString());
                page.Controls.Add(new FilterTab(RolesFilters.GetRoleFilter(role)));
                tabControl.Controls.Add(page);
                
            }

        }

    }
}
