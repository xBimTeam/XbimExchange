using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Xbim.Client
{
    public partial class EditableList : UserControl
    {
        public bool AllUpper { get; set; }

        /// <summary>
        /// Get And Set displayed strings in list
        /// </summary>
        public List<string> Items 
        {
            get
            {
                List<string> strItems = new List<string>();
                foreach (ListViewItem item in editlistView.Items)
                {
                    if (!string.IsNullOrEmpty(item.Text))
                    {
                        if (AllUpper)
                        {
                            strItems.Add(item.Text.ToUpper());
                        }
                        else
                        {
                            strItems.Add(item.Text);
                        }
                    }

                }
                return strItems;
            }
            set
            {
                editlistView.Items.Clear();
                foreach (var item in value)
                {
                    editlistView.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Associated key for the ListView data
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Event to handle passing of the updated data in the list
        /// </summary>
        public event EventHandler<ListUpdateEventArgs> UpdatedListView;

        
        /// <summary>
        /// Constructor
        /// </summary>
        public EditableList()
        {
            InitializeComponent();
            editlistView.Columns[0].Width = editlistView.Width - 4;
            editlistView.Height = this.Height - 4;
            AllUpper = true;
            
        }

        /// <summary>
        /// Clear list
        /// </summary>
        public void Clear()
        {
            editlistView.Items.Clear();
        }

        /// <summary>
        /// Add Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_Click(object sender, EventArgs e)
        {
            ListViewItem item = editlistView.Items.Add(string.Empty);
            item.BeginEdit();
        }

        /// <summary>
        /// Remove Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remove_Click(object sender, EventArgs e)
        {
            foreach (var item in editlistView.SelectedIndices)
            {
                editlistView.Items.RemoveAt((int)item);
            }
            
        }
        
        /// <summary>
        /// Leave Event, passes current list to EventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditableList_Leave(object sender, EventArgs e)
        {
            //get current held items and pass to EventArgs
            ListUpdateEventArgs evArg = new ListUpdateEventArgs(Key, Items);
            //fire this event
            if (UpdatedListView != null)
            {
                UpdatedListView(this, evArg);
            }
             
        }

        public void SetReadOnly()
        {
            add.Enabled = false;
            remove.Enabled = false;
            editlistView.LabelEdit = false;
        }

        /// <summary>
        /// Controls sizing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditableList_SizeChanged(object sender, EventArgs e)
        {
            editlistView.Width = this.Width - 30;
            editlistView.Height = this.Height ;
            editlistView.Columns[0].Width = editlistView.Width;
            add.Location = new Point(this.Width - 24, 3);
            remove.Location = new Point(this.Width - 24, 27);
        }
    }

    /// <summary>
    /// Event Args Class to pass list view data
    /// </summary>
    public class ListUpdateEventArgs : EventArgs
    {
        public List<string> Items { get; private set; }
        public string Key { get; private set; }
        public ListUpdateEventArgs(string key, List<string> items)
        {
            Key = key;
            Items = items;
        }
    }

    
}
