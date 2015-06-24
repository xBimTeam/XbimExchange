using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieLiteUK.FilterHelper
{
    /// <summary>
    /// Filter on object type names, used to filter Type and Component COBie Sheets
    /// </summary>
    public class ObjectFilter
    {
        /// <summary>
        /// IfcProducts to filter out
        /// </summary>
        public List<string> ElementsToExclude { get;  set; }

        /// <summary>
        /// IfcTypeObjects to filter out
        /// </summary>
        public List<string> TypesToExclude { get; set; }

        /// <summary>
        /// keyed by IfcElement to element property PredefinedType
        /// </summary>
        public SerializableDictionary<string, string[]> PreDefinedType { get; set; }

        

        public ObjectFilter()
        {
            ElementsToExclude = new List<string>();
            TypesToExclude = new List<string>();
            PreDefinedType = new SerializableDictionary<string, string[]>();
            //PreDefinedType.Add("TEST", new string[] { "ONE", "TWO" });
        }
        /// <summary>
        /// Set Object Filters constructor
        /// </summary>
        /// <param name="elementsToExclude">';' delimited string for IfcProducts to exclude from components(Assets)</param>
        /// <param name="typesToExclude">';' delimited string for IfcTypeObjects to exclude from Types</param>
        public ObjectFilter(string elementsToExclude, string typesToExclude) : this()
        {
            //IfcProducts and IfcTypeObjects to exclude
            if (!string.IsNullOrEmpty(elementsToExclude))
            {
                ElementsToExclude.AddRange(elementsToExclude.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(s => s.ToUpper()));
            }
            if (!string.IsNullOrEmpty(typesToExclude))
            {
                TypesToExclude.AddRange(typesToExclude.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(s => s.ToUpper()));
            }
        }

        /// <summary>
        /// Set Property Filters constructor via ConfigurationSection from configuration file
        /// </summary>
        /// <param name="section">ConfigurationSection from configuration file</param>
        public ObjectFilter(ConfigurationSection section) : this()
        {
            //initialize fields
            switch (section.SectionInformation.Name.ToUpper())
            {
                case "IFCELEMENTINCLUSION":
                    SetElements(section);
                    break;
                case "IFCTYPEINCLUSION":
                    SetTypes(section);
                    break;
                default:
#if DEBUG
                    Debug.WriteLine(string.Format("Invalid Key - {0}", section.SectionInformation.Name.ToUpper()));
#endif
                    break;
            }
        }

        /// <summary>
        /// Set up IfcProducts To exclude
        /// </summary>
        /// <param name="section">AppSettingsSection from configuration file</param>
        private void SetElements(ConfigurationSection section)
        {
            if (section != null)
            {
                foreach (KeyValueConfigurationElement keyVal in ((AppSettingsSection)section).Settings)
                {
                    if (!string.IsNullOrEmpty(keyVal.Key))
                    {
                        if (String.Compare(keyVal.Value, "NO", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            ElementsToExclude.Add(keyVal.Key.ToUpper());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set up IfcTypeObjects To exclude
        /// </summary>
        /// <param name="section">AppSettingsSection from configuration file</param>
        private void SetTypes(ConfigurationSection section)
        {
            if (section != null)
            {
                foreach (KeyValueConfigurationElement keyVal in ((AppSettingsSection)section).Settings)
                {
                    if (!string.IsNullOrEmpty(keyVal.Key))
                    {
                        if (String.Compare(keyVal.Value, "NO", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            TypesToExclude.Add(keyVal.Key.ToUpper());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// add PreDefined types associated with ifcelements
        /// </summary>
        /// <param name="ifcElement">string name of ifcElement</param>
        /// <param name="definedTypes">array of strings for the ifcElement predefinedtype enum property </param>
        /// <returns></returns>
        public bool AddPreDefinedType(string ifcElement, string[] definedTypes)
        {
            if (PreDefinedType.ContainsKey(ifcElement))
            { 
                return false;
            }
            else
            {
                PreDefinedType.Add(ifcElement, definedTypes);
            }
            return true;
        }
        
        /// <summary>
        /// Test for string exists in ElementsToExclude string lists
        /// </summary>
        /// <param name="testStr">String to test</param>
        /// <returns>bool</returns>
        public bool ElementsFilter(string testStr, string preDefinedType = null)
        {
            testStr = testStr.ToUpper();
            //check for predefinedtype enum value passed as string
            bool hasDefinedType = true; //if preDefinedType is null or testStr does not exist in PredefinedType dictionary we need to just test on testStr in return so set to true as default
            if ((preDefinedType != null) &&
                PreDefinedType.ContainsKey(testStr)
                )
            {
                preDefinedType = preDefinedType.ToUpper();
                hasDefinedType = PreDefinedType[testStr].Contains(preDefinedType);
            }

            return (hasDefinedType && (ElementsToExclude.Where(a => testStr.Equals(a)).Count() > 0));
        }

        /// <summary>
        /// Test for string exists in TypesToExclude string lists
        /// </summary>
        /// <param name="testStr">String to test</param>
        /// <returns>bool</returns>
        public bool TypeObjFilter(string testStr)
        {
            testStr = testStr.ToUpper();
            return (TypesToExclude.Where(a => testStr.Equals(a)).Count() > 0);
        }

        /// <summary>
        /// Merge together properties
        /// </summary>
        /// <param name="mergeFilter">ObjectFilter to merge</param>
        public void Merge(ObjectFilter mergeFilter)
        {
            var mergeElements = mergeFilter.ElementsToExclude.Where(p => !this.ElementsToExclude.Contains(p));
            this.ElementsToExclude.AddRange(mergeElements);

            var mergeTypes = mergeFilter.TypesToExclude.Where(p => !this.TypesToExclude.Contains(p));
            this.ElementsToExclude.AddRange(mergeTypes);

           // var mergePreDefined = mergeFilter.PreDefinedType.Where(p => !this.PreDefinedType.ContainsKey(p.Key));

            var mergeData = this.PreDefinedType.Concat(mergeFilter.PreDefinedType).GroupBy(v => v.Key).ToDictionary(k => k.Key, v => v.SelectMany(x => x.Value).Distinct().ToArray());
            this.PreDefinedType.Clear();
            foreach (var item in mergeData)
            {
                this.PreDefinedType.Add(item.Key, item.Value);
            }
            

        }

        
    }
}
