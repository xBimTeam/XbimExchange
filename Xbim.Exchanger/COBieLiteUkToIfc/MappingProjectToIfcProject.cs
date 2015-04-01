using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.GeometryResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.RepresentationResource;

namespace XbimExchanger.COBieLiteUkToIfc
{
    internal class MappingProjectToIfcProject : CoBieLiteUkIfcMappings<string, Project, IfcProject>
    {

        protected override IfcProject Mapping(Project projectType, IfcProject ifcProject)
        {
            ifcProject.Name = projectType.Name;
            ifcProject.Description = projectType.Description; 
            ifcProject.RepresentationContexts.Add(Exchanger.Model3DContext);
           
            return ifcProject;
        }

    }
}
