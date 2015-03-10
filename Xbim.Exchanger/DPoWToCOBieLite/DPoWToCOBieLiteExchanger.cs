using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW;

namespace XbimExchanger.DPoWToCOBieLite
{
    public class DPoWToCOBieLiteExchanger: XbimExchanger<PlanOfWork, FacilityType>
    {
       /// <summary>
        /// Constructs new exchanger. When converting data from DPoW to COBieLite it is possible to convert only one stage to COBieLite
        /// as there is no concept of project stages in COBieLite schema which is more focused on exchange of actual existing data at certain
        /// project stage rather than modelling multiple stages.
        /// </summary>
        /// <param name="source">Source DPoW model</param>
        /// <param name="target">Target COBieLite model</param>
        /// <param name="stage">Specific project stage</param>
        public DPoWToCOBieLiteExchanger(PlanOfWork source, FacilityType target, ProjectStage stage) : base(source, target)
        {
            Context = stage;
        }

        /// <summary>
        /// Constructs new exchanger. When converting data from DPoW to COBieLite it is possible to convert only one stage to COBieLite
        /// as there is no concept of project stages in COBieLite schema which is more focused on exchange of actual existing data at certain
        /// project stage rather than modelling multiple stages. If you don't specify a project stage only current project stage will be converted.
        /// </summary>
        /// <param name="source">Source DPoW model</param>
        /// <param name="target">Target COBieLite model</param>
        public DPoWToCOBieLiteExchanger(PlanOfWork source, FacilityType target)
            : base(source, target)
        {
            Context = source.Project.GetCurrentProjectStage(source);
        }

        /// <summary>
        /// Converts DPoW model to COBieLite where FacilityType is the root element of the data model
        /// </summary>
        /// <returns>COBieLite root element</returns>
        public override FacilityType Convert()
        {
            var mappings = GetOrCreateMappings<MappingPlanOfWorkToFacilityType>();
            return mappings.AddMapping(SourceRepository, TargetRepository);
        }
    }
}
