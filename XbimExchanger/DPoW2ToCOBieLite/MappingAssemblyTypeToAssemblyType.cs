using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.DPoW;
using Assembly = Xbim.COBieLite.AssemblyType;

namespace XbimExchanger.DPoW2ToCOBieLite
{
    class MappingAssemblyTypeToAssemblyType:DPoWToCOBieLiteMapping<AssemblyType, Assembly>
    {
        protected override Assembly Mapping(AssemblyType source, Assembly target)
        {
            throw new NotImplementedException();
        }
    }
}
