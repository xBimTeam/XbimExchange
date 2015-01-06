using Xbim.Ifc2x3.ProductExtension;

namespace Xbim.COBieLite
{
    public partial class SpaceKeyType
    {
        public SpaceKeyType()
        {
            
        }
        public SpaceKeyType(IfcSpace ifcSpace, CoBieLiteHelper helper)
            : this()
        {
            FloorName = helper.SpaceFloorLookup[ifcSpace].Name;
            SpaceName = ifcSpace.Name;
        }
    }
}
