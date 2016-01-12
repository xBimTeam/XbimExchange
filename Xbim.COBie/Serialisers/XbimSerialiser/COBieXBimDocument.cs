using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.ConstructionMgmtDomain;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.IO.Esent;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimDocument : COBieXBim
    {


        #region Properties
        public IEnumerable<IfcTypeObject> IfcTypeObjects { get; private set; }
        public IEnumerable<IfcElement> IfcElements { get; private set; }
        #endregion

        public COBieXBimDocument(COBieXBimContext xBimContext)
            : base(xBimContext)
        {

        }

        #region Properties

        /// <summary>
        /// Add the IfcDocumentInformation to the Model object
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieDocumentRow to read data from</param>
        public void SerialiseDocument(COBieSheet<COBieDocumentRow> cOBieSheet)
        {

            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Document"))
            {
                try
                {
                    int count = 1;
                    ProgressIndicator.ReportMessage("Starting Documents...");
                    ProgressIndicator.Initialise("Creating Documents", cOBieSheet.RowCount);
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        BumpTransaction(trans, count);
                        count++;
                        ProgressIndicator.IncrementAndUpdate();
                        COBieDocumentRow row = cOBieSheet[i];
                        AddDocument(row);
                    }
                    ProgressIndicator.Finalise();
                    trans.Commit();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        

        /// <summary>
        /// Add the data to the Document Information object
        /// </summary>
        /// <param name="row">COBieDocumentRow holding the data</param>
        private void AddDocument(COBieDocumentRow row)
        {
            if (CheckIfExistOnMerge(row)) 
                return; //already in document
            IfcDocumentInformation ifcDocumentInformation = Model.Instances.New<IfcDocumentInformation>();
            IfcRelAssociatesDocument ifcRelAssociatesDocument = Model.Instances.New<IfcRelAssociatesDocument>();
            //Add Created By, Created On and ExtSystem to Owner History. 
            SetUserHistory(ifcRelAssociatesDocument, row.ExtSystem, row.CreatedBy, row.CreatedOn );
            
            if (Contacts.ContainsKey(row.CreatedBy))
                ifcDocumentInformation.DocumentOwner = Contacts[row.CreatedBy];

            //using statement will set the Model.OwnerHistoryAddObject to IfcConstructionProductResource.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
            using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcRelAssociatesDocument.OwnerHistory))
            {
                //create relationship between Document and ifcObjects it relates too
                ifcRelAssociatesDocument.RelatingDocument = ifcDocumentInformation;
                //Add Name
                if (ValidateString(row.Name)) ifcDocumentInformation.Name = row.Name;

                //Add Category
                if (ValidateString(row.Category)) ifcDocumentInformation.Purpose = row.Category;

                //Add approved By
                if (ValidateString(row.ApprovalBy)) ifcDocumentInformation.IntendedUse = row.ApprovalBy;
                
                //Add Stage
                if (ValidateString(row.Stage)) ifcDocumentInformation.Scope = row.Stage;

                //Add GlobalId
                AddGlobalId(row.ExtIdentifier, ifcRelAssociatesDocument);

                //Add Object Relationship
                IfcRoot ifcRoot = GetObjectRelationship(row);
                //add to the document relationship object
                if (ifcRoot != null)
                    ifcRelAssociatesDocument.RelatedObjects.Add(ifcRoot);
                

                //Add Document reference
                AddDocumentReference(row, ifcDocumentInformation);
                
                //add Description
                if (ValidateString(row.Description)) ifcDocumentInformation.Description = row.Description;

                //Add Reference
                if (ValidateString(row.Reference)) 
                    ifcDocumentInformation.DocumentId = row.Reference;
                else
                    ifcDocumentInformation.DocumentId = ""; //required field so add blank string
               
            }
        }

        
        /// <summary>
        /// Check if the document information is already within the model
        /// </summary>
        /// <param name="row">COBieDocumentRow data</param>
        /// <returns>bool</returns>
        private bool CheckIfExistOnMerge(COBieDocumentRow row)
        {
            if (XBimContext.IsMerge)
            {
                if (ValidateString(row.Name)) //we have a primary key to check
                {
                    IfcRoot ifcRoot = GetObjectRelationship(row);
                    if (ifcRoot != null)
                    {
                        IfcRelAssociatesDocument ifcRelAssociatesDocument = Model.Instances.Where<IfcRelAssociatesDocument>(di => di.RelatedObjects.Contains(ifcRoot)).FirstOrDefault();
                        if (ifcRelAssociatesDocument != null)
                        {
                            string testName = row.Name.ToLower().Trim();
                            if ((ifcRelAssociatesDocument.RelatingDocument is IfcDocumentInformation) &&
                                ((ifcRelAssociatesDocument.RelatingDocument as IfcDocumentInformation).Name.ToString().ToLower().Trim() == testName)
                                )
                            {
#if DEBUG
                                Console.WriteLine("{0} : with document {1} attached to {2} exists so skip on merge", ifcRelAssociatesDocument.GetType().Name, row.Name, ifcRoot.Name);
#endif
                                return true; //we have it so no need to create
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Add the document references to the IfcDocumentInformation object
        /// </summary>
        /// <param name="row">COBieDocumentRow holding row data</param>
        /// <param name="ifcDocumentInformation">IfcDocumentInformation object to add references too</param>
        private void AddDocumentReference(COBieDocumentRow row, IfcDocumentInformation ifcDocumentInformation)
        {
            if (ValidateString(row.File))
            {
                //get locations, assume we have the same number of locations and document names
                List<string> strLocationValues = null;
                if (ValidateString(row.Directory)) strLocationValues = SplitString(row.Directory, ':');
                List<string> strNameValues = SplitString(row.File, ':');
                //see if we have a location match to every document name
                if ((strLocationValues != null) && (strNameValues.Count != strLocationValues.Count))
                {
                    strLocationValues = null; //cannot match locations to document so just supply document names
                }
                //create the Document References
                if (strNameValues.Count > 0)
                {
                    int i = 0;
                    foreach (var name in strNameValues)
                    {
                        var docref = Model.Instances.New<IfcDocumentReference>();
                        docref.Name=new IfcLabel(name); 
                        if (strLocationValues != null)
                            docref.Location = strLocationValues[i];
                        ifcDocumentInformation.DocumentReferences.Add(docref);
                        i++;
                    }
                  
                }

            }
        }

        /// <summary>
        /// Add the object the document relates too
        /// </summary>
        /// <param name="row">COBieDocumentRow holding row data</param>
        private IfcRoot GetObjectRelationship(COBieDocumentRow row)
        {
            IfcRoot ifcRoot = null;
                
            if ((ValidateString(row.SheetName)) &&  (ValidateString(row.RowName)))
            {
                string sheetName = row.SheetName.ToLower().Trim();
                sheetName = char.ToUpper(sheetName[0]) + sheetName.Substring(1);
            
                string rowName = row.RowName.ToLower().Trim();

                string extObject = string.Empty;
                if (ValidateString(row.ExtObject)) //if valid change to correct type
                    extObject = row.ExtObject.Trim().ToUpper();

                switch (sheetName)
                {
                    case Constants.WORKSHEET_TYPE:
                        //get all types, one time only
                        if (IfcTypeObjects == null)
                            IfcTypeObjects = Model.Instances.OfType<IfcTypeObject>();
                        ifcRoot = IfcTypeObjects.Where(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                        if (ifcRoot == null)
                        {
                            string typeName = string.Empty;
                            if (ValidateString(extObject))
                                typeName = extObject;
                            else
                                typeName = "IFCBUILDINGELEMENTPROXYTYPE";
                            ifcRoot = COBieXBimType.GetTypeInstance(typeName, Model); 
                            ifcRoot.Name = row.RowName;
                            ifcRoot.Description = "Created to maintain relationship with document object from COBie information";
                        }
                        break;
                    case Constants.WORKSHEET_COMPONENT:
                        //get all types, one time only
                        if (IfcElements == null)
                            IfcElements = Model.Instances.OfType<IfcElement>();
                        ifcRoot = IfcElements.Where(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                        string elementTypeName = "IFCVIRTUALELEMENT";
                        if (ifcRoot == null)
                        {
                            if (ValidateString(extObject)) //if valid change to correct type
                                elementTypeName = extObject;
                                
                            ifcRoot = COBieXBimComponent.GetElementInstance(elementTypeName, Model);
                            ifcRoot.Name = row.RowName;
                            ifcRoot.Description = "Created to maintain relationship with document object from COBie information";
                        }
                        else if ((ifcRoot.GetType().Name.ToUpper() == elementTypeName) &&//check type, if IFCVIRTUALELEMENT and
                                  (ValidateString(extObject)) &&
                                  (extObject != elementTypeName) && //not IFCVIRTUALELEMENT then delete virtual, and add correct type
                                  (ValidateString(ifcRoot.Description)) && //ensure we set to maintain relationship on another sheet
                                  (ifcRoot.Description.ToString().Contains("COBie information"))
                                )
                        {
                            try
                            {
                                Model.Delete(ifcRoot); //remove IFCVIRTUALELEMENT, probably added by system sheet
                                elementTypeName = extObject;
                                ifcRoot = COBieXBimComponent.GetElementInstance(elementTypeName, Model);
                                ifcRoot.Name = row.RowName;
                                ifcRoot.Description = "Created to maintain relationship with document object from COBie information";
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(string.Format("Failed to delete ifcRelDecomposes in AddObjectRelationship() - {0}", ex.Message));
                            }
                        }
                        break;
                    case Constants.WORKSHEET_JOB:
                        ifcRoot = Model.Instances.Where<IfcProcess>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                        break;
                    case Constants.WORKSHEET_ASSEMBLY:
                        ifcRoot = Model.Instances.Where<IfcRelDecomposes>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                        break;
                    case Constants.WORKSHEET_CONNECTION:
                        ifcRoot = Model.Instances.Where<IfcRelConnects>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                        break;
                    case Constants.WORKSHEET_FACILITY:
                        ifcRoot = Model.Instances.Where<IfcBuilding>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                        if (ifcRoot == null)
                            ifcRoot = Model.Instances.Where<IfcSite>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                        if (ifcRoot == null)
                            ifcRoot = Model.Instances.Where<IfcProject>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                        break;
                    case Constants.WORKSHEET_FLOOR:
                        ifcRoot = Model.Instances.Where<IfcBuildingStorey>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                        break;
                    case Constants.WORKSHEET_RESOURCE:
                        ifcRoot = Model.Instances.Where<IfcConstructionEquipmentResource>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                        break;
                    case Constants.WORKSHEET_SPACE:
                        ifcRoot = Model.Instances.Where<IfcSpace>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                        break;
                    case Constants.WORKSHEET_SPARE:
                        ifcRoot = Model.Instances.Where<IfcConstructionProductResource>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                        break;
                    case Constants.WORKSHEET_SYSTEM:
                        ifcRoot = Model.Instances.Where<IfcGroup>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                        break;
                    case Constants.WORKSHEET_ZONE:
                        ifcRoot = Model.Instances.Where<IfcZone>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                        break;
                    //case "document": //not derived from IfcRoot
                    //    ifcRoot = Model.Instances.Where<IfcDocumentInformation>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                    //    break;
                    //case "contact": //not derived from IfcRoot
                    //    ifcRoot = Model.Instances.Where<IfcPersonAndOrganization>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                    //    break;
                    //case "issue": //not derived from IfcRoot
                    //    ifcRoot = Model.Instances.Where<IfcApproval>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                    //    break;
                    default:
                        break;
                }
            }
            return ifcRoot;
        }
        
        #endregion
    }
}
