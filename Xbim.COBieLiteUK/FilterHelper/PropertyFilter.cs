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
    /// Filter on property set names, and properties names held within the set, used to extract attribute sheets
    /// </summary>
    public class PropertyFilter
    {
        /// <summary>
        /// Property Name exclude filter strings where name equal one of the strings
        /// </summary>
        public List<string> EqualTo { get; private set; }

        /// <summary>
        /// Property Name exclude filter strings where name starts with one of the strings
        /// </summary>
        public List<string> StartWith { get; private set; }

        /// <summary>
        /// Property Name exclude filter strings where name contains with one of the strings
        /// </summary>
        public List<string> Contain { get; private set; }
        /// <summary>
        /// Property Set Name exclude filter strings where name equals with one of the strings
        /// </summary>
        public List<string> PropertySetsEqualTo { get; private set; }

        /// <summary>
        /// Initialize fields
        /// </summary>
        private void Init()
        {
            EqualTo = new List<string>();
            StartWith = new List<string>();
            Contain = new List<string>();
            PropertySetsEqualTo = new List<string>();
        }
        /// <summary>
        /// Set Property Filters constructor
        /// </summary>
        /// <param name="equalTo">';' delimited string for property names to equal</param>
        /// <param name="startWith">';' delimited string for property names to start with</param>
        /// <param name="contain">';' delimited string for property names containing</param>
        /// <param name="pSetEqualTo">';' delimited string for Property Set names to equal</param>
        public PropertyFilter(string equalTo, string startWith, string contain, string pSetEqualTo)
        {
            //initialize fields
            Init();

            if (!string.IsNullOrEmpty(equalTo))
            {
                EqualTo.AddRange(equalTo.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }
            if (!string.IsNullOrEmpty(startWith))
            {
                StartWith.AddRange(startWith.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }
            if (!string.IsNullOrEmpty(contain))
            {
                Contain.AddRange(contain.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }
            if (!string.IsNullOrEmpty(pSetEqualTo))
            {
                PropertySetsEqualTo.AddRange(pSetEqualTo.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }

        }

        /// <summary>
        /// Set Property Filters constructor via ConfigurationSection from configuration file
        /// </summary>
        /// <param name="section">ConfigurationSection from configuration file</param>
        public PropertyFilter(ConfigurationSection section)
        {
            //initialize fields
            Init();
            if (section != null)
            {
                foreach (KeyValueConfigurationElement keyVal in ((AppSettingsSection)section).Settings)
                {
                    if (!string.IsNullOrEmpty(keyVal.Value))
                    {
                        switch (keyVal.Key)
                        {
                            case "EqualTo":
                                EqualTo.AddRange(keyVal.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                                break;
                            case "StartWith":
                                StartWith.AddRange(keyVal.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                                break;
                            case "Contain":
                                Contain.AddRange(keyVal.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                                break;
                            case "PropertySetsEqualTo":
                                PropertySetsEqualTo.AddRange(keyVal.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                                break;
                            default:
#if DEBUG
                                Debug.WriteLine(string.Format("Invalid Key - {0}", keyVal.Key.ToUpper()));
#endif
                                break;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Test for string exists in EqTo, Contains, or StartWith string lists
        /// </summary>
        /// <param name="testStr">String to test</param>
        /// <returns>bool</returns>
        public bool NameFilter (string testStr)
        {
            return ((EqualTo.Where(a => testStr.Equals(a)).Count() > 0) ||
                    (StartWith.Where(a => testStr.StartsWith(a)).Count() > 0) ||
                    (Contain.Where(a => testStr.Contains(a)).Count() > 0)
                   );
        }

        /// <summary>
        /// Test for string exists in PropertySetsEqualTo string lists
        /// </summary>
        /// <param name="testStr">String to test</param>
        /// <returns>bool</returns>
        public bool PSetNameFilter(string testStr)
        {
            return (PropertySetsEqualTo.Where(a => testStr.Equals(a)).Count() > 0);
        }

    }
}
