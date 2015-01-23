using Xbim.COBieLite;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingFacilityTypeToIfcBuilding : XbimMappings<XbimModel, string, FacilityType, IfcBuilding>
    {
        public MappingFacilityTypeToIfcBuilding(XbimMappingsCollection<XbimModel> mappingsCollection)
            : base(mappingsCollection)
        {
        }

        public MappingFacilityTypeToIfcBuilding()
        {
            
        }


        protected override IfcBuilding CreateToObject(string key)
        {          
            return MappingsCollection.Repository.Instances.New<IfcBuilding>(); 
        }

        protected override IfcBuilding Mapping(FacilityType @from, IfcBuilding to)
        {
            to.Name = from.FacilityName;
            to.Description = from.FacilityDescription;
            var mappings = MappingsCollection.GetOrCreateMappings<MappingProjectTypeToIfcProject>();
            mappings.AddMapping(from.ProjectAssignment, mappings.GetOrCreate(from.externalID));
            return to;
        }
    }
}
