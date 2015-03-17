using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;
using System.Xml.Serialization;
using Xsd2Code.Library.Extensions;
using Xsd2Code.Library.Helpers;
using System.Xml;
using System.Linq;

namespace Xsd2Code.Library
{
    /// <summary>
    /// Generator class
    /// </summary>
    /// <remarks>
    /// Revision history:
    /// 
    ///     Modified 2009-02-20 by Ruslan Urban
    ///     Changed signature of the GeneratorFacade class constructor
    ///     Modified 2009-05-18 by Pascal Cabanel
    ///     Use CodeExtentionFactory
    /// </remarks>
    public sealed class Generator
    {
        /// <summary>
        /// Extension Namespace const
        /// </summary>
        public const string ExtensionNamespace = "http://www.myxaml.fr";

        /// <summary>
        /// Processes the specified XSD file.
        /// </summary>
        /// <param name="xsdFile">The XSD file.</param>
        /// <param name="targetNamespace">The target namespace.</param>
        /// <param name="language">The language.</param>
        /// <param name="collectionType">Type of the collection.</param>
        /// <param name="enableDataBinding">if set to <c>true</c> [enable data binding].</param>
        /// <param name="hidePrivate">if set to <c>true</c> [hide private].</param>
        /// <param name="enableSummaryComment">if set to <c>true</c> [enable summary comment].</param>
        /// <param name="customUsings">The custom usings.</param>
        /// <param name="collectionBase">The collection base.</param>
        /// <param name="includeSerializeMethod">if set to <c>true</c> [include serialize method].</param>
        /// <param name="serializeMethodName">Name of the serialize method.</param>
        /// <param name="deserializeMethodName">Name of the deserialize method.</param>
        /// <param name="saveToFileMethodName">Name of the save to file method.</param>
        /// <param name="loadFromFileMethodName">Name of the load from file method.</param>
        /// <param name="generateCloneMethod"></param>
        /// <param name="targetFramework"></param>
        /// <returns>result CodeNamespace</returns>
        [Obsolete("Do not use", true)]
        internal static Result<CodeNamespace> Process(string xsdFile, string targetNamespace,
                                                      GenerationLanguage language,
                                                      CollectionType collectionType, bool enableDataBinding,
                                                      bool hidePrivate,
                                                      bool enableSummaryComment, List<NamespaceParam> customUsings,
                                                      string collectionBase, bool includeSerializeMethod,
                                                      string serializeMethodName, string deserializeMethodName,
                                                      string saveToFileMethodName, string loadFromFileMethodName,
                                                      bool generateCloneMethod, TargetFramework targetFramework)
        {
            var generatorParams = new GeneratorParams
                                      {
                                          CollectionObjectType = collectionType,
                                          EnableDataBinding = enableDataBinding,
                                          Language = language,
                                          CustomUsings = customUsings,
                                          CollectionBase = collectionBase,
                                          GenerateCloneMethod = generateCloneMethod,
                                          TargetFramework = targetFramework
                                      };

            generatorParams.Miscellaneous.HidePrivateFieldInIde = hidePrivate;
            generatorParams.Miscellaneous.EnableSummaryComment = enableSummaryComment;
            generatorParams.Serialization.Enabled = includeSerializeMethod;
            generatorParams.Serialization.SerializeMethodName = serializeMethodName;
            generatorParams.Serialization.DeserializeMethodName = deserializeMethodName;
            generatorParams.Serialization.SaveToFileMethodName = saveToFileMethodName;
            generatorParams.Serialization.LoadFromFileMethodName = loadFromFileMethodName;

            return Process(generatorParams);
        }

        /// <summary>
        /// Initiate code generation process
        /// </summary>
        /// <param name="generatorParams">Generator parameters</param>
        /// <returns></returns>
        internal static Result<CodeNamespace> Process(GeneratorParams generatorParams)
        {
            var ns = new CodeNamespace();

            XmlReader reader = null;
            try
            {

                #region Set generation context

                GeneratorContext.GeneratorParams = generatorParams;

                #endregion

                #region Get XmlTypeMapping

                XmlSchema xsd;
                var schemas = new XmlSchemas();

                reader = XmlReader.Create(generatorParams.InputFilePath);
                xsd = XmlSchema.Read(reader, new ValidationEventHandler(Validate));

                var schemaSet = new XmlSchemaSet();
                schemaSet.Add(xsd);
                schemaSet.Compile();

                foreach (XmlSchema schema in schemaSet.Schemas())
                {
                    schemas.Add(schema);
                }

                var exporter = new XmlCodeExporter(ns);

                var generationOptions = CodeGenerationOptions.None;
                if (generatorParams.Serialization.GenerateOrderXmlAttributes)
                {
                    generationOptions = CodeGenerationOptions.GenerateOrder;
                }

                var importer = new XmlSchemaImporter(schemas, generationOptions, new ImportContext(new CodeIdentifiers(), false));

                foreach (XmlSchemaElement element in xsd.Elements.Values)
                {
                    var mapping = importer.ImportTypeMapping(element.QualifiedName);
                    exporter.ExportTypeMapping(mapping);
                }

                //Fixes/handles http://xsd2code.codeplex.com/WorkItem/View.aspx?WorkItemId=6941
                foreach (XmlSchemaType type in xsd.Items.OfType<XmlSchemaType>())
                {
                    var mapping = importer.ImportSchemaType(type.QualifiedName);
                    exporter.ExportTypeMapping(mapping);
                }

                #endregion

                #region Execute extensions

                var getExtensionResult = GeneratorFactory.GetCodeExtension(generatorParams);
                if (!getExtensionResult.Success) return new Result<CodeNamespace>(ns, false, getExtensionResult.Messages);

                var ext = getExtensionResult.Entity;
                ext.Process(ns, xsd);

                #endregion Execute extensions

                return new Result<CodeNamespace>(ns, true);
            }
            catch (Exception e)
            {
                return new Result<CodeNamespace>(ns, false, e.Message, MessageType.Error);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        private static void Validate(Object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error)
                throw new Exception("Schema validation failed:\n" + e.Message);
        }
    }
}