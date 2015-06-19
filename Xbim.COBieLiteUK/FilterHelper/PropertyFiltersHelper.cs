using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.IO;
using System.Reflection;
using Xbim.COBieLiteUK.FilterHelper;
using Xbim.COBieLiteUK;

namespace XbimExchanger.COBieLiteHelpers
{
    public class PropertyFiltersHelper
    {
        #region Filters
        /// <summary>
        /// Zone attribute filters
        /// </summary>
        public PropertyFilter ZoneFilter { get; private set; }
        /// <summary>
        /// Type attribute filters
        /// </summary>
        public PropertyFilter TypeFilter { get; private set; }
        /// <summary>
        /// Space attribute filters
        /// </summary>
        public PropertyFilter SpaceFilter { get; private set; }
        /// <summary>
        /// Floor attribute filters
        /// </summary>
        public PropertyFilter FloorFilter { get; private set; }
        /// <summary>
        /// Facility attribute filters
        /// </summary>
        public PropertyFilter FacilityFilter { get; private set; }
        /// <summary>
        /// Spare attribute filters
        /// </summary>
        public PropertyFilter SpareFilter { get; private set; }
        /// <summary>
        /// Component attribute filters
        /// </summary>
        public PropertyFilter ComponentFilter { get; private set; }
        /// <summary>
        /// Common attribute filters
        /// </summary>
        public PropertyFilter CommonFilter { get; private set; }
        #endregion

        /// <summary>
        /// Constructor, will read Configuration file if passed, or default COBieAttributesFilters.config
        /// </summary>
        /// <param name="configFileName">Full path/name for config file</param>
        public PropertyFiltersHelper(string configFileName = null)
        {
            var tmpFile = configFileName;
            if (configFileName == null)
            {
                tmpFile = Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";

                var asss = System.Reflection.Assembly.GetExecutingAssembly();

                using (var input = asss.GetManifestResourceStream("Xbim.COBieLiteUK.FilterHelper.COBieAttributesFilters.config"))
                {
                    if (input != null)
                    {
                        using (var output = File.Create(tmpFile))
                        {
                            if (input != null) input.CopyTo(output);
                        }
                    }
                }
            }
            
            if (!File.Exists(tmpFile))
            {
                var directory = new DirectoryInfo(".");
                throw new FileNotFoundException(string.Format(@"Error loading configuration file ""{0}"". App folder is ""{1}"".", tmpFile,directory.FullName) );
            }
            
            Configuration config;

            try
            {
                var configMap = new ExeConfigurationFileMap { ExeConfigFilename = tmpFile };
                config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
               
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException(string.Format(@"Error loading configuration file ""{0}"". Error: {1}", tmpFile, ex.Message ));
            }
            
            ZoneFilter = new PropertyFilter(config.GetSection("ZoneFilter"));
            TypeFilter = new PropertyFilter(config.GetSection("TypeFilter"));
            SpaceFilter = new PropertyFilter(config.GetSection("SpaceFilter"));
            FloorFilter = new PropertyFilter(config.GetSection("FloorFilter"));
            FacilityFilter = new PropertyFilter(config.GetSection("FacilityFilter"));
            SpareFilter = new PropertyFilter(config.GetSection("SpareFilter"));
            ComponentFilter = new PropertyFilter(config.GetSection("ComponentFilter"));
            CommonFilter = new PropertyFilter(config.GetSection("CommonFilter"));
        }

        /// <summary>
        /// Test property Names against sheets
        /// </summary>
        /// <param name="testStr">Name string to test</param>
        /// <param name="parent">Parent object</param>
        /// <returns>bool</returns>
        public bool NameFilterOnParent(string testStr, CobieObject parent = null)
        {
            bool result = CommonFilter.NameFilter(testStr);
            if (!result)
            {
                if (parent is Zone)
                    result = ZoneFilter.NameFilter(testStr);
                else if (parent is AssetType)
                    result = TypeFilter.NameFilter(testStr);
                else if (parent is Space)
                    result = SpaceFilter.NameFilter(testStr);
                else if (parent is Floor)
                    result = FloorFilter.NameFilter(testStr);
                else if (parent is Facility)
                    result = FacilityFilter.NameFilter(testStr);
                else if (parent is Spare)
                    result = SpareFilter.NameFilter(testStr);
                else if (parent is Asset)
                    result = ComponentFilter.NameFilter(testStr);
                else
                    result = false;
            }
            return result;
        }

        /// <summary>
        /// Test Property Set Names against sheets
        /// </summary>
        /// <param name="testStr">Name string to test</param>
        /// <param name="parent">Parent object</param>
        /// <returns>bool</returns>
        public bool PSetNameFilterOnSheetName(string testStr, CobieObject parent = null)
        {
            bool result = CommonFilter.PSetNameFilter(testStr);
            if (!result)
            {
                if (parent is Zone)
                    result = ZoneFilter.PSetNameFilter(testStr);
                else if (parent is AssetType)
                    result = TypeFilter.PSetNameFilter(testStr);
                else if (parent is Space)
                    result = SpaceFilter.PSetNameFilter(testStr);
                else if (parent is Floor)
                    result = FloorFilter.PSetNameFilter(testStr);
                else if (parent is Facility)
                    result = FacilityFilter.PSetNameFilter(testStr);
                else if (parent is Spare)
                    result = SpareFilter.PSetNameFilter(testStr);
                else if (parent is Asset)
                    result = ComponentFilter.PSetNameFilter(testStr);
                else
                    result = false;
            }
            return result;
        }
    }

   
}
