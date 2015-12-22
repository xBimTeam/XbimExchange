using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xbim.COBieLite.CollectionTypes;
using Xbim.Ifc2x3.ProductExtension;

namespace Xbim.COBieLite
{
    public partial class SpaceType: ICOBieObject
    {
        
        //private IfcSpace _ifcSpace;

        public SpaceType(IfcSpace ifcSpace, CoBieLiteHelper helper)
            : this()
        {
           // _ifcSpace = ifcSpace;
            externalEntityName = helper.ExternalEntityName(ifcSpace);
            externalID = helper.ExternalEntityIdentity(ifcSpace);
            externalSystemName = helper.ExternalSystemName(ifcSpace);
            SpaceName = ifcSpace.Name;
            SpaceCategory = helper.GetClassification(ifcSpace);
            SpaceDescription = ifcSpace.Description;
            SpaceSignageName = helper.GetCoBieAttribute<StringValueType>("SpaceSignageName", ifcSpace).StringValue;
            SpaceUsableHeightValue = helper.GetCoBieAttribute<DecimalValueType>("SpaceUsableHeightValue", ifcSpace);
            SpaceGrossAreaValue = helper.GetCoBieAttribute<DecimalValueType>("SpaceGrossAreaValue", ifcSpace);
            SpaceNetAreaValue = helper.GetCoBieAttribute<DecimalValueType>("SpaceNetAreaValue", ifcSpace);

            //Zone Assignment
            var zones = helper.GetZones(ifcSpace);
            if (zones != null)
            {
                var ifcZones = zones.ToArray();
                SpaceZoneAssignments = new ZoneAssignmentCollectionType { ZoneAssignment = new List<ZoneKeyType>(ifcZones.Length) };
                for (int i = 0; i < ifcZones.Length; i++)
                {
                    var zoneAssignment = new ZoneKeyType();
                    zoneAssignment.ZoneCategory = helper.GetClassification(ifcZones[i]);
                    zoneAssignment.ZoneName = ifcZones[i].Name;
                    zoneAssignment.externalIDReference = helper.ExternalEntityIdentity(ifcZones[i]);
                    SpaceZoneAssignments.Add(zoneAssignment);
                }
            }
            //Attributes
            var ifcAttributes = helper.GetAttributes(ifcSpace);
            if (ifcAttributes != null && ifcAttributes.Any())
                SpaceAttributes = new AttributeCollectionType { Attribute = ifcAttributes };
    
            //TODO:
            //Space Issues
            //Space Documents
        }

        [XmlIgnore, JsonIgnore]
        DocumentCollectionType ICOBieObject.Documents
        {
            get { return SpaceDocuments; }
            set { SpaceDocuments = value; }
        }

        [XmlIgnore, JsonIgnore]
        IssueCollectionType ICOBieObject.Issues
        {
            get { return SpaceIssues; }
            set { SpaceIssues = value; }
        }

        [XmlIgnore, JsonIgnore]
        AttributeCollectionType ICOBieObject.Attributes
        {
            get { return SpaceAttributes; }
            set { SpaceAttributes = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Name
        {
            get { return SpaceName; }
            set { SpaceName = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Description
        {
            get { return SpaceDescription; }
            set { SpaceDescription = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Category
        {
            get { return SpaceCategory; }
            set { SpaceCategory = value; }
        }


        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Id
        {
            get { return externalID; }
            set { externalID = value; }
        }
    }
}
