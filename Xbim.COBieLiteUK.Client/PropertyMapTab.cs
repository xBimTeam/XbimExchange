using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Xbim.FilterHelper;

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
            if (listBoxFieldNames.Items.Count > 0)
            {
                listBoxFieldNames.SelectedIndex = 0;
            }
            
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
            if (listBoxFieldNames.Items.Count > 0)
            {
                listBoxFieldNames.SelectedIndex = 0;
            }
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
                listViewMaps.SetEnabled();
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
            if (listBoxFieldNames.SelectedIndex >= 0)
            {
                PropPaths.RemoveAt(listBoxFieldNames.SelectedIndex);
                PropPaths.Insert(listBoxFieldNames.SelectedIndex, new AttributePaths(e.Key, string.Join(";", e.Items))); 
            }
         }
    }
}
