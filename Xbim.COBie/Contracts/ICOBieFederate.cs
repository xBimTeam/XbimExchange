using System.Collections.Generic;

namespace Xbim.COBie.Contracts
{
    interface ICOBieFederate
    {
        COBieWorkbook Merge(List<COBieWorkbook> workbooks);
    }
}
