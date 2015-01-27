using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.Exchange
{
    public interface IOperation
    {
        object Execute(object input);
        bool PostProcess();
    }

}
