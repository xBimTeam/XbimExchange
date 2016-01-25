using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Xbim.FilterHelper
{
    public class COBiePropertyMapping
    {
        /// <summary>
        /// current section names in config file
        /// </summary>
        private readonly string[] _sectionKeys = { "SpacePropertyMaps", "FloorPropertyMaps", "AssetPropertyMaps", "AssetTypePropertyMaps", "SystemPropertyMaps", "CommonPropertyMaps", "SparePropertyMaps" };


        /// <summary>
        /// Common List of attribute paths
        /// </summary>
        public List<AttributePaths> CommonPaths { get; set; }

        public List<AttributePaths> SparePaths { get; set; }
        /// <summary>
        /// Space List of attribute paths
        /// </summary>
        public List<AttributePaths> SpacePaths { get; set; }
        /// <summary>
        /// Floor List of attribute paths
        /// </summary>
        public List<AttributePaths> FloorPaths { get; set; }
        /// <summary>
        /// Asset/Component List of attribute paths
        /// </summary>
        public List<AttributePaths> AssetPaths { get; set; }
        /// <summary>
        /// Asset Types List of attribute paths
        /// </summary>
        public List<AttributePaths> AssetTypePaths { get; set; }
        /// <summary>
        /// Property Set mappings to be considered as systems,
        /// </summary>
        public List<AttributePaths> PSetsAsSystem { get; set; }

        /// <summary>
        /// File info for config file
        /// </summary>
        public FileInfo ConfigFile{ get; set; }

        /// <summary>
        /// Constructor to initialise objects
        /// </summary>
        public COBiePropertyMapping()
        {
            CommonPaths = new List<AttributePaths>();
            SparePaths = new List<AttributePaths>();
            SpacePaths = new List<AttributePaths>();
            FloorPaths = new List<AttributePaths>();
            AssetPaths = new List<AttributePaths>();
            AssetTypePaths = new List<AttributePaths>();
            PSetsAsSystem = new List<AttributePaths>();

        }

        /// <summary>
        /// get the current hard coded section keys
        /// </summary>
        public List<string> SectionKeys
        {
            get
            {
                return _sectionKeys.ToList();
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configFileName">FileInfo, config file</param>
        public COBiePropertyMapping(FileInfo configFileName ) : this()
        {
            if (configFileName.Exists)
            {
                ConfigFile = configFileName;
                try
                {
                    var configMap = new ExeConfigurationFileMap { ExeConfigFilename = ConfigFile.FullName };
                    var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                    foreach (var sectionKey in _sectionKeys)
                    {
                        var proxy = GetStorageList(sectionKey); //swap to correct path list
                        var section = config.GetSection(sectionKey);
                        proxy.AddRange(from KeyValueConfigurationElement keyVal in ((AppSettingsSection) section).Settings select new AttributePaths(keyVal.Key, keyVal.Value));
                    }
                    
                }
                catch (Exception)
                {
                    throw new FormatException(string.Format("Formate incorrect: Delete {0} and restart application", ConfigFile.FullName));
                }
            }
            
        }

        /// <summary>
        /// Save values back to config file
        /// </summary>
        public void Save()
        {
            //save any changes back to the config file
            var configMap = new ExeConfigurationFileMap { ExeConfigFilename = ConfigFile.FullName };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            foreach (var sectionKey in _sectionKeys)
            {
                var proxy = GetStorageList(sectionKey); //swap to correct path list
                var section = (AppSettingsSection)config.GetSection(sectionKey);
                foreach (var item in proxy)
                {
                    section.Settings[item.Key].Value = item.Value;
                }
            }
            //save back to file
            config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// get the current list in loop
        /// </summary>
        /// <param name="sectionKey">string, Section Key</param>
        /// <returns>List of AttributePaths</returns>
        private List<AttributePaths> GetStorageList(string sectionKey)
        {
            switch (sectionKey)
            {
                case "CommonPropertyMaps":
                    return CommonPaths;
                case "SparePropertyMaps":
                    return SparePaths;
                case "SpacePropertyMaps":
                    return SpacePaths;
                case "FloorPropertyMaps":
                    return FloorPaths;
                case "AssetPropertyMaps":
                    return AssetPaths;
                case "AssetTypePropertyMaps":
                    return AssetTypePaths;
                case "SystemPropertyMaps":
                    return PSetsAsSystem;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Get the Property mapping for all sections
        /// </summary>
        /// <returns>Dictionary </returns>
        public Dictionary<string, string[]> GetDictOfProperties()
        {
            return _sectionKeys.SelectMany(GetStorageList).ToDictionary(item => item.Key, item => item.PSetPaths);
        }
    }

    //----------------------------------------------------------------------------------------------------------------------------------------------------------
    ///AttributePaths Class
    //----------------------------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Class to hole the attribute map for the field key
    /// </summary>
    public class AttributePaths 
    {
        /// <summary>
        /// Field key
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        /// List of Pset.Name mapps
        /// </summary>
        public String[] PSetPaths { get; private set; }
        /// <summary>
        /// convert back to ; delimited string
        /// </summary>
        public string Value 
        {
            get 
            { 
                return string.Join(";",PSetPaths);
            }
        }

        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key">Field key</param>
        /// <param name="attPaths">; delimited sting of pset.name paths</param>
        public AttributePaths(string key, string attPaths)
        {
            Key = key;
            
            PSetPaths = attPaths.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        
    }


    
}
