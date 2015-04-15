using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieLiteUK
{
    public partial class Spare
    {
        internal override IEnumerable<IEntityKey> GetKeys()
        {
            foreach (var key in base.GetKeys())
                yield return key;
            if (Suppliers == null) yield break;
            foreach (var key in Suppliers)
                yield return key;
        }

        internal override void RemoveKey(IEntityKey key)
        {
            base.RemoveKey(key);
            var contact = key as ContactKey;
            if (contact != null && Suppliers != null)
                Suppliers.Remove(contact);
        }
    }
}
