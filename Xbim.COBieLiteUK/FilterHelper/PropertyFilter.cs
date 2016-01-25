using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace Xbim.FilterHelper
{
    /// <summary>
    /// Filter on property set names, and properties names held within the set, used to extract attribute sheets
    /// </summary>
    public class PropertyFilter
    {

        /// <summary>
        /// Property Name exclude filter strings where name equal one of the strings
        /// </summary>
        public List<string> EqualTo { get; set; }

        /// <summary>
        /// Property Name exclude filter strings where name starts with one of the strings
        /// </summary>
        public List<string> StartWith { get; set; }

        /// <summary>
        /// Property Name exclude filter strings where name contains with one of the strings
        /// </summary>
        public List<string> Contain { get; set; }
        /// <summary>
        /// Property Set Name exclude filter strings where name equals with one of the strings
        /// </summary>
        public List<string> PropertySetsEqualTo { get; set; }

        public PropertyFilter()
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
        public PropertyFilter(string equalTo, string startWith, string contain, string pSetEqualTo) : this()
        {
             //Property names to exclude 
            if (!string.IsNullOrEmpty(equalTo))
            {
                EqualTo.AddRange(equalTo.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(s => s.ToUpper()));
            }
            if (!string.IsNullOrEmpty(startWith))
            {
                StartWith.AddRange(startWith.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(s => s.ToUpper()));
            }
            if (!string.IsNullOrEmpty(contain))
            {
                Contain.AddRange(contain.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(s => s.ToUpper()));
            }

            //PropertySet names to exclude
            if (!string.IsNullOrEmpty(pSetEqualTo))
            {
                PropertySetsEqualTo.AddRange(pSetEqualTo.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(s => s.ToUpper()));
            }

        }

        /// <summary>
        /// Set Property Filters constructor via ConfigurationSection from configuration file
        /// </summary>
        /// <param name="section">ConfigurationSection from configuration file</param>
        public PropertyFilter(ConfigurationSection section) : this()
        {
            //initialize fields
            if (section == null) return;

            foreach (KeyValueConfigurationElement keyVal in ((AppSettingsSection)section).Settings)
            {
                if (string.IsNullOrEmpty(keyVal.Value)) continue;

                switch (keyVal.Key.ToUpper())
                {
                    case "EQUALTO":
                        EqualTo.AddRange(keyVal.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(s => s.ToUpper()));
                        break;
                    case "STARTWITH":
                        StartWith.AddRange(keyVal.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(s => s.ToUpper()));
                        break;
                    case "CONTAIN":
                        Contain.AddRange(keyVal.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(s => s.ToUpper()));
                        break;
                    case "PROPERTYSETSEQUALTO":
                        PropertySetsEqualTo.AddRange(keyVal.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(s => s.ToUpper()));
                        break;
                    default:
#if DEBUG
                        Debug.WriteLine(string.Format("Invalid Key - {0}", keyVal.Key.ToUpper()));
#endif
                        break;
                }
            }
        }

        /// <summary>
        /// see if object is empty of any values
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return ((EqualTo.Count == 0) && (StartWith.Count == 0) && (Contain.Count == 0) && (PropertySetsEqualTo.Count == 0));
        }

        /// <summary>
        /// Test for string exists in EqTo, Contains, or StartWith string lists
        /// </summary>
        /// <param name="testStr">String to test</param>
        /// <returns>bool</returns>
        public bool NameFilter (string testStr)
        {
            testStr = testStr.ToUpper();
            return EqualTo.Any(a => testStr.Equals(a)) ||
                   StartWith.Any(a => testStr.StartsWith(a)) ||
                   (Contain.Count(a => testStr.Contains(a)) > 0);
        }

        /// <summary>
        /// Test for string exists in PropertySetsEqualTo string lists
        /// </summary>
        /// <param name="testStr">String to test</param>
        /// <returns>bool</returns>
        public bool PSetNameFilter(string testStr)
        {
            testStr = testStr.ToUpper();
            return PropertySetsEqualTo.Any(a => testStr.Equals(a));
        }


        /// <summary>
        /// Merge PropertyFilter
        /// </summary>
        /// <param name="mergeFilter">PropertyFilter to merge</param>
        public void Merge (PropertyFilter mergeFilter)
        {
            EqualTo = EqualTo.Concat(mergeFilter.EqualTo.Where(s => !EqualTo.Contains(s))).ToList();
            StartWith = StartWith.Concat(mergeFilter.StartWith.Where(s => !StartWith.Contains(s))).ToList();
            Contain = Contain.Concat(mergeFilter.Contain.Where(s => !Contain.Contains(s))).ToList();
            PropertySetsEqualTo = PropertySetsEqualTo.Concat(mergeFilter.PropertySetsEqualTo.Where(s => !PropertySetsEqualTo.Contains(s))).ToList();
        }


        /// <summary>
        /// Copy values from passed PropertyFilter
        /// </summary>
        /// <param name="copyFilter">PropertyFilter to copy</param>
        public void Copy(PropertyFilter copyFilter)
        {
            EqualTo.Clear();
            EqualTo = EqualTo.Concat(copyFilter.EqualTo).ToList();
            StartWith.Clear();
            StartWith = StartWith.Concat(copyFilter.StartWith).ToList();
            Contain.Clear();
            Contain = Contain.Concat(copyFilter.Contain).ToList();
            PropertySetsEqualTo.Clear();
            PropertySetsEqualTo = PropertySetsEqualTo.Concat(copyFilter.PropertySetsEqualTo).ToList();
            
        }

        /// <summary>
        /// Clear PropertyFilter
        /// </summary>
        public void Clear()
        {
            EqualTo.Clear();
            StartWith.Clear();
            Contain.Clear();
            PropertySetsEqualTo.Clear();
        }

    }
}
