using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Xbim.COBieLiteUK
{
    public partial class ProjectType1
    {
        public void Save(Stream stream)
        {
            var serializer = new XmlSerializer(this.GetType());
            serializer.Serialize(stream, this, 
                new XmlSerializerNamespaces(new []{
                    new XmlQualifiedName("cobielite", "http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite"),
                    new XmlQualifiedName("core", "http://docs.buildingsmartalliance.org/nbims03/cobie/core"),
                }));
        }
    }
}
