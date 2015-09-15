using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbim.COBie.EqCompare
{
    public static  class StringBuilderExtender
    {

        public static void AppendDouble(this StringBuilder sb, double? value, int noPlaces)
        {
            if (value != null)
                sb.Append(Math.Round((double)value, noPlaces));
            else
                sb.Append(0);
        }

        public static void AppendDouble(this StringBuilder sb, double value, int noPlaces)
        {
            sb.Append(Math.Round((double)value, noPlaces));
        }
    }
}
