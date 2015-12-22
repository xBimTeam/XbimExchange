using System.Collections.Generic;

namespace Xbim.COBieLiteUK
{
    public partial class Floor
    {
        internal override IEnumerable<CobieObject> GetChildren()
        {
            foreach (var child in base.GetChildren())
                yield return child;
            if(Spaces != null)
                foreach (var space in Spaces)
                    yield return space;
        }
    }
}
