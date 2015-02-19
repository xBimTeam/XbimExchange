using Xbim.COBieLite;
using Xbim.Ifc2x3.Kernel;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingProjectTypeToIfcProject : CoBieLiteIfcMappings<string, ProjectType, IfcProject>
    {

        protected override IfcProject Mapping(ProjectType projectType, IfcProject ifcProject)
        {
            ifcProject.Name = projectType.ProjectName;
            ifcProject.Description = projectType.ProjectDescription;
           
            return ifcProject;
        }
    }
}
