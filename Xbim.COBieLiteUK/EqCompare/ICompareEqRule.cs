using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbim.COBie.EqCompare
{
    public interface ICompareEqRule
    {
        StringComparison EqRule {get;}
        CompareType CompareMethod { get;}

    }
}
