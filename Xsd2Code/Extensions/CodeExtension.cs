// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeExtension.cs" company="Xsd2Code">
//   N/A
// </copyright>
// <summary>
//   Base class for code generation extension
// </summary>
// <remarks>
//  Revision history:
//  Created 2009-03-16 by Ruslan Urban
//  based on GeneratorExtension.cs
//  Updated 2009-05-18 move wcf CodeDom generation into Net35Extention.cs by Pascal Cabanel
//  Updated 2009-05-18 Remove .Net 2.0 XML attributes by Pascal Cabanel
//  Updated 2009-06-16 Add EntityBase class.
//                     Add new serialize/deserialize methods.
//                     Dispose object in serialize/deserialize methods.
//  Updated 2010-01-07 Deerwood McCord Jr. (DCM) applied patch from Rob van der Veer
//  Updated 2010-01-20 Deerwood McCord Jr. Cleaned CodeSnippetStatements by replacing with specific CodeDom Expressions
//                     Refactored OnPropertyChanged to use more CodeDom Specific version found in CodeDomHelper.CreateOnPropertyChangeMethod()
//  Updated 2010-08-16 Pascal Cabanel. Add tracking changes class.
//                     Refactored GeneratorContext.GeneratorParams.
// </remarks>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Text;
using Xsd2Code.Library.Properties;

namespace Xsd2Code.Library.Extensions
{
    using System;
    using System.CodeDom;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using Helpers;

    public class ObjectList : List<object> { }

    /// <summary>
    /// Base class for code generation extension
    /// </summary>
    public abstract class CodeExtension : ICodeExtension
    {
        #region private fields
        /// <summary>
        /// Sorted list for custom collection
        /// </summary>
        private static readonly SortedList<string, string> CollectionTypes = new SortedList<string, string>();

        /// <summary>
        /// Contains all enum.
        /// </summary>
        private static List<string> enumListField;

        /// <summary>
        /// Contains all collection fields.
        /// </summary>
        private static readonly List<string> LazyLoadingFields = new List<string>();

        /// <summary>
        /// Contains all collection fields.
        /// </summary>
        protected static List<string> CollectionTypesFields = new List<string>();

        /// <summary>
        /// Contains all collection fields.
        /// </summary>
        protected static List<string> ShouldSerializeFields = new List<string>();

        /// <summary>
        /// List of public properties
        /// </summary>
        protected static List<string> PropertiesListFields = new List<string>();

        /// <summary>
        /// List of private fileds
        /// </summary>
        protected static List<string> MemberFieldsListFields = new List<string>();


        #endregion

        #region public method
        /// <summary>
        /// Process method for cs or vb CodeDom generation
        /// </summary>
        /// <param name="code">CodeNamespace generated</param>
        /// <param name="schema">XmlSchema to generate</param>
        public virtual void Process(CodeNamespace code, XmlSchema schema)
        {
            this.ImportNamespaces(code);
            var types = new CodeTypeDeclaration[code.Types.Count];
            code.Types.CopyTo(types, 0);

            // Generate generic base class
            if (GeneratorContext.GeneratorParams.Language == GenerationLanguage.CSharp && GeneratorContext.GeneratorParams.TrackingChanges.Enabled)
            {
                if (GeneratorContext.GeneratorParams.TrackingChanges.GenerateTrackingClasses)
                {
                    var classList = TrackingChangesExtention.GenerateTrackingChangesClasses();
                    foreach (var codeTypeDeclaration in classList)
                    {
                        code.Types.Insert(0, codeTypeDeclaration);
                    }
                }
            }

            // Generate generic base class
            if (GeneratorContext.GeneratorParams.GenericBaseClass.Enabled && GeneratorContext.GeneratorParams.GenericBaseClass.GenerateBaseClass)
            {
                code.Types.Insert(0, this.GenerateBaseClass());
            }

            enumListField = (from p in types
                             where p.IsEnum
                             select p.Name).ToList();

            foreach (var type in types)
            {
                CollectionTypes.Clear();
                LazyLoadingFields.Clear();
                CollectionTypesFields.Clear();

                // Fixes http://xsd2code.codeplex.com/WorkItem/View.aspx?WorkItemId=8781
                // and http://xsd2code.codeplex.com/WorkItem/View.aspx?WorkItemId=6944
                if (GeneratorContext.GeneratorParams.Miscellaneous.ExcludeIncludedTypes)
                {
                    //if the typeName is NOT defined in the current schema, skip it.
                    if (!ContainsTypeName(schema, type))
                    {
                        code.Types.Remove(type);
                        continue;
                    }
                }


                // Remove default remarks attribute
                type.Comments.Clear();

                // Remove default .Net 2.0 XML attributes if disabled or silverlight project.
                // Fixes http://xsd2code.codeplex.com/workitem/11761
                if (!GeneratorContext.GeneratorParams.Serialization.GenerateXmlAttributes
                    || GeneratorContext.GeneratorParams.TargetFramework == TargetFramework.Silverlight)
                {
                    this.RemoveDefaultXmlAttributes(type.CustomAttributes);
                }

                if (!type.IsClass && !type.IsStruct) continue;

                this.ProcessClass(code, schema, type);
            }

            foreach (var collName in CollectionTypes.Keys)
                this.CreateCollectionClass(code, collName);
        }

        /// <summary>
        /// Determines whether the specified schema contains the type.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if the specified schema contains the type; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>Used to Exclude Included Types from Schema</remarks>
        private bool ContainsTypeName(XmlSchema schema, CodeTypeDeclaration type)
        {
            foreach (var item in schema.Items)
            {
                var complexItem = item as XmlSchemaComplexType;
                if (complexItem != null)
                {
                    if (complexItem.Name == type.Name)
                    {
                        return true;
                    }
                }
                var elementItem = item as XmlSchemaElement;
                if (elementItem != null)
                {
                    if (elementItem.Name == type.Name)
                    {
                        return true;
                    }
                }
            }

            //TODO: Does not work for combined anonymous types 
            //fallback: Check if the namespace attribute of the type equals the namespace of the file.
            //first, find the XmlType attribute.
            foreach (CodeAttributeDeclaration attribute in type.CustomAttributes)
            {
                if (attribute.Name == "System.Xml.Serialization.XmlTypeAttribute")
                {
                    foreach (CodeAttributeArgument argument in attribute.Arguments)
                    {
                        if (argument.Name == "Namespace")
                        {
                            if (((CodePrimitiveExpression)argument.Value).Value == schema.TargetNamespace)
                            {
                                return true;
                            }
                        }
                    }
                }

            }

            return false;
        }
        #endregion

        #region protedted methods
        /// <summary>
        /// Generate defenition of the Clone() method
        /// </summary>
        /// <param name="type">Represents a type declaration for a class, structure, interface, or enumeration</param>
        /// <returns>return CodeDom clone method</returns>
        protected static CodeTypeMember GetCloneMethod(CodeTypeDeclaration type)
        {
            string typeName = GeneratorContext.GeneratorParams.GenericBaseClass.Enabled ? "T" : type.Name;

            // ----------------------------------------------------------------------
            // /// <summary>
            // /// Create clone of this TClass object
            // /// </summary>
            // public TClass Clone()
            // {
            //    return ((TClass)this.MemberwiseClone());
            // }
            // ----------------------------------------------------------------------
            var cloneMethod = new CodeMemberMethod
                                  {
                                      Attributes = MemberAttributes.Public,
                                      Name = "Clone",
                                      ReturnType = new CodeTypeReference(typeName)
                                  };

            CodeDomHelper.CreateSummaryComment(
                cloneMethod.Comments,
                string.Format("Create a clone of this {0} object", typeName));

            var memberwiseCloneMethod = new CodeMethodInvokeExpression(
                new CodeThisReferenceExpression(),
                "MemberwiseClone");

            var statement = new CodeMethodReturnStatement(new CodeCastExpression(typeName, memberwiseCloneMethod));
            cloneMethod.Statements.Add(statement);
            return cloneMethod;
        }

        protected static CodeTypeMember GetShouldSerializeMethod(string propertyName)
        {
            // ----------------------------------------------------------------------
            // /// <summary>
            // /// Test whether PropertyName should be serialized
            // /// </summary>
            // public bool ShouldSerializePropertyName()
            // {
            //    return this.PropertyName.HasValue;
            // }
            // ----------------------------------------------------------------------
            var cloneMethod = new CodeMemberMethod
                                  {
                                      Attributes = MemberAttributes.Public,
                                      Name = string.Format("ShouldSerialize{0}", propertyName),
                                      ReturnType = new CodeTypeReference(typeof(bool))
                                  };

            CodeDomHelper.CreateSummaryComment(
                cloneMethod.Comments,
                string.Format("Test whether {0} should be serialized", propertyName));

            var hasValueStatment = new CodeFieldReferenceExpression(null, string.Format("{0}.HasValue", propertyName));
            var statement = new CodeMethodReturnStatement(hasValueStatment);
            cloneMethod.Statements.Add(statement);
            return cloneMethod;
        }
        /// <summary>
        /// Processes the class.
        /// </summary>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="schema">The input xsd schema.</param>
        /// <param name="type">Represents a type declaration for a class, structure, interface, or enumeration</param>
        protected virtual void ProcessClass(CodeNamespace codeNamespace, XmlSchema schema, CodeTypeDeclaration type)
        {
            var addedToConstructor = false;
            var newCTor = false;

            var ctor = this.GetConstructor(type, ref newCTor);
            ShouldSerializeFields.Clear();
            MemberFieldsListFields.Clear();
            PropertiesListFields.Clear();

            // Inherits from EntityBase
            if (GeneratorContext.GeneratorParams.GenericBaseClass.Enabled && type.BaseTypes.Count == 0)
            {
                var ctr = new CodeTypeReference(GeneratorContext.GeneratorParams.GenericBaseClass.BaseClassName);
                ctr.TypeArguments.Add(new CodeTypeReference(type.Name));
                type.BaseTypes.Add(ctr);
            }
            else
            {
                if (GeneratorContext.GeneratorParams.EnableDataBinding)
                    type.BaseTypes.Add(typeof(INotifyPropertyChanged));
            }

            // Generate WCF DataContract
            this.CreateDataContractAttribute(type, schema);

            XmlSchemaElement currentElement = null;
            if (GeneratorContext.GeneratorParams.Miscellaneous.EnableSummaryComment)
                currentElement = this.CreateSummaryCommentFromSchema(type, schema, currentElement);

            foreach (CodeTypeMember member in type.Members)
            {
                // Remove default remarks attribute
                member.Comments.Clear();

                // Remove default .Net 2.0 XML attributes if disabled or silverlight project.
                // Fixes http://xsd2code.codeplex.com/workitem/11761
                if (!GeneratorContext.GeneratorParams.Serialization.GenerateXmlAttributes
                    || GeneratorContext.GeneratorParams.TargetFramework == TargetFramework.Silverlight)
                {
                    this.RemoveDefaultXmlAttributes(member.CustomAttributes);
                }

                var codeMember = member as CodeMemberField;
                if (codeMember != null)
                {
                    MemberFieldsListFields.Add(codeMember.Name);
                    this.ProcessFields(codeMember, ctor, codeNamespace, ref addedToConstructor);
                }

                var codeMemberProperty = member as CodeMemberProperty;
                if (codeMemberProperty != null)
                {
                    PropertiesListFields.Add(codeMemberProperty.Name);
                    this.ProcessProperty(type, codeNamespace, codeMemberProperty, currentElement, schema);
                }
            }

            //DCM: Moved From GeneraterFacade File based removal to CodeDom Style Attribute-based removal
            if (!GeneratorContext.GeneratorParams.Miscellaneous.DisableDebug)
            {
                this.RemoveDebugAttributes(type.CustomAttributes);
            }

            // Add new ctor if required
            if (addedToConstructor && newCTor)
                type.Members.Add(ctor);

            if (GeneratorContext.GeneratorParams.PropertyParams.GenerateShouldSerializeProperty)
            {
                foreach (var shouldSerialize in ShouldSerializeFields)
                {
                    this.CreateShouldSerializeMethod(type, shouldSerialize);
                }
            }

            // If don't use base class, generate all methods inside class
            if (!GeneratorContext.GeneratorParams.GenericBaseClass.Enabled)
            {
                if (GeneratorContext.GeneratorParams.EnableDataBinding)
                    this.CreateDataBinding(type);

                if (GeneratorContext.GeneratorParams.Serialization.Enabled)
                {
                    CreateStaticSerializer(type);
                    this.CreateSerializeMethods(type);
                }

                if (GeneratorContext.GeneratorParams.GenerateCloneMethod)
                    this.CreateCloneMethod(type);

                // Add plublic ObjectChangeTracker property
                if (GeneratorContext.GeneratorParams.TrackingChanges.Enabled)
                    this.CreateChangeTrackerProperty(type);
            }

            if (GeneratorContext.GeneratorParams.PropertyParams.GeneratePropertyNameSpecified != PropertyNameSpecifiedType.Default)
            {
                GeneratePropertyNameSpecified(type);
            }
        }

        /// <summary>
        /// Generates the property name specified.
        /// </summary>
        /// <param name="type">The type.</param>
        private static void GeneratePropertyNameSpecified(CodeTypeDeclaration type)
        {
            foreach (var propertyName in PropertiesListFields)
            {
                if (!propertyName.EndsWith("Specified"))
                {
                    CodeMemberProperty specifiedProperty = null;
                    // Search in all properties if PropertyNameSpecified exist
                    string searchPropertyName = string.Format("{0}Specified", propertyName);
                    specifiedProperty = CodeDomHelper.FindProperty(type, searchPropertyName);

                    if (specifiedProperty != null)
                    {
                        if (GeneratorContext.GeneratorParams.PropertyParams.GeneratePropertyNameSpecified == PropertyNameSpecifiedType.None)
                        {
                            type.Members.Remove(specifiedProperty);
                            var field = CodeDomHelper.FindField(type, CodeDomHelper.GetSpecifiedFieldName(propertyName));
                            if (field != null)
                            {
                                type.Members.Remove(field);
                            }
                        }
                    }
                    else
                    {
                        if (GeneratorContext.GeneratorParams.PropertyParams.GeneratePropertyNameSpecified == PropertyNameSpecifiedType.All)
                        {

                            CodeDomHelper.CreateBasicProperty(type, propertyName, typeof(bool), true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates the change tracker property.
        /// </summary>
        /// <param name="type">The type.</param>
        private void CreateChangeTrackerProperty(CodeTypeDeclaration type)
        {
            var changeTrackerPropertyPrivate = new CodeMemberField()
                                                   {
                                                       Attributes = MemberAttributes.Final | MemberAttributes.Private,
                                                       Name = "changeTrackerField",
                                                       Type = new CodeTypeReference("ObjectChangeTracker")
                                                   };

            type.Members.Add(changeTrackerPropertyPrivate);

            var changeTrackerProperty = new CodeMemberProperty()
                                            {
                                                Attributes = MemberAttributes.Final | MemberAttributes.Public,
                                                Name = "ChangeTracker",
                                                HasGet = true,
                                                Type = new CodeTypeReference("ObjectChangeTracker")
                                            };
            changeTrackerProperty.CustomAttributes.Add(new CodeAttributeDeclaration("XmlIgnore"));
            changeTrackerProperty.GetStatements.Add(this.CreateInstanceIfNotNull(changeTrackerPropertyPrivate.Name, changeTrackerProperty.Type, new CodeSnippetExpression("this")));
            changeTrackerProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeSnippetExpression("changeTrackerField")));
            type.Members.Add(changeTrackerProperty);
        }

        /// <summary>
        /// Create data binding
        /// </summary>
        /// <param name="type">Code type declaration</param>
        protected virtual void CreateDataBinding(CodeTypeDeclaration type)
        {
            // -------------------------------------------------------------------------------
            // public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
            // -------------------------------------------------------------------------------
            var propertyChangedEvent =
                new CodeMemberEvent
                    {
                        Attributes = MemberAttributes.Final | MemberAttributes.Public,
                        Name = "PropertyChanged",
                        Type =
                            new CodeTypeReference(typeof(PropertyChangedEventHandler))
                    };
            propertyChangedEvent.ImplementationTypes.Add(new CodeTypeReference("INotifyPropertyChanged"));
            type.Members.Add(propertyChangedEvent);

            var propertyChangedMethod = CodeDomHelper.CreatePropertyChangedMethod();

            type.Members.Add(propertyChangedMethod);
        }


        /// <summary>
        /// Creates the summary comment from schema.
        /// </summary>
        /// <param name="codeTypeDeclaration">The code type declaration.</param>
        /// <param name="schema">The input XML schema.</param>
        /// <param name="currentElement">The current element.</param>
        /// <returns>returns the element found otherwise null</returns>
        protected virtual XmlSchemaElement CreateSummaryCommentFromSchema(CodeTypeDeclaration codeTypeDeclaration, XmlSchema schema, XmlSchemaElement currentElement)
        {
            var xmlSchemaElement = this.SearchElementInSchema(codeTypeDeclaration, schema, new List<XmlSchema>());
            if (xmlSchemaElement != null)
            {
                currentElement = xmlSchemaElement;
                if (xmlSchemaElement.Annotation != null)
                {
                    foreach (var item in xmlSchemaElement.Annotation.Items)
                    {
                        var xmlDoc = item as XmlSchemaDocumentation;
                        if (xmlDoc == null) continue;
                        this.CreateCommentStatement(codeTypeDeclaration.Comments, xmlDoc);
                    }
                }
            }

            return currentElement;
        }

        /// <summary>
        /// Creates the collection class.
        /// </summary>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="collName">Name of the coll.</param>
        protected virtual void CreateCollectionClass(CodeNamespace codeNamespace, string collName)
        {
            var ctd = new CodeTypeDeclaration(collName) { IsClass = true };
            ctd.BaseTypes.Add(new CodeTypeReference(GeneratorContext.GeneratorParams.CollectionBase, new[] { new CodeTypeReference(CollectionTypes[collName]) }));

            ctd.IsPartial = true;

            bool newCTor = false;
            var ctor = this.GetConstructor(ctd, ref newCTor);

            ctd.Members.Add(ctor);
            codeNamespace.Types.Add(ctd);
        }

        /// <summary>
        /// Creates the clone method.
        /// </summary>
        /// <param name="codeTypeDeclaration">Represents a type declaration for a class, structure, interface, or enumeration.</param>
        protected virtual void CreateCloneMethod(CodeTypeDeclaration codeTypeDeclaration)
        {
            var cloneMethod = GetCloneMethod(codeTypeDeclaration);
            cloneMethod.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Clone method"));
            cloneMethod.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, "Clone method"));
            codeTypeDeclaration.Members.Add(cloneMethod);
        }

        /// <summary>
        /// Creates the serialize methods.
        /// </summary>
        /// <param name="type">Represents a type declaration for a class, structure, interface, or enumeration.</param>
        protected virtual void CreateSerializeMethods(CodeTypeDeclaration type)
        {
            // Serialize
            var ser = this.CreateSerializeMethod(type);
            ser.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Serialize/Deserialize"));
            type.Members.Add(ser);
            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                type.Members.Add(this.GetOverrideSerializeMethod(type));
            }

            // Deserialize
            type.Members.AddRange(this.GetOverrideDeserializeMethods(type));
            type.Members.Add(this.GetDeserializeMethod(type));

            // SaveToFile
            type.Members.AddRange(this.GetOverrideSaveToFileMethods(type));
            type.Members.Add(this.GetSaveToFileMethod());

            // LoadFromFile
            type.Members.AddRange(this.GetOverrideLoadFromFileMethods(type));
            var lff = this.GetLoadFromFileMethod(type);
            lff.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, "Serialize/Deserialize"));
            type.Members.Add(lff);
        }

        /// <summary>
        /// Gets the serialize CodeDOM method.
        /// </summary>
        /// <param name="type">The type object to serilize.</param>
        /// <returns>return the CodeDOM serialize method</returns>
        protected virtual CodeMemberMethod CreateSerializeMethod(CodeTypeDeclaration type)
        {
            var serializeMethod = new CodeMemberMethod
                                      {
                                          Attributes = MemberAttributes.Public,
                                          Name = GeneratorContext.GeneratorParams.Serialization.SerializeMethodName
                                      };

            var tryStatmanentsCol = new CodeStatementCollection();
            var finallyStatmanentsCol = new CodeStatementCollection();

            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                serializeMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Encoding), "encoding"));
            }
            // ------------------------------------------------------------
            // System.IO.StreamReader streamReader = null;
            // System.IO.MemoryStream memoryStream = null;
            // ------------------------------------------------------------
            serializeMethod.Statements.Add(
                    new CodeVariableDeclarationStatement(
                        new CodeTypeReference(typeof(StreamReader)),
                        "streamReader",
                        new CodePrimitiveExpression(null)));

            serializeMethod.Statements.Add(
                    new CodeVariableDeclarationStatement(
                        new CodeTypeReference(typeof(MemoryStream)),
                        "memoryStream",
                        new CodePrimitiveExpression(null)));

            tryStatmanentsCol.Add(new CodeAssignStatement(
                new CodeVariableReferenceExpression("memoryStream"),
                CodeDomHelper.CreateInstance(typeof(MemoryStream))));


            var textWriterOrStreamRef = new CodeTypeReferenceExpression("memoryStream");
            //if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            //{
            //    textWriterOrStreamRef = new CodeTypeReferenceExpression("xmlTextWriter");
            //    tryStatmanentsCol.Add(
            //        new CodeVariableDeclarationStatement(
            //            new CodeTypeReference(typeof(XmlTextWriter)),
            //            "xmlTextWriter",
            //            CodeDomHelper.CreateInstance(typeof(XmlTextWriter), "memoryStream", "encoding")));
            //}



            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                tryStatmanentsCol.Add(
                  new CodeVariableDeclarationStatement(
                      new CodeTypeReference(typeof(XmlWriterSettings)),
                      "xmlWriterSettings",
                      CodeDomHelper.CreateInstance(typeof(XmlWriterSettings))));

                tryStatmanentsCol.Add(new CodeAssignStatement(
                    new CodeVariableReferenceExpression("xmlWriterSettings.Encoding"),
                    new CodeArgumentReferenceExpression("encoding")));

                var createXmlWriter = CodeDomHelper.GetInvokeMethod("XmlWriter", "Create", new CodeExpression[]
                        {
                            new CodeArgumentReferenceExpression("memoryStream"),
                            new CodeArgumentReferenceExpression("xmlWriterSettings")
                        });

                textWriterOrStreamRef = new CodeTypeReferenceExpression("xmlWriter");
                tryStatmanentsCol.Add(
                    new CodeVariableDeclarationStatement(
                        new CodeTypeReference(typeof(XmlWriter)),
                        "xmlWriter",
                        createXmlWriter));
            }

            // --------------------------------------------------------------------------
            // Serializer.Serialize(memoryStream, this);
            // --------------------------------------------------------------------------

            tryStatmanentsCol.Add(
                CodeDomHelper.GetInvokeMethod(
                    "Serializer",
                    "Serialize",
                    new CodeExpression[]
                        {
                            textWriterOrStreamRef,
                            new CodeThisReferenceExpression()
                        }));

            // ---------------------------------------------------------------------------
            // memoryStream.Seek(0, SeekOrigin.Begin);
            // System.IO.StreamReader streamReader = new System.IO.StreamReader(memoryStream);
            // ---------------------------------------------------------------------------
            tryStatmanentsCol.Add(
                CodeDomHelper.GetInvokeMethod(
                                              "memoryStream",
                                              "Seek",
                                              new CodeExpression[]
                                                  {
                                                      new CodePrimitiveExpression(0),
                                                      new CodeTypeReferenceExpression("System.IO.SeekOrigin.Begin")
                                                  }));

            tryStatmanentsCol.Add(new CodeAssignStatement(
                new CodeVariableReferenceExpression("streamReader"),
                CodeDomHelper.CreateInstance(typeof(StreamReader), new[] { "memoryStream" })));

            var readToEnd = CodeDomHelper.GetInvokeMethod("streamReader", "ReadToEnd");
            tryStatmanentsCol.Add(new CodeMethodReturnStatement(readToEnd));

            finallyStatmanentsCol.Add(CodeDomHelper.GetDispose("streamReader"));
            finallyStatmanentsCol.Add(CodeDomHelper.GetDispose("memoryStream"));

            var tryfinallyStmt = new CodeTryCatchFinallyStatement(tryStatmanentsCol.ToArray(), new CodeCatchClause[0], finallyStatmanentsCol.ToArray());
            serializeMethod.Statements.Add(tryfinallyStmt);

            serializeMethod.ReturnType = new CodeTypeReference(typeof(string));

            // --------
            // Comments
            // --------
            serializeMethod.Comments.AddRange(
                CodeDomHelper.GetSummaryComment(string.Format("Serializes current {0} object into an XML document", type.Name)));

            serializeMethod.Comments.Add(CodeDomHelper.GetReturnComment("string XML value"));
            return serializeMethod;
        }

        /// <summary>
        /// Gets the serialize CodeDOM method.
        /// </summary>
        /// <param name="type">The type object to serilize.</param>
        /// <returns>return the CodeDOM serialize method</returns>
        protected virtual CodeMemberMethod GetOverrideSerializeMethod(CodeTypeDeclaration type)
        {
            var serializeMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public,
                Name = GeneratorContext.GeneratorParams.Serialization.SerializeMethodName
            };

            var serializeMethodInvoke = new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.Serialization.SerializeMethodName), new CodeExpression[] { new CodeArgumentReferenceExpression(GeneratorContext.GeneratorParams.Serialization.GetEncoderString()) });

            serializeMethod.Statements.Add(new CodeMethodReturnStatement(serializeMethodInvoke));
            serializeMethod.ReturnType = new CodeTypeReference(typeof(string));
            return serializeMethod;
        }
        /// <summary>
        /// Get Deserialize method
        /// </summary>
        /// <param name="type">represent a type declaration of class</param>
        /// <returns>Deserialize CodeMemberMethod</returns>
        protected virtual CodeMemberMethod GetDeserializeMethod(CodeTypeDeclaration type)
        {
            var deserializeTypeName = GeneratorContext.GeneratorParams.GenericBaseClass.Enabled ? "T" : type.Name;

            // ---------------------------------------
            // public static T Deserialize(string xml)
            // ---------------------------------------
            var deserializeMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Name = GeneratorContext.GeneratorParams.Serialization.DeserializeMethodName
            };

            deserializeMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "xml"));
            deserializeMethod.ReturnType = new CodeTypeReference(deserializeTypeName);

            deserializeMethod.Statements.Add(
                new CodeVariableDeclarationStatement(
                    new CodeTypeReference(typeof(StringReader)),
                    "stringReader",
                    new CodePrimitiveExpression(null)));

            var tryStatmanentsCol = new CodeStatementCollection();
            var finallyStatmanentsCol = new CodeStatementCollection();

            // ------------------------------------------------------
            // stringReader = new StringReader(xml);
            // ------------------------------------------------------
            var deserializeStatmanents = new CodeStatementCollection();

            tryStatmanentsCol.Add(new CodeAssignStatement(
                          new CodeVariableReferenceExpression("stringReader"),
                          new CodeObjectCreateExpression(
                              new CodeTypeReference(typeof(StringReader)),
                              new CodeExpression[] { new CodeArgumentReferenceExpression("xml") })));

            // ----------------------------------------------------------
            // obj = (ClassName)serializer.Deserialize(xmlReader);
            // return true;
            // ----------------------------------------------------------
            var deserialize = CodeDomHelper.GetInvokeMethod(
                                                            "Serializer",
                                                            "Deserialize",
                                                            new CodeExpression[]
                                                            {
                                                                CodeDomHelper.GetInvokeMethod(
                                                                "System.Xml.XmlReader", 
                                                                "Create", 
                                                                new CodeExpression[] { new CodeVariableReferenceExpression("stringReader") })
                                                            });

            var castExpr = new CodeCastExpression(deserializeTypeName, deserialize);
            var returnStmt = new CodeMethodReturnStatement(castExpr);

            tryStatmanentsCol.Add(returnStmt);
            tryStatmanentsCol.AddRange(deserializeStatmanents);

            finallyStatmanentsCol.Add(CodeDomHelper.GetDispose("stringReader"));

            var tryfinallyStmt = new CodeTryCatchFinallyStatement(tryStatmanentsCol.ToArray(), new CodeCatchClause[0], finallyStatmanentsCol.ToArray());
            deserializeMethod.Statements.Add(tryfinallyStmt);

            return deserializeMethod;
        }

        /// <summary>
        /// Get Deserialize method
        /// </summary>
        /// <param name="type">represent a type declaration of class</param>
        /// <returns>Deserialize CodeMemberMethod</returns>
        protected virtual CodeMemberMethod[] GetOverrideDeserializeMethods(CodeTypeDeclaration type)
        {
            var deserializeMethodList = new List<CodeMemberMethod>();
            string deserializeTypeName = GeneratorContext.GeneratorParams.GenericBaseClass.Enabled ? "T" : type.Name;

            // -------------------------------------------------------------------------------------
            // public static bool Deserialize(string xml, out T obj, out System.Exception exception)
            // -------------------------------------------------------------------------------------
            var deserializeMethod = new CodeMemberMethod
                                        {
                                            Attributes = MemberAttributes.Public | MemberAttributes.Static,
                                            Name = GeneratorContext.GeneratorParams.Serialization.DeserializeMethodName
                                        };

            deserializeMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "xml"));

            var param = new CodeParameterDeclarationExpression(deserializeTypeName, "obj") { Direction = FieldDirection.Out };
            deserializeMethod.Parameters.Add(param);

            param = new CodeParameterDeclarationExpression(typeof(Exception), "exception") { Direction = FieldDirection.Out };

            deserializeMethod.Parameters.Add(param);

            deserializeMethod.ReturnType = new CodeTypeReference(typeof(bool));

            // -----------------
            // exception = null;
            // -----------------
            deserializeMethod.Statements.Add(
                new CodeAssignStatement(
                    new CodeArgumentReferenceExpression("exception"),
                    new CodePrimitiveExpression(null)));

            // -----------------
            // obj = default(T);
            // -----------------
            deserializeMethod.Statements.Add(
                        new CodeAssignStatement(
                          new CodeArgumentReferenceExpression("obj"),
                            new CodeDefaultValueExpression(new CodeTypeReference(deserializeTypeName))
                            ));

            // ---------------------
            // try {...} catch {...}
            // ---------------------
            var tryStatmanentsCol = new CodeStatementCollection();

            // Call Desrialize method
            var deserializeInvoke =
                new CodeMethodInvokeExpression(
                      new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.Serialization.DeserializeMethodName),
                        new CodeExpression[] { new CodeArgumentReferenceExpression("xml") });

            tryStatmanentsCol.Add(
                new CodeAssignStatement(
                    new CodeArgumentReferenceExpression("obj"),
                        deserializeInvoke));

            tryStatmanentsCol.Add(CodeDomHelper.GetReturnTrue());

            // catch
            var catchClauses = CodeDomHelper.GetCatchClause();

            var trycatch = new CodeTryCatchFinallyStatement(tryStatmanentsCol.ToArray(), catchClauses);
            deserializeMethod.Statements.Add(trycatch);

            // --------
            // Comments
            // --------
            deserializeMethod.Comments.AddRange(
                CodeDomHelper.GetSummaryComment(string.Format("Deserializes workflow markup into an {0} object", type.Name)));

            deserializeMethod.Comments.Add(CodeDomHelper.GetParamComment("xml", "string workflow markup to deserialize"));
            deserializeMethod.Comments.Add(CodeDomHelper.GetParamComment("obj", string.Format("Output {0} object", type.Name)));
            deserializeMethod.Comments.Add(CodeDomHelper.GetParamComment("exception", "output Exception value if deserialize failed"));

            deserializeMethod.Comments.Add(
                CodeDomHelper.GetReturnComment("true if this XmlSerializer can deserialize the object; otherwise, false"));
            deserializeMethodList.Add(deserializeMethod);

            // -----------------------------------------------------
            // public static bool Deserialize(string xml, out T obj)
            // -----------------------------------------------------
            deserializeMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Name = GeneratorContext.GeneratorParams.Serialization.DeserializeMethodName
            };
            deserializeMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "xml"));
            deserializeMethod.ReturnType = new CodeTypeReference(typeof(bool));

            param = new CodeParameterDeclarationExpression(deserializeTypeName, "obj") { Direction = FieldDirection.Out };
            deserializeMethod.Parameters.Add(param);

            // ---------------------------
            // Exception exception = null;
            // ---------------------------
            deserializeMethod.Statements.Add(
            new CodeVariableDeclarationStatement(typeof(Exception), "exception", new CodePrimitiveExpression(null)));

            // ------------------------------------------------
            // return Deserialize(xml, out obj, out exception);
            // ------------------------------------------------
            var xmlStringParam = new CodeArgumentReferenceExpression("xml");
            var objParam = new CodeDirectionExpression(
                FieldDirection.Out, new CodeFieldReferenceExpression(null, "obj"));

            var expParam = new CodeDirectionExpression(
                FieldDirection.Out, new CodeFieldReferenceExpression(null, "exception"));

            deserializeInvoke =
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.Serialization.DeserializeMethodName),
                    new CodeExpression[] { xmlStringParam, objParam, expParam });

            var returnStmt = new CodeMethodReturnStatement(deserializeInvoke);
            deserializeMethod.Statements.Add(returnStmt);
            deserializeMethodList.Add(deserializeMethod);
            return deserializeMethodList.ToArray();
        }

        /// <summary>
        /// Gets the save to file code DOM method.
        /// </summary>
        /// <returns>
        /// return the save to file code DOM method statment
        /// </returns>
        protected virtual CodeMemberMethod GetSaveToFileMethod()
        {
            // -----------------------------------------------
            // public virtual void SaveToFile(string fileName)
            // -----------------------------------------------
            var saveToFileMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public,
                Name = GeneratorContext.GeneratorParams.Serialization.SaveToFileMethodName
            };

            saveToFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));

            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                saveToFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Encoding), "encoding"));
            }

            saveToFileMethod.Statements.Add(
                new CodeVariableDeclarationStatement(
                    new CodeTypeReference(typeof(StreamWriter)),
                    "streamWriter",
                    new CodePrimitiveExpression(null)));

            // ------------------------
            // try {...} finally {...}
            // -----------------------
            var tryExpression = new CodeStatementCollection();

            // ---------------------------------------
            // string xmlString = Serialize(encoding);
            // ---------------------------------------

            CodeMethodInvokeExpression serializeMethodInvoke;
            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                serializeMethodInvoke = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(null,
                                                      GeneratorContext.GeneratorParams.Serialization.SerializeMethodName),
                    new CodeExpression[] { new CodeArgumentReferenceExpression("encoding") });
            }
            else
            {
                serializeMethodInvoke = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(null,
                                                      GeneratorContext.GeneratorParams.Serialization.SerializeMethodName));
            }


            var xmlString = new CodeVariableDeclarationStatement(
                new CodeTypeReference(typeof(string)), "xmlString", serializeMethodInvoke);

            tryExpression.Add(xmlString);

            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                // ----------------------------------------------------------------
                // streamWriter = new StreamWriter(fileName, false, Encoding.UTF8);
                // ----------------------------------------------------------------
                tryExpression.Add(new CodeAssignStatement(
                                      new CodeVariableReferenceExpression("streamWriter"),
                                      new CodeObjectCreateExpression(
                                          typeof(StreamWriter),
                                          new CodeExpression[]
                                              {
                                                  new CodeSnippetExpression("fileName"),
                                                  new CodeSnippetExpression("false"),
                                                  new CodeSnippetExpression(GeneratorContext.GeneratorParams.Serialization.GetEncoderString())
                        })));
            }
            else
            {
                // --------------------------------------------------------------
                // System.IO.FileInfo xmlFile = new System.IO.FileInfo(fileName);
                // --------------------------------------------------------------
                tryExpression.Add(CodeDomHelper.CreateObject(typeof(FileInfo), "xmlFile", new[] { "fileName" }));

                // ----------------------------------------
                // StreamWriter Tex = xmlFile.CreateText();
                // ----------------------------------------
                var createTextMethodInvoke = CodeDomHelper.GetInvokeMethod("xmlFile", "CreateText");

                tryExpression.Add(
                    new CodeAssignStatement(
                        new CodeVariableReferenceExpression("streamWriter"),
                        createTextMethodInvoke));
            }
            // ----------------------------------
            // streamWriter.WriteLine(xmlString);
            // ----------------------------------
            var writeLineMethodInvoke =
                CodeDomHelper.GetInvokeMethod(
                                                "streamWriter",
                                                "WriteLine",
                                                new CodeExpression[]
                                                  {
                                                      new CodeVariableReferenceExpression("xmlString")
                                                  });

            tryExpression.Add(writeLineMethodInvoke);
            var closeMethodInvoke = CodeDomHelper.GetInvokeMethod("streamWriter", "Close");

            tryExpression.Add(closeMethodInvoke);

            var finallyStatmanentsCol = new CodeStatementCollection();
            finallyStatmanentsCol.Add(CodeDomHelper.GetDispose("streamWriter"));

            var trycatch = new CodeTryCatchFinallyStatement(tryExpression.ToArray(), new CodeCatchClause[0], finallyStatmanentsCol.ToArray());
            saveToFileMethod.Statements.Add(trycatch);

            return saveToFileMethod;
        }

        /// <summary>
        /// Gets the save to file code DOM method.
        /// </summary>
        /// <param name="type">CodeTypeDeclaration type.</param>
        /// <returns>
        /// return the save to file code DOM method statment
        /// </returns>
        protected virtual CodeMemberMethod[] GetOverrideSaveToFileMethods(CodeTypeDeclaration type)
        {
            var saveToFileMethodList = new List<CodeMemberMethod>();
            var saveToFileMethod = new CodeMemberMethod
                                       {
                                           Attributes = MemberAttributes.Public,
                                           Name = GeneratorContext.GeneratorParams.Serialization.SaveToFileMethodName
                                       };

            saveToFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));

            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                saveToFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Encoding), "encoding"));
            }

            var paramException = new CodeParameterDeclarationExpression(
                typeof(Exception), "exception")
                                     {
                                         Direction = FieldDirection.Out
                                     };

            saveToFileMethod.Parameters.Add(paramException);

            saveToFileMethod.ReturnType = new CodeTypeReference(typeof(bool));

            saveToFileMethod.Statements.Add(
                new CodeAssignStatement(new CodeArgumentReferenceExpression("exception"), new CodePrimitiveExpression(null)));

            // ---------------------
            // try {...} catch {...}
            // ---------------------
            var tryExpression = new CodeStatementCollection();

            // ---------------------
            // SaveToFile(fileName);
            // ---------------------
            var xmlStringParam = new CodeArgumentReferenceExpression("fileName");

            CodeMethodInvokeExpression saveToFileInvoke;
            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                saveToFileInvoke =
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.Serialization.SaveToFileMethodName),
                        new CodeExpression[] { xmlStringParam, new CodeArgumentReferenceExpression("encoding") });

            }
            else
            {
                saveToFileInvoke =
                   new CodeMethodInvokeExpression(
                       new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.Serialization.SaveToFileMethodName),
                        new CodeExpression[] { xmlStringParam });
            }

            tryExpression.Add(saveToFileInvoke);
            tryExpression.Add(CodeDomHelper.GetReturnTrue());

            // -----------
            // Catch {...}
            // -----------
            var catchstmts = new CodeStatementCollection();
            catchstmts.Add(new CodeAssignStatement(new CodeArgumentReferenceExpression("exception"), new CodeVariableReferenceExpression("e")));

            catchstmts.Add(CodeDomHelper.GetReturnFalse());
            var codeCatchClause = new CodeCatchClause("e", new CodeTypeReference(typeof(Exception)), catchstmts.ToArray());

            var codeCatchClauses = new[] { codeCatchClause };

            var trycatch = new CodeTryCatchFinallyStatement(tryExpression.ToArray(), codeCatchClauses);
            saveToFileMethod.Statements.Add(trycatch);

            saveToFileMethod.Comments.AddRange(
                CodeDomHelper.GetSummaryComment(string.Format("Serializes current {0} object into file", type.Name)));

            saveToFileMethod.Comments.Add(CodeDomHelper.GetParamComment("fileName", "full path of outupt xml file"));
            saveToFileMethod.Comments.Add(CodeDomHelper.GetParamComment("exception", "output Exception value if failed"));
            saveToFileMethod.Comments.Add(CodeDomHelper.GetReturnComment("true if can serialize and save into file; otherwise, false"));

            saveToFileMethodList.Add(saveToFileMethod);

            //--------------------------------------------------------------------------------
            // public virtual bool SaveToFile(string fileName, out System.Exception exception)
            //--------------------------------------------------------------------------------
            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                saveToFileMethod = new CodeMemberMethod
                                       {
                                           Attributes = MemberAttributes.Public,
                                           Name = GeneratorContext.GeneratorParams.Serialization.SaveToFileMethodName
                                       };

                CodeExpression[] encodeingArgs;
                encodeingArgs = new CodeExpression[]
                                        {
                                            xmlStringParam,
                                            new CodeArgumentReferenceExpression(
                                                GeneratorContext.GeneratorParams.Serialization.GetEncoderString()),
                                            new CodeDirectionExpression(FieldDirection.Out,
                                                                        new CodeSnippetExpression("exception"))
                                        };

                var saveToFileMethodInvoke = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        null,
                        GeneratorContext.GeneratorParams.Serialization.SaveToFileMethodName), encodeingArgs);

                saveToFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));
                saveToFileMethod.Parameters.Add(paramException);
                saveToFileMethod.Statements.Add(new CodeMethodReturnStatement(saveToFileMethodInvoke));
                saveToFileMethod.ReturnType = new CodeTypeReference(typeof(bool));

                saveToFileMethodList.Add(saveToFileMethod);
            }
            //--------------------------------------------------------------------------------
            // public virtual void SaveToFile(string fileName)
            //--------------------------------------------------------------------------------
            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                saveToFileMethod = new CodeMemberMethod
                                       {
                                           Attributes = MemberAttributes.Public,
                                           Name = GeneratorContext.GeneratorParams.Serialization.SaveToFileMethodName
                                       };

                var encodeingArgs = new CodeExpression[]
                                        {
                                            xmlStringParam,
                                            new CodeArgumentReferenceExpression(
                                                GeneratorContext.GeneratorParams.Serialization.GetEncoderString())
                                        };

                var saveToFileMethodInvoke = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        null,
                        GeneratorContext.GeneratorParams.Serialization.SaveToFileMethodName), encodeingArgs);

                saveToFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));
                saveToFileMethod.Statements.Add(saveToFileMethodInvoke);
                saveToFileMethodList.Add(saveToFileMethod);
            }
            return saveToFileMethodList.ToArray();
        }

        /// <summary>
        /// Gets the load from file CodeDOM method.
        /// </summary>
        /// <param name="type">The type CodeTypeDeclaration.</param>
        /// <returns>return the codeDom LoadFromFile method</returns>
        protected virtual CodeMemberMethod GetLoadFromFileMethod(CodeTypeDeclaration type)
        {
            var typeName = GeneratorContext.GeneratorParams.GenericBaseClass.Enabled ? "T" : type.Name;

            // ---------------------------------------------
            // public static T LoadFromFile(string fileName)
            // ---------------------------------------------
            var loadFromFileMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Name = GeneratorContext.GeneratorParams.Serialization.LoadFromFileMethodName
            };

            loadFromFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));
            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                loadFromFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Encoding), "encoding"));
            }

            loadFromFileMethod.ReturnType = new CodeTypeReference(typeName);

            loadFromFileMethod.Statements.Add(
                new CodeVariableDeclarationStatement(
                        new CodeTypeReference(typeof(FileStream)),
                        "file",
                        new CodePrimitiveExpression(null)));

            loadFromFileMethod.Statements.Add(
                new CodeVariableDeclarationStatement(
                        new CodeTypeReference(typeof(StreamReader)),
                        "sr",
                        new CodePrimitiveExpression(null)));

            var tryStatmanentsCol = new CodeStatementCollection();
            var finallyStatmanentsCol = new CodeStatementCollection();

            // ---------------------------------------------------------------------------
            // file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            // sr = new StreamReader(file);
            // ---------------------------------------------------------------------------
            tryStatmanentsCol.Add(
                new CodeAssignStatement(
                    new CodeVariableReferenceExpression("file"),
                    new CodeObjectCreateExpression(
                        typeof(FileStream),
                        new CodeExpression[]
                        {
                            new CodeArgumentReferenceExpression("fileName"),
                            CodeDomHelper.GetEnum("FileMode","Open"),
                            CodeDomHelper.GetEnum("FileAccess","Read")
                        })));

            CodeExpression[] codeParamExpression;
            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                codeParamExpression = new CodeExpression[] { new CodeVariableReferenceExpression("file"), new CodeVariableReferenceExpression("encoding") };
            }
            else
            {
                codeParamExpression = new CodeExpression[] { new CodeVariableReferenceExpression("file") };
            }
            tryStatmanentsCol.Add(
                new CodeAssignStatement(
                    new CodeVariableReferenceExpression("sr"),
                    new CodeObjectCreateExpression(
                        typeof(StreamReader), codeParamExpression
                       )));
            // ----------------------------------
            // string xmlString = sr.ReadToEnd();
            // ----------------------------------
            var readToEndInvoke = CodeDomHelper.GetInvokeMethod("sr", "ReadToEnd");

            var xmlString = new CodeVariableDeclarationStatement(
                new CodeTypeReference(typeof(string)), "xmlString", readToEndInvoke);

            tryStatmanentsCol.Add(xmlString);
            tryStatmanentsCol.Add(CodeDomHelper.GetInvokeMethod("sr", "Close"));
            tryStatmanentsCol.Add(CodeDomHelper.GetInvokeMethod("file", "Close"));

            // ------------------------------------------------------
            // return Deserialize(xmlString, out obj, out exception);
            // ------------------------------------------------------            
            var fileName = new CodeVariableReferenceExpression("xmlString");

            var deserializeInvoke =
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.Serialization.DeserializeMethodName),
                    new CodeExpression[] { fileName });

            var rstmts = new CodeMethodReturnStatement(deserializeInvoke);
            tryStatmanentsCol.Add(rstmts);

            finallyStatmanentsCol.Add(CodeDomHelper.GetDispose("file"));
            finallyStatmanentsCol.Add(CodeDomHelper.GetDispose("sr"));

            var tryfinally = new CodeTryCatchFinallyStatement(
                CodeDomHelper.CodeStmtColToArray(tryStatmanentsCol), new CodeCatchClause[0], CodeDomHelper.CodeStmtColToArray(finallyStatmanentsCol));

            loadFromFileMethod.Statements.Add(tryfinally);

            return loadFromFileMethod;
        }

        /// <summary>
        /// Gets the load from file CodeDOM method.
        /// </summary>
        /// <param name="type">The type CodeTypeDeclaration.</param>
        /// <returns>return the codeDom LoadFromFile method</returns>
        protected virtual CodeMemberMethod[] GetOverrideLoadFromFileMethods(CodeTypeDeclaration type)
        {
            string typeName = GeneratorContext.GeneratorParams.GenericBaseClass.Enabled ? "T" : type.Name;

            var teeType = new CodeTypeReference(typeName);
            var loadFromFileMethodList = new List<CodeMemberMethod>();
            var loadFromFileMethod = new CodeMemberMethod
                                         {
                                             Attributes = MemberAttributes.Public | MemberAttributes.Static,
                                             Name = GeneratorContext.GeneratorParams.Serialization.LoadFromFileMethodName
                                         };

            loadFromFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));

            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                loadFromFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Encoding), "encoding"));
            }

            var param = new CodeParameterDeclarationExpression(typeName, "obj") { Direction = FieldDirection.Out };
            loadFromFileMethod.Parameters.Add(param);

            param = new CodeParameterDeclarationExpression(typeof(Exception), "exception") { Direction = FieldDirection.Out };

            loadFromFileMethod.Parameters.Add(param);
            loadFromFileMethod.ReturnType = new CodeTypeReference(typeof(bool));

            // -----------------
            // exception = null;
            // obj = null;
            // -----------------
            loadFromFileMethod.Statements.Add(
                new CodeAssignStatement(new CodeArgumentReferenceExpression("exception"), new CodePrimitiveExpression(null)));

            loadFromFileMethod.Statements.Add(
                new CodeAssignStatement(new CodeArgumentReferenceExpression("obj"), new CodeDefaultValueExpression(teeType)));

            var tryStatmanentsCol = new CodeStatementCollection();

            // Call LoadFromFile method

            CodeExpression[] codeParamExpression;
            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                codeParamExpression = new CodeExpression[]
                {
                    new CodeArgumentReferenceExpression("fileName"),
                    new CodeArgumentReferenceExpression("encoding")
                };
            }
            else
            {
                codeParamExpression = new CodeExpression[]
                {
                    new CodeArgumentReferenceExpression("fileName")
                };
            }

            var loadFromFileInvoke =
                new CodeMethodInvokeExpression(
                      new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.Serialization.LoadFromFileMethodName), codeParamExpression);

            tryStatmanentsCol.Add(
                new CodeAssignStatement(
                    new CodeArgumentReferenceExpression("obj"),
                        loadFromFileInvoke));

            tryStatmanentsCol.Add(CodeDomHelper.GetReturnTrue());

            var trycatch = new CodeTryCatchFinallyStatement(
                CodeDomHelper.CodeStmtColToArray(tryStatmanentsCol), CodeDomHelper.GetCatchClause());

            loadFromFileMethod.Statements.Add(trycatch);

            loadFromFileMethod.Comments.AddRange(
                CodeDomHelper.GetSummaryComment(
                    string.Format("Deserializes xml markup from file into an {0} object", type.Name)));

            loadFromFileMethod.Comments.Add(CodeDomHelper.GetParamComment("fileName", "string xml file to load and deserialize"));
            loadFromFileMethod.Comments.Add(CodeDomHelper.GetParamComment("obj", string.Format("Output {0} object", type.Name)));
            loadFromFileMethod.Comments.Add(CodeDomHelper.GetParamComment("exception", "output Exception value if deserialize failed"));

            loadFromFileMethod.Comments.Add(
                CodeDomHelper.GetReturnComment("true if this XmlSerializer can deserialize the object; otherwise, false"));

            loadFromFileMethodList.Add(loadFromFileMethod);

            //-----------------------------------------------------
            // 
            //-----------------------------------------------------
            var fileName = new CodeArgumentReferenceExpression("fileName");
            var encoder = new CodeArgumentReferenceExpression(GeneratorContext.GeneratorParams.Serialization.GetEncoderString());
            var objParam = new CodeDirectionExpression(FieldDirection.Out, new CodeFieldReferenceExpression(null, "obj"));
            var expParam = new CodeDirectionExpression(FieldDirection.Out, new CodeFieldReferenceExpression(null, "exception"));

            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                loadFromFileMethod = new CodeMemberMethod
                                         {
                                             Attributes = MemberAttributes.Public | MemberAttributes.Static,
                                             Name =
                                                 GeneratorContext.GeneratorParams.Serialization.LoadFromFileMethodName
                                         };

                loadFromFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));

                param = new CodeParameterDeclarationExpression(typeName, "obj") { Direction = FieldDirection.Out };
                loadFromFileMethod.Parameters.Add(param);
                param = new CodeParameterDeclarationExpression(typeof(Exception), "exception") { Direction = FieldDirection.Out };
                loadFromFileMethod.Parameters.Add(param);
                loadFromFileMethod.ReturnType = new CodeTypeReference(typeof(bool));
                var loadFromFileMethodInvoke =
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(null,
                                                          GeneratorContext.GeneratorParams.Serialization.
                                                              LoadFromFileMethodName),
                        new CodeExpression[] { fileName, encoder, objParam, expParam });

                loadFromFileMethod.Statements.Add(new CodeMethodReturnStatement(loadFromFileMethodInvoke));
                loadFromFileMethodList.Add(loadFromFileMethod);
            }

            // ------------------------------------------------------
            // public static bool LoadFromFile(string xml, out T obj)
            // ------------------------------------------------------
            loadFromFileMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Name = GeneratorContext.GeneratorParams.Serialization.LoadFromFileMethodName
            };
            loadFromFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));
            loadFromFileMethod.ReturnType = new CodeTypeReference(typeof(bool));

            param = new CodeParameterDeclarationExpression(typeName, "obj") { Direction = FieldDirection.Out };
            loadFromFileMethod.Parameters.Add(param);

            // ---------------------------
            // Exception exception = null;
            // ---------------------------
            loadFromFileMethod.Statements.Add(
            new CodeVariableDeclarationStatement(typeof(Exception), "exception", new CodePrimitiveExpression(null)));

            // ------------------------------------------------------
            // return LoadFromFile(fileName, out obj, out exception);
            // ------------------------------------------------------
            var loadFromFileMethodInvok =
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.Serialization.LoadFromFileMethodName),
                    new CodeExpression[] { fileName, objParam, expParam });

            var returnStmt = new CodeMethodReturnStatement(loadFromFileMethodInvok);
            loadFromFileMethod.Statements.Add(returnStmt);
            loadFromFileMethodList.Add(loadFromFileMethod);
            //-----------------------------------------------------------
            // public static [TypeOfObject] LoadFromFile(string fileName)
            //-----------------------------------------------------------
            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                loadFromFileMethod = new CodeMemberMethod
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Static,
                    Name = GeneratorContext.GeneratorParams.Serialization.LoadFromFileMethodName
                };
                loadFromFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));
                loadFromFileMethod.ReturnType = new CodeTypeReference(typeName);

                var loadFromFileMethodInvoke = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        null,
                        GeneratorContext.GeneratorParams.Serialization.LoadFromFileMethodName), new CodeArgumentReferenceExpression("fileName"), new CodeArgumentReferenceExpression(GeneratorContext.GeneratorParams.Serialization.GetEncoderString()));

                returnStmt = new CodeMethodReturnStatement(loadFromFileMethodInvoke);
                loadFromFileMethod.Statements.Add(returnStmt);
                loadFromFileMethodList.Add(loadFromFileMethod);
            }
            return loadFromFileMethodList.ToArray();
        }

        /// <summary>
        /// Import namespaces
        /// </summary>
        /// <param name="code">Code namespace</param>
        protected virtual void ImportNamespaces(CodeNamespace code)
        {
            code.Imports.Add(new CodeNamespaceImport("System"));
            code.Imports.Add(new CodeNamespaceImport("System.Diagnostics"));
            code.Imports.Add(new CodeNamespaceImport("System.Xml.Serialization"));
            code.Imports.Add(new CodeNamespaceImport("System.Collections"));
            code.Imports.Add(new CodeNamespaceImport("System.Xml.Schema"));
            code.Imports.Add(new CodeNamespaceImport("System.ComponentModel"));

            if (GeneratorContext.GeneratorParams.CustomUsings != null)
            {
                foreach (var item in GeneratorContext.GeneratorParams.CustomUsings)
                    code.Imports.Add(new CodeNamespaceImport(item.NameSpace));
            }

            // Tracking changes
            if (GeneratorContext.GeneratorParams.Language == GenerationLanguage.CSharp && GeneratorContext.GeneratorParams.TrackingChanges.Enabled)
            {
                if (GeneratorContext.GeneratorParams.TrackingChanges.GenerateTrackingClasses)
                {
                    code.Imports.Add(new CodeNamespaceImport("System.Collections.Specialized"));
                    code.Imports.Add(new CodeNamespaceImport("System.Runtime.Serialization"));
                    code.Imports.Add(new CodeNamespaceImport("System.Collections.ObjectModel"));
                    code.Imports.Add(new CodeNamespaceImport("System.Reflection"));
                }
            }

            if (GeneratorContext.GeneratorParams.Serialization.Enabled)
            {
                code.Imports.Add(new CodeNamespaceImport("System.IO"));
                code.Imports.Add(new CodeNamespaceImport("System.Text"));
            }

            if (GeneratorContext.GeneratorParams.Serialization.EnableEncoding)
            {
                code.Imports.Add(new CodeNamespaceImport("System.Xml"));
            }
            switch (GeneratorContext.GeneratorParams.CollectionObjectType)
            {
                case CollectionType.IList:
                case CollectionType.List:
                    code.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
                    break;
                case CollectionType.ObservableCollection:
                    code.Imports.Add(new CodeNamespaceImport("System.Collections.ObjectModel"));
                    break;
                default:
                    break;
            }

            code.Name = GeneratorContext.GeneratorParams.NameSpace;
        }

        /// <summary>
        /// Create data contract attribute
        /// </summary>
        /// <param name="type">Code type declaration</param>
        /// <param name="schema">XML schema</param>
        protected virtual void CreateDataContractAttribute(CodeTypeDeclaration type, XmlSchema schema)
        {
            // abstract
        }

        /// <summary>
        /// Creates the data member attribute.
        /// </summary>
        /// <param name="prop">Represents a declaration for a property of a type.</param>
        protected virtual void CreateDataMemberAttribute(CodeMemberProperty prop)
        {
            // abstract
        }

        /// <summary>
        /// Recursive search of elemement.
        /// </summary>
        /// <param name="type">Element to search</param>
        /// <param name="xmlElement">Current element</param>
        /// <param name="currentElementName">Name of the current element.</param>
        /// <param name="hierarchicalElmtName">The hierarchical Elmt Name.</param>
        /// <returns>
        /// return found XmlSchemaElement or null value
        /// </returns>
        protected virtual XmlSchemaElement SearchElement(CodeTypeDeclaration type, XmlSchemaElement xmlElement, string currentElementName, string hierarchicalElmtName)
        {
            var found = false;
            if (type.IsClass)
            {
                if (xmlElement.Name == null)
                    return null;

                if (type.Name.Equals(hierarchicalElmtName + xmlElement.Name) ||
                    type.Name.Equals(xmlElement.Name))
                    found = true;
            }
            else
            {
                if (type.Name.Equals(xmlElement.QualifiedName.Name))
                    found = true;
            }

            if (found)
                return xmlElement;

            var xmlComplexType = xmlElement.ElementSchemaType as XmlSchemaComplexType;
            if (xmlComplexType != null)
            {
                var xmlSequence = xmlComplexType.ContentTypeParticle as XmlSchemaSequence;
                if (xmlSequence != null)
                {
                    foreach (XmlSchemaObject item in xmlSequence.Items)
                    {
                        var currentXmlSchemaElement = item as XmlSchemaElement;
                        if (currentXmlSchemaElement == null)
                            continue;

                        if (hierarchicalElmtName == xmlElement.QualifiedName.Name ||
                            currentElementName == xmlElement.QualifiedName.Name)
                            return null;

                        XmlSchemaElement subItem = this.SearchElement(
                                                                      type,
                                                                      currentXmlSchemaElement,
                                                                      xmlElement.QualifiedName.Name,
                                                                      hierarchicalElmtName + xmlElement.QualifiedName.Name);
                        if (subItem != null)
                            return subItem;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Create CodeCommentStatement from schema documentation.
        /// </summary>
        /// <param name="codeStatementColl">CodeCommentStatementCollection collection</param>
        /// <param name="xmlDoc">Schema documentation</param>
        protected virtual void CreateCommentStatement(
                                                     CodeCommentStatementCollection codeStatementColl,
                                                     XmlSchemaDocumentation xmlDoc)
        {
            codeStatementColl.Clear();
            foreach (XmlNode itemDoc in xmlDoc.Markup)
            {
                var textLine = itemDoc.InnerText.Trim();
                if (textLine.Length > 0)
                    CodeDomHelper.CreateSummaryComment(codeStatementColl, textLine);
            }
        }

        /// <summary>
        /// Field process.
        /// </summary>
        /// <param name="member">CodeTypeMember member</param>
        /// <param name="ctor">CodeMemberMethod constructor</param>
        /// <param name="ns">CodeNamespace XSD</param>
        /// <param name="addedToConstructor">Indicates if create a new constructor</param>
        protected virtual void ProcessFields(
                                            CodeTypeMember member,
                                            CodeMemberMethod ctor,
                                            CodeNamespace ns,
                                            ref bool addedToConstructor)
        {
            var field = (CodeMemberField)member;

            // ---------------------------------------------
            // [EditorBrowsable(EditorBrowsableState.Never)]
            // ---------------------------------------------
            if (member.Attributes == MemberAttributes.Private)
            {
                if (GeneratorContext.GeneratorParams.Miscellaneous.HidePrivateFieldInIde)
                {
                    var attributeType = new CodeTypeReference(
                        typeof(EditorBrowsableAttribute).Name.Replace("Attribute", string.Empty));

                    var argument = new CodeAttributeArgument
                                       {
                                           Value = CodeDomHelper.GetEnum(typeof(EditorBrowsableState).Name, "Never")
                                       };

                    field.CustomAttributes.Add(new CodeAttributeDeclaration(attributeType, new[] { argument }));
                }
            }

            // ------------------------------------------
            // protected virtual  List <Actor> nameField;
            // ------------------------------------------
            var thisIsCollectionType = field.Type.ArrayElementType != null;
            if (thisIsCollectionType)
            {
                field.Type = this.GetCollectionType(field.Type);
            }

            // ---------------------------------------
            // if ((this.nameField == null))
            // {
            //    this.nameField = new List<Name>();
            // }
            // ---------------------------------------
            if (GeneratorContext.GeneratorParams.EnableInitializeFields && GeneratorContext.GeneratorParams.CollectionObjectType != CollectionType.Array)
            {
                bool finded;
                CodeTypeDeclaration declaration = this.FindTypeInNamespace(field.Type.BaseType, ns, out finded);
                if (thisIsCollectionType ||
                    (((declaration != null) && declaration.IsClass)
                     && ((declaration.TypeAttributes & TypeAttributes.Abstract) != TypeAttributes.Abstract)))
                {
                    if (GeneratorContext.GeneratorParams.PropertyParams.EnableLazyLoading)
                    {
                        LazyLoadingFields.Add(field.Name);
                    }
                    else
                    {
                        if (field.Type.BaseType != "System.Byte")
                        {
                            ctor.Statements.Insert(0, this.CreateInstance(field.Name, field.Type));
                            addedToConstructor = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create a Class Constructor
        /// </summary>
        /// <param name="type">type of declaration</param>
        /// <returns>return CodeConstructor</returns>
        protected virtual CodeConstructor CreateClassConstructor(CodeTypeDeclaration type)
        {
            var ctor = new CodeConstructor { Attributes = MemberAttributes.Public, Name = type.Name };
            return ctor;
        }

        /// <summary>
        /// Create new instance of object
        /// </summary>
        /// <param name="name">Name of object</param>
        /// <param name="type">CodeTypeReference Type</param>
        /// <returns>return instance CodeConditionStatement</returns>
        protected virtual CodeConditionStatement CreateInstanceIfNotNull(string name, CodeTypeReference type, params CodeExpression[] parameters)
        {

            CodeAssignStatement statement;
            if (type.BaseType.Equals("System.String") && type.ArrayRank == 0)
            {
                statement =
                    new CodeAssignStatement(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), name),
                        CodeDomHelper.GetStaticField(typeof(String), "Empty"));
            }
            else
            {
                statement = CollectionInitilializerStatement(name, type, parameters);
            }

            return
                new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), name),
                        CodeBinaryOperatorType.IdentityEquality,
                        new CodePrimitiveExpression(null)),
                        new CodeStatement[] { statement });
        }

        /// <summary>
        /// Create new instance of object
        /// </summary>
        /// <param name="name">Name of object</param>
        /// <param name="type">CodeTypeReference Type</param>
        /// <returns>return instance CodeConditionStatement</returns>
        protected virtual CodeAssignStatement CreateInstance(string name, CodeTypeReference type)
        {
            CodeAssignStatement statement;
            if (type.BaseType.Equals("System.String") && type.ArrayRank == 0)
            {
                statement = new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), name), CodeDomHelper.GetStaticField(typeof(String), "Empty"));
            }
            else
            {
                statement = CollectionInitilializerStatement(name, type);
            }
            return statement;
        }


        /// <summary>
        /// Collections the initilializer statement.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static CodeAssignStatement CollectionInitilializerStatement(string name, CodeTypeReference type, params CodeExpression[] parameters)
        {
            CodeAssignStatement statement;
            // in case of Interface type the new statement must contain concrete class
            if (type.BaseType == typeof(IList<>).Name || type.BaseType == typeof(IList<>).FullName)
            {
                var cref = new CodeTypeReference(typeof(List<>));
                cref.TypeArguments.AddRange(type.TypeArguments);
                statement =
               new CodeAssignStatement(
                                       new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), name),
                                       new CodeObjectCreateExpression(cref, parameters));
            }
            else
                statement =
                        new CodeAssignStatement(
                                                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), name),
                                                new CodeObjectCreateExpression(type, parameters));
            return statement;
        }

        /// <summary>
        /// Recherche le CodeTypeDeclaration d'un objet en fonction de son type de base (nom de classe)
        /// </summary>
        /// <param name="typeName">Search name</param>
        /// <param name="ns">Seach into</param>
        /// <param name="finded">if set to <c>true</c> [finded].</param>
        /// <returns>CodeTypeDeclaration found</returns>
        protected virtual CodeTypeDeclaration FindTypeInNamespace(string typeName, CodeNamespace ns, out bool finded)
        {
            finded = false;
            foreach (CodeTypeDeclaration declaration in ns.Types)
            {
                if (declaration.Name == typeName)
                {
                    finded = true;
                    return declaration;
                }
            }
            return null;
        }

        /// <summary>
        /// Property process
        /// </summary>
        /// <param name="type">Represents a type declaration for a class, structure, interface, or enumeration</param>
        /// <param name="ns">The ns.</param>
        /// <param name="member">Type members include fields, methods, properties, constructors and nested types</param>
        /// <param name="xmlElement">Represent the root element in schema</param>
        /// <param name="schema">XML Schema</param>
        protected virtual void ProcessProperty(CodeTypeDeclaration type, CodeNamespace ns, CodeTypeMember member, XmlSchemaElement xmlElement, XmlSchema schema)
        {
            if (GeneratorContext.GeneratorParams.Miscellaneous.EnableSummaryComment)
            {
                if (xmlElement != null)
                {
                    var xmlComplexType = xmlElement.ElementSchemaType as XmlSchemaComplexType;
                    bool foundInAttributes = false;
                    if (xmlComplexType != null)
                    {
                        // Search property in attributes for summary comment generation
                        foreach (XmlSchemaObject attribute in xmlComplexType.Attributes)
                        {
                            var xmlAttrib = attribute as XmlSchemaAttribute;
                            if (xmlAttrib != null)
                            {
                                if (member.Name.Equals(xmlAttrib.QualifiedName.Name))
                                {
                                    this.CreateCommentFromAnnotation(xmlAttrib.Annotation, member.Comments);
                                    foundInAttributes = true;
                                }
                            }
                        }

                        // Search property in XmlSchemaElement for summary comment generation
                        if (!foundInAttributes)
                        {
                            var xmlSequence = xmlComplexType.ContentTypeParticle as XmlSchemaSequence;
                            if (xmlSequence != null)
                            {
                                foreach (XmlSchemaObject item in xmlSequence.Items)
                                {
                                    var currentItem = item as XmlSchemaElement;
                                    if (currentItem != null)
                                    {
                                        if (member.Name.Equals(currentItem.QualifiedName.Name))
                                            this.CreateCommentFromAnnotation(currentItem.Annotation, member.Comments);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var prop = (CodeMemberProperty)member;

            if (prop.Type.ArrayElementType != null)
            {
                prop.Type = this.GetCollectionType(prop.Type);
                CollectionTypesFields.Add(prop.Name);
            }

            if (GeneratorContext.GeneratorParams.PropertyParams.EnableVirtualProperties)
            {
                prop.Attributes ^= MemberAttributes.Final;
            }

            if (prop.Type.BaseType.Contains("System.Nullable"))
            {
                ShouldSerializeFields.Add(prop.Name);
            }

            if (GeneratorContext.GeneratorParams.GenerateDataContracts)
            {
                this.CreateDataMemberAttribute(prop);
            }

            if (GeneratorContext.GeneratorParams.EnableInitializeFields)
            {
                var propReturnStatment = prop.GetStatements[0] as CodeMethodReturnStatement;
                if (propReturnStatment != null)
                {
                    var field = propReturnStatment.Expression as CodeFieldReferenceExpression;
                    if (field != null)
                    {
                        if (LazyLoadingFields.IndexOf(field.FieldName) != -1)
                        {
                            prop.GetStatements.Insert(0, this.CreateInstanceIfNotNull(field.FieldName, prop.Type));
                        }
                    }
                }
            }

            // Add OnPropertyChanged in setter
            if (GeneratorContext.GeneratorParams.EnableDataBinding)
            {
                if (type.BaseTypes.IndexOf(new CodeTypeReference(typeof(CollectionBase))) == -1)
                {
                    // -----------------------------
                    // if (handler != null) {
                    //    OnPropertyChanged("PropertyName");
                    // -----------------------------
                    CodeExpression[] propertyChangeParams = null;
                    if (GeneratorContext.GeneratorParams.TrackingChanges.Enabled && GeneratorContext.GeneratorParams.Language == GenerationLanguage.CSharp)
                    {
                        propertyChangeParams = new CodeExpression[] { new CodePrimitiveExpression(prop.Name), new CodeArgumentReferenceExpression(("value")) };
                    }
                    else
                    {
                        propertyChangeParams = new CodeExpression[] { new CodePrimitiveExpression(prop.Name) };
                    }

                    var propChange = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "OnPropertyChanged"), propertyChangeParams);

                    var propAssignStatment = prop.SetStatements[0] as CodeAssignStatement;
                    if (propAssignStatment != null)
                    {
                        var cfreL = propAssignStatment.Left as CodeFieldReferenceExpression;
                        var cfreR = propAssignStatment.Right as CodePropertySetValueReferenceExpression;

                        if (cfreL != null)
                        {
                            var setValueCondition = new CodeStatementCollection { propAssignStatment, propChange };

                            // ---------------------------------------------
                            // if ((xxxField.Equals(value) != true)) { ... }
                            // ---------------------------------------------
                            var condStatmentCondEquals = new CodeConditionStatement(
                                new CodeBinaryOperatorExpression(
                                    new CodeMethodInvokeExpression(
                                        new CodeFieldReferenceExpression(
                                            null,
                                            cfreL.FieldName),
                                        "Equals",
                                        cfreR),
                                    CodeBinaryOperatorType.IdentityInequality,
                                    new CodePrimitiveExpression(true)),
                                CodeDomHelper.CodeStmtColToArray(setValueCondition));

                            // ---------------------------------------------
                            // if ((xxxField != null)) { ... }
                            // ---------------------------------------------
                            var condStatmentCondNotNull = new CodeConditionStatement(
                                new CodeBinaryOperatorExpression(
                                    new CodeFieldReferenceExpression(
                                        new CodeThisReferenceExpression(), cfreL.FieldName),
                                        CodeBinaryOperatorType.IdentityInequality,
                                        new CodePrimitiveExpression(null)),
                                        new CodeStatement[] { condStatmentCondEquals },
                                        CodeDomHelper.CodeStmtColToArray(setValueCondition));

                            var property = member as CodeMemberProperty;
                            if (property != null)
                            {
                                if (property.Type.BaseType != new CodeTypeReference(typeof(long)).BaseType &&
                                    property.Type.BaseType != new CodeTypeReference(typeof(DateTime)).BaseType &&
                                    property.Type.BaseType != new CodeTypeReference(typeof(float)).BaseType &&
                                    property.Type.BaseType != new CodeTypeReference(typeof(double)).BaseType &&
                                    property.Type.BaseType != new CodeTypeReference(typeof(int)).BaseType &&
                                    property.Type.BaseType != new CodeTypeReference(typeof(bool)).BaseType &&
                                    enumListField.IndexOf(property.Type.BaseType) == -1)
                                    prop.SetStatements[0] = condStatmentCondNotNull;
                                else
                                    prop.SetStatements[0] = condStatmentCondEquals;
                            }
                        }
                        else
                            prop.SetStatements.Add(propChange);
                    }
                }
            }
        }

        private void CreateShouldSerializeMethod(CodeTypeDeclaration type, string propertyName)
        {
            type.Members.Add(GetShouldSerializeMethod(propertyName));
        }

        /// <summary>
        /// Determines whether [is complex type] [the specified code type reference].
        /// </summary>
        /// <param name="codeTypeReference">The code type reference.</param>
        /// <param name="ns">The ns.</param>
        /// <param name="finded">if set to <c>true</c> [finded].</param>
        /// <returns>
        /// true if type is complex type (class, List, etc.)"/&gt;
        /// </returns>
        protected bool IsComplexType(CodeTypeReference codeTypeReference, CodeNamespace ns, out bool finded)
        {
            CodeTypeDeclaration declaration = this.FindTypeInNamespace(codeTypeReference.BaseType, ns, out finded);
            if (!finded)
            {
                return false;
            }
            return ((declaration != null) && declaration.IsClass) &&
                   ((declaration.TypeAttributes & TypeAttributes.Abstract) != TypeAttributes.Abstract);
        }

        /// <summary>
        /// Removes the default XML attributes.
        /// </summary>
        /// <param name="customAttributes">
        /// The custom Attributes.
        /// </param>
        protected virtual void RemoveDefaultXmlAttributes(CodeAttributeDeclarationCollection customAttributes)
        {
            var codeAttributes = new List<CodeAttributeDeclaration>();
            foreach (var attribute in customAttributes)
            {
                var attrib = attribute as CodeAttributeDeclaration;
                if (attrib == null)
                {
                    continue;
                }

                if (attrib.Name == "System.Xml.Serialization.XmlAttributeAttribute" ||
                    attrib.Name == "System.Xml.Serialization.XmlTypeAttribute" ||
                    attrib.Name == "System.Xml.Serialization.XmlElementAttribute" ||
                    attrib.Name == "System.CodeDom.Compiler.GeneratedCodeAttribute" ||
                    attrib.Name == "System.SerializableAttribute" ||
                    attrib.Name == "System.ComponentModel.DesignerCategoryAttribute" ||
                    attrib.Name == "System.Xml.Serialization.XmlRootAttribute")
                {
                    codeAttributes.Add(attrib);
                }
            }

            foreach (var item in codeAttributes)
            {
                customAttributes.Remove(item);
            }
        }

        /// <summary>
        /// Removes the debug attributes.
        /// </summary>
        /// <param name="customAttributes">The custom attributes Collection.</param>
        protected virtual void RemoveDebugAttributes(CodeAttributeDeclarationCollection customAttributes)
        {
            var codeAttributes = new List<CodeAttributeDeclaration>();
            foreach (var attribute in customAttributes)
            {
                var attrib = attribute as CodeAttributeDeclaration;
                if (attrib == null)
                {
                    continue;
                }

                if (attrib.Name == "System.Diagnostics.DebuggerStepThroughAttribute")
                {
                    codeAttributes.Add(attrib);
                }
            }
            //DCM: OK not sure why it in this loop other than its like a transaction.
            //Not going to touch it now.
            foreach (var item in codeAttributes)
            {
                customAttributes.Remove(item);
            }
        }

        /// <summary>
        /// Generate summary comment from XmlSchemaAnnotation 
        /// </summary>
        /// <param name="xmlSchemaAnnotation">XmlSchemaAnnotation from XmlSchemaElement or XmlSchemaAttribute</param>
        /// <param name="codeCommentStatementCollection">codeCommentStatementCollection from member</param>
        protected virtual void CreateCommentFromAnnotation(XmlSchemaAnnotation xmlSchemaAnnotation, CodeCommentStatementCollection codeCommentStatementCollection)
        {
            if (xmlSchemaAnnotation != null)
            {
                foreach (XmlSchemaObject annotation in xmlSchemaAnnotation.Items)
                {
                    var xmlDoc = annotation as XmlSchemaDocumentation;
                    if (xmlDoc != null)
                    {
                        this.CreateCommentStatement(codeCommentStatementCollection, xmlDoc);
                    }
                }
            }
        }

        /// <summary>
        /// Get CodeTypeReference for collection
        /// </summary>
        /// <param name="codeType">The code Type.</param>
        /// <returns>return array of or genereric collection</returns>
        protected virtual CodeTypeReference GetCollectionType(CodeTypeReference codeType)
        {
            CodeTypeReference collectionType = codeType;
            if (codeType.BaseType == typeof(byte).FullName)
            {
                // Never change byte[] to List<byte> etc.
                // Fix : when translating hexBinary and base64Binary 
                return codeType;
            }

            switch (GeneratorContext.GeneratorParams.CollectionObjectType)
            {
                case CollectionType.List:
                    collectionType = new CodeTypeReference("List", new[] { new CodeTypeReference(codeType.BaseType) });
                    break;

                case CollectionType.IList:
                    collectionType = new CodeTypeReference("IList", new[] { new CodeTypeReference(codeType.BaseType) });
                    break;

                case CollectionType.BindingList:
                    collectionType = new CodeTypeReference("BindingList", new[] { new CodeTypeReference(codeType.BaseType) });
                    break;

                case CollectionType.ObservableCollection:
                    collectionType = new CodeTypeReference("ObservableCollection", new[] { new CodeTypeReference(codeType.BaseType) });
                    break;

                case CollectionType.DefinedType:
                    string typname = codeType.BaseType.Replace(".", string.Empty) + "Collection";

                    if (!CollectionTypes.Keys.Contains(typname))
                        CollectionTypes.Add(typname, codeType.BaseType);

                    collectionType = new CodeTypeReference(typname);
                    break;
                default:
                    {
                        // If not use generics, remove multiple array Ex. string[][] => string[]
                        // Fix : http://xsd2code.codeplex.com/WorkItem/View.aspx?WorkItemId=7269
                        if (codeType.ArrayElementType.ArrayRank > 0)
                            collectionType.ArrayElementType.ArrayRank = 0;
                    }

                    break;
            }

            return collectionType;
        }

        /// <summary>
        /// Search defaut constructor. If not exist, create a new ctor.
        /// </summary>
        /// <param name="type">CodeTypeDeclaration type</param>
        /// <param name="newCTor">Indicates if new constructor</param>
        /// <returns>return current or new CodeConstructor</returns>
        protected virtual CodeConstructor GetConstructor(CodeTypeDeclaration type, ref bool newCTor)
        {
            CodeConstructor ctor = null;
            foreach (CodeTypeMember member in type.Members)
            {
                if (member is CodeConstructor)
                    ctor = member as CodeConstructor;
            }

            if (ctor == null)
            {
                newCTor = true;
                ctor = this.CreateClassConstructor(type);
            }

            if (GeneratorContext.GeneratorParams.Miscellaneous.EnableSummaryComment)
                CodeDomHelper.CreateSummaryComment(ctor.Comments, string.Format("{0} class constructor", ctor.Name));

            return ctor;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Creates the static serializer.
        /// </summary>
        /// <param name="classType">Type of the class.</param>
        private static void CreateStaticSerializer(CodeTypeDeclaration classType)
        {
            string typeName = GeneratorContext.GeneratorParams.GenericBaseClass.Enabled ? "T" : classType.Name;


            //VB is not Case Sensitive
            string fieldName = GeneratorContext.GeneratorParams.Language == GenerationLanguage.VisualBasic ? "sSerializer" : "serializer";

            // -----------------------------------------------------------------
            // private static System.Xml.Serialization.XmlSerializer serializer;
            // -----------------------------------------------------------------
            var serializerfield = new CodeMemberField(typeof(XmlSerializer), fieldName);
            serializerfield.Attributes = MemberAttributes.Static | MemberAttributes.Private;

            classType.Members.Add(serializerfield);

            var typeRef = new CodeTypeReference(typeName);
            var typeofValue = new CodeTypeOfExpression(typeRef);

            // private static System.Xml.Serialization.XmlSerializer Serializer { get {...} }
            var serializerProperty = new CodeMemberProperty
                                         {
                                             Type = new CodeTypeReference(typeof(XmlSerializer)),
                                             Name = "Serializer",
                                             HasSet = false,
                                             HasGet = true,
                                             Attributes = MemberAttributes.Static | MemberAttributes.Private
                                         };

            var statments = new CodeStatementCollection();

            statments.Add(
                new CodeAssignStatement(
                    new CodeVariableReferenceExpression(fieldName),
                    new CodeObjectCreateExpression(
                        new CodeTypeReference(typeof(XmlSerializer)), new CodeExpression[] { typeofValue })));


            serializerProperty.GetStatements.Add(
                new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeVariableReferenceExpression(fieldName),
                        CodeBinaryOperatorType.IdentityEquality,
                        new CodePrimitiveExpression(null)),
                        statments.ToArray()));


            serializerProperty.GetStatements.Add(
                new CodeMethodReturnStatement(new CodeVariableReferenceExpression(fieldName)));

            classType.Members.Add(serializerProperty);
        }

        /// <summary>
        /// Generates the base class.
        /// </summary>
        /// <returns>Return base class codetype declaration</returns>
        private CodeTypeDeclaration GenerateBaseClass()
        {
            var baseClass = new CodeTypeDeclaration(GeneratorContext.GeneratorParams.GenericBaseClass.BaseClassName)
                                {
                                    IsClass = true,
                                    IsPartial = true,
                                    TypeAttributes = TypeAttributes.Public
                                };

            baseClass.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Base entity class"));
            baseClass.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, "Base entity class"));

            if (GeneratorContext.GeneratorParams.EnableDataBinding)
                baseClass.BaseTypes.Add(typeof(INotifyPropertyChanged));

            baseClass.TypeParameters.Add(new CodeTypeParameter("T"));

            if (GeneratorContext.GeneratorParams.EnableDataBinding)
                this.CreateDataBinding(baseClass);

            // Add plublic ObjectChangeTracker property
            if (GeneratorContext.GeneratorParams.TrackingChanges.Enabled)
                this.CreateChangeTrackerProperty(baseClass);

            if (GeneratorContext.GeneratorParams.Serialization.Enabled)
            {
                CreateStaticSerializer(baseClass);
                this.CreateSerializeMethods(baseClass);
            }

            if (GeneratorContext.GeneratorParams.GenerateCloneMethod)
                this.CreateCloneMethod(baseClass);

            return baseClass;
        }

        /// <summary>
        /// Search XmlElement in schema.
        /// </summary>
        /// <param name="codeTypeDeclaration">Represents a type declaration for a class, structure, interface, or enumeration.</param>
        /// <param name="schema">schema object</param>
        /// <param name="visitedSchemas">The visited schemas.</param>
        /// <returns>
        /// return found XmlSchemaElement or null value
        /// </returns>
        private XmlSchemaElement SearchElementInSchema(CodeTypeDeclaration codeTypeDeclaration, XmlSchema schema, List<XmlSchema> visitedSchemas)
        {
            foreach (var item in schema.Items)
            {
                var xmlElement = item as XmlSchemaElement;
                if (xmlElement == null)
                {
                    continue;
                }

                var xmlSubElement = this.SearchElement(codeTypeDeclaration, xmlElement, string.Empty, string.Empty);
                if (xmlSubElement != null) return xmlSubElement;
            }

            // If not found search in schema inclusion
            foreach (var item in schema.Includes)
            {
                var schemaInc = item as XmlSchemaInclude;

                // avoid to follow cyclic refrence
                if ((schemaInc == null) || visitedSchemas.Exists(loc => schemaInc.Schema == loc))
                    continue;

                visitedSchemas.Add(schemaInc.Schema);
                var includeElmts = this.SearchElementInSchema(codeTypeDeclaration, schemaInc.Schema, visitedSchemas);
                visitedSchemas.Remove(schemaInc.Schema);

                if (includeElmts != null) return includeElmts;
            }

            return null;
        }
        #endregion
    }
}
