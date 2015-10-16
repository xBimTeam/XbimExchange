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
        private OutPutFilters Filter = null;
        private PropertySetFilters _common, _zone, _type, _space, _floor, _facility, _spare, _component;
        public bool ReadOnly { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filter">OutPutFilters</param>
        public FilterTab(OutPutFilters filter, bool readOnly = false)
        {
            Filter = filter;
            ReadOnly = readOnly;
            InitializeComponent();
            listViewDefinedTypes.UpdatedListView += SetPreDefinedTypes;
            if (readOnly)
            {
                listViewDefinedTypes.SetReadOnly(); 
            }
            _common = (PropertySetFilters)tabPropertyCtr.TabPages["tabCommon"].Controls["pSetFiltersCommon"];
            _zone = (PropertySetFilters)tabPropertyCtr.TabPages["tabZone"].Controls["pSetFiltersZone"];
            _type = (PropertySetFilters)tabPropertyCtr.TabPages["tabType"].Controls["pSetFiltersType"];
            _space = (PropertySetFilters)tabPropertyCtr.TabPages["tabSpace"].Controls["pSetFiltersSpace"];
            _floor = (PropertySetFilters)tabPropertyCtr.TabPages["tabFloor"].Controls["pSetFiltersFloor"];
            _facility = (PropertySetFilters)tabPropertyCtr.TabPages["tabFacility"].Controls["pSetFiltersFacility"];
            _spare = (PropertySetFilters)tabPropertyCtr.TabPages["tabSpare"].Controls["pSetFiltersSpare"];
            _component = (PropertySetFilters)tabPropertyCtr.TabPages["tabComponent"].Controls["pSetFiltersComponent"];

            Init(Filter);

            if (readOnly)
            {
                chkListBoxComp.ItemCheck += new ItemCheckEventHandler(this.chkList_OnItemCheck);
                chkListBoxType.ItemCheck += new ItemCheckEventHandler(this.chkList_OnItemCheck);
                chkListBoxAss.ItemCheck += new ItemCheckEventHandler(this.chkList_OnItemCheck); 
            }
        }


        /// <summary>
        /// Set up control items
        /// </summary>
        /// <param name="filter">OutPutFilters</param>
        private void Init (OutPutFilters filter)
        {
            SetUpPropertNameLists(_common, filter.CommonFilter);
            SetUpPropertNameLists(_zone, filter.ZoneFilter);
            SetUpPropertNameLists(_type, filter.TypeFilter);
            SetUpPropertNameLists(_space, filter.SpaceFilter);
            SetUpPropertNameLists(_floor, filter.FloorFilter);
            SetUpPropertNameLists(_facility, filter.FacilityFilter);
            SetUpPropertNameLists(_spare, filter.SpareFilter);
            SetUpPropertNameLists(_component, filter.ComponentFilter);

            SetUpObjectsList(chkListBoxComp, filter.IfcProductFilter.Items);
            SetUpObjectsList(chkListBoxType, filter.IfcTypeObjectFilter.Items);
            SetUpObjectsList(chkListBoxAss, filter.IfcAssemblyFilter.Items);
        }

        /// <summary>
        /// Set Up Object Lists
        /// </summary>
        /// <param name="listbox">CheckedListBox</param>
        /// <param name="objs">Dictionary of string, bool></param>
        private void SetUpObjectsList(CheckedListBox listbox, Dictionary<string, bool> objs)
        {
            
            ((ListBox)listbox).DataSource = objs.Keys.ToList();
            
            for (int i = 0; i < listbox.Items.Count; i++)
            {
                string value = (string)listbox.Items[i];

                listbox.SetItemChecked(i, objs[value]);

            }
           
        }

        /// <summary>
        /// Reset to the default resource config file
        /// </summary>
        private void SetDefault()
        {
            var name = this.Parent.Name;
            RoleFilter role;

            if (Enum.TryParse(name, out role))
            {
                var defaultFilter  = OutPutFilters.GetDefaults(role);
                if (defaultFilter != null)
                {
                    Filter.Copy(defaultFilter);
                    Init(Filter);
                }
            }


        }

        /// <summary>
        /// Set Up Property Name Exclusions
        /// </summary>
        /// <param name="pSetFilter">PropertySetFilters</param>
        /// <param name="filter">PropertyFilter</param>
        private void SetUpPropertNameLists(PropertySetFilters pSetFilter, PropertyFilter filter)
        {
            pSetFilter.FillLists(filter, ReadOnly);
        }

        /// <summary>
        /// Change Index action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkListBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewDefinedTypes.SetEnabled();
            var selitem = chkListBoxType.Text;
            listViewDefinedTypes.Key = selitem;
            if (!string.IsNullOrEmpty(selitem) && (Filter.IfcTypeObjectFilter.PreDefinedType.Keys.Contains(selitem)))
            {
                listViewDefinedTypes.Items = Filter.IfcTypeObjectFilter.PreDefinedType[selitem].ToList();
                

            }
            else
            {
                listViewDefinedTypes.Clear();
            }
            
        }

        /// <summary>
        /// Catch item check event, if read only stop check by reassigning back to original state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkList_OnItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ReadOnly) e.NewValue = e.CurrentValue;
        }

        /// <summary>
        /// PreDefinedType Action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Save lists back to Filter Object
        /// </summary>
        /// <returns></returns>
        public OutPutFilters Save()
        {
            SaveObjectsLists();
            SavePropertyLists();
            return Filter;

        }

        /// <summary>
        /// Save Object Lists
        /// </summary>
        private void SaveObjectsLists()
        {
            //Note SetPreDefinedType save exit from list box as changes on type selection
            foreach (CheckedListBox chkBoxList in new List<CheckedListBox>(){chkListBoxComp, chkListBoxType, chkListBoxAss })
            {
                for (int i = 0; i < chkBoxList.Items.Count; i++)
                {
                    switch (chkBoxList.Name)
                    {
                        case "chkListBoxComp":
                            Filter.IfcProductFilter.Items[(string)chkBoxList.Items[i]] = chkBoxList.GetItemChecked(i);
                            break;
                        case "chkListBoxType":
                            Filter.IfcTypeObjectFilter.Items[(string)chkBoxList.Items[i]] = chkBoxList.GetItemChecked(i);
                            break;
                        case "chkListBoxAss":
                            Filter.IfcAssemblyFilter.Items[(string)chkBoxList.Items[i]] = chkBoxList.GetItemChecked(i);
                            break;
                        default:
                            break;
                    }
                   
                }
            }
        }


        /// <summary>
        /// Save property name exclusions back to filter object
        /// </summary>
        private void SavePropertyLists()
        {
            //Common
            Filter.CommonFilter.EqualTo = _common.EqTo;
            Filter.CommonFilter.StartWith = _common.StartWith;
            Filter.CommonFilter.Contain = _common.ContainsTxt;
            Filter.CommonFilter.PropertySetsEqualTo = _common.PSetEqTo;
            //Zone
            Filter.ZoneFilter.EqualTo = _zone.EqTo;
            Filter.ZoneFilter.StartWith = _zone.StartWith;
            Filter.ZoneFilter.Contain = _zone.ContainsTxt;
            Filter.ZoneFilter.PropertySetsEqualTo = _zone.PSetEqTo;
            //Type
            Filter.TypeFilter.EqualTo = _type.EqTo;
            Filter.TypeFilter.StartWith = _type.StartWith;
            Filter.TypeFilter.Contain = _type.ContainsTxt;
            Filter.TypeFilter.PropertySetsEqualTo = _type.PSetEqTo;
            //Space
            Filter.SpaceFilter.EqualTo = _space.EqTo;
            Filter.SpaceFilter.StartWith = _space.StartWith;
            Filter.SpaceFilter.Contain = _space.ContainsTxt;
            Filter.SpaceFilter.PropertySetsEqualTo = _space.PSetEqTo;
            //Floor
            Filter.FloorFilter.EqualTo = _floor.EqTo;
            Filter.FloorFilter.StartWith = _floor.StartWith;
            Filter.FloorFilter.Contain = _floor.ContainsTxt;
            Filter.FloorFilter.PropertySetsEqualTo = _floor.PSetEqTo;
            //Facility
            Filter.FacilityFilter.EqualTo = _facility.EqTo;
            Filter.FacilityFilter.StartWith = _facility.StartWith;
            Filter.FacilityFilter.Contain = _facility.ContainsTxt;
            Filter.FacilityFilter.PropertySetsEqualTo = _facility.PSetEqTo;
            //Spare
            Filter.SpareFilter.EqualTo = _spare.EqTo;
            Filter.SpareFilter.StartWith = _spare.StartWith;
            Filter.SpareFilter.Contain = _spare.ContainsTxt;
            Filter.SpareFilter.PropertySetsEqualTo = _spare.PSetEqTo;
            //Component
            Filter.ComponentFilter.EqualTo = _component.EqTo;
            Filter.ComponentFilter.StartWith = _component.StartWith;
            Filter.ComponentFilter.Contain = _component.ContainsTxt;
            Filter.ComponentFilter.PropertySetsEqualTo = _component.PSetEqTo;
        }

        /// <summary>
        /// Set tab back to defaults held in resource config file for the tabs assigned role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetDefaults_Click(object sender, EventArgs e)
        {
            SetDefault();
        }

        /// <summary>
        /// Right click menu event to clear all ticks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetAllTicks(sender, false);
        }
        
        /// <summary>
        /// Right click menu event to tick all items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fuillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetAllTicks(sender, true);
        }

        /// <summary>
        /// Set or remove all ticks from checked list box's
        /// </summary>
        /// <param name="sender">ToolStripItem</param>
        /// <param name="isChecked">true = all ticked, false = all not ticked</param>
        private static void SetAllTicks(object sender, bool isChecked)
        {
            var menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                var owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    var listbox = owner.SourceControl as CheckedListBox;
                    if (listbox != null)
                    {
                        for (int i = 0; i < listbox.Items.Count; i++)
                        {
                            string value = (string)listbox.Items[i];

                            listbox.SetItemChecked(i, isChecked);
                        }
                    }
                }
            }
        }

        

        
    }
}
