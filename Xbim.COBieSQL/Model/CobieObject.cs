using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieSQL.Model.Enumerations;

namespace Xbim.COBieSQL.Model
{
    public abstract class CobieObject
    {
        public int CobieObjectId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public int CreatedById { get; set; }
        public virtual Contact CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ExternalSystemId { get; set; }
        public virtual ExternalSystem ExternalSystem { get; set; }

        public string ExternalObject { get; set; }

        public string ExternalIdentifier { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        public virtual ICollection<Attribute> Attributes { get; set; }

        public virtual ICollection<Issue> Issues { get; set; }

        public virtual ICollection<Impact> Impacts { get; set; }
        
        public virtual ICollection<Representation> Representations { get; set; } 
    }
}
