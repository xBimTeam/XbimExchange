using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xbim.COBieLiteUK.Client;

namespace Xbim.Client
{
    public partial class EditableList : UserControl
    {
        public EditableList()
        {
            InitializeComponent();
            editlistView.Columns[0].Width = editlistView.Width - 4;
            editlistView.Height = this.Height - 4;
            //editlistView.UpdatedListView += 
        }

        private void add_Click(object sender, EventArgs e)
        {
            ListViewItem item = editlistView.Items.Add(string.Empty);
            item.BeginEdit();
        }

        private void remove_Click(object sender, EventArgs e)
        {
            foreach (var item in editlistView.SelectedIndices)
            {
                editlistView.Items.RemoveAt((int)item);
            }
            
        }


        private void editlistView_Leave(object sender, EventArgs e)
        {
            //editlistView.UpdatedItems();
        }

        private void EditableList_Leave(object sender, EventArgs e)
        {
            editlistView.UpdatedItems();
             
        }

        private void EditableList_SizeChanged(object sender, EventArgs e)
        {
            editlistView.Width = this.Width - 30;
            editlistView.Height = this.Height ;
            add.Location = new Point(this.Width - 24, 3);
            remove.Location = new Point(this.Width - 24, 27);
        }
    }

    
}
