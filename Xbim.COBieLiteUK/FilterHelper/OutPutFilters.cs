using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Kernel;




namespace Xbim.FilterHelper
{
    public class OutPutFilters
    {
        #region Properties

        /// <summary>
        /// Flip results of true/false 
        /// </summary>
        [XmlIgnore][JsonIgnore]
        public bool FlipResult { get; set; }

        /// <summary>
        /// Roles set on this filter
        /// </summary>
        public RoleFilter AppliedRoles  { get; set; }
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
        [XmlIgnore][JsonIgnore]
        private Dictionary<RoleFilter, OutPutFilters> RolesFilterHolder { get; set; }

        

        /// <summary>
        /// Nothing set in RolesFilterHolder
        /// </summary>
        public bool DefaultsNotSet 
        {
            get
            {
                return RolesFilterHolder.Count == 0;
            }
        }
        #endregion

        #region Constructor methods

        /// <summary>
        /// Empty constructor for Serialize
        /// </summary>
        public OutPutFilters()
        {
            //will flip filter result from true to false
            FlipResult = false;

            //object filters
            IfcProductFilter = new ObjectFilter();
            IfcTypeObjectFilter = new ObjectFilter();
            IfcAssemblyFilter = new ObjectFilter();

            //Property name filters
            ZoneFilter = new PropertyFilter();
            TypeFilter = new PropertyFilter();
            SpaceFilter = new PropertyFilter();
            FloorFilter = new PropertyFilter();
            FacilityFilter = new PropertyFilter();
            SpareFilter = new PropertyFilter();
            ComponentFilter = new PropertyFilter();
            CommonFilter = new PropertyFilter();

            //role storage
            RolesFilterHolder = new Dictionary<RoleFilter, OutPutFilters>();
            
        }
        
        /// <summary>
        /// Constructor for default set configFileName = null, or passed in configuration file path
        /// </summary>
        /// <param name="configFileName"></param>
        public OutPutFilters(string configFileName, RoleFilter role, ImportSet import = ImportSet.All) : this()
        {
            AppliedRoles = role;
            FiltersHelperInit(import, configFileName);
        }


        /// <summary>
        /// Constructor to apply roles, and pass custom role OutPutFilters
        /// </summary>
        /// <param name="roles">RoleFilter flags on roles to filter on</param>
        /// <param name="rolesFilter">Dictionary of role to OutPutFilters</param>
        public OutPutFilters(RoleFilter roles, Dictionary<RoleFilter, OutPutFilters> rolesFilter = null)
            : this()
        {
            ApplyRoleFilters(roles, false, rolesFilter);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Test for empty object
        /// </summary>
        /// <returns>bool</returns>
        public bool IsEmpty()
        {
            return IfcProductFilter.IsEmpty() && IfcTypeObjectFilter.IsEmpty() && IfcAssemblyFilter.IsEmpty() && 
            ZoneFilter.IsEmpty() && TypeFilter.IsEmpty() && SpaceFilter.IsEmpty() && 
            FloorFilter.IsEmpty() && FacilityFilter.IsEmpty() && SpareFilter.IsEmpty() && 
            ComponentFilter.IsEmpty() && CommonFilter.IsEmpty();
        }
        /// <summary>
        /// Will read Configuration file if passed, or default COBieAttributesFilters.config
        /// </summary>
        /// <param name="configFileName">Full path/name for config file</param>
        private void FiltersHelperInit(ImportSet import, string configFileName = null)
        {
            string resFile = configFileName;
            //set default
            if (resFile == null)
            {
                resFile = "Xbim.COBieLiteUK.FilterHelper.COBieDefaultFilters.config";               
            }
            
            Configuration config = GetResourceConfig(resFile);

            //IfcProduct and IfcTypeObject filters
            if ((import == ImportSet.All) || (import == ImportSet.IfcFilters))
            {
                IfcProductFilter = new ObjectFilter(config.GetSection("IfcElementInclusion"));
                IfcTypeObjectFilter = new ObjectFilter(config.GetSection("IfcTypeInclusion"));
                IfcTypeObjectFilter.FillPreDefinedTypes(config.GetSection("IfcPreDefinedTypeFilter"));
                IfcAssemblyFilter = new ObjectFilter(config.GetSection("IfcAssemblyInclusion"));
            }
            
            //Property name filters
            if ((import == ImportSet.All) || (import == ImportSet.PropertyFilters))
            {
                ZoneFilter = new PropertyFilter(config.GetSection("ZoneFilter"));
                TypeFilter = new PropertyFilter(config.GetSection("TypeFilter"));
                SpaceFilter = new PropertyFilter(config.GetSection("SpaceFilter"));
                FloorFilter = new PropertyFilter(config.GetSection("FloorFilter"));
                FacilityFilter = new PropertyFilter(config.GetSection("FacilityFilter"));
                SpareFilter = new PropertyFilter(config.GetSection("SpareFilter"));
                ComponentFilter = new PropertyFilter(config.GetSection("ComponentFilter"));
                CommonFilter = new PropertyFilter(config.GetSection("CommonFilter"));
            }
            

            if (configFileName == null)
            {
                File.Delete(config.FilePath);
            }
            
        }

        /// <summary>
        /// Get Configuration object from the passed file path or embedded resource file
        /// </summary>
        /// <param name="resFileName">file path or resource name</param>
        /// <returns></returns>
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

        /// <summary>
        /// Copy the OutPutFilters
        /// </summary>
        /// <param name="copyFilter">OutPutFilters to copy </param>
        public void Copy(OutPutFilters copyFilter)
        {
            AppliedRoles = copyFilter.AppliedRoles;

            IfcProductFilter.Copy(copyFilter.IfcProductFilter);
            IfcTypeObjectFilter.Copy(copyFilter.IfcTypeObjectFilter);
            IfcAssemblyFilter.Copy(copyFilter.IfcAssemblyFilter);

            ZoneFilter.Copy(copyFilter.ZoneFilter);
            TypeFilter.Copy(copyFilter.TypeFilter);
            SpaceFilter.Copy(copyFilter.SpaceFilter);
            FloorFilter.Copy(copyFilter.FloorFilter);
            FacilityFilter.Copy(copyFilter.FacilityFilter);
            SpareFilter.Copy(copyFilter.SpareFilter);
            ComponentFilter.Copy(copyFilter.ComponentFilter);
            CommonFilter.Copy(copyFilter.CommonFilter);
        }


        /// <summary>
        /// Clear OutPutFilters
        /// </summary>
        public void Clear()
        {
            AppliedRoles = 0;

            IfcProductFilter.Clear();
            IfcTypeObjectFilter.Clear();
            IfcAssemblyFilter.Clear();

            ZoneFilter.Clear();
            TypeFilter.Clear();
            SpaceFilter.Clear();
            FloorFilter.Clear();
            FacilityFilter.Clear();
            SpareFilter.Clear();
            ComponentFilter.Clear();
            CommonFilter.Clear();
        }
        #endregion


        #region Filter Methods
        //TODO: IfcProperty filterining on IfcObjects

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
            return FlipResult ? !result : result;
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
            //don't flip property sets as this excludes all the attributes, so we do not see attribute name exclusions as all property sets get excluded
            //return FlipResult ? !result : result; 
            return result;
        }

        /// <summary>
        /// Filter IfcProduct and IfcTypeObject types
        /// </summary>
        /// <param name="obj">CobieObject</param>
        /// <param name="parent">Parent object</param>
        /// <param name="preDefinedType">strings for the ifcTypeObject predefinedtype enum property</param>
        /// <returns>bool, true = exclude</returns>
        public bool ObjFilter(CobieObject obj, CobieObject parent = null, string preDefinedType = null)
        {
            bool exclude = false;
            if (!string.IsNullOrEmpty(obj.ExternalEntity))
            {
                if (obj is Asset)
                {
                    exclude =  IfcProductFilter.ItemsFilter(obj.ExternalEntity);
                    //check the element is not defined by a type which is excluded, by default if no type, then no element included
                    if (!exclude && (parent != null) && (parent is AssetType))
                    {
                        exclude = IfcTypeObjectFilter.ItemsFilter(parent.ExternalEntity, preDefinedType);
                    }
                    
                }
                else if (obj is AssetType) 
                {
                    exclude =  IfcTypeObjectFilter.ItemsFilter(obj.ExternalEntity, preDefinedType);
                }
            }
            return FlipResult ? !exclude : exclude;
        }
        //TODO: Check function below, see if it works!
        /// <summary>
        /// filter on IfcObjectDefinition objects
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>bool true = exclude</returns>
        public bool ObjFilter(IfcObjectDefinition obj, bool checkType = true)
        {
            bool exclude = false;
            if (obj is IfcProduct)
            {
                exclude = IfcProductFilter.ItemsFilter(obj);
                //check the element is not defined by a type which is excluded, by default if no type, then no element included
                if (!exclude && checkType)
                {
                    IfcTypeObject objType = ((IfcProduct)obj).IsDefinedBy.OfType<IfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).FirstOrDefault(); //assuming only one IfcRelDefinesByType
                    if (objType != null) //if no type defined lets include it for now
                    {
                        exclude = IfcTypeObjectFilter.ItemsFilter(objType); 
                    }
                }
                
            }
            else if (obj is IfcTypeProduct)
            {
                exclude = IfcTypeObjectFilter.ItemsFilter(obj);
            }
            return FlipResult ? !exclude : exclude;
        }
        #endregion

        #region Merge Roles
        /// <summary>
        /// Merge OutPutFilters
        /// </summary>
        /// <param name="mergeFilter">OutPutFilters</param>
        public void Merge(OutPutFilters mergeFilter)
        {
            IfcProductFilter.MergeInc(mergeFilter.IfcProductFilter);
            IfcTypeObjectFilter.MergeInc(mergeFilter.IfcTypeObjectFilter);
            IfcAssemblyFilter.MergeInc(mergeFilter.IfcAssemblyFilter);

            ZoneFilter.Merge(mergeFilter.ZoneFilter);
            TypeFilter.Merge(mergeFilter.TypeFilter);
            SpaceFilter.Merge(mergeFilter.SpaceFilter);
            FloorFilter.Merge(mergeFilter.FloorFilter);
            FacilityFilter.Merge(mergeFilter.FacilityFilter);
            SpareFilter.Merge(mergeFilter.SpareFilter);
            ComponentFilter.Merge(mergeFilter.ComponentFilter);
            CommonFilter.Merge(mergeFilter.CommonFilter);

        }

        /// <summary>
        /// Extension method to use default role configuration resource files
        /// </summary>
        /// <param name="roles">MergeRoles, Flag enum with one or more roles</param>
        /// <param name="append">true = add, false = overwrite existing </param>
        /// <param name="rolesFilter">Dictionary of roles to OutPutFilters to use for merge, overwrites current assigned dictionary</param>
        public void ApplyRoleFilters(RoleFilter roles, bool append = false, Dictionary<RoleFilter, OutPutFilters> rolesFilter = null)
        {
            if (rolesFilter != null)
            {
                RolesFilterHolder = rolesFilter;
            }

            var init = append && !this.IsEmpty();
            
            OutPutFilters mergeFilter = null;
            foreach (RoleFilter role in Enum.GetValues(typeof(RoleFilter)))
            {
                if (roles.HasFlag(role))
                {
                    if (RolesFilterHolder.ContainsKey(role))
                    {
                        mergeFilter = RolesFilterHolder[role];
                    }
                    else
                    {   //load defaults
                        string mergeFile = GetDefaultRoleFile(role);
                        if (!string.IsNullOrEmpty(mergeFile))
                        {
                            mergeFilter = new OutPutFilters(mergeFile, role);
                            RolesFilterHolder[role] = mergeFilter;
                        }
                    }
                    if (mergeFilter != null)
                    {
                        if (!init)
                        {
                            this.Copy(mergeFilter); //start a fresh
                            init = true;//want to merge on next loop iteration
                        }
                        else
                        {
                            this.Merge(mergeFilter);
                        }
                    }
                    mergeFilter = null;
                }

            }
            //add the default property filters
            OutPutFilters defaultPropFilters = new OutPutFilters(null, RoleFilter.Unknown, ImportSet.PropertyFilters);
            this.Merge(defaultPropFilters);

            //save the applied roles at end as this.Copy(mergeFilter) would set to first role in RoleFilter
            AppliedRoles = roles; 
        }

        

        /// <summary>
        /// Set filters for Federated Model, referenced models
        /// </summary>
        /// <param name="modelRoleMap"></param>
        public Dictionary<T, OutPutFilters> SetFedModelFilter<T>(Dictionary<T, RoleFilter> modelRoleMap)
        {
            Dictionary<T, OutPutFilters> modelFilterMap = new Dictionary<T, OutPutFilters>();

            //save this filter before working out all fed models
            OutPutFilters saveFilter = new OutPutFilters();
            saveFilter.Copy(this);

            foreach (var item in modelRoleMap)
            {
                ApplyRoleFilters(item.Value);
                OutPutFilters roleFilter = new OutPutFilters();
                roleFilter.Copy(this);
                modelFilterMap.Add(item.Key, roleFilter);
            }

            this.Copy(saveFilter); //reset this filter back to state at top of function
            return modelFilterMap;
        }

        /// <summary>
        /// Fill RolesFilterHolder with default values
        /// </summary>
        public void FillDefaultRolesFilterHolder()
        {
            foreach (RoleFilter role in Enum.GetValues(typeof(RoleFilter)))
            {
                string roleFile = GetDefaultRoleFile(role);
                if (!string.IsNullOrEmpty(roleFile))
                {
                    RolesFilterHolder[role] = new OutPutFilters(roleFile, role);
                }
            }
        }


        /// <summary>
        /// Fill FilterHolder From Directory, if no file use defaults config files in assembly
        /// </summary>
        /// <param name="dir">DirectoryInfo</param>
        public void FillRolesFilterHolderFromDir(DirectoryInfo dir)
        {
            if (dir.Exists)
            {
                foreach (RoleFilter role in Enum.GetValues(typeof(RoleFilter)))
                {


                    string fileName = Path.Combine(dir.FullName, role.ToString() + "Filters.xml");
                    FileInfo fileInfo = new FileInfo(fileName);
                    if (fileInfo.Exists)
                    {
                        RolesFilterHolder[role] = DeserializeXML(fileInfo);
                    }
                    else
                    {
                        string roleFile = GetDefaultRoleFile(role);
                        if (!string.IsNullOrEmpty(roleFile))
                        {
                            RolesFilterHolder[role] = new OutPutFilters(roleFile, role);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Write to xml roleFilter files on passed directory
        /// </summary>
        /// <param name="dir">DirectoryInfo</param>
        public void WriteXMLRolesFilterHolderToDir(DirectoryInfo dir)
        {
            if (dir.Exists)
            {
                foreach (var item in RolesFilterHolder)
                {
                    string fileName = Path.Combine(dir.FullName, item.Key.ToString() + "Filters.xml");
                    FileInfo fileInfo = new FileInfo(fileName);
                    item.Value.SerializeXML(fileInfo);
                }
            }
        }

        /// <summary>
        /// Get stored role filter
        /// </summary>
        /// <param name="role">RoleFilter with single flag(role) set</param>
        /// <returns>OutPutFilters</returns>
        public OutPutFilters GetRoleFilter(RoleFilter role)
        {
             if ((role & (role - 1)) != 0)
            {
                throw new ArgumentException("More than one flag set on role");
            }
           
            if (RolesFilterHolder.ContainsKey(role))
            {
                return RolesFilterHolder[role];
            }
            else
            {   //load defaults
                var objFilter = GetDefaults(role);
                if (objFilter != null)
                {
                    RolesFilterHolder[role] = objFilter;
                }
            }
            return RolesFilterHolder[role];
        }

        /// <summary>
        /// Get the default filters for a single role
        /// </summary>
        /// <param name="role">RoleFilter with single flag(role) set</param>
        /// <returns>OutPutFilters</returns>
        public static OutPutFilters GetDefaults(RoleFilter role)
        {
             if ((role & (role - 1)) != 0)
            {
                throw new ArgumentException("More than one flag set on role");
            }
             string filterFile = GetDefaultRoleFile(role);
             if (!string.IsNullOrEmpty(filterFile))
             {
                 return new OutPutFilters(filterFile, role);
             }
             return null;
        }

        

        /// <summary>
        /// Add filter for a role, used by ApplyRoleFilters for none default filters
        /// </summary>
        /// <param name="role">RoleFilter, single flag RoleFilter</param>
        /// <param name="filter">OutPutFilters to assign to role</param>
        /// <remarks>Does not apply filter to this object, used ApplyRoleFilters after setting the RolesFilterHolder items </remarks>
        public void AddRoleFilterHolderItem(RoleFilter role, OutPutFilters filter)
        {
            if ((role & (role - 1)) != 0)
            {
                throw new ArgumentException("More than one flag set on role");
            }

            RolesFilterHolder[role] = filter;
        }

        /// <summary>
        /// Get default embedded resource file for a specified role
        /// </summary>
        /// <param name="role">RoleFilter</param>
        /// <returns></returns>
        private static string GetDefaultRoleFile(RoleFilter role)
        {
            if ((role & (role - 1)) != 0)
            {
                throw new ArgumentException("More than one flag set on role");
            }
            switch (role)
            {
                case RoleFilter.Unknown:
                    return "Xbim.COBieLiteUK.FilterHelper.COBieDefaultFilters.config";
                case RoleFilter.Architectural:
                    return "Xbim.COBieLiteUK.FilterHelper.COBieArchitecturalFilters.config";
                case RoleFilter.Mechanical:
                    return "Xbim.COBieLiteUK.FilterHelper.COBieMechanicalFilters.config";
                case RoleFilter.Electrical:
                    return "Xbim.COBieLiteUK.FilterHelper.COBieElectricalFilters.config";
                case RoleFilter.Plumbing:
                    return "Xbim.COBieLiteUK.FilterHelper.COBiePlumbingFilters.config";
                case RoleFilter.FireProtection:
                    return "Xbim.COBieLiteUK.FilterHelper.COBieFireProtectionFilters.config";
                default:
                    return string.Empty;
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

    /// <summary>
    /// used to control import
    /// </summary>
    public enum ImportSet
    {
        All,
        IfcFilters,
        PropertyFilters
    }
}
