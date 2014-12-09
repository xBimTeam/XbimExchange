using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbim.COBie.Contracts
{
    interface ICOBieFederate
    {
        COBieWorkbook Merge(List<COBieWorkbook> workbooks);
    }
}
