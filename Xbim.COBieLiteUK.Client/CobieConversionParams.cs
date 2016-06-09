using Xbim.FilterHelper;
using XbimExchanger.IfcToCOBieLiteUK;

namespace Xbim.COBieLiteUK.Client
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
        public bool FlipFilter { get; set; }
        public bool OpenExcel { get; set; }
        public RoleFilter Roles { get; set; }
        public bool FilterOff { get; set; }
        public EntityIdentifierMode ExtId { get; set; }
        public SystemExtractionMode SysMode { get; set; }
        public OutPutFilters Filter { get; set; }
        public string ConfigFile { get; set; }
        public bool Log { get; set; }        
    }
}
