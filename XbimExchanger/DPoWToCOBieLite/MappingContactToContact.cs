using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.DPoWToCOBieLite
{
    public class MappingContactToContact : DPoWToCOBieLiteMapping<Contact, ContactType>
    {
        protected override ContactType Mapping(Contact source, ContactType target)
        {
            throw new NotImplementedException();
        }
    }
}
