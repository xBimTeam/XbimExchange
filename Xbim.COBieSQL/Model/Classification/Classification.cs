using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieSQL.Model.Classification
{
    public class Classification
    {
        public uint ClassificationId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ClassificationItem> Items { get; set; }
    }
}
