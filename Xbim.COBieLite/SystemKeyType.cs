using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc2x3.ProductExtension;

namespace Xbim.COBieLite
{
    public partial class SystemKeyType
    {
        public SystemKeyType()
        {
            
        }

        public SystemKeyType(IfcSystem ifcSystem, CoBieLiteHelper helper)
        {
            SystemCategory = helper.GetClassification(ifcSystem);
            SystemName = ifcSystem.Name;
            externalIDReference = helper.ExternalEntityIdentity(ifcSystem);
        }
    }
}
