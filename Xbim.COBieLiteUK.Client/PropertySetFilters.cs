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

namespace Xbim.Client
{
    public partial class PropertySetFilters : UserControl
    {
        /// <summary>
        /// Equal To Property names to match
        /// </summary>
        public List<string> EqTo { get { return this.listViewEqTo.Items; } }
        /// <summary>
        /// Start With Property names to match
        /// </summary>
        public List<string> StartWith { get { return this.listViewStartWith.Items; } }
        /// <summary>
        /// Property names to Contain text
        /// </summary>
        public List<string> ContainsTxt { get { return this.listViewContains.Items; } }
        /// <summary>
        /// Property Set Names that are equal to
        /// </summary>
        public List<string> PSetEqTo { get { return this.listViewPSetEqTo.Items; } }

        public PropertySetFilters()
        {
            InitializeComponent();
        }

        public void FillLists(PropertyFilter psetFilter, bool readOnly = false)
        {
            //clear lists
            listViewEqTo.Items.Clear();
            listViewStartWith.Items.Clear();
            listViewContains.Items.Clear();
            listViewPSetEqTo.Items.Clear();

            //fill lists
            listViewEqTo.Items = psetFilter.EqualTo;
            listViewStartWith.Items = psetFilter.StartWith;
            listViewContains.Items = psetFilter.Contain;
            listViewPSetEqTo.Items = psetFilter.PropertySetsEqualTo;
            //set read only
            if (readOnly)
            {
                listViewEqTo.SetReadOnly();
                listViewStartWith.SetReadOnly();
                listViewContains.SetReadOnly();
                listViewPSetEqTo.SetReadOnly();
            }
        }

    }
}
