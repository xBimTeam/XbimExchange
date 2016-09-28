using Xbim.DPoW;

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    class MappingProjectStageToProjectStage : MappingAttributableObjectToCOBieObject<ProjectStage, Xbim.CobieLiteUk.ProjectStage>
    {
        protected override Xbim.CobieLiteUk.ProjectStage Mapping(ProjectStage source, Xbim.CobieLiteUk.ProjectStage target)
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
