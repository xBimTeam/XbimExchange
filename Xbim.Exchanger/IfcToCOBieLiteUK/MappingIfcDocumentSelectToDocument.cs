using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xbim.Common;
using Xbim.CobieLiteUk;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    internal class MappingIfcDocumentSelectToDocument : XbimMappings<IModel, List<Facility>, string, IIfcDocumentSelect, Document> 
    {

        /// <summary>
        /// Helper
        /// </summary>
        private CoBieLiteUkHelper Helper
        { get; set; }
        
        /// <summary>
        /// List of created documents names, used to get next duplicate name
        /// </summary>
        private List<string> UsedNames
        { get; set; }

        /// <summary>
        /// Stop infinite loops
        /// </summary>
        private HashSet<IIfcDocumentInformation> ChainInstMap
        { get; set; }

        
        /// <summary>
        /// Required by Interface
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        protected override Document Mapping(IIfcDocumentSelect source, Document target)
        {
            throw new NotImplementedException(); //see MappingMulti method
        }

        /// <summary>
        /// Return a document object list, in case IfcDocumentInformation.DocumentReferences more than one file
        /// </summary>
        /// <param name="ifcDocumentSelect"></param>
        /// <returns></returns>
        public List<Document> MappingMulti(IIfcDocumentSelect ifcDocumentSelect)
        {
            if (UsedNames == null) UsedNames = new List<string>();

            List<Document> docList = new List<Document>();
            if (Helper == null) Helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            //process IfcDocumentReference first
            if (ifcDocumentSelect is IIfcDocumentReference)
            {
                var ifcDocumentReference = ifcDocumentSelect as IIfcDocumentReference;
                if (ifcDocumentReference.ReferencedDocument != null)
                {
                    docList.Add(ConvertToDocument(ifcDocumentReference, ifcDocumentReference.ReferencedDocument));//ReferenceToDocument is a SET [0:1]
                    return docList;
                }
            }
            //must be IfcDocumentInformation
            var ifcDocumentInformation = ifcDocumentSelect as IIfcDocumentInformation;

            foreach (IIfcDocumentReference ifcDocumentReference in ifcDocumentInformation.HasDocumentReferences)
            {
                docList.Add(ConvertToDocument(ifcDocumentReference, ifcDocumentInformation));
            }

            //Do the children files
            var childDocList = GetChildDocs(ifcDocumentInformation);
            //link child documents to first document in the list
            var linkDoc = docList.FirstOrDefault();
            if (linkDoc != null)
            {
                linkDoc.Documents = childDocList;
            }
            else //no docs to link too, then just add to the root document list
            {
                docList.Concat(childDocList);
            }

            return docList;
        }

        /// <summary>
        /// Get the child documents with drill down into children of child....
        /// </summary>
        /// <param name="ifcDocumentInformation">IfcDocumentInformation</param>
        /// <returns>List of Document</returns>
        private List<Document> GetChildDocs(IIfcDocumentInformation ifcDocumentInformation)
        {
           

            List<Document> childDocList = new List<Document>();
            var ChildRels = ifcDocumentInformation.IsPointer.FirstOrDefault();//SET[0:1] gets the relationship when ifcDocumentInformation is the RelatingDocument (parent) document
            if (ChildRels != null && ChildRels.RelatedDocuments != null)
            {
                foreach (IIfcDocumentInformation item in ChildRels.RelatedDocuments)
                {
                    if (ChainInstMap == null) //set ChainInstMap, used to avoid infinite loop
                        ChainInstMap = new HashSet<IIfcDocumentInformation>();

                    if (!ChainInstMap.Contains(item)) //check we have not already evaluated this IfcDocumentInformation
                    {
                        childDocList.AddRange(MappingMulti(item)); //drill down
                    }
                }
            }
            return childDocList;
        }

        
        /// <summary>
        /// Convert a IfcDocumentReference to Document
        /// </summary>
        /// <param name="ifcDocumentReference">Document Reference Object</param>
        /// <param name="document">Document Object</param>
        /// <returns>Document</returns>
        private Document ConvertToDocument(IIfcDocumentReference ifcDocumentReference, IIfcDocumentInformation ifcDocumentInformation)
        {
           
            string name = GetName(ifcDocumentInformation) ?? GetName(ifcDocumentReference);
            //fail to get from IfcDocumentReference, so try assign a default
            if (string.IsNullOrEmpty(name))
            {
                name = "Document";
            }
            //check for duplicates, if found add a (#) => "DocName(1)", if none return name unchanged
            name = Helper.GetNextName(name, UsedNames); 

            var document = new Document();
            document.Name= name;
            document.CreatedBy = ifcDocumentInformation != null ?  GetCreatedBy(ifcDocumentInformation) : null;
            document.CreatedOn = ifcDocumentInformation != null ? GetCreatedOn(ifcDocumentInformation) : null;

            document.Categories = (ifcDocumentInformation != null) && (!string.IsNullOrEmpty(ifcDocumentInformation.Purpose)) ? new List<Category>(new[] { new Category { Code = ifcDocumentInformation.Purpose } }) : null;

            document.ApprovalBy = (ifcDocumentInformation != null) && (!string.IsNullOrEmpty(ifcDocumentInformation.IntendedUse)) ? ifcDocumentInformation.IntendedUse : null; //once fixed
            
            document.Stage = (ifcDocumentInformation != null) && (!string.IsNullOrEmpty(ifcDocumentInformation.Scope)) ? ifcDocumentInformation.Scope : null;

            document.Directory = GetFileDirectory(ifcDocumentReference);
            document.File = GetFileName(ifcDocumentReference);
            
            document.ExternalSystem = null;
            document.ExternalEntity = ifcDocumentReference.GetType().Name;
            document.ExternalId = null;

            document.Description = (ifcDocumentInformation != null) && (!string.IsNullOrEmpty(ifcDocumentInformation.Description)) ? ifcDocumentInformation.Description.ToString() : null;
            document.Reference = ifcDocumentInformation.Identification;
                                    
            UsedNames.Add(document.Name);
            return document;
        }

        /// <summary>
        /// Get createdOn 
        /// </summary>
        /// <param name="ifcDocumentInformation"></param>
        /// <returns></returns>
        private DateTime? GetCreatedOn(IIfcDocumentInformation ifcDocumentInformation)
        {
            if (ifcDocumentInformation.CreationTime != null)
            {
                var created = ifcDocumentInformation.CreationTime ?? ifcDocumentInformation.LastRevisionTime;
                if (created.HasValue)
                {
                    return DateTime.Parse(created);
                }
                
            } 
            
            if (Helper.DocumentOwnerLookup.ContainsKey(ifcDocumentInformation))
            {
                return Helper.GetCreatedOn(Helper.DocumentOwnerLookup[ifcDocumentInformation]);
            }

            return DateTime.Now;
        }

        /// <summary>
        /// Get created by
        /// </summary>
        /// <param name="ifcDocumentInformation"></param>
        /// <returns></returns>
        private ContactKey GetCreatedBy(IIfcDocumentInformation ifcDocumentInformation)
        {
            if (ifcDocumentInformation.DocumentOwner != null)
            {
                return Helper.GetCreatedBy(ifcDocumentInformation.DocumentOwner);
            }

            //get owner from the IfcRelAssociatesDocument object
            if (Helper.DocumentOwnerLookup.ContainsKey(ifcDocumentInformation))
            {
                return Helper.GetCreatedBy(Helper.DocumentOwnerLookup[ifcDocumentInformation]);
            }
            return null;
        }

        
        

        /// <summary>
        /// Get Name from IfcDocumentReference
        /// </summary>
        /// <param name="ifcDocumentReference">Document Reference Object</param>
        /// <returns>string or null</returns>
        private string GetName(IIfcDocumentReference ifcDocumentReference)
        {
            if (ifcDocumentReference == null)
                return null;
            
            if (!string.IsNullOrEmpty(ifcDocumentReference.Name))
            {
                return ifcDocumentReference.Name;
            }
            if (!string.IsNullOrEmpty(ifcDocumentReference.Location))
            {
                return Path.GetFileNameWithoutExtension(ifcDocumentReference.Location);
            }
            //we ignore  ItemReference, "which refers to a system interpretable position within the document" from http://www.buildingsmart-tech.org/ifc/IFC2x3/TC1/html/ifcexternalreferenceresource/lexical/ifcdocumentreference.htm

            
            return null;
        }

        /// <summary>
        /// Get Name from IfcDocumentInformation
        /// </summary>
        /// <param name="ifcDocumentInformation">Document Information Object</param>
        /// <returns>string or null</returns>
        private string GetName(IIfcDocumentInformation ifcDocumentInformation)
        {
            if (ifcDocumentInformation == null)
                return null;
            
            if (!string.IsNullOrEmpty(ifcDocumentInformation.Name))
            {
                return ifcDocumentInformation.Name;
            }

            return null;
        }

        /// <summary>
        /// Get the file directory/location
        /// </summary>
        /// <param name="ifcDocumentReference">Document Reference Object</param>
        /// <returns>string</returns>
        private string GetFileDirectory(IIfcDocumentReference ifcDocumentReference)
        {
            if (ifcDocumentReference == null)
                return null;

            if (!string.IsNullOrEmpty(ifcDocumentReference.Location))
            {
                return ifcDocumentReference.Location;
            }
            return null;
        }
        /// <summary>
        /// Get file name
        /// </summary>
        /// <param name="ifcDocumentReference">Document Reference Object</param>
        /// <returns>string</returns>
        private string GetFileName(IIfcDocumentReference ifcDocumentReference)
        {
            if (ifcDocumentReference == null)
                return null;
            if (!string.IsNullOrEmpty(ifcDocumentReference.Name))
            {
                return ifcDocumentReference.Name;
            }

            if (!string.IsNullOrEmpty(ifcDocumentReference.Location))
            {
                try
                {
                    return Path.GetFileName(ifcDocumentReference.Location);
                }
                catch (Exception) //if exception just return the stored string
                {
                    return ifcDocumentReference.Location;
                }
            }
            return null;
        }


        public override Document CreateTargetObject()
        {
            return new Document();
        }
    }
}
