using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xml.Serialization.GeneratedAssembly;

namespace Xbim.COBieLiteUK
{
    public partial class ProjectType
    {
        public static System.Xml.Serialization.XmlSerializer GetSerializer()
        {
            return new ProjectTypeSerializer();
        }

        public void Save(Stream stream)
        {
            var serializer = ProjectType.GetSerializer();
            serializer.Serialize(stream, this, 
                new XmlSerializerNamespaces(new []{
                    new XmlQualifiedName("cobielite", "http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite"),
                    new XmlQualifiedName("core", "http://docs.buildingsmartalliance.org/nbims03/cobie/core"),
                }));
        }
    }
}
