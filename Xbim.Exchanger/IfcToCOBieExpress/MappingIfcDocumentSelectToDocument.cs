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
    internal class MappingIfcDocumentSelectToDocument : XbimMappings<IfcStore, IModel, int, IIfcDocumentSelect, List<CobieDocument>> 
    {

        /// <summary>
        /// Helper
        /// </summary>
        private COBieExpressHelper Helper { get; set; }
        
        /// <summary>
        /// List of created documents names, used to get next duplicate name
        /// </summary>
        private List<string> UsedNames { get; set; }

        /// <summary>
        /// Stop infinite loops
        /// </summary>
        private HashSet<IIfcDocumentInformation> ChainInstMap { get; set; }

        
        /// <summary>
        /// Required by Interface
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        protected override List<CobieDocument> Mapping(IIfcDocumentSelect source, List<CobieDocument> target)
        {
            target.Clear();
            target.AddRange(MappingMulti(source));
            return target;
        }

        /// <summary>
        /// Return a document object list, in case IfcDocumentInformation.DocumentReferences more than one file
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<CobieDocument> MappingMulti(IIfcDocumentSelect source)
        {
            if (UsedNames == null) UsedNames = new List<string>();

            var docList = new List<CobieDocument>();
            if (Helper == null) Helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper;
            //process IfcDocumentReference first
            var documentReference = source as IIfcDocumentReference;
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
            var information = source as IIfcDocumentInformation;
            if(information != null)
                docList.AddRange(information.HasDocumentReferences.Select(reference => ConvertToDocument(reference, information)));

            //Do the children files
            var childDocList = GetChildDocs(information);
            //link child documents to first document in the list
            //var linkDoc = docList.FirstOrDefault();
            //if (linkDoc != null)
            //{
            //    linkDoc.Documents = childDocList;
            //}
            //else //no docs to link too, then just add to the root document list
            docList = docList.Concat(childDocList).ToList();

            return docList;
        }

        /// <summary>
        /// Get the child documents with drill down into children of child....
        /// </summary>
        /// <param name="ifcDocumentInformation">IfcDocumentInformation</param>
        /// <returns>List of Document</returns>
        private IEnumerable<CobieDocument> GetChildDocs(IIfcDocumentInformation ifcDocumentInformation)
        {
            var childDocList = new List<CobieDocument>();
            var childRels = ifcDocumentInformation?.IsPointer.FirstOrDefault();//SET[0:1] gets the relationship when ifcDocumentInformation is the RelatingDocument (parent) document
            if (childRels == null || childRels.RelatedDocuments == null) 
                return childDocList;

            foreach (var item in childRels.RelatedDocuments)
            {
                if (ChainInstMap == null) //set ChainInstMap, used to avoid infinite loop
                    ChainInstMap = new HashSet<IIfcDocumentInformation>();

                if (!ChainInstMap.Contains(item)) //check we have not already evaluated this IfcDocumentInformation
                {
                    childDocList.AddRange(MappingMulti(item)); //drill down
                }
            }
            return childDocList;
        }


        /// <summary>
        /// Convert a IfcDocumentReference to Document
        /// </summary>
        /// <param name="docReference">Document Reference Object</param>
        /// <param name="docInformation"></param>
        /// <returns>Document</returns>
        private CobieDocument ConvertToDocument(IIfcDocumentReference docReference, IIfcDocumentInformation docInformation)
        {
           
            var name = GetName(docInformation) ?? GetName(docReference);
            //fail to get from IfcDocumentReference, so try assign a default
            if (string.IsNullOrEmpty(name))
            {
                name = "Document";
            }
            //check for duplicates, if found add a (#) => "DocName(1)", if none return name unchanged
            name = Helper.GetNextName(name, UsedNames);


            var document = Exchanger.TargetRepository.Instances.New<CobieDocument>();
            document.Name= name;
            document.Created = docInformation != null ?  GetCreatedInfo(docInformation) : null;
            document.DocumentType = (docInformation != null) && !string.IsNullOrEmpty(docInformation.Purpose) ?
                Helper.GetPickValue<CobieDocumentType>(docInformation.Purpose) : 
                null;

            document.ApprovalType = (docInformation != null) && (!string.IsNullOrEmpty(docInformation.IntendedUse)) ?
                Helper.GetPickValue<CobieApprovalType>(docInformation.IntendedUse) : 
                null; //once fixed
            
            document.Stage = (docInformation != null) && (!string.IsNullOrEmpty(docInformation.Scope)) ? 
                Helper.GetPickValue<CobieStageType>(docInformation.Scope) : 
                null;

            document.Directory = GetFileDirectory(docReference);
            document.File = GetFileName(docReference);
            
            document.ExternalSystem = null;
            document.ExternalObject = Helper.GetExternalObject(docReference);
            document.ExternalId = null;

            document.Description = (docInformation != null) && !string.IsNullOrEmpty(docInformation.Description) ? docInformation.Description.ToString() : null;
            document.Reference = docInformation != null ? docInformation.Identification : null;
                                    
            UsedNames.Add(document.Name);
            return document;
        }

        /// <summary>
        /// Get created by
        /// </summary>
        /// <param name="docInfo"></param>
        /// <returns></returns>
        private CobieCreatedInfo GetCreatedInfo(IIfcDocumentInformation docInfo)
        {
            if (docInfo.DocumentOwner != null)
            {
                return Helper.GetCreatedInfo(docInfo.DocumentOwner);
            }

            //get owner from the IfcRelAssociatesDocument object
            return Helper.DocumentOwnerLookup.ContainsKey(docInfo) ? 
                Helper.GetCreatedInfo(Helper.DocumentOwnerLookup[docInfo]) : 
                null;
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


        public override List<CobieDocument> CreateTargetObject()
        {
            return new List<CobieDocument>();
        }
    }
}
