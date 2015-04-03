using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.COBieLiteUK
{
    public partial class Contact
    {
        [XmlIgnore][JsonIgnore]
        public override string Name
        {
            get { return Email; }
            set { Email = value; }
        }

        /// <summary>
        /// Description is invalid for Contact object
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public override string Description
        {
            get
            {
                if (Attributes == null) return null;
                var attr = Attributes.FirstOrDefault(a => a.Name == "Description");
                if (attr == null) return null;

                var strAttrVal = attr.Value as StringAttributeValue;
                if (strAttrVal == null) return null;

                return strAttrVal.Value;
            }
            set
            {
                if (Attributes == null) Attributes = new List<Attribute>();
                var attr = Attributes.FirstOrDefault(a => a.Name == "Description");
                if (attr == null)
                {
                    attr = new Attribute() { Name = "Description" };
                    Attributes.Add(attr);
                }

                if (value == null)
                {
                    attr.Value = null;
                    return;
                }

                var strAttrVal = attr.Value as StringAttributeValue;
                if (strAttrVal == null)
                {
                    strAttrVal = new StringAttributeValue();
                    attr.Value = strAttrVal;
                }

                strAttrVal.Value = value;
            }
        }
    }
}
