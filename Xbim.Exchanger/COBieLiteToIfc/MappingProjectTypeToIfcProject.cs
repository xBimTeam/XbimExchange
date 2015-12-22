using Xbim.COBieLite;
using Xbim.Ifc2x3.Kernel;

namespace XbimExchanger.COBieLiteToIfc
{
    internal class MappingProjectTypeToIfcProject : CoBieLiteIfcMappings<string, ProjectType, IfcProject>
    {

        protected override IfcProject Mapping(ProjectType projectType, IfcProject ifcProject)
        {
            ifcProject.Name = projectType.ProjectName;
            ifcProject.Description = projectType.ProjectDescription; 
            ifcProject.RepresentationContexts.Add(Exchanger.Model3DContext);
            return ifcProject;
        }

    }
}
