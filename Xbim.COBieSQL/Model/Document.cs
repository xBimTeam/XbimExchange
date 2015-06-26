using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieSQL.Model
{
    [Table("Documents")]
    public class Document: CobieObject
    {
        public int DocumentId { get; set; }
    }
}
