using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Xbim.COBie.Contracts
{
    interface ICOBieDeserialiser
    {
        COBieWorkbook Deserialise();
    }
}
