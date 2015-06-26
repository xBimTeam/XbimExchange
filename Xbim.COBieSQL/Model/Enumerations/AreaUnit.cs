using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieSQL.Model.Enumerations
{
    public class AreaUnit : ICobieEnumeration
    {
        public int AreaUnitId { get; set; }
        public string Name { get; set; }
    }
}
