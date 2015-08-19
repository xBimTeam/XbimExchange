using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xbim.FilterHelper;

namespace Xbim.Client
{
    public partial class RolesList : UserControl
    {

        public RoleFilter Roles
        {
            get
            {
                RoleFilter roles = RoleFilter.Unknown; //reset to unknown
                var checkedRoles = chkBoxList.CheckedItems;
                //set selected roles
                foreach (var item in checkedRoles)
                {
                    RoleFilter role = (RoleFilter)Enum.Parse(typeof(RoleFilter), (string)item);
                    roles |= role;

                }
                //we have selected roles so remove unknown
                if (checkedRoles.Count > 0)
                {
                    roles &= ~RoleFilter.Unknown;//remove unknown
                }
                return roles;
            }

            set
            {
                foreach (RoleFilter role in Enum.GetValues(typeof(RoleFilter)))
                {
                    var idx = chkBoxList.Items.IndexOf(role.ToString());
                    if (idx >= 0) //passes unknown on init
                    {
                        chkBoxList.SetItemChecked(idx, value.HasFlag(role));
                    }
                }
            }
        }

        public RolesList()
        {
            InitializeComponent();
            var roleList = Enum.GetNames(typeof(RoleFilter));
            chkBoxList.Items.AddRange(roleList.Where(r => r != RoleFilter.Unknown.ToString()).ToArray());
        }
       
    }
}
