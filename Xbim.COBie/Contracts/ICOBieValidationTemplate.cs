using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbim.COBie.Contracts
{
    public interface ICOBieValidationTemplate
    {
        Dictionary<String, ICOBieSheetValidationTemplate> Sheet { get; }
    }
}
