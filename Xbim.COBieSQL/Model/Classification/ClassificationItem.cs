using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieSQL.Model.Classification
{
    public class ClassificationItem
    {
        public uint ClassificationItemId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public virtual ClassificationItem Parent { get; set; }
        
        public uint ClassificationId { get; set; }
        public virtual Classification Classification { get; set; }
    }
}
