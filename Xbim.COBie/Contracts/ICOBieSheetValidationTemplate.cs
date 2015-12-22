using System;
using System.Collections.Generic;

namespace Xbim.COBie.Contracts
{
    public interface ICOBieSheetValidationTemplate
    {
        Dictionary<Int32, Boolean> IsRequired { get; }
    }
}
