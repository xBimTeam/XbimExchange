using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.COBieLite
{
    [JsonObject]
    public partial class DocumentCollectionType : ICollectionType<DocumentType>
    {
        public IEnumerator<DocumentType> GetEnumerator()
        {
            return this.Document.OfType<DocumentType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<DocumentType> InnerList
        {
            get { return Document; }
        }
		
    }
}
