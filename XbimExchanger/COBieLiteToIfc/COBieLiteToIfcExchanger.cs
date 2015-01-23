using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;

namespace XbimExchanger.COBieLiteToIfc
{
    public class CoBieLiteToIfcExchanger
    {
        public bool Convert(XbimModel model, FacilityType facility)
        {
            var mapper = new XbimMappingsCollection<XbimModel>(model);
            var mapping = mapper.GetOrCreateMappings<MappingFacilityTypeToIfcBuilding>();
            var building = mapping.GetOrCreateTargetObject(facility.externalID);
            building = mapping.AddMapping(facility, building);
            return true;
        }
    }
}
