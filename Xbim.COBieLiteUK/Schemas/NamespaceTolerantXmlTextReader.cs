using System.Xml;
using FrameworkSystemIO = System.IO;

/// <summary>
/// This class is taken from http://stackoverflow.com/questions/870293/can-i-make-xmlserializer-ignore-the-namespace-on-deserialization
/// 
/// It's been used to try to read an XML file that does not fit the expeced namespace elements; 
/// In particular from the 
/// </summary>
namespace Xbim.CobieLiteUk.Schemas
{
    internal class NamespaceTolerantXmlTextReader : XmlTextReader
    {
        public NamespaceTolerantXmlTextReader(FrameworkSystemIO.TextReader reader) : base(reader) { }

        public override string NamespaceURI
        {
            get
            {
                // the schema expects a specific namespace only for Facility items.
                if (LocalName == "Facility")
                    return "http://openbim.org/schemas/cobieliteuk";
                return "";
            }
        }
    }
}