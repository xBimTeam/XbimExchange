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
            throw new NotImplementedException();
        }
    }
}
