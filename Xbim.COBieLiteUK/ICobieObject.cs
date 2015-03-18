using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieLiteUK
{
    interface ICobieObject
    {
        List<Attribute> Attributes { get; set; }
        DateTime? CreatedOn { get; set; }
        ContactKey CreatedByAssignment { get; set; }
        List<Document> Documents { get; set; }
        List<Impact> Impacts { get; set; }
        List<Issue> Issues { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string ExternalEntity { get; set; }
        string ExternalID { get; set; }
        string ExternalSystem { get; set; }
    }
}
