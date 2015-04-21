using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;
using FacilityType = Xbim.COBieLiteUK.Facility;
using Xbim.DPoW;

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    public class DPoWToCOBieLiteUKExchanger: XbimExchanger<PlanOfWork, FacilityType>
    {
       /// <summary>
        /// Constructs new exchanger. When converting data from DPoW to COBieLite it is possible to convert only one stage to COBieLite
        /// as there is no concept of project stages in COBieLite schema which is more focused on exchange of actual existing data at certain
        /// project stage rather than modelling multiple stages.
        /// </summary>
        /// <param name="source">Source DPoW model</param>
        /// <param name="target">Target COBieLite model</param>
        /// <param name="stage">Specific project stage</param>
        public DPoWToCOBieLiteUKExchanger(PlanOfWork source, FacilityType target, Xbim.DPoW.ProjectStage stage) : base(source, target)
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
        public DPoWToCOBieLiteUKExchanger(PlanOfWork source, FacilityType target)
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
            var result = mappings.AddMapping(SourceRepository, TargetRepository);

            //run validate with optional fix to get data as good as possible
            var log = new StringWriter();
            result.ValidateUK2012(log, true);

            return result;
        }

        /// <summary>
        /// Converts DPoW model to COBieLite where FacilityType is the root element of the data model
        /// </summary>
        /// <param name="createdOn">Date when Plan of Work has been created</param>
        /// <returns>COBieLite root element</returns>
        public FacilityType Convert(DateTime createdOn)
        {
            SourceRepository.CreatedOn = createdOn;
            return Convert();
        }
    }
}
