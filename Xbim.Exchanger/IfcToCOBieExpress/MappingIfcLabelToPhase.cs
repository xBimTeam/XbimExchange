using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.MeasureResource;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingIfcLabelToPhase : XbimMappings<IfcStore, IModel, int, IfcLabel?, CobiePhase>
    {
        protected override CobiePhase Mapping(IfcLabel? source, CobiePhase target)
        {
            target.Name = source?.Value as string;
            target.Project = target.Model.Instances.OfType<CobieProject>().FirstOrDefault();
            return target;
        }

        public override CobiePhase CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobiePhase>();
        }
    }
}
