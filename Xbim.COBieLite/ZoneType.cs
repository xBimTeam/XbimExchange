using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xbim.Ifc4.Interfaces;


namespace Xbim.COBieLite
{
    public partial class ZoneType: ICOBieObject
    {
    

        public ZoneType(IIfcZone ifcZone, CoBieLiteHelper helper)
            : this()
        {
            externalEntityName = helper.ExternalEntityName(ifcZone);
            externalID = helper.ExternalEntityIdentity(ifcZone);
            externalSystemName = helper.ExternalSystemName(ifcZone);
            ZoneDescription = ifcZone.Description;
            ZoneCategory = helper.GetClassification(ifcZone);
            ZoneName = ifcZone.Name;
            //Attributes
            var ifcAttributes = helper.GetAttributes(ifcZone);
            if (ifcAttributes != null && ifcAttributes.Any())
                ZoneAttributes = new AttributeCollectionType { Attribute = ifcAttributes };

            //TODO:
            //Space Issues
            //Space Documents
        }

        [XmlIgnore, JsonIgnore]
        DocumentCollectionType ICOBieObject.Documents
        {
            get { return ZoneDocuments; }
            set { ZoneDocuments = value; }
        }

        [XmlIgnore, JsonIgnore]
        IssueCollectionType ICOBieObject.Issues
        {
            get { return ZoneIssues; }
            set { ZoneIssues = value; }
        }

        [XmlIgnore, JsonIgnore]
        AttributeCollectionType ICOBieObject.Attributes
        {
            get { return ZoneAttributes; }
            set { ZoneAttributes = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Name
        {
            get { return ZoneName; }
            set { ZoneName = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Description
        {
            get { return ZoneDescription; }
            set { ZoneDescription = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Category
        {
            get { return ZoneCategory; }
            set { ZoneCategory = value; }
        }


        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Id
        {
            get { return externalID; }
            set { externalID = value; }
        }
    }
}
