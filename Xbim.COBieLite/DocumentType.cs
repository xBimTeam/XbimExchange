using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.COBieLite
{
    public partial class DocumentType: ICOBieObject
    {
        [XmlIgnore, JsonIgnore]
        DocumentCollectionType ICOBieObject.Documents
        {
            get { return null; }
            set {  }
        }

        [XmlIgnore, JsonIgnore]
        IssueCollectionType ICOBieObject.Issues
        {
            get { return DocumentIssues; }
            set { DocumentIssues = value; }
        }

        [XmlIgnore, JsonIgnore]
        AttributeCollectionType ICOBieObject.Attributes
        {
            get { return DocumentAttributes; }
            set { DocumentAttributes = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Name
        {
            get { return DocumentName; }
            set { DocumentName = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Description
        {
            get { return DocumentDescription; }
            set { DocumentDescription = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Category
        {
            get { return DocumentCategory; }
            set { DocumentCategory = value; }
        }


        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Id
        {
            get { return externalID; }
            set { externalID = value; }
        }
    }
}
