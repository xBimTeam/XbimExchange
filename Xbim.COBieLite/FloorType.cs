using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLite.CollectionTypes;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.ProductExtension;

namespace Xbim.COBieLite
{
    public partial class FloorType
    {
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //private IfcBuildingStorey _ifcBuildingStorey;

        public FloorType()
        {
            
        }
        public FloorType(IfcBuildingStorey ifcBuildingStorey, CoBieLiteHelper helper)
            : this()
        {
         //   _ifcBuildingStorey = ifcBuildingStorey;
            externalEntityName = helper.ExternalEntityName(ifcBuildingStorey);
            externalID = helper.ExternalEntityIdentity(ifcBuildingStorey);
            externalSystemName = helper.ExternalSystemName(ifcBuildingStorey);
            FloorName = ifcBuildingStorey.Name;
            FloorCategory = helper.GetClassification(ifcBuildingStorey);
            FloorDescription = ifcBuildingStorey.Description;
            //set the fall backs
            
            if (ifcBuildingStorey.Elevation.HasValue)
            {
                FloorElevationValue = new DecimalValueType
                {
                    DecimalValue = ifcBuildingStorey.Elevation.Value,
                    DecimalValueSpecified = true
                };
                //TODO: work out if the units can change
                //FloorElevationValue.UnitName = 
            }
            FloorHeightValue = helper.GetCoBieAttribute<DecimalValueType>("FloorHeightValue", ifcBuildingStorey);
            //var heightProperty = ifcBuildingStorey.GetTotalHeightProperty();
            //if (heightProperty != null)
            //{
            //    FloorHeightValue = new DecimalValueType
            //    {
            //        DecimalValue = heightProperty.LengthValue,
            //        DecimalValueSpecified = true,
            //        UnitName = heightProperty.Unit.GetName()
            //    };
            //}
           
            var spaces = ifcBuildingStorey.GetSpaces();
            var ifcSpaces = spaces as IList<IfcSpace> ?? spaces.ToList();
            if (ifcSpaces.Any())
            {
                Spaces = new SpaceCollectionType {Space = new List<SpaceType>(ifcSpaces.Count)};
                for (var i = 0; i < ifcSpaces.Count; i++)
                {
                    Spaces.Add(new SpaceType(ifcSpaces[i], helper));
                }
            }

            //Attributes
            var ifcAttributes = helper.GetAttributes(ifcBuildingStorey);
            if (ifcAttributes != null && ifcAttributes.Any())
                FloorAttributes = new AttributeCollectionType { Attribute = ifcAttributes };

            //TODO:
            //Space Issues
            //Space Documents
        }
    }
}
