using System.Collections.Generic;
using System.Linq;
using Xbim.CobieExpress;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingIfcZoneToZone : MappingIfcObjectToAsset<IIfcZone, CobieZone>
    {
        private MappingStringToCategory _stringToCategory;
        private MappingIfcSpatialElementToSpace _elementToSpace;

        public MappingStringToCategory StringToCategory
        {
            get { return _stringToCategory ?? (_stringToCategory = Exchanger.GetOrCreateMappings<MappingStringToCategory>()); }
        }

        public MappingIfcSpatialElementToSpace ElementToSpace
        {
            get { return _elementToSpace ?? (_elementToSpace = Exchanger.GetOrCreateMappings<MappingIfcSpatialElementToSpace>()); }
        }

        protected override CobieZone Mapping(IIfcZone ifcZone, CobieZone target)
        {
            base.Mapping(ifcZone, target);

            target.Description = FirstNonEmptyString(ifcZone.Description, ifcZone.Name);

            if (!target.Categories.Any() || target.Categories.Contains(Helper.UnknownCategory))
                if (!string.IsNullOrWhiteSpace(ifcZone.ObjectType))
                {
                    target.Categories.Clear();
                    var category =  StringToCategory.GetOrCreate(ifcZone.ObjectType);
                    target.Categories.AddIfNotPresent(category);
                }

            //get spaces in zones
            var spaces = Helper.GetSpaces(ifcZone);
            var ifcSpaces = spaces as IList<IIfcSpace> ?? spaces.ToList();
            if (ifcSpaces.Any())
            {
                foreach (var space in ifcSpaces)
                {
                    CobieSpace cSpace;
                    if (ElementToSpace.GetOrCreateTargetObject(space.EntityLabel, out cSpace))
                        ElementToSpace.AddMapping(space, cSpace);
                    target.Spaces.Add(cSpace);
                }
            }

            //TODO: Space Issues
            
            return target;
        }

        public override CobieZone CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieZone>();
        }
    }
}
