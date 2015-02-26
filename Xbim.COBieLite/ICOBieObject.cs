using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieLite
{
    public interface ICOBieObject
    {
        DocumentCollectionType Documents { get; set; }
        IssueCollectionType Issues { get; set; }
        AttributeCollectionType Attributes { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Category { get; set; }
        string Id { get; set; }
    }
}
