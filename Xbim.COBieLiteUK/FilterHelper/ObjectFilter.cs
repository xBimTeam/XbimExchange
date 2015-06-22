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
        public List<string> ElementsToExclude { get; private set; }

        /// <summary>
        /// IfcTypeObjects to filter out
        /// </summary>
        public List<string> TypesToExclude { get; private set; }

        private void Init()
        {
            ElementsToExclude = new List<string>();
            TypesToExclude = new List<string>();
        }
        /// <summary>
        /// Set Object Filters constructor
        /// </summary>
        /// <param name="elementsToExclude">';' delimited string for IfcProducts to exclude from components(Assets)</param>
        /// <param name="typesToExclude">';' delimited string for IfcTypeObjects to exclude from Types</param>
        public ObjectFilter(string elementsToExclude, string typesToExclude)
        {
            //IfcProducts and IfcTypeObjects to exclude
            if (!string.IsNullOrEmpty(elementsToExclude))
            {
                ElementsToExclude.AddRange(elementsToExclude.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }
            if (!string.IsNullOrEmpty(typesToExclude))
            {
                TypesToExclude.AddRange(typesToExclude.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        /// <summary>
        /// Set Property Filters constructor via ConfigurationSection from configuration file
        /// </summary>
        /// <param name="section">ConfigurationSection from configuration file</param>
        public ObjectFilter(ConfigurationSection section)
        {
            //initialize fields
            Init();
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
                            ElementsToExclude.Add(keyVal.Key);
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
                            TypesToExclude.Add(keyVal.Key);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Test for string exists in ElementsToExclude string lists
        /// </summary>
        /// <param name="testStr">String to test</param>
        /// <returns>bool</returns>
        public bool ElementsFilter(string testStr)
        {
            return (ElementsToExclude.Where(a => testStr.Equals(a)).Count() > 0);
        }

        /// <summary>
        /// Test for string exists in TypesToExclude string lists
        /// </summary>
        /// <param name="testStr">String to test</param>
        /// <returns>bool</returns>
        public bool TypeObjFilter(string testStr)
        {
            return (TypesToExclude.Where(a => testStr.Equals(a)).Count() > 0);
        }

        
    }
}
