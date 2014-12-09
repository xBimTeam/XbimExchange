using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xbim.COBie.Contracts;
using Xbim.COBie.Rows;
using Xbim.COBie.Serialisers.XbimSerialiser;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.Kernel;
using Xbim.IO;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Transactions;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.UtilityResource;
using Xbim.XbimExtensions.Interfaces;
using Xbim.Ifc2x3.GeometryResource;
//using Xbim.XbimExtensions.Parser;

namespace Xbim.COBie.Serialisers
{
    public class COBieXBimSerialiser : ICOBieSerialiser , IDisposable
    {
        #region Fileds
        #endregion

        #region Properties
        /// <summary>
        /// Context holder
        /// </summary>
        public COBieXBimContext XBimContext { get; private set; }
        /// <summary>
        /// If set to true will only merge the sheets required for Geometry
        /// </summary>
        public bool MergeGeometryOnly { get; set; }
        /// <summary>
        /// COBieWorkbook to convert to XBim Model Object
        /// </summary>
        public COBieWorkbook WorkBook 
        {
            get { return XBimContext.WorkBook; }
        }
        
        /// <summary>
        /// XBim Model Object
        /// </summary>
        public XbimModel Model
        {
            get { return XBimContext.Model; }
        }
        /// <summary>
        /// File to write too
        /// </summary>
        public string FileName { get; private set; }

        #endregion
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName">.xBIM file name and path</param>
        public COBieXBimSerialiser(string fileName)
        {
            string fileNameDB = Path.ChangeExtension(fileName, ".xBIM");
            XBimContext = new COBieXBimContext(XbimModel.CreateModel(fileNameDB));
            XBimContext.IsMerge = false;
            FileName = fileName;
            MergeGeometryOnly = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public COBieXBimSerialiser(string fileName, ReportProgressDelegate progressHandler) 
        {
            string fileNameDB = Path.ChangeExtension(fileName, ".xBIM");
            XBimContext = new COBieXBimContext(XbimModel.CreateModel(fileNameDB), progressHandler);
            XBimContext.IsMerge = false;
            FileName = fileName;
            MergeGeometryOnly = true;
        }


        #region Methods

        /// <summary>
        /// XBim Serialise
        /// </summary>
        /// <param name="workbook">COBieWorkbook to Serialise</param>
        public void Serialise(COBieWorkbook workbook, ICOBieValidationTemplate ValidationTemplate = null)
        {
            Create(workbook);
            if (!string.IsNullOrEmpty(FileName))
            {
                Save();
            }
            
        }
        /// <summary>
        /// XBim Serialise
        /// </summary>
        /// <param name="workbook">COBieWorkbook to Serialise</param>
        public void Create(COBieWorkbook workbook)
        {
            
            XBimContext.Reset(); //clear out the dictionaries
            XBimContext.WorkBook = workbook;
            ModelSetUp();

            COBieXBimContact xBimContact = new COBieXBimContact(XBimContext);
            xBimContact.SerialiseContacts((COBieSheet<COBieContactRow>)WorkBook[Constants.WORKSHEET_CONTACT]);

            COBieXBimFacility xBimFacility = new COBieXBimFacility(XBimContext);
            xBimFacility.SerialiseFacility((COBieSheet<COBieFacilityRow>)WorkBook[Constants.WORKSHEET_FACILITY]);

            COBieXBimFloor xBimFloor = new COBieXBimFloor(XBimContext);
            xBimFloor.SerialiseFloor((COBieSheet<COBieFloorRow>)WorkBook[Constants.WORKSHEET_FLOOR]);
            
            COBieXBimSpace xBimSpace = new COBieXBimSpace(XBimContext);
            xBimSpace.SerialiseSpace((COBieSheet<COBieSpaceRow>)WorkBook[Constants.WORKSHEET_SPACE]);

            COBieXBimZone xBimZone = new COBieXBimZone(XBimContext);
            xBimZone.SerialiseZone((COBieSheet<COBieZoneRow>)WorkBook[Constants.WORKSHEET_ZONE]);

            COBieXBimType xBimType = new COBieXBimType(XBimContext);
            xBimType.SerialiseType((COBieSheet<COBieTypeRow>)WorkBook[Constants.WORKSHEET_TYPE]);

            COBieXBimComponent xBimComponent = new COBieXBimComponent(XBimContext);
            xBimComponent.SerialiseComponent((COBieSheet<COBieComponentRow>)WorkBook[Constants.WORKSHEET_COMPONENT]);

            COBieXBimSystem xBimSystem = new COBieXBimSystem(XBimContext);
            xBimSystem.SerialiseSystem((COBieSheet<COBieSystemRow>)WorkBook[Constants.WORKSHEET_SYSTEM]);

            COBieXBimAssembly xBimAssembly = new COBieXBimAssembly(XBimContext);
            xBimAssembly.SerialiseAssembly((COBieSheet<COBieAssemblyRow>)WorkBook[Constants.WORKSHEET_ASSEMBLY]);

            COBieXBimConnection xBimConnection = new COBieXBimConnection(XBimContext);
            xBimConnection.SerialiseConnection((COBieSheet<COBieConnectionRow>)WorkBook[Constants.WORKSHEET_CONNECTION]);
            
            COBieXBimSpare xBimSpare = new COBieXBimSpare(XBimContext);
            xBimSpare.SerialiseSpare((COBieSheet<COBieSpareRow>)WorkBook[Constants.WORKSHEET_SPARE]);

            COBieXBimResource xBimResource = new COBieXBimResource(XBimContext);
            xBimResource.SerialiseResource((COBieSheet<COBieResourceRow>)WorkBook[Constants.WORKSHEET_RESOURCE]);

            COBieXBimJob xBimJob = new COBieXBimJob(XBimContext);
            xBimJob.SerialiseJob((COBieSheet<COBieJobRow>)WorkBook[Constants.WORKSHEET_JOB]);

            COBieXBimImpact xBimImpact = new COBieXBimImpact(XBimContext);
            xBimImpact.SerialiseImpact((COBieSheet<COBieImpactRow>)WorkBook[Constants.WORKSHEET_IMPACT]);

            COBieXBimDocument xBimDocument = new COBieXBimDocument(XBimContext);
            xBimDocument.SerialiseDocument((COBieSheet<COBieDocumentRow>)WorkBook[Constants.WORKSHEET_DOCUMENT]);

            COBieXBimAttribute xBimAttribute = new COBieXBimAttribute(XBimContext);
            xBimAttribute.SerialiseAttribute((COBieSheet<COBieAttributeRow>)WorkBook[Constants.WORKSHEET_ATTRIBUTE]);
            
            COBieXBimCoordinate xBimCoordinate = new COBieXBimCoordinate(XBimContext);
            xBimCoordinate.SerialiseCoordinate((COBieSheet<COBieCoordinateRow>)WorkBook[Constants.WORKSHEET_COORDINATE]);
            
            COBieXBimIssue xBimIssue = new COBieXBimIssue(XBimContext);
            xBimIssue.SerialiseIssue((COBieSheet<COBieIssueRow>)WorkBook[Constants.WORKSHEET_ISSUE]);
            
            
        }

        /// <summary>
        /// XBim Merge
        /// </summary>
        /// <param name="workbook">COBieWorkbook to Serialise</param>
        public void Merge(COBieWorkbook workbook)
        {
            XBimContext.IsMerge = true; //flag as a merge
            XBimContext.WorkBook = workbook;

            if (!MergeGeometryOnly)
            {
                COBieXBimContact xBimContact = new COBieXBimContact(XBimContext);
                xBimContact.SerialiseContacts((COBieSheet<COBieContactRow>)WorkBook[Constants.WORKSHEET_CONTACT]);
            }
            
            //Make the assumption we are merging on the same building
            //COBieXBimFacility xBimFacility = new COBieXBimFacility(XBimContext);
            //xBimFacility.SerialiseFacility((COBieSheet<COBieFacilityRow>)WorkBook[Constants.WORKSHEET_FACILITY]);

            COBieXBimFloor xBimFloor = new COBieXBimFloor(XBimContext);
            xBimFloor.SerialiseFloor((COBieSheet<COBieFloorRow>)WorkBook[Constants.WORKSHEET_FLOOR]);

            COBieXBimSpace xBimSpace = new COBieXBimSpace(XBimContext);
            xBimSpace.SerialiseSpace((COBieSheet<COBieSpaceRow>)WorkBook[Constants.WORKSHEET_SPACE]);

            if (!MergeGeometryOnly)
            {
                COBieXBimZone xBimZone = new COBieXBimZone(XBimContext);
                xBimZone.SerialiseZone((COBieSheet<COBieZoneRow>)WorkBook[Constants.WORKSHEET_ZONE]);
            } 
            COBieXBimType xBimType = new COBieXBimType(XBimContext);
            xBimType.SerialiseType((COBieSheet<COBieTypeRow>)WorkBook[Constants.WORKSHEET_TYPE]);

            COBieXBimComponent xBimComponent = new COBieXBimComponent(XBimContext);
            xBimComponent.SerialiseComponent((COBieSheet<COBieComponentRow>)WorkBook[Constants.WORKSHEET_COMPONENT]);

            if (!MergeGeometryOnly)
            {
                COBieXBimSystem xBimSystem = new COBieXBimSystem(XBimContext);
                xBimSystem.SerialiseSystem((COBieSheet<COBieSystemRow>)WorkBook[Constants.WORKSHEET_SYSTEM]);

                COBieXBimAssembly xBimAssembly = new COBieXBimAssembly(XBimContext);
                xBimAssembly.SerialiseAssembly((COBieSheet<COBieAssemblyRow>)WorkBook[Constants.WORKSHEET_ASSEMBLY]);

                COBieXBimConnection xBimConnection = new COBieXBimConnection(XBimContext);
                xBimConnection.SerialiseConnection((COBieSheet<COBieConnectionRow>)WorkBook[Constants.WORKSHEET_CONNECTION]);

                COBieXBimSpare xBimSpare = new COBieXBimSpare(XBimContext);
                xBimSpare.SerialiseSpare((COBieSheet<COBieSpareRow>)WorkBook[Constants.WORKSHEET_SPARE]);

                COBieXBimResource xBimResource = new COBieXBimResource(XBimContext);
                xBimResource.SerialiseResource((COBieSheet<COBieResourceRow>)WorkBook[Constants.WORKSHEET_RESOURCE]);

                COBieXBimJob xBimJob = new COBieXBimJob(XBimContext);
                xBimJob.SerialiseJob((COBieSheet<COBieJobRow>)WorkBook[Constants.WORKSHEET_JOB]);

                COBieXBimImpact xBimImpact = new COBieXBimImpact(XBimContext);
                xBimImpact.SerialiseImpact((COBieSheet<COBieImpactRow>)WorkBook[Constants.WORKSHEET_IMPACT]);

                COBieXBimDocument xBimDocument = new COBieXBimDocument(XBimContext);
                xBimDocument.SerialiseDocument((COBieSheet<COBieDocumentRow>)WorkBook[Constants.WORKSHEET_DOCUMENT]);

                COBieXBimAttribute xBimAttribute = new COBieXBimAttribute(XBimContext);
                xBimAttribute.SerialiseAttribute((COBieSheet<COBieAttributeRow>)WorkBook[Constants.WORKSHEET_ATTRIBUTE]);
            }

            COBieXBimCoordinate xBimCoordinate = new COBieXBimCoordinate(XBimContext);
            xBimCoordinate.SerialiseCoordinate((COBieSheet<COBieCoordinateRow>)WorkBook[Constants.WORKSHEET_COORDINATE]);
            if (!MergeGeometryOnly)
            {
                COBieXBimIssue xBimIssue = new COBieXBimIssue(XBimContext);
                xBimIssue.SerialiseIssue((COBieSheet<COBieIssueRow>)WorkBook[Constants.WORKSHEET_ISSUE]);
            }
            
        }
        /// <summary>
        /// Set up the Model Object
        /// </summary>
        private void ModelSetUp()
        {
            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Model initialization"))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.ProductVersion;

                Model.DefaultOwningApplication.ApplicationIdentifier = fvi.Comments;
                Model.DefaultOwningApplication.ApplicationDeveloper.Name = fvi.CompanyName;
                Model.DefaultOwningApplication.ApplicationFullName = fvi.ProductName;
                Model.DefaultOwningApplication.Version = fvi.ProductVersion;
                //TODO add correct IfcPersonAndOrganization to DefaultOwningUser
                Model.DefaultOwningUser.ThePerson.FamilyName = "Unknown";
                Model.DefaultOwningUser.TheOrganization.Name = "Unknown";
                if (Model.Header == null)
                {
                    Model.Header = new IfcFileHeader(IfcFileHeader.HeaderCreationMode.InitWithXbimDefaults);
                }
                Model.Header.FileDescription.Description.Clear();
                Model.Header.FileDescription.Description.Add("ViewDefinition[CoordinationView]");
                Model.Header.FileName.Name = Path.GetFileName(FileName);
                Model.Header.FileName.AuthorName.Add("4Projects");
                Model.Header.FileName.AuthorizationName = "4Projects";
                IfcProject project = Model.Instances.New<IfcProject>();
                //set world coordinate system
                XBimContext.WCS = Model.Instances.New<IfcAxis2Placement3D>();
                XBimContext.WCS.SetNewDirectionOf_XZ(1, 0, 0, 0, 0, 1);
                XBimContext.WCS.SetNewLocation(0, 0, 0);
                trans.Commit();
            }
        }

        /// <summary>
        /// Dispose of the Model Object and close transaction
        /// </summary>
        void IDisposable.Dispose()
        {
            if (Model != null)
            {
                //close model server if it is the case
                if (Model is XbimModel)
                {
                    (Model as XbimModel).Dispose();
                    XBimContext.Model = null;
                }

            }
        }

        /// <summary>
        /// Validate Model Object Foe Errors
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public String Validate(XbimReadWriteTransaction trans)
        {
            StringWriter sw = new StringWriter();
            int errors = Model.Validate(trans.Modified(), sw);
            if (errors > 0)
                return sw.ToString();
            else
                return null;
        }

        /// <summary>
        /// Save Model Object To A File
        /// </summary>
        /// <param name="fileName"></param>
        public void Save()
        {
            try
            {
                //write the Ifc File
                Model.SaveAs(FileName, XbimStorageType.IFC);
                //Model.Close(); //let dispose close use COBieXBimSerialiser in a using statement
#if DEBUG
                Console.WriteLine(FileName + " has been successfully written");
#endif
            }
            catch (Exception)
            {
                throw;
            }
            
            
        }
        #endregion
        
    }

    
}
