using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.COBieLite
{
    public partial class AssemblyType: ICOBieObject
    {
        [XmlIgnore, JsonIgnore]
        DocumentCollectionType ICOBieObject.Documents
        {
            get { return AssemblyDocuments; }
            set { AssemblyDocuments = value; }
        }

        [XmlIgnore, JsonIgnore]
        IssueCollectionType ICOBieObject.Issues
        {
            get { return AssemblyIssues; }
            set { AssemblyIssues = value; }
        }

        [XmlIgnore, JsonIgnore]
        AttributeCollectionType ICOBieObject.Attributes
        {
            get { return AssemblyAttributes; }
            set { AssemblyAttributes = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Name
        {
            get { return AssemblyName; }
            set { AssemblyName = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Description
        {
            get { return AssemblyDescription; }
            set { AssemblyDescription = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Category
        {
            get { return AssemblyCategory; }
            set { AssemblyCategory = value; }
        }


        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Id
        {
            get { return externalID; }
            set { externalID = value; }
        }

    }
}
