using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace Xsd2Code.Library.Extensions
{
    [CodeExtension(TargetFramework.CobieLiteUk)]
    public class CoBieLiteUkExtension : Net35Extension
    {
        protected override void ProcessProperty(CodeTypeDeclaration type, CodeNamespace ns, CodeTypeMember member,
            XmlSchemaElement xmlElement,
            XmlSchema schema)
        {
            base.ProcessProperty(type, ns, member, xmlElement, schema);

            var propertyMember = member as CodeMemberProperty;
            if (propertyMember == null) return;

            //rename AttributeValue Item to Value
            if (propertyMember.Name == "Item" && propertyMember.Type.BaseType == "AttributeValue")
            {
                propertyMember.Name = "Value";
            }

            //Make Name virtual property
            if (type.Name == "CobieObject")
            {
                propertyMember.Attributes = MemberAttributes.Public;
            }

            var xsdType =
                schema.SchemaTypes.Values.OfType<XmlSchemaComplexType>().FirstOrDefault(ct => ct.Name == type.Name);
            if (xsdType == null) return;

            //get sequence
            var sequence = xsdType.ContentTypeParticle as XmlSchemaSequence;
            if (sequence == null) return;


            var element = sequence.Items.OfType<XmlSchemaElement>().FirstOrDefault(se => se.Name == member.Name);

            if (element == null || element.Annotation == null || element.Annotation.Items == null ||
                element.Annotation.Items.Count == 0) return;

            //find mapping information in appinfo of the element and create Attribute definition
            //foreach (var item in element.Annotation.Items)
            //{
            //    var appInfo = item as XmlSchemaAppInfo;
            //    if (appInfo == null) continue;
            //
            //}

            //do any postprocessing here

            
        }

        protected override void ProcessClass(CodeNamespace codeNamespace, XmlSchema schema, CodeTypeDeclaration type)
        {
            base.ProcessClass(codeNamespace, schema, type);

            var xsdType =
                schema.SchemaTypes.Values.OfType<XmlSchemaComplexType>().FirstOrDefault(ct => ct.Name == type.Name);
            if (xsdType == null || xsdType.Annotation == null || xsdType.Annotation.Items == null ||
                xsdType.Annotation.Items.Count == 0) return;

            //find mapping information in appinfo of the element and create Attribute definition
            foreach (var item in xsdType.Annotation.Items)
            {
                //get documentation string
                var doc = item as XmlSchemaDocumentation;
                if (doc != null && doc.Markup != null && doc.Markup.Length != 0)
                {
                    var markup = doc.Markup[0].InnerText;
                    type.Comments.Add(new CodeCommentStatement()
                    {
                        Comment = new CodeComment() {DocComment = true, Text = markup}
                    });
                }


                //add meta attribtutes from AppInfo
                var appInfo = item as XmlSchemaAppInfo;
                if (appInfo == null) continue;

                var source = appInfo.Source;
                if (source == "sheet")
                {
                    var parts = appInfo.Markup[0].InnerText.Split('|');
                    var isExtension = parts.Length > 2 && parts[2].Trim().ToLower() == "extension";
                    type.CustomAttributes.Add(new CodeAttributeDeclaration("Xbim.COBieLiteUK.SheetMapping",
                        new CodeAttributeArgument("Type", new CodePrimitiveExpression(parts[0])),
                        new CodeAttributeArgument("Sheet", new CodePrimitiveExpression(parts[1])),
                        new CodeAttributeArgument("IsExtension", new CodePrimitiveExpression(isExtension))
                        ));
                }
                if (source == "mapping")
                {
                    var parts = appInfo.Markup[0].InnerText.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length < 7) 
                        throw new Exception("Unexpected number of arguments for Xbim.COBieLiteUK.Mapping");

                    //trim all parts
                    for (var i = 0; i < parts.Length; i++)
                        parts[i] = parts[i].Trim();
                    var isExtension = parts.Length > 7 && parts[7].Trim().ToLower() == "extension";

                    type.CustomAttributes.Add(new CodeAttributeDeclaration("Xbim.COBieLiteUK.Mapping",
                        new CodeAttributeArgument("Type", new CodePrimitiveExpression(parts[0])),
                        new CodeAttributeArgument("Column", new CodePrimitiveExpression(parts[1])),
                        new CodeAttributeArgument("Header", new CodePrimitiveExpression(parts[2])),
                        new CodeAttributeArgument("Required", new CodePrimitiveExpression(parts[3].ToLower() == "required")),
                        new CodeAttributeArgument("List", new CodePrimitiveExpression(parts[4].ToLower() == "list")),
                        new CodeAttributeArgument("PickList", new CodePrimitiveExpression(parts[5]== "-" ? null : parts[5])),
                        new CodeAttributeArgument("Path", new CodePrimitiveExpression(parts[6])),
                        new CodeAttributeArgument("IsExtension", new CodePrimitiveExpression(isExtension))
                        ));
                }
                if (source == "parent")
                {
                    type.CustomAttributes.Add(new CodeAttributeDeclaration("Xbim.COBieLiteUK.Parent", 
                        new CodeAttributeArgument("DataType", new CodeTypeOfExpression(appInfo.Markup[0].InnerText))));
                }
            }
        }

        public override void Process(CodeNamespace code, XmlSchema schema)
        {
            base.Process(code, schema);

            //remove collection types as they are not used 
            var collections =
                code.Types.Cast<CodeTypeDeclaration>().Where(ctd => ctd.Name.Contains("Collection")).ToList();
            foreach (var collection in collections)
            {
                code.Types.Remove(collection);
            }

            //remove properties which will be implemented in partial classes
            var toRemove = new[] { "AreaUnits", "LinearUnits", "VolumeUnits", "CurrencyUnit", "AssetTypeEnum" };
            foreach (CodeTypeDeclaration type in code.Types)
            {
                foreach (var member in type.Members.OfType<CodeMemberProperty>().ToList())
                {
                    if (toRemove.Contains(member.Name))
                    {
                        type.Members.Remove(member);
                    }    
                }
            }
            

            //add attributes to enumeration members
            var enums = code.Types.OfType<CodeTypeDeclaration>().Where(t => t.IsEnum);
            var xsdEnums = schema.SchemaTypes.Values.OfType<XmlSchemaSimpleType>();
            foreach (var @enum in enums)
            {
                var definition = xsdEnums.FirstOrDefault(t => t.Name == @enum.Name);
                if (definition == null || !(definition.Content is XmlSchemaSimpleTypeRestriction))
                    continue;
                var facets = ((XmlSchemaSimpleTypeRestriction) definition.Content).Facets.OfType<XmlSchemaEnumerationFacet>();
                foreach (CodeTypeMember member in @enum.Members)
                {
                    var facet = facets.FirstOrDefault(f => f.Value.Replace(" ", "") == member.Name);
                    if(facet == null || facet.Annotation == null) continue;

                    //parse all appinfo elements from annotation
                    foreach (var item in facet.Annotation.Items.OfType<XmlSchemaAppInfo>())
                    {
                        if (item.Source == "alias")
                        {
                            var parts = item.Markup[0].InnerText.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            var attr = parts.Length == 2 ? new CodeAttributeDeclaration("Xbim.COBieLiteUK.Alias",
                                new CodeAttributeArgument("Type", new CodePrimitiveExpression(parts[0])),
                                new CodeAttributeArgument("Value", new CodePrimitiveExpression(parts[1]))
                                ) : 
                                new CodeAttributeDeclaration("Xbim.COBieLiteUK.Alias",
                                new CodeAttributeArgument("Value", new CodePrimitiveExpression(parts[0]))
                                )
                                ;
                            member.CustomAttributes.Add(attr);
                        }
                    }

                }
            }
        }
    }
}