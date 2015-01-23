using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;

namespace XbimExchanger.COBieLiteToIfc
{
    public class MappingProjectTypeToIfcProject : XbimMappings<XbimModel, string, ProjectType, IfcProject>
    {
        public MappingProjectTypeToIfcProject(XbimMappingsCollection<XbimModel> mappingsCollection, XbimModel repository)
            : base(mappingsCollection)
        {

        }

        public MappingProjectTypeToIfcProject() 
        {
            
        }

        protected override IfcProject CreateToObject(string key)
        {
            return MappingsCollection.Repository.Instances.New<IfcProject>(); 
        }

        protected override IfcProject Mapping(ProjectType @from, IfcProject to)
        {
            to.Name = from.ProjectName;
            to.Description = from.ProjectDescription;
            return to;
        }
    }
}
