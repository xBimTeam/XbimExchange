using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Xbim.COBieLite;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;

namespace XbimExchanger.COBieLiteToIfc
{
    public class CoBieLiteToIfcExchanger : XbimExchanger<FacilityType, XbimModel>
    {
        public struct NamedProperty
        {
            public string PropertySetName;
            public string PropertyName;

            public NamedProperty(string propertyName, string propertySetName) : this()
            {
                PropertyName = propertyName;
                PropertySetName = propertySetName;
            }
        }

        static readonly IDictionary<string, NamedProperty> CobieToIfcPropertyMap = new Dictionary<string, NamedProperty>();
        static CoBieLiteToIfcExchanger()
        {
           
            var configMap = new ExeConfigurationFileMap { ExeConfigFilename = "COBieAttributes.config" };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            var cobiePropertyMaps = (AppSettingsSection)config.GetSection("COBiePropertyMaps");

            foreach (KeyValueConfigurationElement keyVal in cobiePropertyMaps.Settings)
            {
                var values = keyVal.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var selected = values.FirstOrDefault();
                if (selected != null)
                {
                    var names = selected.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    if(names.Length==2)
                        CobieToIfcPropertyMap.Add(keyVal.Key, new NamedProperty(names[0], names[1]));
                }
            }
            
        }

        public CoBieLiteToIfcExchanger(FacilityType facility, XbimModel xbimModel) : base(facility, xbimModel)
        {

        }


        public override XbimModel Convert()
        {
            ConvertBuilding();
            return TargetRepository;
        }


        public IfcBuilding ConvertBuilding()
        {
            var mapping = GetOrCreateMappings<MappingFacilityTypeToIfcBuilding>();
            var building = mapping.GetOrCreateTargetObject(SourceRepository.externalID);
            return  mapping.AddMapping(SourceRepository, building);
            
        }


    }
}
