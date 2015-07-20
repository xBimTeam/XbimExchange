using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc2x3.Kernel;
using Xbim.IO;

namespace Xbim.FilterHelper
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
        private List<string> _itemsToExclude = null;
        private List<string> ItemsToExclude
        {
            get
            {
                return _itemsToExclude != null ? _itemsToExclude : Items.Where(e => e.Value == false).Select(e => e.Key).ToList();
            }
        }

        #endregion
         
        #region Constructors

        public ObjectFilter()
        {
            Items = new SerializableDictionary<string, bool>();
            PreDefinedType = new SerializableDictionary<string, string[]>();
        }

        /// <summary>
        /// Set Property Filters constructor via ConfigurationSection from configuration file
        /// </summary>
        /// <param name="section">ConfigurationSection from configuration file</param>
        public ObjectFilter(ConfigurationSection section) : this()
        {
            if (section != null)
            {
                foreach (KeyValueConfigurationElement keyVal in ((AppSettingsSection)section).Settings)
                {
                    if (!string.IsNullOrEmpty(keyVal.Key))
                    {
                        bool include = false;
                        if (String.Compare(keyVal.Value, "YES", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            include = true;
                        }
                        Items.Add(keyVal.Key.ToUpper(), include);
                    }
                }
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
                else
                {
                    PreDefinedType.Add(ifcElement, definedTypes);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void FillPreDefinedTypes(ConfigurationSection section)
        {
            if (section != null)
            {
                foreach (KeyValueConfigurationElement keyVal in ((AppSettingsSection)section).Settings)
                {
                    if (!string.IsNullOrEmpty(keyVal.Value))
                    {
                        var values = keyVal.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(s => s.ToUpper()).ToArray();
                        PreDefinedType.Add(keyVal.Key.ToUpper(), values);
                    }
                }
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
            bool ExcludeDefinedType = false; //if preDefinedType is null or preDefinedType does not exist in PredefinedType dictionary 
            if ((preDefinedType != null) &&
                PreDefinedType.ContainsKey(testStr)
                )
            {
                preDefinedType = preDefinedType.ToUpper();
                ExcludeDefinedType = !PreDefinedType[testStr].Contains(preDefinedType);
            }

            return (ItemsToExclude.Contains(testStr) || ExcludeDefinedType);
        }


        //TODO: Check function below, see if it works!
        /// <summary>
        /// Test for IfcObjectDefinition exists in IfcToExclude type lists
        /// </summary>
        /// <param name="obj">IfcObjectDefinition object</param>
        /// <returns>bool, true = exclude</returns>
        public bool ItemsFilter(IfcObjectDefinition obj)
        {
            if (ItemsToExclude.Count == 0) return false; //nothing to test against

            var objType = obj.GetType();
            var objString = objType.Name.ToUpper(); //obj.ToString().ToUpper(); //or this might work, obj.IfcType().IfcTypeEnum.ToString();
            bool result = ItemsToExclude.Contains(objString);
            
            if (!result && (PreDefinedType.ContainsKey(objString)))
            {
                var objPreDefinedProp = objType.GetProperty("PredefinedType");
            
                if (objPreDefinedProp != null)
                {

                    var objPreDefValue = objPreDefinedProp.GetValue(obj,null).ToString();

                    result = !PreDefinedType[objString].Contains(objPreDefValue);
                }
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
            var mergeInc = mergeFilter.Items.Where(i => i.Value == true).ToDictionary(i => i.Key, v => v.Value);
            //set the true flag on 'this' Items with same key as incoming merges found above in mergeInc
            foreach (var pair in mergeInc)
            {
                Items[pair.Key] = pair.Value;
            }
            
            var mergeData = this.PreDefinedType.Concat(mergeFilter.PreDefinedType).GroupBy(v => v.Key).ToDictionary(k => k.Key, v => v.SelectMany(x => x.Value).Distinct().ToArray());
            //rebuild PreDefinedType from merge linq statement
            this.PreDefinedType.Clear();
            foreach (var item in mergeData)
            {
                this.PreDefinedType.Add(item.Key, item.Value);
            }

        }

        /// <summary>
        /// Copy values from passed ObjectFilter
        /// </summary>
        /// <param name="copyFilter">ObjectFilter to copy</param>
        public void Copy(ObjectFilter copyFilter)
        {
            _itemsToExclude = null; //reset exclude

            this.Items.Clear();
            //fill dictionary from passed argument  Items
            foreach (var pair in copyFilter.Items)
            {
                this.Items[pair.Key] = pair.Value;
            }

            this.PreDefinedType.Clear();
            foreach (var pair in copyFilter.PreDefinedType)
            {
                this.PreDefinedType[pair.Key] = pair.Value;
            }
        }

        #endregion


    }
}
