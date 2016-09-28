using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Xbim.CobieLiteUk;
using Xbim.CobieLiteUk.FilterHelper;

namespace Xbim.CobieLiteUk.Client
{
    public partial class FilterTab : UserControl
    {
        private readonly OutPutFilters _filter;
        private readonly PropertySetFilters _common, _zone, _type, _space, _floor, _facility, _spare, _component;
        public bool ReadOnly { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filter">OutPutFilters</param>
        /// <param name="readOnly"></param>
        public FilterTab(OutPutFilters filter, bool readOnly = false)
        {
            _filter = filter;
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

            Init(_filter);

            if (!readOnly) 
                return;
            chkListBoxComp.ItemCheck += chkList_OnItemCheck;
            chkListBoxType.ItemCheck += chkList_OnItemCheck;
            chkListBoxAss.ItemCheck += chkList_OnItemCheck;
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
        private static void SetUpObjectsList(CheckedListBox listbox, Dictionary<string, bool> objs)
        {
            ((ListBox)listbox).DataSource = objs.Keys.ToList();   
            for (var i = 0; i < listbox.Items.Count; i++)
            {
                var value = (string)listbox.Items[i];
                listbox.SetItemChecked(i, objs[value]);
            }           
        }

        /// <summary>
        /// Reset to the default resource config file
        /// </summary>
        private void SetDefault()
        {
            var name = Parent.Name;
            RoleFilter role;

            if (!Enum.TryParse(name, out role)) 
                return;
            var defaultFilter  = OutPutFilters.GetDefaults(role);
            if (defaultFilter == null) 
                return;
            _filter.Copy(defaultFilter);
            Init(_filter);
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
            if (!string.IsNullOrEmpty(selitem) && (_filter.IfcTypeObjectFilter.PreDefinedType.Keys.Contains(selitem)))
            {
                listViewDefinedTypes.Items = _filter.IfcTypeObjectFilter.PreDefinedType[selitem].ToList();
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
            _filter.IfcTypeObjectFilter.SetPreDefinedType(e.Key, e.Items.ConvertAll(x => x.ToUpper()).ToArray());

        }

        /// <summary>
        /// Save lists back to Filter Object
        /// </summary>
        /// <returns></returns>
        public OutPutFilters Save()
        {
            SaveObjectsLists();
            SavePropertyLists();
            return _filter;
        }

        /// <summary>
        /// Save Object Lists
        /// </summary>
        private void SaveObjectsLists()
        {
            //Note SetPreDefinedType save exit from list box as changes on type selection
            foreach (var chkBoxList in new List<CheckedListBox>() {chkListBoxComp, chkListBoxType, chkListBoxAss})
            {
                for (var i = 0; i < chkBoxList.Items.Count; i++)
                {
                    switch (chkBoxList.Name)
                    {
                        case "chkListBoxComp":
                            _filter.IfcProductFilter.Items[(string) chkBoxList.Items[i]] = chkBoxList.GetItemChecked(i);
                            break;
                        case "chkListBoxType":
                            _filter.IfcTypeObjectFilter.Items[(string) chkBoxList.Items[i]] = chkBoxList.GetItemChecked(i);
                            break;
                        case "chkListBoxAss":
                            _filter.IfcAssemblyFilter.Items[(string) chkBoxList.Items[i]] = chkBoxList.GetItemChecked(i);
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
            _filter.CommonFilter.EqualTo = _common.EqTo;
            _filter.CommonFilter.StartWith = _common.StartWith;
            _filter.CommonFilter.Contain = _common.ContainsTxt;
            _filter.CommonFilter.PropertySetsEqualTo = _common.PSetEqTo;
            //Zone
            _filter.ZoneFilter.EqualTo = _zone.EqTo;
            _filter.ZoneFilter.StartWith = _zone.StartWith;
            _filter.ZoneFilter.Contain = _zone.ContainsTxt;
            _filter.ZoneFilter.PropertySetsEqualTo = _zone.PSetEqTo;
            //Type
            _filter.TypeFilter.EqualTo = _type.EqTo;
            _filter.TypeFilter.StartWith = _type.StartWith;
            _filter.TypeFilter.Contain = _type.ContainsTxt;
            _filter.TypeFilter.PropertySetsEqualTo = _type.PSetEqTo;
            //Space
            _filter.SpaceFilter.EqualTo = _space.EqTo;
            _filter.SpaceFilter.StartWith = _space.StartWith;
            _filter.SpaceFilter.Contain = _space.ContainsTxt;
            _filter.SpaceFilter.PropertySetsEqualTo = _space.PSetEqTo;
            //Floor
            _filter.FloorFilter.EqualTo = _floor.EqTo;
            _filter.FloorFilter.StartWith = _floor.StartWith;
            _filter.FloorFilter.Contain = _floor.ContainsTxt;
            _filter.FloorFilter.PropertySetsEqualTo = _floor.PSetEqTo;
            //Facility
            _filter.FacilityFilter.EqualTo = _facility.EqTo;
            _filter.FacilityFilter.StartWith = _facility.StartWith;
            _filter.FacilityFilter.Contain = _facility.ContainsTxt;
            _filter.FacilityFilter.PropertySetsEqualTo = _facility.PSetEqTo;
            //Spare
            _filter.SpareFilter.EqualTo = _spare.EqTo;
            _filter.SpareFilter.StartWith = _spare.StartWith;
            _filter.SpareFilter.Contain = _spare.ContainsTxt;
            _filter.SpareFilter.PropertySetsEqualTo = _spare.PSetEqTo;
            //Component
            _filter.ComponentFilter.EqualTo = _component.EqTo;
            _filter.ComponentFilter.StartWith = _component.StartWith;
            _filter.ComponentFilter.Contain = _component.ContainsTxt;
            _filter.ComponentFilter.PropertySetsEqualTo = _component.PSetEqTo;
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
            if (menuItem == null) 
                return;
            var owner = menuItem.Owner as ContextMenuStrip;
            if (owner == null) 
                return;
            var listbox = owner.SourceControl as CheckedListBox;
            if (listbox == null) 
                return;
            for (var i = 0; i < listbox.Items.Count; i++)
            {
                listbox.SetItemChecked(i, isChecked);
            }
        }
    }
}
