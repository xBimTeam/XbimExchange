using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xbim.FilterHelper;
using System.Diagnostics;

namespace Xbim.Client
{
    public partial class FilterTab : UserControl
    {
        OutPutFilters Filter = null;
        EditListView _listview, _eqTo, _startWidth, _containsText, _pSetEqTo;
        
        public FilterTab(OutPutFilters filter)
        {
            Filter = filter;
            InitializeComponent();
            _listview = (EditListView)listViewDefinedTypes.Controls.Find("editlistView", false).FirstOrDefault();
            _listview.UpdatedListView += SetPreDefinedTypes;
            _eqTo = (EditListView)listViewEqTo.Controls.Find("editlistView", false).FirstOrDefault();

            _startWidth = (EditListView)listViewStartWith.Controls.Find("editlistView", false).FirstOrDefault();

            _containsText = (EditListView)listViewContains.Controls.Find("editlistView", false).FirstOrDefault();

            _pSetEqTo = (EditListView)listViewPSet.Controls.Find("editlistView", false).FirstOrDefault();

            SetUpObjectsList(chkListBoxComp, filter.IfcProductFilter.Items);
            SetUpObjectsList(chkListBoxType, filter.IfcTypeObjectFilter.Items);
            SetUpObjectsList(chkListBoxAss, filter.IfcAssemblyFilter.Items);
            SetUpPropertyNameLists();
            
        }

        private void SetUpObjectsList(CheckedListBox listbox, Dictionary<string, bool> objs)
        {
            ((ListBox)listbox).DataSource = objs.Keys.ToList();
            
            for (int i = 0; i < listbox.Items.Count; i++)
            {
                string value = (string)listbox.Items[i];

                if (objs[value]) listbox.SetItemChecked(i, true);

            }
        }

        private void SetUpPropertyNameLists()
        {
            foreach (var item in Filter.ZoneFilter.EqualTo)
            {
                _eqTo.Items.Add(item);
            }
            
        }

        private void chkListBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selitem = chkListBoxType.Text; 
            _listview.Key = selitem;
            _listview.Items.Clear();
            if (!string.IsNullOrEmpty(selitem) && (Filter.IfcTypeObjectFilter.PreDefinedType.Keys.Contains(selitem)))
            {
                var value = Filter.IfcTypeObjectFilter.PreDefinedType[selitem];
                foreach (var item in value)
                {
                    _listview.Items.Add(item);
                }
                
            }
            
        }

        private void SetPreDefinedTypes(object sender, ListUpdateEventArgs e)
        {
           
#if DEBUG
            Debug.WriteLine(string.Format("Type name = {0}", e.Key));
            foreach (var item in e.Items)
            {
                Debug.Write(string.Format(" : {0} ", item));
            }
#endif
            Filter.IfcTypeObjectFilter.SetPreDefinedType(e.Key, e.Items.ConvertAll(x => x.ToUpper()).ToArray());

        }

        private void listBoxDefinedTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //foreach (ListViewItem item in listViewDefinedTypes.SelectedItems)
            //{
            //    item.BeginEdit();
            //}
            
            //((ListViewItem)listBoxDefinedTypes.Items[listBoxDefinedTypes.SelectedIndex]).BeginEdit();
            //((ListViewItem)listViewDefinedTypes.sel).BeginEdit();
        }

        
    }
}
