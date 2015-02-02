using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.DPoWToCOBieLite
{
    public class MappingDocumentToDocumentType: DPoWToCOBieLiteMapping<Document, DocumentType>
    {
        protected override DocumentType Mapping(Document source, DocumentType target)
        {
            target.externalID = Exchanger.GetStringIdentifier();
            target.DocumentName = source.DocumentName;
            target.DocumentDescription = source.DocumentDescription;
            target.DocumentCategory = source.DocumentCategory != null ? source.DocumentCategory.ClassificationName : null;
            target.DocumentURI = source.DocumentURI;
            target.externalSystemName = source.Scope.ToString();

            target.DocumentAttributes = new AttributeCollectionType();
            return target;
        }

        public static string GetKey(Document document)
        {
            return String.Format("{0} {1} {2}", document.DocumentName, document.DocumentURI, document.GetHashCode());
        }
    }
}
