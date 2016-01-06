using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingIfcDocumentSelectToDocument : XbimMappings<IfcStore, IModel, string, IIfcDocumentSelect, CobieDocument> 
    {

        /// <summary>
        /// Helper
        /// </summary>
        private COBieExpressHelper Helper
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
        protected override CobieDocument Mapping(IIfcDocumentSelect source, CobieDocument target)
        {
            throw new NotImplementedException(); //see MappingMulti method
        }

        /// <summary>
        /// Return a document object list, in case IfcDocumentInformation.DocumentReferences more than one file
        /// </summary>
        /// <param name="ifcDocumentSelect"></param>
        /// <returns></returns>
        public List<CobieDocument> MappingMulti(IIfcDocumentSelect ifcDocumentSelect)
        {
            if (UsedNames == null) UsedNames = new List<string>();

            var docList = new List<CobieDocument>();
            if (Helper == null) Helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper;
            //process IfcDocumentReference first
            var documentReference = ifcDocumentSelect as IIfcDocumentReference;
            if (documentReference != null)
            {
                var ifcDocumentReference = documentReference;
                if (ifcDocumentReference.ReferencedDocument != null)
                {
                    docList.Add(ConvertToDocument(ifcDocumentReference, ifcDocumentReference.ReferencedDocument));//ReferenceToDocument is a SET [0:1]
                    return docList;
                }
            }
            //must be IfcDocumentInformation
            var ifcDocumentInformation = ifcDocumentSelect as IIfcDocumentInformation;
            if(ifcDocumentInformation != null)
            foreach (var ifcDocumentReference in ifcDocumentInformation.HasDocumentReferences)
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
                docList = docList.Concat(childDocList).ToList();
            }

            return docList;
        }

        /// <summary>
        /// Get the child documents with drill down into children of child....
        /// </summary>
        /// <param name="ifcDocumentInformation">IfcDocumentInformation</param>
        /// <returns>List of Document</returns>
        private List<CobieDocument> GetChildDocs(IIfcDocumentInformation ifcDocumentInformation)
        {


            List<CobieDocument> childDocList = new List<CobieDocument>();
            var ChildRels = ifcDocumentInformation.IsPointer.FirstOrDefault();//SET[0:1] gets the relationship when ifcDocumentInformation is the RelatingDocument (parent) document
            if (ChildRels != null && ChildRels.RelatedDocuments != null)
            {
                foreach (var item in ChildRels.RelatedDocuments)
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
        /// <param name="ifcDocumentInformation"></param>
        /// <returns>Document</returns>
        private CobieDocument ConvertToDocument(IIfcDocumentReference ifcDocumentReference, IIfcDocumentInformation ifcDocumentInformation)
        {
           
            var name = GetName(ifcDocumentInformation) ?? GetName(ifcDocumentReference);
            //fail to get from IfcDocumentReference, so try assign a default
            if (string.IsNullOrEmpty(name))
            {
                name = "Document";
            }
            //check for duplicates, if found add a (#) => "DocName(1)", if none return name unchanged
            name = Helper.GetNextName(name, UsedNames); 

            
            var document = CreateTargetObject();
            document.Name= name;
            document.Created = ifcDocumentInformation != null ?  GetCreatedInfo(ifcDocumentInformation) : null;
            document.DocumentType = (ifcDocumentInformation != null) && !string.IsNullOrEmpty(ifcDocumentInformation.Purpose) ?
                Helper.GetPickValue<CobieDocumentType>(ifcDocumentInformation.Purpose) : 
                null;

            document.ApprovalType = (ifcDocumentInformation != null) && (!string.IsNullOrEmpty(ifcDocumentInformation.IntendedUse)) ?
                Helper.GetPickValue<CobieApprovalType>(ifcDocumentInformation.IntendedUse) : 
                null; //once fixed
            
            document.Stage = (ifcDocumentInformation != null) && (!string.IsNullOrEmpty(ifcDocumentInformation.Scope)) ? 
                Helper.GetPickValue<CobieStageType>(ifcDocumentInformation.Scope) : 
                null;

            document.Directory = GetFileDirectory(ifcDocumentReference);
            document.File = GetFileName(ifcDocumentReference);
            
            document.ExternalSystem = null;
            document.ExternalObject = Helper.GetExternalObject(ifcDocumentReference);
            document.ExternalId = null;

            document.Description = (ifcDocumentInformation != null) && (!string.IsNullOrEmpty(ifcDocumentInformation.Description)) ? ifcDocumentInformation.Description.ToString() : null;
            document.Reference = ifcDocumentInformation.Identification;
                                    
            UsedNames.Add(document.Name);
            return document;
        }

        /// <summary>
        /// Get created by
        /// </summary>
        /// <param name="ifcDocumentInformation"></param>
        /// <returns></returns>
        private CobieCreatedInfo GetCreatedInfo(IIfcDocumentInformation ifcDocumentInformation)
        {
            if (ifcDocumentInformation.DocumentOwner != null)
            {
                return Helper.GetCreatedInfo(ifcDocumentInformation.DocumentOwner);
            }

            //get owner from the IfcRelAssociatesDocument object
            if (Helper.DocumentOwnerLookup.ContainsKey(ifcDocumentInformation))
            {
                return Helper.GetCreatedInfo(Helper.DocumentOwnerLookup[ifcDocumentInformation]);
            }
            return null;
        }

        
        

        /// <summary>
        /// Get Name from IfcDocumentReference
        /// </summary>
        /// <param name="ifcDocumentReference">Document Reference Object</param>
        /// <returns>string or null</returns>
        private static string GetName(IIfcExternalReference ifcDocumentReference)
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
            
            //we ignore  ItemReference, "which refers to a system interpretable position within the document" 
            //from http://www.buildingsmart-tech.org/ifc/IFC2x3/TC1/html/ifcexternalreferenceresource/lexical/ifcdocumentreference.htm
            return null;
        }

        /// <summary>
        /// Get Name from IfcDocumentInformation
        /// </summary>
        /// <param name="ifcDocumentInformation">Document Information Object</param>
        /// <returns>string or null</returns>
        private static string GetName(IIfcDocumentInformation ifcDocumentInformation)
        {
            if (ifcDocumentInformation == null)
                return null;
            
            return !string.IsNullOrEmpty(ifcDocumentInformation.Name) ? 
                ifcDocumentInformation.Name : 
                null;
        }

        /// <summary>
        /// Get the file directory/location
        /// </summary>
        /// <param name="ifcDocumentReference">Document Reference Object</param>
        /// <returns>string</returns>
        private static string GetFileDirectory(IIfcExternalReference ifcDocumentReference)
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
        private static string GetFileName(IIfcDocumentReference ifcDocumentReference)
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


        public override CobieDocument CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieDocument>();
        }
    }
}
