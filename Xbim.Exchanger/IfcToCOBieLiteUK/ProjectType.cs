
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Kernel;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectType:Project
    {
        /// <summary>
        /// 
        /// </summary>
        public ProjectType()
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcProject"></param>
        /// <param name="helper"></param>
        public ProjectType(IfcProject ifcProject, CoBieLiteUkHelper helper)
            : this()
        {
            ExternalEntity = helper.ExternalEntityName(ifcProject);
            ExternalId = helper.ExternalEntityIdentity(ifcProject);
            Name = ifcProject.Name;
            Description = ifcProject.Description;
        }
    }
}
