using Xbim.COBieLite;
using Xbim.Ifc2x3.Kernel;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingProjectTypeToIfcProject : XbimIfcMappings<string, ProjectType, IfcProject>
    {

        protected override IfcProject Mapping(ProjectType @from, IfcProject to)
        {
            to.Name = from.ProjectName;
            to.Description = from.ProjectDescription;
            return to;
        }
    }
}
