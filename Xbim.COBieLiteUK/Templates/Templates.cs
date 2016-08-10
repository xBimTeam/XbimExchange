using System.Collections.Generic;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace Xbim.CobieLiteUk
{
    public static class Templates
    {
        public static string FullResourceName(string templateType, ExcelTypeEnum excelFormatType)
        {
            var extension = excelFormatType == ExcelTypeEnum.XLS
                ? "xls"
                : "xlsx";
            return string.Format("Xbim.CobieLiteUk.Templates.{0}.{1}", templateType, extension.ToLowerInvariant());
        }

        public static IEnumerable<string> GetAvalilableTemplateTypes()
        {
            var re = new Regex("Xbim.CobieLiteUk.Templates.([a-zA-Z0-9]+).xls[x]*");
            var available = global::System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();

            var emitted = new List<string>();
            foreach (var iter in available)
            {
                var m = re.Match(iter);
                if (!m.Success) 
                    continue;
                var ret = m.Groups[1].Value;
                if (emitted.Contains(ret))
                    continue;
                emitted.Add(ret);
                yield return ret;
            }
        }
    }
}
