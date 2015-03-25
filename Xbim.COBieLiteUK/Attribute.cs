using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.COBieLiteUK
{
    public partial class Attribute
    {
        /// <summary>
        /// This is a proxy property for ExternalEntity. It is not a part of the schema but it is part of the API to make sure the
        /// data schema is used in an uniform way.
        /// </summary>
        [XmlIgnore][JsonIgnore]
        public string PropertySetName { get { return ExternalEntity; } set { ExternalEntity = value; } }
    }
}
