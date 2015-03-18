using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.COBieLiteUK
{
    public partial class Contact : ICobieObject
    {
        [XmlIgnore][JsonIgnore]
        new public string Name
        {
            get { return String.Format("{0} {1}", FamilyName, GivenName); }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    FamilyName = null;
                    GivenName = null;
                    return;
                }
                var parts = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1)
                {
                    FamilyName = parts[0];
                    GivenName = null;
                }
                else
                {
                    FamilyName = parts[0].Trim();
                    GivenName = value.Substring(FamilyName.Length).Trim();
                }
            }
        }

        /// <summary>
        /// Description is invalid for Contact object
        /// </summary>
        string ICobieObject.Description
        {
            get { return null; }
            set { }
        }
    }
}
