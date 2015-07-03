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

        [XmlIgnore, JsonIgnore]
        public override string ObjectType
        {
            get { return FloorType; }
            set { FloorType = value; }
        }
    }
}
