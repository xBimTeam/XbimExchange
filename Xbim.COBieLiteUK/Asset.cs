using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieLiteUK
{
    public partial class Asset
    {
        internal override IEnumerable<CobieObject> GetChildren()
        {
            //enumerate base
            foreach (var child in base.GetChildren())
                yield return child;
            if (Connections != null)
                foreach (var connection in Connections)
                    yield return connection;
            if(AssemblyOf != null)
                yield return AssemblyOf;
        }

        internal override IEnumerable<IEntityKey> GetKeys()
        {
            foreach (var key in base.GetKeys())
                yield return key;
            if (Spaces == null) yield break;
            foreach (var key in Spaces)
                yield return key;
        }
    }
}
