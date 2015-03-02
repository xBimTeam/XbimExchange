using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Xbim.COBieLite.CollectionTypes;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLite
{
    [JsonObject]
    public partial class DocumentCollectionType : ICollectionType<DocumentType>, IEnumerable<DocumentType>
    {
        public IEnumerator<DocumentType> GetEnumerator()
        {
            return Document.OfType<DocumentType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<DocumentType> InnerList
        {
            get { return Document ?? (Document = new List<DocumentType>()); }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return Document.OfType<DocumentType>().GetEnumerator();
        }
    }
}
