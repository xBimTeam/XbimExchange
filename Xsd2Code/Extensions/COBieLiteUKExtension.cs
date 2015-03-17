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
        protected override void ProcessProperty(CodeTypeDeclaration type, CodeNamespace ns, CodeTypeMember member, XmlSchemaElement xmlElement,
            XmlSchema schema)
        {
            base.ProcessProperty(type, ns, member, xmlElement, schema);

            var xsdType = schema.SchemaTypes.Values.OfType<XmlSchemaComplexType>().FirstOrDefault(ct => ct.Name == type.Name);
            if (xsdType == null) return;

            //get sequence
            var sequence = xsdType.ContentTypeParticle as XmlSchemaSequence;
            if (sequence == null) return;


            var element = sequence.Items.OfType<XmlSchemaElement>().FirstOrDefault(se => se.Name == member.Name);

            if (element == null || element.Annotation == null || element.Annotation.Items == null || element.Annotation.Items.Count == 0) return;

            //find mapping information in appinfo of the element and create Attribute definition
            foreach (var item in element.Annotation.Items)
            {
                var appInfo = item as XmlSchemaAppInfo;
                if(appInfo == null) continue;

                var source = appInfo.Source;

                if (source == "mapping")
                {
                    var parts = appInfo.Markup[0].InnerText.Split('|');
                    member.CustomAttributes.Add(new CodeAttributeDeclaration("Xbim.COBieLiteUK.PropertyMapping",
                        new CodeAttributeArgument( "Type", new CodePrimitiveExpression(parts[0])),
                        new CodeAttributeArgument( "Column", new CodePrimitiveExpression(parts[1])),
                        new CodeAttributeArgument( "Header", new CodePrimitiveExpression(parts[2])),
                        new CodeAttributeArgument("Required", new CodePrimitiveExpression(Boolean.Parse(parts[3])))
                        ));
                }
                if (source == "path")
                    member.CustomAttributes.Add(new CodeAttributeDeclaration("Xbim.COBieLiteUK.Path",
                        new CodeAttributeArgument("Path", new CodePrimitiveExpression(appInfo.Markup[0].InnerText))
                        ));
                if (source == "picklist")
                    member.CustomAttributes.Add(new CodeAttributeDeclaration("Xbim.COBieLiteUK.Picklist",
                        new CodeAttributeArgument("PicklistItem", new CodePrimitiveExpression(appInfo.Markup[0].InnerText))
                        ));
                if (source == "list")
                    member.CustomAttributes.Add(new CodeAttributeDeclaration("Xbim.COBieLiteUK.List"));

            }
        }

        protected override void ProcessClass(CodeNamespace codeNamespace, XmlSchema schema, CodeTypeDeclaration type)
        {
            base.ProcessClass(codeNamespace, schema, type);

            var xsdType = schema.SchemaTypes.Values.OfType<XmlSchemaComplexType>().FirstOrDefault(ct => ct.Name == type.Name);
            if (xsdType == null || xsdType.Annotation == null || xsdType.Annotation.Items == null || xsdType.Annotation.Items.Count == 0) return;

            //find mapping information in appinfo of the element and create Attribute definition
            foreach (var item in xsdType.Annotation.Items)
            {
                var appInfo = item as XmlSchemaAppInfo;
                if (appInfo == null) continue;

                var source = appInfo.Source;
                if (source == "sheet")
                {
                    var parts = appInfo.Markup[0].InnerText.Split('|');
                    type.CustomAttributes.Add(new CodeAttributeDeclaration("Xbim.COBieLiteUK.SheetMapping",
                            new CodeAttributeArgument("Type", new CodePrimitiveExpression(parts[0])),
                            new CodeAttributeArgument("Sheet", new CodePrimitiveExpression(parts[1]))
                            ));
                }
                if (source == "mapping::parent")
                {
                    var parts = appInfo.Markup[0].InnerText.Split('|');
                    type.CustomAttributes.Add(new CodeAttributeDeclaration("Xbim.COBieLiteUK.ParentMapping",
                        new CodeAttributeArgument("Type", new CodePrimitiveExpression(parts[0])),
                        new CodeAttributeArgument("Column", new CodePrimitiveExpression(parts[1])),
                        new CodeAttributeArgument("Header", new CodePrimitiveExpression(parts[2])),
                        new CodeAttributeArgument("Path", new CodePrimitiveExpression(parts[3]))
                        ));
                }
                
            }
        }
    }
}
