using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xbim.Client
{
    public class EditListView : ListView
    {
        public event EventHandler<ListUpdateEventArgs> UpdatedListView;
        public string Key { get; set; }

        public void UpdatedItems()
        {
            List<string> strItems = new List<string>();
            foreach (ListViewItem item in this.Items)
            {
                strItems.Add(item.Text);
            }
            ListUpdateEventArgs evArg = new ListUpdateEventArgs(Key, strItems);
            if (UpdatedListView != null)
            {
                UpdatedListView(this, evArg);
            }
            
        }

    }

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
