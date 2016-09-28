using System.Collections.Generic;

namespace Xbim.CobieLiteUk
{
    public partial class Connection
    {
        internal override IEnumerable<IEntityKey> GetKeys()
        {
            foreach (var key in base.GetKeys())
                yield return key;
            if (ConnectedTo != null) 
                yield return ConnectedTo;
            if (RealizingElement != null)
                yield return RealizingElement;
        }

        internal override void RemoveKey(IEntityKey key)
        {
            base.RemoveKey(key);
            if (ConnectedTo == key)
                ConnectedTo = null;
            if (RealizingElement == key)
                RealizingElement = null;
        }
    }
}
