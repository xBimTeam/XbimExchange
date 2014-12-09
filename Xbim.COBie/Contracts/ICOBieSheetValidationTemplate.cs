using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbim.COBie.Contracts
{
    public interface ICOBieSheetValidationTemplate
    {
        Dictionary<Int32, Boolean> IsRequired { get; }
    }
}
