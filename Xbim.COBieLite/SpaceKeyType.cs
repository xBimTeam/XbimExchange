using Xbim.Ifc4.Interfaces;

namespace Xbim.COBieLite
{
    public partial class SpaceKeyType
    {
        public SpaceKeyType()
        {
            
        }
        public SpaceKeyType(IIfcSpace ifcSpace, CoBieLiteHelper helper)
            : this()
        {
            FloorName = helper.SpaceFloorLookup[ifcSpace].Name;
            SpaceName = ifcSpace.Name;
        }
    }
}
