using System.Collections.Generic;
using Xbim.COBieLiteUK;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    class MappingIfcProjectToProject : XbimMappings<IfcStore, List<Facility>, string, IIfcProject, Project>
    {
        protected override Project Mapping(IIfcProject source, Project target)
        {
            var helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper;
            target.ExternalEntity = helper.ExternalEntityName(source);
            target.ExternalId = helper.ExternalEntityIdentity(source);
            target.AltExternalId = source.GlobalId;
            target.Name = source.Name;
            target.Description = source.Description;
            return target;
        }

        public override Project CreateTargetObject()
        {
            return new Project();
        }
    }
}
