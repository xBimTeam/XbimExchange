using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.CobieLiteUk
{
    public partial class Category
    {
        [XmlIgnore][JsonIgnore]
        internal string CategoryString
        {
            get { return string.Format("{0} : {1}", Code, Description).Trim(':', ' '); }
            set 
            { 
                Code = null;
                Description = null;
                if (string.IsNullOrEmpty(value)) return;
                var parts = value.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0)
                    Code = parts[0].Trim();
                if (parts.Length > 1)
                    Description = parts[1].Trim();
            }
        }
    }
}
