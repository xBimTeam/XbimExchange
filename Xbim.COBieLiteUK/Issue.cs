using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

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

        internal override void RemoveKey(IEntityKey key)
        {
            base.RemoveKey(key);
            if (Owner == key)
                Owner = null;
            if (IssueWith == key)
                IssueWith = null;
        }

        [XmlIgnore, JsonIgnore]
        public override string ObjectType
        {
            get { return IssueType; }
            set { IssueType = value; }
        }
    }
}
