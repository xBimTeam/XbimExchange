
using Xbim.Ifc4.Interfaces;
namespace Xbim.COBieLite
{
    public partial class SystemKeyType
    {
        public SystemKeyType()
        {
            
        }

        public SystemKeyType(IIfcSystem ifcSystem, CoBieLiteHelper helper)
        {
            SystemCategory = helper.GetClassification(ifcSystem);
            SystemName = ifcSystem.Name;
            externalIDReference = helper.ExternalEntityIdentity(ifcSystem);
        }
    }
}
