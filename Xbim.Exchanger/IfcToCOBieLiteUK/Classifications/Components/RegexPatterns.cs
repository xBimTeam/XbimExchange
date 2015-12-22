namespace Xbim.Exchanger.IfcToCOBieLiteUK.Classifications.Components
{
    public static class RegexPatterns
    {
        public static string CsvPattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"; // Splits CSV files by commas, unless the comma is inside quotes
        public static string UniclassPattern = "[A-z][a-z]_[0-9]*"; // Used to find Uniclass patterns (i.e. Ss_75_50_28_97)
        public static string NbsPattern = @"[0-9]*-[0-9]*-[0-9]*\/[0-9]*"; // Used to find NBS Patterns (i.e. 75-65-95/110)
        public static string NrmPattern = @"[0-9]*\.[0-9]*\.[0-9]*"; // Used to find NRM Patterns (i.e. 5.12.1)
    }
}
