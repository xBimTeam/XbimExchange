using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieSQL.Model.Enumerations
{
    public class VolumeUnit: ICobieEnumeration
    {
        public int VolumeUnitId { get; set; }
        public string Name { get; set; }

        public int FacilityId { get; set; }
        public Facility Facility { get; set; }
    }
}
