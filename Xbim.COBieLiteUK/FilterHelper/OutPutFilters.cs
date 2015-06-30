using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;
using Xbim.COBieLiteUK;




namespace Xbim.FilterHelper
{
    public class OutPutFilters
    {
        #region Properties
        /// <summary>
        /// IfcProduct Exclude filters
        /// </summary>
        public ObjectFilter IfcProductFilter { get;  set; }
        /// <summary>
        /// IfcTypeObject Exclude filters
        /// </summary>
        public ObjectFilter IfcTypeObjectFilter { get;  set; }
        /// <summary>
        /// IfcAssembly Exclude filters
        /// </summary>
        public ObjectFilter IfcAssemblyFilter { get; set; }

        /// <summary>
        /// Zone attribute filters
        /// </summary>
        public PropertyFilter ZoneFilter { get;  set; }
        /// <summary>
        /// Type attribute filters
        /// </summary>
        public PropertyFilter TypeFilter { get;  set; }
        /// <summary>
        /// Space attribute filters
        /// </summary>
        public PropertyFilter SpaceFilter { get;  set; }
        /// <summary>
        /// Floor attribute filters
        /// </summary>
        public PropertyFilter FloorFilter { get;  set; }
        /// <summary>
        /// Facility attribute filters
        /// </summary>
        public PropertyFilter FacilityFilter { get;  set; }
        /// <summary>
        /// Spare attribute filters
        /// </summary>
        public PropertyFilter SpareFilter { get;  set; }
        /// <summary>
        /// Component attribute filters
        /// </summary>
        public PropertyFilter ComponentFilter { get;  set; }
        /// <summary>
        /// Common attribute filters
        /// </summary>
        public PropertyFilter CommonFilter { get;  set; }

        /// <summary>
        /// Temp storage for role OutPutFilters
        /// </summary>
        private Dictionary<RoleFilter, OutPutFilters> RolesFilter { get;  set; }

        #endregion

        #region Constructor methods

        /// <summary>
        /// Empty constructor for Serialize
        /// </summary>
        public OutPutFilters()
        {
            RolesFilter = new Dictionary<RoleFilter, OutPutFilters>();
        }
        
        /// <summary>
        /// Constructor for default set configFileName = null, or passed in configuration file path
        /// </summary>
        /// <param name="configFileName"></param>
        public OutPutFilters(string configFileName) : this()
        {
            FiltersHelperInit(configFileName);
        }
       

        /// <summary>
        /// Constructor, will read Configuration file if passed, or default COBieAttributesFilters.config
        /// </summary>
        /// <param name="configFileName">Full path/name for config file</param>
        private void FiltersHelperInit(string configFileName = null)
        {
            string resFile = configFileName;
            //set default
            if (resFile == null)
            {
                 resFile = "Xbim.COBieLiteUK.FilterHelper.COBieAttributesFilters.config";               
            }
            
            Configuration config = GetResourceConfig(resFile);

            //IfcProduct and IfcTypeObject filters
            IfcProductFilter = new ObjectFilter(config.GetSection("IfcElementInclusion"));
            IfcTypeObjectFilter = new ObjectFilter(config.GetSection("IfcTypeInclusion"));
            IfcTypeObjectFilter.FillPreDefinedTypes(config.GetSection("IfcPreDefinedTypeFilter"));
            IfcAssemblyFilter = new ObjectFilter(config.GetSection("IfcAssemblyInclusion"));
            
            //Property name filters
            ZoneFilter = new PropertyFilter(config.GetSection("ZoneFilter"));
            TypeFilter = new PropertyFilter(config.GetSection("TypeFilter"));
            SpaceFilter = new PropertyFilter(config.GetSection("SpaceFilter"));
            FloorFilter = new PropertyFilter(config.GetSection("FloorFilter"));
            FacilityFilter = new PropertyFilter(config.GetSection("FacilityFilter"));
            SpareFilter = new PropertyFilter(config.GetSection("SpareFilter"));
            ComponentFilter = new PropertyFilter(config.GetSection("ComponentFilter"));
            CommonFilter = new PropertyFilter(config.GetSection("CommonFilter"));

            if (configFileName == null)
            {
                File.Delete(config.FilePath);
            }
            
        }

        private Configuration GetResourceConfig(string resFileName)
        {
            string tmpFile = resFileName;
            bool isResourcFile = !File.Exists(resFileName);
            if (isResourcFile)
            {
                tmpFile = Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";

                var asss = System.Reflection.Assembly.GetExecutingAssembly();

                using (var input = asss.GetManifestResourceStream(resFileName))
                {
                    if (input != null)
                    {
                        using (var output = File.Create(tmpFile))
                        {
                            if (input != null) input.CopyTo(output);
                        }
                    }
                }

                if (!File.Exists(tmpFile))
                {
                    throw new FileNotFoundException(string.Format(@"File not found ""{0}"".", tmpFile));
                }
            }

            Configuration config;
            try
            {
                var configMap = new ExeConfigurationFileMap { ExeConfigFilename = tmpFile };
                config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException(string.Format(@"Error loading configuration file ""{0}"". Error: {1}", tmpFile, ex.Message));
            }

            return config;
        }
        #endregion

        #region Filter Methods

        /// <summary>
        /// Test property Names against sheets
        /// </summary>
        /// <param name="testStr">Name string to test</param>
        /// <param name="parent">Parent object</param>
        /// <returns>bool</returns>
        public bool NameFilterOnParent(string testStr, CobieObject parent = null)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(testStr))
            {
                result = CommonFilter.NameFilter(testStr);
                if (!result)
                {
                    if (parent == null)
                        result = false;
                    else if ((parent is Zone) && (ZoneFilter != null))
                        result = ZoneFilter.NameFilter(testStr);
                    else if ((parent is AssetType) && (TypeFilter != null))
                        result = TypeFilter.NameFilter(testStr);
                    else if ((parent is Space) && (SpaceFilter != null))
                        result = SpaceFilter.NameFilter(testStr);
                    else if ((parent is Floor) && (FloorFilter != null))
                        result = FloorFilter.NameFilter(testStr);
                    else if ((parent is Facility) && (FacilityFilter != null))
                        result = FacilityFilter.NameFilter(testStr);
                    else if ((parent is Spare) && (SpareFilter != null))
                        result = SpareFilter.NameFilter(testStr);
                    else if ((parent is Asset) && (ComponentFilter != null))
                        result = ComponentFilter.NameFilter(testStr);
                    else
                        result = false;
                }
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
            bool result = false;
            if (!string.IsNullOrEmpty(testStr))
            {
                result = CommonFilter.PSetNameFilter(testStr);
                if (!result)
                {
                    if (parent == null)
                        result = false;
                    else if ((parent is Zone) && (ZoneFilter != null))
                        result = ZoneFilter.PSetNameFilter(testStr);
                    else if ((parent is AssetType) && (TypeFilter != null))
                        result = TypeFilter.PSetNameFilter(testStr);
                    else if ((parent is Space) && (SpaceFilter != null))
                        result = SpaceFilter.PSetNameFilter(testStr);
                    else if ((parent is Floor) && (FloorFilter != null))
                        result = FloorFilter.PSetNameFilter(testStr);
                    else if ((parent is Facility) && (FacilityFilter != null))
                        result = FacilityFilter.PSetNameFilter(testStr);
                    else if ((parent is Spare) && (SpareFilter != null))
                        result = SpareFilter.PSetNameFilter(testStr);
                    else if ((parent is Asset) && (ComponentFilter != null))
                        result = ComponentFilter.PSetNameFilter(testStr);
                    else
                        result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Filter IfcProduct and IfcTypeObject types
        /// </summary>
        /// <param name="obj">CobieObject</param>
        /// <param name="preDefinedType">strings for the ifcTypeObject predefinedtype enum property</param>
        /// <returns>bool, true = exclude</returns>
        public bool ObjFilter(CobieObject obj, string preDefinedType = null)
        {
            if (!string.IsNullOrEmpty(obj.ExternalEntity))
            {
                if ((obj is Asset) && (IfcProductFilter != null))
                    return IfcProductFilter.ItemsFilter(obj.ExternalEntity);
                else if ((obj is AssetType) && (IfcTypeObjectFilter != null))
                    return IfcTypeObjectFilter.ItemsFilter(obj.ExternalEntity, preDefinedType);
            }
            return false;
        }
        #endregion

        #region Merge Roles
        /// <summary>
        /// Merge OutPutFilters
        /// </summary>
        /// <param name="mergeFilter">OutPutFilters</param>
        public void Merge(OutPutFilters mergeFilter)
        {
            IfcProductFilter.Merge(mergeFilter.IfcProductFilter);
            IfcTypeObjectFilter.Merge(mergeFilter.IfcTypeObjectFilter);
            IfcAssemblyFilter.Merge(mergeFilter.IfcAssemblyFilter);


        }

        /// <summary>
        /// Extension method to use default role configuration resource files
        /// </summary>
        /// <param name="roles">MergeRoles, Flag enum with one or more roles</param>
        public void AddRoleFilters(RoleFilter roles)
        {
            OutPutFilters mergeFilter = new OutPutFilters();
            foreach (RoleFilter role in Enum.GetValues(typeof(RoleFilter)))
            {
                if (roles.HasFlag(role))
                {
                    if (RolesFilter.ContainsKey(role))
                    {
                        mergeFilter = RolesFilter[role];
                    }
                    else
                    {   //load defaults
                        switch (role)
                        {
                            case RoleFilter.Unknown:
                                break;
                            case RoleFilter.Architectural:
                                mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBieArchitecturalFilters.config");
                                break;
                            case RoleFilter.Mechanical:
                                mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBieMechanicalFilters.config");
                                break;
                            case RoleFilter.Electrical:
                                mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBieElectricalFilters.config");
                                break;
                            case RoleFilter.Plumbing:
                                mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBiePlumbingFilters.config");
                                break;
                            case RoleFilter.FireProtection:
                                mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBieFireProtectionFilters.config");
                                break;
                            default:
                                break;
                        }
                        RolesFilter[role] = mergeFilter;
                    }
                    this.Merge(mergeFilter);
                }

            }
        }

        #endregion

        #region Serialize

        /// <summary>
        /// Save object as xml file
        /// </summary>
        /// <param name="filename">FileInfo</param>
        public void SerializeXML(FileInfo filename)
        {
            XmlSerializer writer = new XmlSerializer(typeof(OutPutFilters));
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename.FullName))
            {
                writer.Serialize(file, this);
            }
        }

        /// <summary>
        /// Create a OutPutFilters object from a XML file
        /// </summary>
        /// <param name="filename">FileInfo</param>
        /// <returns>OutPutFilters</returns>
        public static OutPutFilters DeserializeXML(FileInfo filename)
        {
            OutPutFilters result = null;
            XmlSerializer writer = new XmlSerializer(typeof(OutPutFilters));
            using (System.IO.StreamReader file = new System.IO.StreamReader(filename.FullName))
            {
                result =  (OutPutFilters)writer.Deserialize(file);
            }
            return result;
        }

        /// <summary>
        /// Save object as JSON 
        /// </summary>
        /// <param name="filename">FileInfo</param>
        public void SerializeJSON (FileInfo filename)
        {
            JsonSerializer writer = new JsonSerializer();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename.FullName))
            {
                writer.Serialize(file, this);
            }
        }

        /// <summary>
        /// Create a OutPutFilters object from a JSON file
        /// </summary>
        /// <param name="filename">FileInfo</param>
        /// <returns>OutPutFilters</returns>
        public static OutPutFilters DeserializeJSON(FileInfo filename)
        {
            OutPutFilters result = null;
            JsonSerializer writer = new JsonSerializer();
            using (System.IO.StreamReader file = new System.IO.StreamReader(filename.FullName))
            {
                result = (OutPutFilters)writer.Deserialize(file, typeof(OutPutFilters));
            }
            return result;
        }

        #endregion
    }
    /// <summary>
    /// Merge Flags for roles in deciding if an object is allowed or discarded depending on the role of the model
    /// </summary>
    [Flags] //allows use to | and & values for multiple boolean tests
    public enum RoleFilter
    {
        Unknown = 0x1,
        Architectural = 0x2,
        Mechanical = 0x4,
        Electrical = 0x8,
        Plumbing = 0x10,
        FireProtection = 0x20

    }
}
