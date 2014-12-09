using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Xbim.COBie.Client
{
    public partial class ClassFilter : Form
    {
        public FilterValues DefaultFilters { get; set; }
        public FilterValues UserFilters { get; set; }
        
        public ClassFilter(FilterValues userFilters)
        {
            InitializeComponent();

            DefaultFilters = new FilterValues(); //gives us the initial list of types
            UserFilters = userFilters; //hold the amendments, as required by the user
        }

        private void ClassFilter_Load(object sender, EventArgs e)
        {
            InitExcludes(chkListBoxComp, DefaultFilters.ObjectType.Component, UserFilters.ObjectType.Component);
            InitExcludes(chkListBoxType, DefaultFilters.ObjectType.Types, UserFilters.ObjectType.Types);
            InitExcludes(chkListBoxAss, DefaultFilters.ObjectType.Assembly, UserFilters.ObjectType.Assembly);
        }

        /// <summary>
        /// Sets up check list boxs 
        /// </summary>
        /// <param name="listbox"></param>
        /// <param name="defaultExcludeTypes"></param>
        /// <param name="userExcludeTypes"></param>
        private void InitExcludes(CheckedListBox listbox, List<Type> defaultExcludeTypes, List<Type> userExcludeTypes)
        {
            ((ListBox)listbox).DataSource = defaultExcludeTypes;
            ((ListBox)listbox).DisplayMember = "Name";

            for (int i = 0; i < listbox.Items.Count; i++)
            {
                Type typeObj = (Type)listbox.Items[i];
                if (userExcludeTypes.Contains(typeObj))
                    listbox.SetItemChecked(i, true);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SetExcludes(chkListBoxComp, UserFilters.ObjectType.Component);
            SetExcludes(chkListBoxType, UserFilters.ObjectType.Types);
            SetExcludes(chkListBoxAss, UserFilters.ObjectType.Assembly);
        }

        /// <summary>
        /// Save the excludes to the UserFilters
        /// </summary>
        /// <param name="listbox"></param>
        /// <param name="userExcludeTypes"></param>
        private void SetExcludes(CheckedListBox listbox, List<Type> userExcludeTypes)
        {
            for (int i = 0; i < listbox.Items.Count; i++)
            {
                Type typeObj = (Type)listbox.Items[i];

                if (listbox.GetItemChecked(i))
                {
                    if (!userExcludeTypes.Contains(typeObj))
                        userExcludeTypes.Add(typeObj);
                }
                else
                    userExcludeTypes.Remove(typeObj);
            }
        }
    }
}
