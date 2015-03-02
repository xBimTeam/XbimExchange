using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xbim.Ifc2x3.ProductExtension;

namespace Xbim.COBieLite
{
    public partial class SystemType: ICOBieObject
    {
      
        public SystemType(IfcSystem ifcSystem, CoBieLiteHelper helper)
            : this()
        {
            externalEntityName = helper.ExternalEntityName(ifcSystem);
            externalID = helper.ExternalEntityIdentity(ifcSystem);
            externalSystemName = helper.ExternalSystemName(ifcSystem);
            SystemName = ifcSystem.Name;
            SystemDescription = ifcSystem.Description;
            SystemCategory = helper.GetClassification(ifcSystem);
            
            //Attributes
           var ifcAttributes = helper.GetAttributes(ifcSystem);
            if (ifcAttributes != null && ifcAttributes.Any())
                SystemAttributes = new AttributeCollectionType { Attribute = ifcAttributes };

             //TODO:
            //System Issues
            //System Documents
        }

        [XmlIgnore, JsonIgnore]
        DocumentCollectionType ICOBieObject.Documents
        {
            get { return SystemDocuments; }
            set { SystemDocuments = value; }
        }

        [XmlIgnore, JsonIgnore]
        IssueCollectionType ICOBieObject.Issues
        {
            get { return SystemIssues; }
            set { SystemIssues = value; }
        }

        [XmlIgnore, JsonIgnore]
        AttributeCollectionType ICOBieObject.Attributes
        {
            get { return SystemAttributes; }
            set { SystemAttributes = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Name
        {
            get { return SystemName; }
            set { SystemName = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Description
        {
            get { return SystemDescription; }
            set { SystemDescription = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Category
        {
            get { return SystemCategory; }
            set { SystemCategory = value; }
        }


        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Id
        {
            get { return externalID; }
            set { externalID = value; }
        }
    }
}
