using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLiteUK
{
    public partial class ContactKey : IEntityKey
    {
        [XmlIgnore, JsonIgnore]
        Type IEntityKey.ForType
        {
            get { return typeof(Contact); }
        }


        [XmlIgnore, JsonIgnore]
        string IEntityKey.Name
        {
            get { return Email; }
            set { Email = value; }
        }
    }
}
