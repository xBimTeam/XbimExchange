using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xbim.Common;
using Xbim.Common.Step21;
using Xbim.COBie.Contracts;
using Xbim.COBie.Rows;
using Xbim.COBie.Serialisers.XbimSerialiser;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.GeometryResource;
using Xbim.IO;
using Xbim.IO.Esent;
using Xbim.Ifc;

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
        public IfcStore Model
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
            var fileNameDB = Path.ChangeExtension(fileName, ".xBIM");
            XBimContext = new COBieXBimContext(IfcStore.Create(fileNameDB, null, XbimSchemaVersion.Ifc2X3)) {IsMerge = false};
            FileName = fileName;
            MergeGeometryOnly = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public COBieXBimSerialiser(string fileName, ReportProgressDelegate progressHandler) 
        {
            var fileNameDB = Path.ChangeExtension(fileName, ".xBIM");
            XBimContext = new COBieXBimContext(IfcStore.Create(fileNameDB, null, XbimSchemaVersion.Ifc2X3), progressHandler) {IsMerge = false};
            FileName = fileName;
            MergeGeometryOnly = true;
        }


        #region Methods

        /// <summary>
        /// XBim Serialise
        /// </summary>
        /// <param name="workbook">COBieWorkbook to Serialise</param>
        /// <param name="validationTemplate"></param>
        public void Serialise(COBieWorkbook workbook, ICOBieValidationTemplate validationTemplate = null)
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

            var xBimContact = new COBieXBimContact(XBimContext);
            xBimContact.SerialiseContacts((COBieSheet<COBieContactRow>)WorkBook[Constants.WORKSHEET_CONTACT]);

            var xBimFacility = new COBieXBimFacility(XBimContext);
            xBimFacility.SerialiseFacility((COBieSheet<COBieFacilityRow>)WorkBook[Constants.WORKSHEET_FACILITY]);

            var xBimFloor = new COBieXBimFloor(XBimContext);
            xBimFloor.SerialiseFloor((COBieSheet<COBieFloorRow>)WorkBook[Constants.WORKSHEET_FLOOR]);
            
            var xBimSpace = new COBieXBimSpace(XBimContext);
            xBimSpace.SerialiseSpace((COBieSheet<COBieSpaceRow>)WorkBook[Constants.WORKSHEET_SPACE]);

            var xBimZone = new COBieXBimZone(XBimContext);
            xBimZone.SerialiseZone((COBieSheet<COBieZoneRow>)WorkBook[Constants.WORKSHEET_ZONE]);

            var xBimType = new COBieXBimType(XBimContext);
            xBimType.SerialiseType((COBieSheet<COBieTypeRow>)WorkBook[Constants.WORKSHEET_TYPE]);

            var xBimComponent = new COBieXBimComponent(XBimContext);
            xBimComponent.SerialiseComponent((COBieSheet<COBieComponentRow>)WorkBook[Constants.WORKSHEET_COMPONENT]);

            var xBimSystem = new COBieXBimSystem(XBimContext);
            xBimSystem.SerialiseSystem((COBieSheet<COBieSystemRow>)WorkBook[Constants.WORKSHEET_SYSTEM]);

            var xBimAssembly = new COBieXBimAssembly(XBimContext);
            xBimAssembly.SerialiseAssembly((COBieSheet<COBieAssemblyRow>)WorkBook[Constants.WORKSHEET_ASSEMBLY]);

            var xBimConnection = new COBieXBimConnection(XBimContext);
            xBimConnection.SerialiseConnection((COBieSheet<COBieConnectionRow>)WorkBook[Constants.WORKSHEET_CONNECTION]);
            
            var xBimSpare = new COBieXBimSpare(XBimContext);
            xBimSpare.SerialiseSpare((COBieSheet<COBieSpareRow>)WorkBook[Constants.WORKSHEET_SPARE]);

            var xBimResource = new COBieXBimResource(XBimContext);
            xBimResource.SerialiseResource((COBieSheet<COBieResourceRow>)WorkBook[Constants.WORKSHEET_RESOURCE]);

            var xBimJob = new COBieXBimJob(XBimContext);
            xBimJob.SerialiseJob((COBieSheet<COBieJobRow>)WorkBook[Constants.WORKSHEET_JOB]);

            var xBimImpact = new COBieXBimImpact(XBimContext);
            xBimImpact.SerialiseImpact((COBieSheet<COBieImpactRow>)WorkBook[Constants.WORKSHEET_IMPACT]);

            var xBimDocument = new COBieXBimDocument(XBimContext);
            xBimDocument.SerialiseDocument((COBieSheet<COBieDocumentRow>)WorkBook[Constants.WORKSHEET_DOCUMENT]);

            var xBimAttribute = new COBieXBimAttribute(XBimContext);
            xBimAttribute.SerialiseAttribute((COBieSheet<COBieAttributeRow>)WorkBook[Constants.WORKSHEET_ATTRIBUTE]);
            
            var xBimCoordinate = new COBieXBimCoordinate(XBimContext);
            xBimCoordinate.SerialiseCoordinate((COBieSheet<COBieCoordinateRow>)WorkBook[Constants.WORKSHEET_COORDINATE]);
            
            var xBimIssue = new COBieXBimIssue(XBimContext);
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
                var xBimContact = new COBieXBimContact(XBimContext);
                xBimContact.SerialiseContacts((COBieSheet<COBieContactRow>)WorkBook[Constants.WORKSHEET_CONTACT]);
            }
            
            //Make the assumption we are merging on the same building
            //COBieXBimFacility xBimFacility = new COBieXBimFacility(XBimContext);
            //xBimFacility.SerialiseFacility((COBieSheet<COBieFacilityRow>)WorkBook[Constants.WORKSHEET_FACILITY]);

            var xBimFloor = new COBieXBimFloor(XBimContext);
            xBimFloor.SerialiseFloor((COBieSheet<COBieFloorRow>)WorkBook[Constants.WORKSHEET_FLOOR]);

            var xBimSpace = new COBieXBimSpace(XBimContext);
            xBimSpace.SerialiseSpace((COBieSheet<COBieSpaceRow>)WorkBook[Constants.WORKSHEET_SPACE]);

            if (!MergeGeometryOnly)
            {
                var xBimZone = new COBieXBimZone(XBimContext);
                xBimZone.SerialiseZone((COBieSheet<COBieZoneRow>)WorkBook[Constants.WORKSHEET_ZONE]);
            } 
            var xBimType = new COBieXBimType(XBimContext);
            xBimType.SerialiseType((COBieSheet<COBieTypeRow>)WorkBook[Constants.WORKSHEET_TYPE]);

            var xBimComponent = new COBieXBimComponent(XBimContext);
            xBimComponent.SerialiseComponent((COBieSheet<COBieComponentRow>)WorkBook[Constants.WORKSHEET_COMPONENT]);

            if (!MergeGeometryOnly)
            {
                var xBimSystem = new COBieXBimSystem(XBimContext);
                xBimSystem.SerialiseSystem((COBieSheet<COBieSystemRow>)WorkBook[Constants.WORKSHEET_SYSTEM]);

                var xBimAssembly = new COBieXBimAssembly(XBimContext);
                xBimAssembly.SerialiseAssembly((COBieSheet<COBieAssemblyRow>)WorkBook[Constants.WORKSHEET_ASSEMBLY]);

                var xBimConnection = new COBieXBimConnection(XBimContext);
                xBimConnection.SerialiseConnection((COBieSheet<COBieConnectionRow>)WorkBook[Constants.WORKSHEET_CONNECTION]);

                var xBimSpare = new COBieXBimSpare(XBimContext);
                xBimSpare.SerialiseSpare((COBieSheet<COBieSpareRow>)WorkBook[Constants.WORKSHEET_SPARE]);

                var xBimResource = new COBieXBimResource(XBimContext);
                xBimResource.SerialiseResource((COBieSheet<COBieResourceRow>)WorkBook[Constants.WORKSHEET_RESOURCE]);

                var xBimJob = new COBieXBimJob(XBimContext);
                xBimJob.SerialiseJob((COBieSheet<COBieJobRow>)WorkBook[Constants.WORKSHEET_JOB]);

                var xBimImpact = new COBieXBimImpact(XBimContext);
                xBimImpact.SerialiseImpact((COBieSheet<COBieImpactRow>)WorkBook[Constants.WORKSHEET_IMPACT]);

                var xBimDocument = new COBieXBimDocument(XBimContext);
                xBimDocument.SerialiseDocument((COBieSheet<COBieDocumentRow>)WorkBook[Constants.WORKSHEET_DOCUMENT]);

                var xBimAttribute = new COBieXBimAttribute(XBimContext);
                xBimAttribute.SerialiseAttribute((COBieSheet<COBieAttributeRow>)WorkBook[Constants.WORKSHEET_ATTRIBUTE]);
            }

            var xBimCoordinate = new COBieXBimCoordinate(XBimContext);
            xBimCoordinate.SerialiseCoordinate((COBieSheet<COBieCoordinateRow>)WorkBook[Constants.WORKSHEET_COORDINATE]);
            if (!MergeGeometryOnly)
            {
                var xBimIssue = new COBieXBimIssue(XBimContext);
                xBimIssue.SerialiseIssue((COBieSheet<COBieIssueRow>)WorkBook[Constants.WORKSHEET_ISSUE]);
            }
            
        }
        /// <summary>
        /// Set up the Model Object
        /// </summary>
        private void ModelSetUp()
        {
            using (var trans = Model.BeginTransaction("Model initialization"))
            {
                var assembly = Assembly.GetExecutingAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                var version = fvi.ProductVersion;

                Model.DefaultOwningApplication.ApplicationIdentifier = fvi.Comments;
                Model.DefaultOwningApplication.ApplicationDeveloper.Name = fvi.CompanyName;
                Model.DefaultOwningApplication.ApplicationFullName = fvi.ProductName;
                Model.DefaultOwningApplication.Version = fvi.ProductVersion;
                //TODO add correct IfcPersonAndOrganization to DefaultOwningUser
                Model.DefaultOwningUser.ThePerson.FamilyName = "Unknown";
                Model.DefaultOwningUser.TheOrganization.Name = "Unknown";
                
                Model.Header.FileDescription.Description.Clear();
                Model.Header.FileDescription.Description.Add("ViewDefinition[CoordinationView]");
                Model.Header.FileName.Name = Path.GetFileName(FileName);
                Model.Header.FileName.AuthorName.Add("4Projects");
                Model.Header.FileName.AuthorizationName = "4Projects";
                var project = Model.Instances.New<IfcProject>();
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

                Model.Dispose();
                XBimContext.Model = null;
            }
        }

        /// <summary>
        /// Validate Model Object Foe Errors
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public String Validate(ITransaction trans)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save Model Object To A File
        /// </summary>
        public void Save()
        {
            try
            {
                //write the Ifc File
                Model.SaveAs(FileName, StorageType.Ifc);
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
