using System;
using System.Collections.Generic;

namespace Xbim.COBie.Contracts
{
    public interface ICOBieValidationTemplate
    {
        Dictionary<String, ICOBieSheetValidationTemplate> Sheet { get; }
    }
}
