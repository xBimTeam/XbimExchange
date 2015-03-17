using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Xsd2Code.Library.Helpers;

namespace Xsd2Code.Library
{
    public class GeneratorParamsBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    [Serializable]
    public class MiscellaneousParams : GeneratorParamsBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether [disable debug].
        /// </summary>
        /// <value><c>true</c> if [disable debug]; otherwise, <c>false</c>.</value>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating whether if generate attribute for debug into generated code.")]
        public bool DisableDebug { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if generate EditorBrowsableState.Never attribute
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating whether if generate EditorBrowsableState.Never attribute.")]
        public bool HidePrivateFieldInIde { get; set; }

        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating to exclude class generation types includes/imported into schema.")]
        public bool ExcludeIncludedTypes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if generate summary documentation
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating whether if generate summary documentation from xsd annotation.")]
        public bool EnableSummaryComment { get; set; }
    }

    [Serializable]
    public class PropertyParams : GeneratorParamsBase
    {
        private readonly GeneratorParams mainParamsFields;

        /// <summary>
        /// Indicate if use automatic properties
        /// </summary>
        private bool automaticPropertiesField;

        public PropertyParams(GeneratorParams mainParams)
        {
            mainParamsFields = mainParams;
        }

        /// <summary>
        /// Gets or sets a value indicating whether if implement INotifyPropertyChanged
        /// </summary>
        [Category("Property")]
        [DefaultValue(false)]
        [Description("Use lazy pattern when possible.")]
        public bool EnableLazyLoading { get; set; }

        [Category("Property")]
        [DefaultValue(false)]
        [Description("Enable/Disable virtual properties. Use with NHibernate.")]
        public bool EnableVirtualProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether serialize/deserialize method support
        /// </summary>
        [Category("Property")]
        [DefaultValue(false)]
        [Description("Generate automatic properties when possible. (Work only for csharp with target framework 3.0 or 3.5 and EnableDataBinding disable)")]
        public bool AutomaticProperties
        {
            get
            {
                return this.automaticPropertiesField;
            }

            set
            {
                if (value)
                {
                    if (this.mainParamsFields.TargetFramework != TargetFramework.Net20)
                    {
                        this.automaticPropertiesField = true;
                        this.mainParamsFields.EnableDataBinding = false;
                    }
                }
                else
                {
                    this.automaticPropertiesField = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if generate EditorBrowsableState.Never attribute
        /// </summary>
        [Category("Property")]
        [DefaultValue(false)]
        [Description("ShouldSerializeProperty is only useful for nullable type. If nullable type has no value,  the XMLSerialiser will skip the property.")]
        public bool GenerateShouldSerializeProperty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if generate EditorBrowsableState.Never attribute
        /// </summary>
        [Category("Property")]
        [DefaultValue(false)]
        [Description("[PropertyName]Specified property indicates whether the property should be ignored by the XMLSerialiser.")]
        public PropertyNameSpecifiedType GeneratePropertyNameSpecified { get; set; }
    }

    [Serializable]
    public class SerializeParams : GeneratorParamsBase
    {
        /// <summary>
        /// Gets or sets a value indicating the name of Serialize method.
        /// </summary>
        [Category("Serialize"), DefaultValue("Serialize"), Description("The name of Serialize method.")]
        public string SerializeMethodName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name of Deserialize method.
        /// </summary>
        [Category("Serialize"), DefaultValue("Deserialize"), Description("The name of deserialize method.")]
        public string DeserializeMethodName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name of Serialize method.
        /// </summary>
        [Category("Serialize"), DefaultValue("SaveToFile"), Description("The name of save to xml file method.")]
        public string SaveToFileMethodName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name of SaveToFile method.
        /// </summary>
        [Category("Serialize"), DefaultValue("LoadFromFile"), Description("The name of load from xml file method.")]
        public string LoadFromFileMethodName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether serialize/deserialize method support
        /// </summary>
        [Category("Serialize")]
        [DefaultValue(false)]
        [Description("Indicating whether serialize/deserialize method must be generate.")]
        public bool Enabled { get; set; }

        public override string ToString()
        {
            return string.Format("(Serialize methods={0})", Enabled);
        }

        /// <summary>
        /// Gets or sets a value indicating whether suppoort  .
        /// </summary>
        [Category("Serialize")]
        [DefaultValue(false)]
        [Description("Enable/Disable text encoding.")]
        public bool EnableEncoding { get; set; }

        /// <summary>
        /// Gets or sets a value indicating dafault encoder for serialize/deserialize
        /// </summary>
        [Category("Serialize")]
        [DefaultValue(DefaultEncoder.UTF8)]
        [Description("Specifies the default encoding for Xml Serialization (ASCII, UNICODE, UTF8...).")]
        public DefaultEncoder DefaultEncoder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if generate EditorBrowsableState.Never attribute
        /// </summary>
        [Category("Serialize")]
        [DefaultValue(false)]
        [Description("Indicating whether if generate .NET 2.0 serialization attributes. If false, serilisation will use propertyName")]
        public bool GenerateXmlAttributes { get; set; }

        private bool _generateOrderXmlAttributes;

        /// <summary>
        /// Gets or sets a value indicating the name of SaveToFile method.
        /// </summary>
        [Category("Serialize"), DefaultValue(false),
         Description("Generate order xml Attribute (Work only if GenerateXmlAttributes is true).")]
        public bool GenerateOrderXmlAttributes
        {
            get { return _generateOrderXmlAttributes; }
            set
            {
                _generateOrderXmlAttributes = value;
                OnPropertyChanged("GenerateOrderXmlAttributes");
            }
        }

        public string GetEncoderString()
        {
            switch (DefaultEncoder)
            {
                case DefaultEncoder.ASCII:
                    return "Encoding.ASCII";
                case DefaultEncoder.Unicode:
                    return "Encoding.Unicode";
                case DefaultEncoder.BigEndianUnicode:
                    return "Encoding.BigEndianUnicode";
                case DefaultEncoder.UTF32:
                    return "Encoding.UTF32";
                case DefaultEncoder.Default:
                    return "Encoding.Default";
            }
            return "Encoding.UTF8";
        }
    }


    [Serializable]
    public class TrackingChangesParams : GeneratorParamsBase
    {
        private bool enabledField;

        [DefaultValue(false)]
        public bool Enabled
        {
            get { return enabledField; }
            set
            {
                if (!enabledField.Equals(value))
                {
                    enabledField = value;
                    OnPropertyChanged("Enabled");
                }
            }
        }

        [DefaultValue(true), Description("If true, generate tracking changes classes inside [SchemaName].designed.cs file.")]
        public bool GenerateTrackingClasses { get; set; }

        public override string ToString()
        {
            return string.Format("(Tracking changes={0})", enabledField);
        }
    }

    public class GenericBaseClassParames : GeneratorParamsBase
    {
        private bool enabledField;

        [Category("Behavior"), DefaultValue(false), Description("Use generic patial base class for all methods")]
        public bool Enabled
        {
            get { return enabledField; }
            set
            {
                if (!enabledField.Equals(value))
                {
                    enabledField = value;
                    OnPropertyChanged("Enabled");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the name of Serialize method.
        /// </summary>
        [DefaultValue("EntityBase"), Description("Name of generic patial base class")]
        public string BaseClassName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name of Serialize method.
        /// </summary>
        [DefaultValue("true"), Description("Generate base class code inside [ShemaName].designer.cs file")]
        public bool GenerateBaseClass { get; set; }

        public override string ToString()
        {
            return string.Format("(Use generic base class={0})", enabledField);
        }
    }
    /// <summary>
    /// Represents all generation parameters
    /// </summary>
    /// <remarks>
    /// Develeper Notes:
    /// When adding a new parameter:
    /// 
    ///     Add the Property
    ///     Add it to ToXmlTag() (if for saving/loading)
    ///     Add it to LoadFromFile(string xsdFilePath, out string outputFile) (if for saving/loading)
    ///     Add The Tag Constant to GeneratorContext
    ///     Add it to the CommandLine Options Project Xsd2Code->EntryPoint.Main
    /// 
    /// Revision history:
    /// 
    ///     Modified 2009-02-20 by Ruslan Urban
    ///     Added TargetFramework and GenerateCloneMethod properties
    ///     Modified 2009-05-18 by Pascal Cabanel.
    ///     Added NET 2.0 serialization attributes as an option
    ///     Added The ability to create a default param: defaults.xsd2code,
    ///     
    /// </remarks>
    public class GeneratorParams
    {
        #region Private

        /// <summary>
        /// Type of collection
        /// </summary>
        private CollectionType collectionObjectTypeField = CollectionType.List;

        /// <summary>
        /// List of custom usings
        /// </summary>
        private List<NamespaceParam> customUsingsField = new List<NamespaceParam>();

        /// <summary>
        /// Indicate if use generic base class for isolate all methods
        /// </summary>
        private GenericBaseClassParames genericBaseClassField;

        /// <summary>
        /// Indicate if use tracking change algrithm.
        /// </summary>
        private TrackingChangesParams trackingChangesField;

        /// <summary>
        /// Serilisation params
        /// </summary>
        private SerializeParams serializeFiledField;

        /// <summary>
        /// Miscellaneous properties
        /// </summary>
        private MiscellaneousParams miscellaneousParamsField;

        /// <summary>
        /// Configure options properties
        /// </summary>
        private bool enableDataBindingField;

        /// <summary>
        /// Indicate if implement INotifyPropertyChanged
        /// </summary>
        private PropertyParams propertyParamsField;
        #endregion

        /// <summary>
        /// Indicate the target framework
        /// </summary>
        private TargetFramework targetFrameworkField = default(TargetFramework);


        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorParams"/> class.
        /// </summary>
        public GeneratorParams()
        {
            this.Serialization.LoadFromFileMethodName = "LoadFromFile";
            this.Serialization.SaveToFileMethodName = "SaveToFile";
            this.Serialization.DeserializeMethodName = "Deserialize";
            this.Serialization.SerializeMethodName = "Serialize";
            this.GenericBaseClass.BaseClassName = "EntityBase";
            this.GenericBaseClass.Enabled = false;
            this.EnableInitializeFields = true;
            this.Miscellaneous.ExcludeIncludedTypes = false;
            this.TrackingChanges.PropertyChanged += TrackingChangesPropertyChanged;
            this.Serialization.DefaultEncoder = DefaultEncoder.UTF8;
        }

        /// <summary>
        /// Trackings the changes property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        void TrackingChangesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Enabled")
            {
                if (this.TrackingChanges.Enabled)
                {
                    this.EnableDataBinding = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name space.
        /// </summary>
        /// <value>The name space.</value>
        [Category("Code")]
        [Description("namespace of generated file")]
        public string NameSpace { get; set; }

        /// <summary>
        /// Gets or sets generation language
        /// </summary>
        [Category("Code")]
        [Description("Language")]
        public GenerationLanguage Language { get; set; }

        /// <summary>
        /// Gets or sets the output file path.
        /// </summary>
        /// <value>The output file path.</value>
        [Browsable(false)]
        public string OutputFilePath { get; set; }

        /// <summary>
        /// Gets or sets the input file path.
        /// </summary>
        /// <value>The input file path.</value>
        [Browsable(false)]
        public string InputFilePath { get; set; }

        /// <summary>
        /// Gets or sets collection type to use for code generation
        /// </summary>
        [Category("Collection")]
        [Description("Set type of collection for unbounded elements")]
        public CollectionType CollectionObjectType
        {
            get { return this.collectionObjectTypeField; }
            set { this.collectionObjectTypeField = value; }
        }

        /// <summary>
        /// Gets or sets collection base
        /// </summary>
        [Category("Collection")]
        [Description("Set the collection base if using CustomCollection")]
        public string CollectionBase { get; set; }

        /// <summary>
        /// Gets or sets custom usings
        /// </summary>
        [Category("Code")]
        [Description("list of custom using for CustomCollection")]
        public List<NamespaceParam> CustomUsings
        {
            get { return this.customUsingsField; }
            set { this.customUsingsField = value; }
        }

        /// <summary>
        /// Gets or sets the custom usings string.
        /// </summary>
        /// <value>The custom usings string.</value>
        [Browsable(false)]
        public string CustomUsingsString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if implement INotifyPropertyChanged
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating whether if implement INotifyPropertyChanged.")]
        public bool EnableDataBinding
        {
            get
            {
                return this.enableDataBindingField;
            }

            set
            {
                this.enableDataBindingField = value;
                if (this.enableDataBindingField)
                {
                    this.PropertyParams.AutomaticProperties = false;
                }
                else
                {
                    this.TrackingChanges.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether serialize/deserialize method support
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Generate clone method based on MemberwiseClone")]
        public bool GenerateCloneMethod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether serialize/deserialize method support
        /// </summary>
        [Category("Code")]
        [DefaultValue(Library.TargetFramework.Net20)]
        [Description("Generated code base")]
        public TargetFramework TargetFramework
        {
            get
            {
                return this.targetFrameworkField;
            }

            set
            {
                this.targetFrameworkField = value;
                if (this.targetFrameworkField == TargetFramework.Net20)
                {
                    this.PropertyParams.AutomaticProperties = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if generate summary documentation
        /// </summary>
        [Category("Behavior")]
        [Description("Track changes.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Browsable(false)]
        public TrackingChangesParams TrackingChanges
        {
            get
            {
                if (trackingChangesField == null)
                {
                    trackingChangesField = new TrackingChangesParams();
                }
                return trackingChangesField;
            }

            set
            {
                trackingChangesField = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if generate summary documentation
        /// </summary>
        [Category("Behavior")]
        [Description("Configure options properties.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public PropertyParams PropertyParams
        {
            get
            {
                if (propertyParamsField == null)
                {
                    propertyParamsField = new PropertyParams(this);
                }
                return propertyParamsField;
            }

            set
            {
                propertyParamsField = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether if generate summary documentation
        /// </summary>
        [Category("Behavior")]
        [Description("XML Serilisation configuration.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SerializeParams Serialization
        {
            get
            {
                if (serializeFiledField == null)
                {
                    serializeFiledField = new SerializeParams();
                }
                return serializeFiledField;
            }

            set
            {
                serializeFiledField = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if generate summary documentation
        /// </summary>
        [Category("Behavior")]
        [Description("Miscellaneous")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MiscellaneousParams Miscellaneous
        {
            get
            {
                if (miscellaneousParamsField == null)
                {
                    miscellaneousParamsField = new MiscellaneousParams();
                }
                return miscellaneousParamsField;
            }

            set
            {
                miscellaneousParamsField = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if generate summary documentation
        /// </summary>
        [Category("Behavior")]
        [Description("Generic base class configuration.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public GenericBaseClassParames GenericBaseClass
        {
            get
            {
                if (genericBaseClassField == null)
                {
                    genericBaseClassField = new GenericBaseClassParames();
                }
                return genericBaseClassField;
            }

            set
            {
                genericBaseClassField = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if generate summary documentation
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Generate WCF data contract attributes")]
        public bool GenerateDataContracts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether accessing a property will initialize it
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Enable/Disable Global initialisation of the fields in both Constructors, Lazy Properties. Maximum override")]
        public bool EnableInitializeFields { get; set; }

        /// <summary>
        /// Loads from file.
        /// </summary>
        /// <param name="xsdFilePath">The XSD file path.</param>
        /// <returns>GeneratorParams instance</returns>
        public static GeneratorParams LoadFromFile(string xsdFilePath)
        {
            string outFile;
            return LoadFromFile(xsdFilePath, out outFile);
        }

        /// <summary>
        /// Loads from file.
        /// </summary>
        /// <param name="xsdFilePath">The XSD file path.</param>
        /// <param name="outputFile">The output file.</param>
        /// <returns>GeneratorParams instance</returns>

        public static GeneratorParams LoadFromFile(string xsdFilePath, out string outputFile)
        {
            var parameters = new GeneratorParams();


            #region Search generationFile
            outputFile = string.Empty;

            var localDir = Path.GetDirectoryName(xsdFilePath);

            var schemaConfigPath = Path.ChangeExtension(xsdFilePath, "xsd.xsd2code");

            var defaultsConfigPath = Path.Combine(localDir, "defaults.xsd2code");

            var configFile = string.Empty;

            //Try local <schemaName>.xsd.xsd2code if exist Use this file always.
            if (File.Exists(schemaConfigPath))
            {
                configFile = schemaConfigPath;
                //outputFile = Utility.GetOutputFilePath(xsdFilePath, language);
            }
            else
            {
                //Load from the output files if one of them exist
                foreach (GenerationLanguage language in Enum.GetValues(typeof(GenerationLanguage)))
                {
                    string fileName = Utility.GetOutputFilePath(xsdFilePath, language);
                    if (File.Exists(fileName))
                    {
                        outputFile = fileName;
                        configFile = fileName;
                        break;
                    }
                }
                //If there isn't a Schema Config File or an output File see if there is a defaults file
                if (outputFile.Length == 0)
                {
                    if (File.Exists(defaultsConfigPath))
                    {
                        configFile = defaultsConfigPath;
                    }
                }
            }


            if (configFile.Length == 0)
                return null;

            #endregion

            #region Try to get Last options

            //DCM Created Routine to Search for Auto-Generated Paramters
            var optionLine = ExtractAutoGeneratedParams(configFile);

            //DCM Fall back to old method because of some invalid Tag names
            if (optionLine == null)
            {
                using (TextReader streamReader = new StreamReader(configFile))
                {
                    streamReader.ReadLine();
                    streamReader.ReadLine();
                    streamReader.ReadLine();
                    optionLine = streamReader.ReadLine();
                }
            }


            if (optionLine != null)
            {
                parameters.NameSpace = optionLine.ExtractStrFromXML(GeneratorContext.NAMESPACETAG);
                parameters.CollectionObjectType = Utility.ToEnum<CollectionType>(optionLine.ExtractStrFromXML(GeneratorContext.COLLECTIONTAG));
                parameters.Language = Utility.ToEnum<GenerationLanguage>(optionLine.ExtractStrFromXML(GeneratorContext.CODETYPETAG));
                parameters.EnableDataBinding = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.ENABLEDATABINDINGTAG));
                parameters.PropertyParams.EnableLazyLoading = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.ENABLELAZYLOADINGTAG));
                parameters.Miscellaneous.HidePrivateFieldInIde = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.HIDEPRIVATEFIELDTAG));
                parameters.Miscellaneous.EnableSummaryComment = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.ENABLESUMMARYCOMMENTTAG));
                parameters.TrackingChanges.Enabled = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.ENABLETRACKINGCHANGESTAG));
                parameters.TrackingChanges.GenerateTrackingClasses = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.GENERATETRACKINGCLASSESTAG));
                parameters.Serialization.Enabled = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.INCLUDESERIALIZEMETHODTAG));
                parameters.Serialization.EnableEncoding = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.ENABLEENCODINGTAG));
                parameters.Serialization.DefaultEncoder = Utility.ToEnum<DefaultEncoder>(optionLine.ExtractStrFromXML(GeneratorContext.DEFAULTENCODERTAG));
                parameters.GenerateCloneMethod = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.GENERATECLONEMETHODTAG));
                parameters.GenerateDataContracts = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.GENERATEDATACONTRACTSTAG));
                parameters.TargetFramework = Utility.ToEnum<TargetFramework>(optionLine.ExtractStrFromXML(GeneratorContext.CODEBASETAG));
                parameters.Miscellaneous.DisableDebug = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.DISABLEDEBUGTAG));
                parameters.Serialization.GenerateXmlAttributes = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.GENERATEXMLATTRIBUTESTAG));
                parameters.Serialization.GenerateOrderXmlAttributes = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.ORDERXMLATTRIBUTETAG));
                parameters.PropertyParams.AutomaticProperties = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.AUTOMATICPROPERTIESTAG));
                parameters.PropertyParams.EnableVirtualProperties = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.ENABLEVIRTUALPROPERTIESTAG));
                parameters.GenericBaseClass.Enabled = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.USEGENERICBASECLASSTAG));
                parameters.GenericBaseClass.GenerateBaseClass = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.GENERATEBASECLASSTAG));
                parameters.PropertyParams.GenerateShouldSerializeProperty = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.GENERATESHOULDSERIALIZETAG));
                parameters.EnableInitializeFields = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.ENABLEINITIALIZEFIELDSTAG), true);
                parameters.Miscellaneous.ExcludeIncludedTypes = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.EXCLUDEINCLUDEDTYPESTAG));
                parameters.PropertyParams.GeneratePropertyNameSpecified = Utility.ToEnum<PropertyNameSpecifiedType>(optionLine.ExtractStrFromXML(GeneratorContext.GENERATEPROPERTYNAMESPECIFIEDTAG));

                string str = optionLine.ExtractStrFromXML(GeneratorContext.SERIALIZEMETHODNAMETAG);
                parameters.Serialization.SerializeMethodName = str.Length > 0 ? str : "Serialize";

                str = optionLine.ExtractStrFromXML(GeneratorContext.DESERIALIZEMETHODNAMETAG);
                parameters.Serialization.DeserializeMethodName = str.Length > 0 ? str : "Deserialize";

                str = optionLine.ExtractStrFromXML(GeneratorContext.SAVETOFILEMETHODNAMETAG);
                parameters.Serialization.SaveToFileMethodName = str.Length > 0 ? str : "SaveToFile";

                str = optionLine.ExtractStrFromXML(GeneratorContext.LOADFROMFILEMETHODNAMETAG);
                parameters.Serialization.LoadFromFileMethodName = str.Length > 0 ? str : "LoadFromFile";

                str = optionLine.ExtractStrFromXML(GeneratorContext.BASECLASSNAMETAG);
                parameters.GenericBaseClass.BaseClassName = str.Length > 0 ? str : "EntityBase";

                // TODO:get custom using
                string customUsingString = optionLine.ExtractStrFromXML(GeneratorContext.CUSTOMUSINGSTAG);
                if (!string.IsNullOrEmpty(customUsingString))
                {
                    string[] usings = customUsingString.Split(';');
                    foreach (string item in usings)
                        parameters.CustomUsings.Add(new NamespaceParam { NameSpace = item });
                }
                parameters.CollectionBase = optionLine.ExtractStrFromXML(GeneratorContext.COLLECTIONBASETAG);
            }

            return parameters;

            #endregion
        }

        /// <summary>
        /// Gets the params.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>GeneratorParams instance</returns>
        public static GeneratorParams GetParams(string parameters)
        {
            var newparams = new GeneratorParams();

            return newparams;
        }

        /// <summary>
        /// Save values into xml string
        /// </summary>
        /// <returns>xml string value</returns>
        public string ToXmlTag()
        {
            var optionsLine = new StringBuilder();

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.NAMESPACETAG, this.NameSpace));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.COLLECTIONTAG, this.CollectionObjectType.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.CODETYPETAG, this.Language.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.ENABLEDATABINDINGTAG, this.EnableDataBinding.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.ENABLELAZYLOADINGTAG, this.PropertyParams.EnableLazyLoading.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.ENABLETRACKINGCHANGESTAG, this.TrackingChanges.Enabled.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.GENERATETRACKINGCLASSESTAG, this.TrackingChanges.GenerateTrackingClasses.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.HIDEPRIVATEFIELDTAG, this.Miscellaneous.HidePrivateFieldInIde.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.ENABLESUMMARYCOMMENTTAG, this.Miscellaneous.EnableSummaryComment.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.ENABLEVIRTUALPROPERTIESTAG, this.PropertyParams.EnableVirtualProperties.ToString()));

            if (!string.IsNullOrEmpty(this.CollectionBase))
                optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.COLLECTIONBASETAG, this.CollectionBase));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.INCLUDESERIALIZEMETHODTAG, this.Serialization.Enabled.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.USEGENERICBASECLASSTAG, this.GenericBaseClass.Enabled.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.GENERATEBASECLASSTAG, this.GenericBaseClass.GenerateBaseClass.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.GENERATECLONEMETHODTAG, this.GenerateCloneMethod.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.GENERATEDATACONTRACTSTAG, this.GenerateDataContracts.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.CODEBASETAG, this.TargetFramework.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.SERIALIZEMETHODNAMETAG, this.Serialization.SerializeMethodName));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.DESERIALIZEMETHODNAMETAG, this.Serialization.DeserializeMethodName));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.SAVETOFILEMETHODNAMETAG, this.Serialization.SaveToFileMethodName));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.LOADFROMFILEMETHODNAMETAG, this.Serialization.LoadFromFileMethodName));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.GENERATEXMLATTRIBUTESTAG, this.Serialization.GenerateXmlAttributes.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.ORDERXMLATTRIBUTETAG, this.Serialization.GenerateOrderXmlAttributes.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.ENABLEENCODINGTAG, this.Serialization.EnableEncoding.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.AUTOMATICPROPERTIESTAG, this.PropertyParams.AutomaticProperties.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.GENERATESHOULDSERIALIZETAG, this.PropertyParams.GenerateShouldSerializeProperty.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.DISABLEDEBUGTAG, this.Miscellaneous.DisableDebug.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.GENERATEPROPERTYNAMESPECIFIEDTAG, this.PropertyParams.GeneratePropertyNameSpecified.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.DEFAULTENCODERTAG, this.Serialization.DefaultEncoder.ToString()));

            var customUsingsStr = new StringBuilder();
            if (this.CustomUsings != null)
            {
                foreach (NamespaceParam usingStr in this.CustomUsings.Where(usingStr => usingStr.NameSpace.Length > 0))
                {
                    customUsingsStr.Append(usingStr.NameSpace + ";");
                }

                // remove last ";"
                if (customUsingsStr.Length > 0)
                {
                    if (customUsingsStr[customUsingsStr.Length - 1] == ';')
                        customUsingsStr = customUsingsStr.Remove(customUsingsStr.Length - 1, 1);
                }

                optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                              GeneratorContext.CUSTOMUSINGSTAG,
                                                              customUsingsStr.ToString()));
            }

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.EXCLUDEINCLUDEDTYPESTAG, this.Miscellaneous.ExcludeIncludedTypes.ToString()));
            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.ENABLEINITIALIZEFIELDSTAG, this.EnableInitializeFields.ToString()));

            return optionsLine.ToString();
        }

        /// <summary>
        /// Shallow clone
        /// </summary>
        /// <returns></returns>
        public GeneratorParams Clone()
        {
            return MemberwiseClone() as GeneratorParams;
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns></returns>
        public Result Validate()
        {
            var result = new Result(true);

            #region Validate input

            if (string.IsNullOrEmpty(this.NameSpace))
            {
                result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the NameSpace");
            }

            if (this.CollectionObjectType.ToString() == CollectionType.DefinedType.ToString())
            {
                if (string.IsNullOrEmpty(this.CollectionBase))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the custom collection base type");
                }
            }

            if (this.Serialization.Enabled)
            {
                if (string.IsNullOrEmpty(this.Serialization.SerializeMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the Serialize method name.");
                }

                if (!IsValidMethodName(this.Serialization.SerializeMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, string.Format("Serialize method name {0} is invalid.",
                                                  this.Serialization.SerializeMethodName));
                }

                if (string.IsNullOrEmpty(this.Serialization.DeserializeMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the Deserialize method name.");
                }

                if (!IsValidMethodName(this.Serialization.DeserializeMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, string.Format("Deserialize method name {0} is invalid.",
                                                  this.Serialization.DeserializeMethodName));
                }

                if (string.IsNullOrEmpty(this.Serialization.SaveToFileMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the save to xml file method name.");
                }

                if (!IsValidMethodName(this.Serialization.SaveToFileMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, string.Format("Save to file method name {0} is invalid.",
                                                  this.Serialization.SaveToFileMethodName));
                }

                if (string.IsNullOrEmpty(this.Serialization.LoadFromFileMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the load from xml file method name.");
                }

                if (!IsValidMethodName(this.Serialization.LoadFromFileMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, string.Format("Load from file method name {0} is invalid.",
                                                  this.Serialization.LoadFromFileMethodName));
                }
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static bool IsValidMethodName(string value)
        {
            foreach (char item in value)
            {
                int ascii = Convert.ToInt16(item);
                if ((ascii < 65 || ascii > 90) && (ascii < 97 || ascii > 122) && (ascii != 8))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Extracts the auto generated params from a File.
        /// Method doesn't rely on the position with in the file.
        /// extracts the values contained in the GeneratorContext.AUTOGENERATEDTAG
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>String containing the Psuedo XML tags for Generator Params</returns>
        /// <remarks>Doesn't rely on Position with in the file</remarks>
        private static string ExtractAutoGeneratedParams(string filePath)
        {

            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("File containing Generator Parameters was not found {0}", filePath), filePath);
            }

            var options = new StringBuilder();
            using (var r = new StreamReader(filePath))
            {
                // Loop over each line in file
                bool appendLine = false;

                // Store line contents in this String.
                string line = null;

                do
                {
                    // Read in the next line.
                    line = r.ReadLine();

                    Console.WriteLine(line);

                    //skip the Empty lines
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    //Start appending
                    if ((line.ToUpper().Contains("<" + GeneratorContext.AUTOGENERATEDTAG.ToUpper() + ">")))
                    {
                        appendLine = true;
                    }

                    //No matter what Append this line
                    if ((appendLine == true))
                    {
                        options.Append(line).AppendLine();
                    }

                    //if this line contains the Closing tag exit
                    if ((line.ToUpper().Contains("</" + GeneratorContext.AUTOGENERATEDTAG.ToUpper() + ">")))
                    {
                        break;
                    }
                }

                while (!r.EndOfStream);
            }
            return options.ToString();
        }


    }
}