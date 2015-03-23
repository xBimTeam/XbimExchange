using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieLiteUK
{
    public partial class AssetType
    {
        internal override IEnumerable<CobieObject> GetChildren()
        {
            foreach (var child in base.GetChildren())
                yield return child;
            if(Assets != null)
                foreach (var asset in Assets)
                {
                    yield return asset;
                }
            if (Spares != null)
                foreach (var spare in Spares)
                {
                    yield return spare;
                }
            if (Jobs != null)
                foreach (var job in Jobs)
                {
                    yield return job;
                }
            if (AssemblyOf != null)
                yield return AssemblyOf;
        }
    }
}
