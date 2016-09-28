using System;

namespace Xbim.COBie.EqCompare
{
    public interface ICompareEqRule
    {
        StringComparison EqRule {get;}
        CompareType CompareMethod { get;}

    }
}
