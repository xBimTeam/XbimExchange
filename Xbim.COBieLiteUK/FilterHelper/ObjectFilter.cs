using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Xbim.Ifc4.Interfaces;

namespace Xbim.CobieLiteUk.FilterHelper
{
    /// <summary>
    /// Filter on object type names, used to filter Type and Component COBie Sheets
    /// </summary>
    public class ObjectFilter
    {
        #region Properties
        /// <summary>
        /// Keyed list with true or false values, true to include. false to exclude
        /// </summary>
        public SerializableDictionary<string, bool> Items { get; set; }

        /// <summary>
        /// keyed by IfcElement to element property PredefinedType to include list
        /// </summary>
        public SerializableDictionary<string, string[]> PreDefinedType { get; set; }

        /// <summary>
        /// Items to filter out
        /// </summary>
        private List<string> _itemsToExclude;
        private List<string> ItemsToExclude
        {
            get {
                return _itemsToExclude ??
                       (_itemsToExclude = Items.Where(e => e.Value == false).Select(e => e.Key).ToList());
            }
        }

        #endregion
         
        #region Constructors

        public ObjectFilter()
        {
            Items = new SerializableDictionary<string, bool>();
            PreDefinedType = new SerializableDictionary<string, string[]>();
            _itemsToExclude = null;
        }

        /// <summary>
        /// Set Property Filters constructor via ConfigurationSection from configuration file
        /// </summary>
        /// <param name="section">ConfigurationSection from configuration file</param>
        public ObjectFilter(ConfigurationSection section) : this()
        {
            if (section == null) return;

            foreach (KeyValueConfigurationElement keyVal in ((AppSettingsSection)section).Settings)
            {
                if (string.IsNullOrEmpty(keyVal.Key)) continue;
                var include = string.Compare(keyVal.Value, "YES", StringComparison.OrdinalIgnoreCase) == 0;
                Items.Add(keyVal.Key.ToUpper(), include);
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// see if object is empty of any values
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return ((Items.Count == 0) && (PreDefinedType.Count == 0));
        }

        /// <summary>
        /// add PreDefined types associated with ifcElements
        /// </summary>
        /// <param name="ifcElement">string name of ifcElement</param>
        /// <param name="definedTypes">array of strings for the ifcElement predefinedtype enum property </param>
        /// <returns></returns>
        public bool SetPreDefinedType(string ifcElement, string[] definedTypes)
        {
            ifcElement = ifcElement.ToUpper();
            try
            {
                if (PreDefinedType.ContainsKey(ifcElement))
                {
                    PreDefinedType[ifcElement] = definedTypes;
                    return true;
                }
                PreDefinedType.Add(ifcElement, definedTypes);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// fill pre defined types
        /// </summary>
        /// <param name="section"></param>
        public void FillPreDefinedTypes(ConfigurationSection section)
        {
            if (section == null) return;
            foreach (KeyValueConfigurationElement keyVal in ((AppSettingsSection)section).Settings)
            {
                if (string.IsNullOrEmpty(keyVal.Value)) continue;

                var values = keyVal.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(s => s.ToUpper()).ToArray();
                PreDefinedType.Add(keyVal.Key.ToUpper(), values);
            }
        }

        /// <summary>
        /// Test for string exists in ItemsToExclude string lists
        /// </summary>
        /// <param name="testStr">String to test</param>
        /// <param name="preDefinedType">strings for the ifcTypeObject predefinedtype enum property</param>
        /// <returns>bool, true = exclude</returns>
        public bool ItemsFilter(string testStr, string preDefinedType = null)
        {
            if (ItemsToExclude.Count == 0) return false; //nothing to test against
            
            testStr = testStr.ToUpper();
            //check for predefinedtype enum value passed as string
            if ((preDefinedType == null) || !PreDefinedType.ContainsKey(testStr))
                return ItemsToExclude.Contains(testStr);
            preDefinedType = preDefinedType.ToUpper();
            var excludeDefinedType = !PreDefinedType[testStr].Contains(preDefinedType);

            return ItemsToExclude.Contains(testStr) || excludeDefinedType;
        }


        //TODO: Check function below, see if it works!
        /// <summary>
        /// Test for IfcObjectDefinition exists in IfcToExclude type lists
        /// </summary>
        /// <param name="obj">IfcObjectDefinition object</param>
        /// <returns>bool, true = exclude</returns>
        public bool ItemsFilter(IIfcObjectDefinition obj)
        {
            if (ItemsToExclude.Count == 0) 
                return false; //nothing to test against

            var objType = obj.GetType();
            var objString = objType.Name.ToUpper(); //obj.ToString().ToUpper(); //or this might work, obj.IfcType().IfcTypeEnum.ToString();
            var result = ItemsToExclude.Contains(objString);

            if (result || !PreDefinedType.ContainsKey(objString)) return result;
            var objPreDefinedProp = objType.GetProperty("PredefinedType");

            if (objPreDefinedProp == null) return false;
            var objPreDefValue = objPreDefinedProp.GetValue(obj,null);

            if (objPreDefValue == null) return false;
            var preDefType = objPreDefValue.ToString();
            if (!string.IsNullOrEmpty(preDefType))
            {
                result = !PreDefinedType[objString].Contains(preDefType.ToUpper());
            }
            return result;
        }

        /// <summary>
        /// Merge together ObjectFilter
        /// </summary>
        /// <param name="mergeFilter">ObjectFilter to merge</param>
        public void MergeInc(ObjectFilter mergeFilter)
        {
            _itemsToExclude = null; //reset exclude

            //find all includes for the incoming merge ObjectFilter
            var mergeInc = mergeFilter.Items.Where(i => i.Value).ToDictionary(i => i.Key, v => v.Value);
            //set the true flag on 'this' Items with same key as incoming merges found above in mergeInc
            foreach (var pair in mergeInc)
            {
                Items[pair.Key] = pair.Value;
            }
            
            var mergeData = PreDefinedType.Concat(mergeFilter.PreDefinedType).GroupBy(v => v.Key).ToDictionary(k => k.Key, v => v.SelectMany(x => x.Value).Distinct().ToArray());
            //rebuild PreDefinedType from merge linq statement
            PreDefinedType.Clear();
            foreach (var item in mergeData)
            {
                PreDefinedType.Add(item.Key, item.Value);
            }

        }

        /// <summary>
        /// Copy values from passed ObjectFilter
        /// </summary>
        /// <param name="copyFilter">ObjectFilter to copy</param>
        public void Copy(ObjectFilter copyFilter)
        {
            _itemsToExclude = null; //reset exclude

            Items.Clear();
            //fill dictionary from passed argument  Items
            foreach (var pair in copyFilter.Items)
            {
                Items[pair.Key] = pair.Value;
            }

            PreDefinedType.Clear();
            foreach (var pair in copyFilter.PreDefinedType)
            {
                PreDefinedType[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// Clear ObjectFilter
        /// </summary>
        public void Clear()
        {
            _itemsToExclude = null; //reset exclude
            Items.Clear();
            PreDefinedType.Clear();
        }

        #endregion
    }
}
