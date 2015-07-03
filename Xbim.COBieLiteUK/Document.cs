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
    public partial class Document
    {
        internal override IEnumerable<IEntityKey> GetKeys()
        {
            foreach (var key in base.GetKeys())
                yield return key;
            if (ApprovalBy != null) 
                yield return ApprovalBy;
        }

        internal override void RemoveKey(IEntityKey key)
        {
            base.RemoveKey(key);
            if (ApprovalBy == key)
                ApprovalBy = null;
        }

        [XmlIgnore, JsonIgnore]
        public override string ObjectType
        {
            get { return DocumentType; }
            set { DocumentType = value; }
        }
    }
}
