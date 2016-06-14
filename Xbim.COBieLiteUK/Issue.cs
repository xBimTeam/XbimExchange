using System.Collections.Generic;

namespace Xbim.CobieLiteUk
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

        internal override void RemoveKey(IEntityKey key)
        {
            base.RemoveKey(key);
            if (Owner == key)
                Owner = null;
            if (IssueWith == key)
                IssueWith = null;
        }
    }
}
