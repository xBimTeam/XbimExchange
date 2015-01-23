using Xbim.COBieLite;
using Xbim.Ifc2x3.Kernel;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingProjectTypeToIfcProject : COBieLiteIfcMappings<string, ProjectType, IfcProject>
    {

        protected override IfcProject Mapping(ProjectType @from, IfcProject to)
        {
            to.Name = from.ProjectName;
            to.Description = from.ProjectDescription;
            return to;
        }
    }
}
