using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace XbimExchanger.DPoWToCOBieLite
{
    public class ClassificationMappings
    {
        private const string ConfigFileName = "DPoW2ToCOBieLite\\ClassificationMappings.config";
        private IEnumerable<ClassificationMapping> _propertySetMaps = new List<ClassificationMapping>();
        private IEnumerable<ClassificationMapping> _typeMaps = new List<ClassificationMapping>();
        private IEnumerable<ClassificationMapping> _instanceMaps = new List<ClassificationMapping>(); 

        public void LoadCobieMaps()
        {
            string path;
            if (File.Exists(ConfigFileName))
                path = ConfigFileName;
            else
            {
                path = Path.GetDirectoryName(GetType().Assembly.Location) ?? "";
                path = Path.Combine(path, ConfigFileName);
            }
            if (!File.Exists(path))
                throw new Exception("Mapping file not found.");



            try
            {
                var configMap = new ExeConfigurationFileMap {ExeConfigFilename = path};
                var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                var propertySetMapSections = (AppSettingsSection)config.GetSection("PropertySetMaps");
                var ifcTypeMapSections = (AppSettingsSection)config.GetSection("IfcTypeMaps");
                var ifcInstanceMapSections = (AppSettingsSection)config.GetSection("IfcInstanceMaps");

                _propertySetMaps = CreateMappings(propertySetMapSections);
                _typeMaps = CreateMappings(ifcTypeMapSections);
                _instanceMaps = CreateMappings(ifcInstanceMapSections);
            }
            catch (Exception ex)
            {
                var directory = new DirectoryInfo(".");
                throw new Exception(
                    @"Error loading configuration file ""COBieAttributes.config"". App folder is " + directory.FullName,
                    ex);
            }
        }

        private IEnumerable<ClassificationMapping> CreateMappings(AppSettingsSection section)
        {
            return from KeyValueConfigurationElement kvp in section.Settings select new ClassificationMapping(kvp.Key, kvp.Value);
        }
    }



    public class ClassificationMapping
    {
        private readonly Func<string, bool> _predicate;

        public string Key { get; private set; }
        public string Value { get; private set; }
        public string Classification { get; private set; }

        public bool IsForClassificationCode(string code)
        {
            return _predicate == null || _predicate(code);
        }

        public ClassificationMapping(string compositeKey, string value)
        {
            Value = value;
            var parts = compositeKey.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                throw new Exception("Invalid composite key");
            if (parts.Length == 1)
                Key = parts[0].Trim();
            if (parts.Length == 2)
            {
                Classification = parts[0].Trim();
                _predicate = ParsePredicate(parts[1].Trim());
                Key = "";
            }
            if (parts.Length == 3)
            {
                Classification = parts[0].Trim();
                _predicate = ParsePredicate(parts[1].Trim());
                Key = parts[2].Trim();
            }
        }

        private Func<string, bool> ParsePredicate(string value)
        {
            value = value.ToLower();
            var main = value.Trim('*');
            return s => value.EndsWith("*") ? s.ToLower().StartsWith(main) : s.ToLower() == main;
        }
    }
}