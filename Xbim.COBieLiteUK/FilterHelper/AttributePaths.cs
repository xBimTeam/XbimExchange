using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.FilterHelper
{
    public class COBiePropertyMapping
    {
        public IEnumerable<AttributePaths> SpacePaths { get; set; }
        public IEnumerable<AttributePaths> FloorPaths { get; set; }
        public IEnumerable<AttributePaths> AssetPaths { get; set; }
        public IEnumerable<AttributePaths> AssetTypePaths { get; set; }

        public COBiePropertyMapping(FileInfo configFileName )
        {
            if (configFileName.Exists)
            {

                var configMap = new ExeConfigurationFileMap { ExeConfigFilename = configFileName.FullName };
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                //AppSettingsSection cobiePropertyMaps = (AppSettingsSection)config.GetSection("COBiePropertyMaps");
                
            }


        }

    }

    public class AttributePaths 
    {
        public string Key { get; set; }
        public String[] PSetPaths { get; set; }

        public AttributePaths(string key, ConfigurationSection section)
        {
            Key = key;
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
            PSetPaths = paths.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        
    }


    
}
