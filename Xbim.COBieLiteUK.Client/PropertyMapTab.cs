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
    public partial class PropertyMapTab : UserControl
    {
        public List<AttributePaths> PropPaths { get; set; }

        public PropertyMapTab(List<AttributePaths> paths)
        {
            InitializeComponent();
            //hook in to item value change event
            listViewMaps.UpdatedListView += SetPropPaths;
            listViewMaps.AllUpper = false; //force return lists to be as typed, not forced to uppercase
            PropPaths = paths;
            SetUp();
        }

        /// <summary>
        /// Reset up controls
        /// </summary>
        /// <param name="paths">List of AttributePaths</param>
        public void ReSet(List<AttributePaths> paths)
        {
            PropPaths = paths;
            listBoxFieldNames.Items.Clear();
            listViewMaps.Clear();
            SetUp();

        }

        /// <summary>
        /// Initial setup
        /// </summary>
         private void SetUp()
        {
            foreach (AttributePaths item in PropPaths)
            {
                listBoxFieldNames.Items.Add(item.Key);
            }
            
        }

        /// <summary>
        /// Set map list upon Field name click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         private void listBoxFieldNames_SelectedIndexChanged(object sender, EventArgs e)
         {
             var selitem = listBoxFieldNames.Text;
             listViewMaps.Key = selitem;
             if (!string.IsNullOrEmpty(selitem))
             {
                 listViewMaps.Items = PropPaths.Where(p => p.Key == selitem).First().PSetPaths.ToList();
             }
                
         }

         /// <summary>
         /// save back any changes to mappings
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void SetPropPaths(object sender, ListUpdateEventArgs e)
         {

#if DEBUG
             Debug.WriteLine(string.Format("Type name = {0}", e.Key));
             foreach (var item in e.Items)
             {
                 Debug.Write(string.Format(" : {0} ", item));
             }
#endif
             PropPaths.RemoveAt(listBoxFieldNames.SelectedIndex);
             PropPaths.Insert(listBoxFieldNames.SelectedIndex, new AttributePaths(e.Key, string.Join(";", e.Items)));
         }
    }
}
