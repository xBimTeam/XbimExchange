using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.Exchanger.IfcToCOBieLiteUK.Classifications.Components
{
    public static class RegexPatterns
    {
        public static string CSVpattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"; // Splits CSV files by commas, unless the comma is inside quotes
        public static string UNIPattern = "[A-z][a-z]_[0-9]*"; // Used to find Uniclass patterns (i.e. Ss_75_50_28_97)
        public static string NBSPattern = @"[0-9]*-[0-9]*-[0-9]*\/[0-9]*"; // Used to find NBS Patterns (i.e. 75-65-95/110)
        public static string NRMPattern = @"[0-9]*\.[0-9]*\.[0-9]*"; // Used to find NRM Patterns (i.e. 5.12.1)
    }
}
