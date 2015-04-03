using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieLiteUK
{
    public partial class Issue
    {
        internal override IEnumerable<IEntityKey> GetKeys()
        {
            foreach (var key in base.GetKeys())
                yield return key;
            if (Owner != null) 
                yield return Owner;
            if(IssueWith != null)
                yield return IssueWith;
        }
    }
}
