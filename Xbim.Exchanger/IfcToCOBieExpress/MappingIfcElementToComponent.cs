using System.Linq;
using Xbim.CobieExpress;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingIfcElementToComponent : MappingIfcObjectToAsset<IIfcElement, CobieComponent>
    {
        private MappingIfcSpatialElementToSpace _elementToSpace;

        public MappingIfcSpatialElementToSpace ElementToSpace
        {
            get { return _elementToSpace ?? (_elementToSpace = Exchanger.GetOrCreateMappings<MappingIfcSpatialElementToSpace>()); }
        }

        protected override CobieComponent Mapping(IIfcElement ifcElement, CobieComponent target)
        {
            base.Mapping(ifcElement, target);

            target.AssetIdentifier = Helper.GetCoBieProperty("AssetIdentifier", ifcElement);
            target.BarCode = Helper.GetCoBieProperty("AssetBarCode", ifcElement);
            if(!string.IsNullOrWhiteSpace(ifcElement.Description))
                target.Description = ifcElement.Description;
            else
            {
                 target.Description = Helper.GetCoBieProperty("AssetSerialNumber", ifcElement);
            }
            target.InstallationDate = Helper.GetCoBieProperty<DateTimeValue>("AssetInstallationDate", ifcElement);
            target.SerialNumber = Helper.GetCoBieProperty("AssetSerialNumber", ifcElement);
            target.TagNumber = Helper.GetCoBieProperty("AssetTagNumber", ifcElement);
            target.WarrantyStartDate = Helper.GetCoBieProperty<DateTimeValue>("AssetWarrantyStartDate", ifcElement);


            //System Assignments

            //Space Assignments
            var spatialElements = Helper.GetSpaces(ifcElement);

            var ifcSpatialStructureElements = spatialElements.ToList();
            if (ifcSpatialStructureElements.Any())
            {
                foreach (var spatialElement in ifcSpatialStructureElements)
                {
                    CobieSpace space;
                    if (ElementToSpace.GetOrCreateTargetObject(spatialElement.EntityLabel, out space))
                        ElementToSpace.AddMapping(spatialElement, space);
                    target.Spaces.Add(space);
                }
            }

            //Issues
            return target;
        }

        public override CobieComponent CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieComponent>();
        }
    }
}
