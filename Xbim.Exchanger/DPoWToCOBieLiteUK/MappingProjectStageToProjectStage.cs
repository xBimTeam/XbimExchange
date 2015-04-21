using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.DPoW;

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    class MappingProjectStageToProjectStage : MappingAttributableObjectToCOBieObject<ProjectStage, Xbim.COBieLiteUK.ProjectStage>
    {
        protected override Xbim.COBieLiteUK.ProjectStage Mapping(ProjectStage source, Xbim.COBieLiteUK.ProjectStage target)
        {
            var result = base.Mapping(source, target);
            target.Name = source.Name;
            target.Description = source.Description;
            target.ExternalId = source.Id.ToString();
            target.ExternalSystem = "DPoW";
            
            return result;
        }
    }
}
