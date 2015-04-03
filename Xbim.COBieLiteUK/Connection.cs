using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieLiteUK
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
    }
}
