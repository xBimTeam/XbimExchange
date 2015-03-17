// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Net30Extension.cs" company="Xsd2Code">
//   N/A
// </copyright>
// <summary>
//   Implements code generation extension for .Net Framework 3.0
// </summary>
// <remarks>
//  Updated 2010-01-20 Deerwood McCord Jr. Cleaned CodeSnippetStatements by replacing with specific CodeDom Expressions
// </remarks>
// --------------------------------------------------------------------------------------------------------------------

namespace Xsd2Code.Library.Extensions
{
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Schema;
    using Helpers;

    /// <summary>
    /// Implements code generation extension for .Net Framework 3.0
    /// </summary>
    [CodeExtension(TargetFramework.Net30)]
    public class Net30Extension : CodeExtension
    {
        #region Private Fields

        /// <summary>
        /// List the properties that will change to auto properties
        /// </summary>
        private readonly List<CodeMemberProperty> autoPropertyListField = new List<CodeMemberProperty>();

        /// <summary>
        /// List the fields to be deleted
        /// </summary>
        private readonly List<CodeMemberField> fieldListToRemoveField = new List<CodeMemberField>();

        /// <summary>
        /// List fields that require an initialization in the constructor
        /// </summary>
        private readonly List<string> fieldWithAssignementInCtorListField = new List<string>();

        #endregion

        #region Protected methods

        /// <summary>
        /// Processes the class.
        /// </summary>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="schema">The input xsd schema.</param>
        /// <param name="type">Represents a type declaration for a class, structure, interface, or enumeration</param>
        protected override void ProcessClass(CodeNamespace codeNamespace, XmlSchema schema, CodeTypeDeclaration type)
        {
            this.autoPropertyListField.Clear();
            this.fieldListToRemoveField.Clear();
            this.fieldWithAssignementInCtorListField.Clear();

            // looks for properties that can not become automatic property
            CodeConstructor ctor = null;
            foreach (CodeTypeMember member in type.Members)
            {
                if (member is CodeConstructor)
                    ctor = member as CodeConstructor;
            }

            if (ctor != null)
            {
                foreach (var statement in ctor.Statements)
                {
                    var codeAssignStatement = statement as CodeAssignStatement;
                    if (codeAssignStatement == null) continue;
                    var code = codeAssignStatement.Left as CodeFieldReferenceExpression;
                    if (code != null)
                    {
                        this.fieldWithAssignementInCtorListField.Add(code.FieldName);
                    }
                }
            }

            base.ProcessClass(codeNamespace, schema, type);

            // generate automatic properties
            this.GenerateAutomaticProperties(type);
        }

        /// <summary>
        /// Create data contract attribute
        /// </summary>
        /// <param name="type">Code type declaration</param>
        /// <param name="schema">XML schema</param>
        protected override void CreateDataContractAttribute(CodeTypeDeclaration type, XmlSchema schema)
        {
            base.CreateDataContractAttribute(type, schema);

            if (GeneratorContext.GeneratorParams.GenerateDataContracts)
            {
                var attributeType = new CodeTypeReference("System.Runtime.Serialization.DataContractAttribute");
                var codeAttributeArgument = new List<CodeAttributeArgument>();

                //var typeName = string.Concat('"', type.Name, '"');
                //codeAttributeArgument.Add(new CodeAttributeArgument("Name", new CodeSnippetExpression(typeName)));
                codeAttributeArgument.Add(new CodeAttributeArgument("Name", new CodePrimitiveExpression(type.Name)));

                if (!string.IsNullOrEmpty(schema.TargetNamespace))
                {
                    //var targetNamespace = string.Concat('\"', schema.TargetNamespace, '\"');
                    //codeAttributeArgument.Add(new CodeAttributeArgument("Namespace", new CodeSnippetExpression(targetNamespace)));
                    codeAttributeArgument.Add(new CodeAttributeArgument("Namespace", new CodePrimitiveExpression(schema.TargetNamespace)));
                }

                type.CustomAttributes.Add(new CodeAttributeDeclaration(attributeType, codeAttributeArgument.ToArray()));
            }
        }

        /// <summary>
        /// Creates the data member attribute.
        /// </summary>
        /// <param name="prop">Represents a declaration for a property of a type.</param>
        protected override void CreateDataMemberAttribute(CodeMemberProperty prop)
        {
            base.CreateDataMemberAttribute(prop);

            if (GeneratorContext.GeneratorParams.GenerateDataContracts)
            {
                var attrib = new CodeTypeReference("System.Runtime.Serialization.DataMemberAttribute");
                prop.CustomAttributes.Add(new CodeAttributeDeclaration(attrib));
            }
        }

        /// <summary>
        /// Import namespaces
        /// </summary>
        /// <param name="code">Code namespace</param>
        protected override void ImportNamespaces(CodeNamespace code)
        {
            base.ImportNamespaces(code);

            if (GeneratorContext.GeneratorParams.GenerateDataContracts)
                code.Imports.Add(new CodeNamespaceImport("System.Runtime.Serialization"));
        }

        /// <summary>
        /// Property process
        /// </summary>
        /// <param name="type">Represents a type declaration for a class, structure, interface, or enumeration</param>
        /// <param name="ns">The ns.</param>
        /// <param name="member">Type members include fields, methods, properties, constructors and nested types</param>
        /// <param name="xmlElement">Represent the root element in schema</param>
        /// <param name="schema">XML Schema</param>
        protected override void ProcessProperty(CodeTypeDeclaration type, CodeNamespace ns, CodeTypeMember member, XmlSchemaElement xmlElement, XmlSchema schema)
        {
            // Get now if property is array before base.ProcessProperty call.
            var prop = (CodeMemberProperty)member;

            base.ProcessProperty(type, ns, member, xmlElement, schema);

            int i = 0;
            // Generate automatic properties.
            if (GeneratorContext.GeneratorParams.Language == GenerationLanguage.CSharp)
            {
                if (GeneratorContext.GeneratorParams.PropertyParams.AutomaticProperties)
                {
                    bool excludeType = false;
                        // Exclude collection type
                        if (CollectionTypesFields.IndexOf(prop.Name) == -1)
                        {
                            // Get private fieldName
                            var propReturnStatment = prop.GetStatements[0] as CodeMethodReturnStatement;
                            if (propReturnStatment != null)
                            {
                                var field = propReturnStatment.Expression as CodeFieldReferenceExpression;
                                if (field != null)
                                {
                                    // Check if private field don't need initialisation in ctor (defaut value).
                                    if (this.fieldWithAssignementInCtorListField.FindIndex(p => p == field.FieldName) == -1)
                                    {
                                        this.autoPropertyListField.Add(member as CodeMemberProperty);
                                    }
                                }
                            }
                        }
                }
            }
        }

        /// <summary>
        /// process Fields.
        /// </summary>
        /// <param name="member">CodeTypeMember member</param>
        /// <param name="ctor">CodeMemberMethod constructor</param>
        /// <param name="ns">CodeNamespace XSD</param>
        /// <param name="addedToConstructor">Indicates if create a new constructor</param>
        protected override void ProcessFields(CodeTypeMember member, CodeMemberMethod ctor, CodeNamespace ns, ref bool addedToConstructor)
        {
            // Get now if filed is array before base.ProcessProperty call.
            var field = (CodeMemberField)member;
            bool isArray = field.Type.ArrayElementType != null;

            base.ProcessFields(member, ctor, ns, ref addedToConstructor);

            // Generate automatic properties.
            if (GeneratorContext.GeneratorParams.Language == GenerationLanguage.CSharp)
            {
                if (GeneratorContext.GeneratorParams.PropertyParams.AutomaticProperties)
                {
                    if (!isArray)
                    {
                        bool finded;
                        if (!this.IsComplexType(field.Type, ns, out finded))
                        {
                            if (finded)
                            {
                                // If this field is not assigned in ctor, add it in remove list.
                                // with automatic property, don't need to keep private field.
                                if (this.fieldWithAssignementInCtorListField.FindIndex(p => p == field.Name) == -1)
                                {
                                    this.fieldListToRemoveField.Add(field);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region static methods
        /// <summary>
        /// Outputs the attribute argument.
        /// </summary>
        /// <param name="arg">Represents an argument used in a metadata attribute declaration.</param>
        /// <returns>transform attribute into srting</returns>
        private static string AttributeArgumentToString(CodeAttributeArgument arg)
        {
            var strWriter = new StringWriter();
            var provider = CodeDomProviderFactory.GetProvider(GeneratorContext.GeneratorParams.Language);

            if (!string.IsNullOrEmpty(arg.Name))
            {
                strWriter.Write(arg.Name);
                strWriter.Write("=");
            }

            provider.GenerateCodeFromExpression(arg.Value, strWriter, new CodeGeneratorOptions());
            var strrdr = new StringReader(strWriter.ToString());
            return strrdr.ReadToEnd();
        }
        #endregion

        /// <summary>
        /// Outputs the attribute argument.
        /// </summary>
        /// <param name="arg">Represents an argument used in a metadata attribute declaration.</param>
        /// <returns>transform attribute into srting</returns>
        private static string ExpressionToString(CodeExpression arg)
        {
            var strWriter = new StringWriter();
            var provider = CodeDomProviderFactory.GetProvider(GeneratorContext.GeneratorParams.Language);
            provider.GenerateCodeFromExpression(arg, strWriter, new CodeGeneratorOptions());
            var strrdr = new StringReader(strWriter.ToString());
            return strrdr.ReadToEnd();
        }


        #region Private methods
        /// <summary>
        /// Generates the automatic properties.
        /// </summary>
        /// <param name="type">Represents a type declaration for a class, structure, interface, or enumeration.</param>
        private void GenerateAutomaticProperties(CodeTypeDeclaration type)
        {
            if (Equals(GeneratorContext.GeneratorParams.Language, GenerationLanguage.CSharp))
            {
                // If databinding is disable, use automatic property
                if (GeneratorContext.GeneratorParams.PropertyParams.AutomaticProperties)
                {
                    foreach (var item in this.autoPropertyListField)
                    {
                        var cm = new CodeSnippetTypeMember();
                        bool transformToAutomaticproperty = true;

                        var attributesString = new List<string>();
                        foreach (var attribute in item.CustomAttributes)
                        {
                            var attrib = attribute as CodeAttributeDeclaration;
                            if (attrib != null)
                            {
                                // Don't transform property with default value.
                                if (attrib.Name == "System.ComponentModel.DefaultValueAttribute")
                                {
                                    transformToAutomaticproperty = false;
                                }
                                else
                                {
                                    string attributesArguments = string.Empty;
                                    foreach (var arg in attrib.Arguments)
                                    {
                                        var argument = arg as CodeAttributeArgument;
                                        if (argument != null)
                                        {
                                            attributesArguments += AttributeArgumentToString(argument) + ",";
                                        }
                                    }
                                    // Remove last ","
                                    if (attributesArguments.Length > 0)
                                        attributesArguments = attributesArguments.Remove(attributesArguments.Length - 1);

                                    attributesString.Add(string.Format("[{0}({1})]", attrib.Name, attributesArguments));
                                }
                            }
                        }

                        if (transformToAutomaticproperty)
                        {
                            foreach (var attribute in attributesString)
                            {
                                cm.Text += "    " + attribute + "\n";
                            }
                            var ct = new CodeTypeReferenceExpression(item.Type);
                            var prop = ExpressionToString(ct);
                            var text = string.Format("    public {0} {1} ", prop, item.Name);
                            cm.Text += string.Concat(text, "{get; set;}\n");
                            cm.Comments.AddRange(item.Comments);

                            type.Members.Add(cm);
                            type.Members.Remove(item);
                        }
                    }

                    // Now remove all private fileds
                    foreach (var item in this.fieldListToRemoveField)
                    {
                        if (item.Name == "mailClassField" && type.Name == "uspsSummaryType")
                        {
                            ;
                        }
                        type.Members.Remove(item);
                    }
                }
            }
        }
        #endregion
    }
}