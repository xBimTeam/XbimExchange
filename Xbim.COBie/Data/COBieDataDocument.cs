using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.COBie.Serialisers.XbimSerialiser;
using Xbim.Ifc4.Interfaces;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Document tab.
    /// </summary>
    public class COBieDataDocument : COBieData<COBieDocumentRow>
    {
        /// <summary>
        /// Data Document constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataDocument(COBieContext context) : base(context)
        { }

        #region Methods

        /// <summary>
        /// Fill sheet rows for Document sheet
        /// </summary>
        /// <returns>COBieSheet</returns>
        public override COBieSheet<COBieDocumentRow> Fill()
        {
            ProgressIndicator.ReportMessage("Starting Documents...");
            var ifcProject = Model.Instances.FirstOrDefault<IIfcProject>();
            Debug.Assert(ifcProject != null);

            //create new sheet
            COBieSheet<COBieDocumentRow> documents = new COBieSheet<COBieDocumentRow>(Constants.WORKSHEET_DOCUMENT);

            // get all IfcBuildingStory objects from IFC file
            IEnumerable<IfcDocumentInformation> docInfos = Model.FederatedInstances.OfType<IfcDocumentInformation>();
            ProgressIndicator.Initialise("Creating Documents", docInfos.Count());

            foreach (IfcDocumentInformation di in docInfos)
            {
                ProgressIndicator.IncrementAndUpdate();

                COBieDocumentRow doc = new COBieDocumentRow(documents);
                
                
                doc.Name = (di == null) ? "" : di.Name.ToString();
                //get the first associated document to extract the objects the document refers to
                IfcRelAssociatesDocument ifcRelAssociatesDocument = DocumentInformationForObjects(di).FirstOrDefault();
                
                
                if ((ifcRelAssociatesDocument != null) && (ifcRelAssociatesDocument.OwnerHistory != null))
                    doc.CreatedBy = GetTelecomEmailAddress(ifcRelAssociatesDocument.OwnerHistory);
                else if (di.DocumentOwner != null)
                {
                   if (di.DocumentOwner is IfcPersonAndOrganization)
                        doc.CreatedBy = GetTelecomEmailAddress(di.DocumentOwner as IfcPersonAndOrganization);
                    else if (di.DocumentOwner is IfcPerson)
                        doc.CreatedBy = GetEmail(null, di.DocumentOwner as IfcPerson);
                    else if (di.DocumentOwner is IfcOrganization)
                        doc.CreatedBy = GetEmail(di.DocumentOwner as IfcOrganization, null);
                }
                else if (ifcProject.OwnerHistory != null)
                    doc.CreatedBy = GetTelecomEmailAddress(ifcProject.OwnerHistory);


                if ((ifcRelAssociatesDocument != null) && (ifcRelAssociatesDocument.OwnerHistory != null))
                    doc.CreatedOn = GetCreatedOnDateAsFmtString(ifcRelAssociatesDocument.OwnerHistory);
                else if (di.CreationTime != null)
                    doc.CreatedOn = di.CreationTime.ToString();
                else if (ifcProject.OwnerHistory != null)
                    doc.CreatedOn = Context.RunDateTime;

                
                doc.Category = (string.IsNullOrEmpty(di.Purpose.ToString()) ) ? DEFAULT_STRING :di.Purpose.ToString();

                doc.ApprovalBy = (string.IsNullOrEmpty(di.IntendedUse.ToString())) ? DEFAULT_STRING : di.IntendedUse.ToString();
                doc.Stage = (string.IsNullOrEmpty(di.Scope.ToString())) ? DEFAULT_STRING : di.Scope.ToString();

                
                RelatedObjectInformation relatedObjectInfo = GetRelatedObjectInformation(ifcRelAssociatesDocument);
                doc.SheetName = relatedObjectInfo.SheetName;
                doc.RowName = relatedObjectInfo.Name;
                doc.ExtObject = relatedObjectInfo.ExtObject;
                doc.ExtIdentifier = relatedObjectInfo.ExtIdentifier;
                doc.ExtSystem = relatedObjectInfo.ExtSystem;
                
                FileInformation fileInfo = GetFileInformation(ifcRelAssociatesDocument);
                doc.File = fileInfo.Name;
                doc.Directory = GetDirectory(!string.IsNullOrWhiteSpace(fileInfo.Location) ? fileInfo.Location : fileInfo.Name);

                
                doc.Description = (string.IsNullOrEmpty(di.Description)) ? DEFAULT_STRING : di.Description.ToString();
                doc.Reference = (string.IsNullOrEmpty(di.DocumentId.Value.ToString())) ? DEFAULT_STRING : di.DocumentId.Value.ToString();

                documents.AddRow(doc);
            }

            documents.OrderBy(s => s.Name);

            ProgressIndicator.Finalise();
            return documents;
        }

        private string GetDirectory(string dir)
        {
            if (!Uri.IsWellFormedUriString(dir, UriKind.Absolute))
            {
                if (dir.Last() != '/')
                {
                    dir = dir + "/";
                }
            }
            return dir;
        }

        /// <summary>
        /// Get the file information for the document attached to the ifcRelAssociatesDocument
        /// </summary>
        /// <param name="ifcRelAssociatesDocument">IfcRelAssociatesDocument object</param>
        /// <returns>FileInformation structure </returns>
        private FileInformation GetFileInformation(IfcRelAssociatesDocument ifcRelAssociatesDocument)
        {
            FileInformation DocInfo = new FileInformation() { Name = DEFAULT_STRING, Location = DEFAULT_STRING };
            string value = "";
                
            if (ifcRelAssociatesDocument != null)
            {
                //test for single document
                IfcDocumentReference ifcDocumentReference = ifcRelAssociatesDocument.RelatingDocument as IfcDocumentReference;
                if (ifcDocumentReference != null) 
                {
                    //this is possibly incorrect, think it references information within a document
                    value = ifcDocumentReference.ItemReference.ToString();
                    if (!string.IsNullOrEmpty(value)) DocInfo.Name = value;
                    value = ifcDocumentReference.Location.ToString();
                    if (!string.IsNullOrEmpty(value)) DocInfo.Location = value;
                }
                else //test for a document list
                {
                    IfcDocumentInformation ifcDocumentInformation = ifcRelAssociatesDocument.RelatingDocument as IfcDocumentInformation;
                    if (ifcDocumentInformation != null)
                    {
                        IEnumerable<IfcDocumentReference> ifcDocumentReferences = ifcDocumentInformation.DocumentReferences;
                        if (ifcDocumentReferences != null)
                        {
                            List<string> strNameValues = new List<string>();
                            List<string> strLocationValues = new List<string>();
                            foreach (IfcDocumentReference docRef in ifcDocumentReferences)
                            {
                                //get file name
                                value = docRef.ItemReference.ToString();
                                if (!string.IsNullOrEmpty(value))
                                    strNameValues.Add(value);
                                else
                                {
                                    value = docRef.Name.ToString();
                                    if (!string.IsNullOrEmpty(value))
                                        strNameValues.Add(value);
                                }
                                //get file location
                                value = docRef.Location.ToString();
                                if ((!string.IsNullOrEmpty(value)) && (!strNameValues.Contains(value))) strLocationValues.Add(value);
                            }
                            //set values to return
                            if (strNameValues.Count > 0) DocInfo.Name = COBieXBim.JoinStrings(':', strNameValues);
                            if (strLocationValues.Count > 0) DocInfo.Location = COBieXBim.JoinStrings(':', strLocationValues);
                            
                        }
                       
                    }
                }
            }
            return DocInfo;
        }
       
        /// <summary>
        /// Get the related object information for the document
        /// </summary>
        /// <param name="ifcRelAssociatesDocument">IfcRelAssociatesDocument object</param>
        /// <returns>RelatedObjectInformation structure</returns>
        private RelatedObjectInformation GetRelatedObjectInformation(IfcRelAssociatesDocument ifcRelAssociatesDocument)
        {
            RelatedObjectInformation objectInfo = new RelatedObjectInformation { SheetName = DEFAULT_STRING, Name = DEFAULT_STRING, ExtIdentifier = DEFAULT_STRING, ExtObject = DEFAULT_STRING, CreatedBy = DEFAULT_STRING, CreatedOn = DEFAULT_STRING, ExtSystem = DEFAULT_STRING };
            if (ifcRelAssociatesDocument != null)
            {
                IfcRoot relatedObject = ifcRelAssociatesDocument.RelatedObjects.FirstOrDefault();
                if (relatedObject != null)
                {
                    string value = GetSheetByObjectType(relatedObject.GetType());
                    
                    if (!string.IsNullOrEmpty(value)) objectInfo.SheetName = value;
                    value = relatedObject.Name.ToString();
                    if (!string.IsNullOrEmpty(value)) objectInfo.Name = value;
                    objectInfo.ExtObject = relatedObject.GetType().Name;
                    objectInfo.ExtIdentifier = ifcRelAssociatesDocument.GlobalId;
                    objectInfo.ExtSystem = GetExternalSystem(ifcRelAssociatesDocument.OwnerHistory); 

                    objectInfo.CreatedBy = GetTelecomEmailAddress(ifcRelAssociatesDocument.OwnerHistory);
                    objectInfo.CreatedOn = GetCreatedOnDateAsFmtString(ifcRelAssociatesDocument.OwnerHistory);
                }
            }
            return objectInfo;
        }


        //fields for DocumentInformationForObjects
        List<IfcRelAssociatesDocument> ifcRelAssociatesDocuments = null;

        /// <summary>
        /// Missing Inverse method on  IfcDocumentInformation, need to be implemented on IfcDocumentInformation class
        /// </summary>
        /// <param name="ifcDocumentInformation">IfcDocumentInformation object</param>
        /// <returns>IEnumerable of IfcRelAssociatesDocument objects</returns>
        public  IEnumerable<IfcRelAssociatesDocument> DocumentInformationForObjects (IfcDocumentInformation ifcDocumentInformation )
        {
            if (ifcRelAssociatesDocuments == null)
            {
                ifcRelAssociatesDocuments = Model.FederatedInstances.OfType<IfcRelAssociatesDocument>().ToList();
            }
            return ifcRelAssociatesDocuments.Where<IfcRelAssociatesDocument>(irad => (irad.RelatingDocument as IfcDocumentInformation) == ifcDocumentInformation);
        }


        #endregion

        public struct FileInformation
        {
            public string Name { get; set; }
            public string Location { get; set; }
        }

        public struct RelatedObjectInformation
        {
            public string SheetName { get; set; }
            public string Name { get; set; }
            public string ExtObject { get; set; }
            public string ExtIdentifier { get; set; }
            public string ExtSystem { get; set; }
            public string CreatedBy { get; set; }
            public string CreatedOn { get; set; }
        }
    }
}
