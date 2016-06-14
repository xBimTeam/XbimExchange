using System.Collections.Generic;
using Xbim.Common;
using Xbim.CobieLiteUk;
using Xbim.Ifc4.Interfaces;


namespace XbimExchanger.IfcToCOBieLiteUK
{
    class MappingIfcProjectToProject : XbimMappings<IModel, List<Facility>, string, IIfcProject, Project>
    {
        protected override Project Mapping(IIfcProject source, Project target)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
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
