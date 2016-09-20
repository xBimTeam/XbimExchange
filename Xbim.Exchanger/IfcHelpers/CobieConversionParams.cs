using Xbim.CobieLiteUk;
using Xbim.CobieLiteUk.FilterHelper;

namespace XbimExchanger.IfcHelpers
{
    /// <summary>
    /// Params Class, holds parameters for worker to access
    /// </summary>
    public class CobieConversionParams
    {
        public object Source { get; set; }
        public string OutputFileName { get; set; }
        public string TemplateFile { get; set; }
        public ExportFormatEnum ExportFormat { get; set; }
        public EntityIdentifierMode ExtId { get; set; }
        public SystemExtractionMode SysMode { get; set; }
        public OutPutFilters Filter { get; set; }
        public string ConfigFile { get; set; }
        /// <summary>
        /// Produce COBie validation log file
        /// </summary>
        public bool Log { get; set; }        
    }
}
