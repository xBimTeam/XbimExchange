using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLite;
using Xbim.DPoW;

namespace XbimExchanger.DPoW2ToCOBieLite
{
    public class MappingDocumentToDocumentType: DPoWToCOBieLiteMapping<Documentation, DocumentType>
    {
        protected override DocumentType Mapping(Documentation source, DocumentType target)
        {
            throw new NotImplementedException();
        }
    }
}
