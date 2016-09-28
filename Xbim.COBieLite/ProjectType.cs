
using Xbim.Ifc4.Interfaces;

namespace Xbim.COBieLite
{
    public partial class ProjectType
    {
        
        //private IfcProject _ifcProject;

        public ProjectType()
        {
            
        }
        public ProjectType(IIfcProject ifcProject, CoBieLiteHelper helper)
            : this()
        {

          //  _ifcProject = ifcProject;
            externalEntityName = helper.ExternalEntityName(ifcProject);
            externalID = helper.ExternalEntityIdentity(ifcProject);
            externalSystemName = helper.ExternalSystemName(ifcProject);
            ProjectName = ifcProject.Name;
            ProjectDescription = ifcProject.Description;

        }
    }
}
