using System.Collections.Generic;
using System.Windows.Forms;
using Xbim.CobieLiteUk.FilterHelper;

namespace Xbim.CobieLiteUk.Client
{
    public partial class PropertySetFilters : UserControl
    {
        /// <summary>
        /// Equal To Property names to match
        /// </summary>
        public List<string> EqTo { get { return listViewEqTo.Items; } }
        /// <summary>
        /// Start With Property names to match
        /// </summary>
        public List<string> StartWith { get { return listViewStartWith.Items; } }
        /// <summary>
        /// Property names to Contain text
        /// </summary>
        public List<string> ContainsTxt { get { return listViewContains.Items; } }
        /// <summary>
        /// Property Set Names that are equal to
        /// </summary>
        public List<string> PSetEqTo { get { return listViewPSetEqTo.Items; } }

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
            else
            {
                listViewEqTo.SetEnabled();
                listViewStartWith.SetEnabled();
                listViewContains.SetEnabled();
                listViewPSetEqTo.SetEnabled();
            }
        }

    }
}
