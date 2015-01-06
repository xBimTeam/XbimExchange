using System.IO;
using System.Text;
using System.Xml;

namespace Xbim.COBieLite
{

    /// <summary>
    /// Specialised Xml writer that writes out COBieLite Xml to comply with Nibs standards
    /// </summary>
    public class XbimCoBieLiteXmlWriter : XmlTextWriter
    {
        public XbimCoBieLiteXmlWriter(TextWriter w)
            : base(w) { }
        public XbimCoBieLiteXmlWriter(Stream w, Encoding encoding)
            : base(w, encoding) { }
        public XbimCoBieLiteXmlWriter(string filename, Encoding encoding)
            : base(filename, encoding) { }

        bool _skip;

        public override void WriteStartAttribute(string prefix,
                                                 string localName,
                                                 string ns)
        {
            if (ns == "http://www.w3.org/2001/XMLSchema-instance" &&
                localName == "type")
            {
                _skip = true;
            }
            else
            {
                base.WriteStartAttribute(prefix, localName, ns);
            }
        }

        public override void WriteString(string text)
        {
            if (!_skip) base.WriteString(text);
        }

        public override void WriteEndAttribute()
        {
            if (!_skip) base.WriteEndAttribute();
            _skip = false;
        }
    }
}
