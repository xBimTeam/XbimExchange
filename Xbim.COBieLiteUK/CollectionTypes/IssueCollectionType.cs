using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xbim.COBieLite.CollectionTypes;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLiteUK
{
    [JsonObject]
    public partial class IssueCollectionType : ICollectionType<IssueType>, IEnumerable<IssueType>
    {
        public IEnumerator<IssueType> GetEnumerator()
        {
            return Issue.OfType<IssueType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<IssueType> InnerList
        {
            get { return Issue; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Issue.OfType<IssueType>().GetEnumerator();
        }
    }
}
