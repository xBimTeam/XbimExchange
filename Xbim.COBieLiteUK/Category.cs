using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.COBieLiteUK
{
    public partial class Category
    {
        [XmlIgnore][JsonIgnore]
        internal string CategoryString
        {
            get { return String.Format("{0} : {1}", Code, Description).Trim(':', ' '); }
            set 
            { 
                Code = null;
                Description = null;
                if (String.IsNullOrEmpty(value)) return;
                var parts = value.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1)
                    Code = parts[0].Trim();
                if (parts.Length > 1)
                {
                    Code = parts[0].Trim();
                    Description = parts[1].Trim();
                }
            }
        }
    }
}
