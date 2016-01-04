using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xbim.COBieLite.CollectionTypes;
using Xbim.Ifc.Extensions;
using Xbim.Ifc4.Interfaces;

namespace Xbim.COBieLite
{
    public partial class FloorType: ICOBieObject
    {
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //private IIfcBuildingStorey _ifcBuildingStorey;

   
        public FloorType(IIfcBuildingStorey ifcBuildingStorey, CoBieLiteHelper helper)
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
           
            var spaces = ifcBuildingStorey.Spaces;
            var ifcSpaces = spaces as IList<IIfcSpace> ?? spaces.ToList();
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

        [XmlIgnore, JsonIgnore]
        DocumentCollectionType ICOBieObject.Documents
        {
            get { return FloorDocuments; }
            set { FloorDocuments = value; }
        }

        [XmlIgnore, JsonIgnore]
        IssueCollectionType ICOBieObject.Issues
        {
            get { return FloorIssues; }
            set { FloorIssues = value; }
        }

        [XmlIgnore, JsonIgnore]
        AttributeCollectionType ICOBieObject.Attributes
        {
            get { return FloorAttributes; }
            set { FloorAttributes = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Name
        {
            get { return FloorName; }
            set { FloorName = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Description
        {
            get { return FloorDescription; }
            set { FloorDescription = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Category
        {
            get { return FloorCategory; }
            set { FloorCategory = value; }
        }


        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Id
        {
            get { return externalID; }
            set { externalID = value; }
        }
    }
}
