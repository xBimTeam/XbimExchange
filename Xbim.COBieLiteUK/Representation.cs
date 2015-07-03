using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.COBieLiteUK
{
    public partial class Representation
    {
        [XmlIgnore, JsonIgnore]
        public override string ObjectType
        {
            get { return CoordinateType; }
            set { CoordinateType = value; }
        }
    }
}
