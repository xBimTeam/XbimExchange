using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Defines an interface used by COBieData classes, in order to allow population of the Attributes tab
    /// </summary>
    interface IAttributeProvider
    {
        void InitialiseAttributes(ref COBieSheet<COBieAttributeRow> attributeSheet);
    }
}
