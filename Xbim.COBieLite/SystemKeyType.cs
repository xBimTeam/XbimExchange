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
