using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Xbim.Common.Federation;
using Xbim.Common.Step21;
using Xbim.FilterHelper;
using Xbim.Ifc;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.IO;
using Xbim.Ifc2x3.MeasureResource;

using Xbim.IO.Esent;

namespace Xbim.Client
{
    public class FederatedModel : IDisposable
    {
        #region Properties
        private XbimModel _fedModel = null;

        // <summary>
        /// Return the federated XBim Model
        /// </summary>
        public XbimModel Model
        { get { return _fedModel; } }

        /// <summary>
        /// XBimF File name
        /// </summary>
        public FileInfo FileNameXbimf
        { get; set; }
        /// <summary>
        /// Mapping of RoleFilter to ActorRole
        /// </summary>
        public Dictionary<RoleFilter, IfcActorRole> RoleMap
        { get; set; }


        /// <summary>
        /// Project Name
        /// </summary>
        public string ProjectName
        {
            get
            {
                return _fedModel != null ? _fedModel.IfcProject.Name.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Project author
        /// </summary>
        public string Author
        {
            get
            {
                return _fedModel != null ? _fedModel.IfcProject.OwnerHistory.OwningUser.ThePerson.FamilyName.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Project organisation
        /// </summary>
        public string Organisation
        {
            get
            {
                return _fedModel != null ? _fedModel.IfcProject.OwnerHistory.OwningUser.TheOrganization.Name.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Refrenced files and roles
        /// </summary>
        public Dictionary<IReferencedModel, RoleFilter> RefModelRoles
        {
            get
            {
                return _fedModel != null ? GetFileRoles() : new Dictionary<IReferencedModel, RoleFilter>();
            }
        } 
        #endregion

        /// <summary>
        /// Open file constructor
        /// </summary>
        /// <param name="file"></param>
        public FederatedModel(FileInfo file)
        {
            FileNameXbimf = file;
            Open(file);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="file">FileInfo for the xbimf file</param>
        /// <param name="author">Author name</param>
        /// <param name="organisation">Orgsnisation Name</param>
        /// <param name="prjName">Project Name</param>
        public FederatedModel(FileInfo file, string author, string organisation, string prjName = null)
        {
            FileNameXbimf = file;
            Create(file, author, organisation, prjName);            
        }

        /// <summary>
        /// Creat the federated file
        /// </summary>
        /// <param name="file">FileInfo for the xbimf file</param>
        /// <param name="author">Author name</param>
        /// <param name="organisation">Orgsnisation Name</param>
        /// <param name="prjName">Project Name</param>
        public void Create(FileInfo file, string author, string organisation, string prjName = null)
        {
            FileNameXbimf = file;
            //_fedModel = XbimModel.CreateTemporaryModel();
            _fedModel = XbimModel.CreateModel(file.FullName, XbimDBAccess.ReadWrite);

            _fedModel.Initialise(author, organisation, "xBIM", "xBIM Team", ""); //"" is version, but none to grab as yet

            using (var txn = _fedModel.BeginTransaction())
            {
                _fedModel.IfcProject.Name = (prjName != null) ? prjName : string.Empty;
                _fedModel.Header = new StepFileHeader(StepFileHeader.HeaderCreationMode.InitWithXbimDefaults);
                txn.Commit();
            }
        }

        /// <summary>
        /// Open federated file
        /// </summary>
        /// <param name="file">FileInfo</param>
        /// <returns>bool</returns>
        public bool Open(FileInfo file)
        {
            if (_fedModel != null)
            {
                _fedModel.Close();
            }

            if (file.Exists)
            {
                _fedModel = new XbimModel();
                return _fedModel.Open(file.FullName, XbimDBAccess.ReadWrite);
            }
            else
                return false;
        }

        /// <summary>
        /// Add the refrence models
        /// </summary>
        /// <param name="file">FileInfo for the xbimf file</param>
        /// <param name="organizationName">Organisation Name</param>
        /// <param name="role">RoleFilter</param>
        /// <param name="displayName"></param>
        public void AddRefModel(FileInfo file, string organizationName, RoleFilter roles = RoleFilter.Unknown, string displayName = null)
        {
            
            if (!file.Exists)
                throw new FileNotFoundException("Cannot find reference model file", file.FullName);

            //add model to list of referenced models
            IfcIdentifier docId = _fedModel.AddModelReference(file.FullName, organizationName, IfcRoleEnum.USERDEFINED).Identifier;
            //TODO resolve federated models
            //using (var txn = _fedModel.BeginTransaction())
            //{
            //    IfcDocumentInformation addDocId = _fedModel.ReferencedModels.Where(item => item.DocumentInformation.DocumentId == docId).Select(item => item.DocumentInformation).FirstOrDefault();
            //    IfcOrganization org = addDocId.DocumentOwner as IfcOrganization;
            //    if (org != null)
            //    {
            //        org.Roles.Clear();
            //        //Add Roles
            //        foreach (var item in GetActorRoles(roles))
            //        {
            //            org.AddRole(item);
            //        }
            //    }
            //    //add display name if required
            //    if (displayName != null)
            //    {
            //        addDocId.Description = displayName;
                   
            //    }
            //    txn.Commit();
            //}
        }

        /// <summary>
        /// Get the file to roles information
        /// </summary>
        /// <returns>Dictionary or FileInfo to RoleFilter</returns>
        private Dictionary<IReferencedModel, RoleFilter> GetFileRoles()
        {
            Dictionary<IReferencedModel, RoleFilter> modelRoles = new Dictionary<IReferencedModel, RoleFilter>();
            foreach (var refModel in _fedModel.ReferencedModels)
            {
                var docRole = MapActorRole(refModel.Role);
                try
                {
                    FileInfo file = new FileInfo(refModel.Name);
                    if (file.Exists)
                    {
                        modelRoles[refModel] = docRole;
                    }
                    else
                    {
                        MessageBox.Show("File path does not exist: {0}", refModel.Name);
                    }
                }
                catch (System.ArgumentException)
                {
                    MessageBox.Show("File path is incorrect: {0}", refModel.Name);
                }
            }
            return modelRoles;
        }

        /// <summary>
        /// Dispose method, close host model, and run through referencedModel list in the XBimModel itself and close all referenced models
        /// </summary>
        public void Dispose()
        {
            if (_fedModel != null)
            {
                //TODO:  hate this work around to avoid header Object set to null in XBimModel Close method, but small file so time hit limited
                //Check with steve as to why header set to null 

                //save the xbim model back to a temp ifc
                //string tempFile = Path.GetTempFileName();
                //tempFile = Path.ChangeExtension(tempFile, ".ifc");
                //_fedModel.SaveAs(tempFile, XbimExtensions.Interfaces.XbimStorageType.IFC);

                //dispose of fed xbim databse
                _fedModel.Close(); //will close referenced models as well
                _fedModel.Dispose(); //might not need this, as close method can call dispose 
                _fedModel = null;

                //create new xbimf database from temp file ifc
                //using (var model = new XbimModel())
                //{
                //    model.CreateFrom(tempFile, FileNameXbimf.FullName, null, true, true);
                //}
                //if (File.Exists(tempFile))
                //{
                //    File.Delete(tempFile);
                //}

            }
        }

        /// <summary>
        /// Close Host and Referenced models
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }


        #region roles

        /// <summary>
        /// Get ActorRoles mapped to roles held in RoleFilter Flags
        /// </summary>
        /// <param name="role">RoleFilter</param>
        /// <returns>List of IfcActorRole</returns>
        private List<IfcActorRole> GetActorRoles(RoleFilter role)
        {
            if (RoleMap == null) RoleMap = new Dictionary<RoleFilter, IfcActorRole>();
            
            List<IfcActorRole> actorRoles = new List<IfcActorRole>();
            if (role.HasFlag(RoleFilter.Architectural))
            {
                actorRoles.Add(RoleMap.ContainsKey(RoleFilter.Architectural) ? RoleMap[RoleFilter.Architectural] : MapRole(RoleFilter.Architectural, IfcRoleEnum.ARCHITECT));
            }
            if (role.HasFlag(RoleFilter.Mechanical))
            {
                actorRoles.Add(RoleMap.ContainsKey(RoleFilter.Mechanical) ? RoleMap[RoleFilter.Mechanical] : MapRole(RoleFilter.Mechanical, IfcRoleEnum.MECHANICALENGINEER));
            }
            if (role.HasFlag(RoleFilter.Electrical))
            {
                actorRoles.Add(RoleMap.ContainsKey(RoleFilter.Electrical) ? RoleMap[RoleFilter.Electrical] : MapRole(RoleFilter.Electrical, IfcRoleEnum.ELECTRICALENGINEER));
            }
            if (role.HasFlag(RoleFilter.Plumbing))
            {
                actorRoles.Add(RoleMap.ContainsKey(RoleFilter.Plumbing) ? RoleMap[RoleFilter.Plumbing] : MapRole(RoleFilter.Plumbing, IfcRoleEnum.USERDEFINED));
            }
            if (role.HasFlag(RoleFilter.FireProtection))
            {
                actorRoles.Add(RoleMap.ContainsKey(RoleFilter.FireProtection) ? RoleMap[RoleFilter.FireProtection] : MapRole(RoleFilter.FireProtection, IfcRoleEnum.USERDEFINED));
            }
            if (role.HasFlag(RoleFilter.Unknown))
            {
                actorRoles.Add(RoleMap.ContainsKey(RoleFilter.Unknown) ? RoleMap[RoleFilter.Unknown] : MapRole(RoleFilter.Unknown, IfcRoleEnum.USERDEFINED));
            }
            return actorRoles;
        }

        /// <summary>
        /// Convert RoleFilter to IfcActorRole, need to be within a transaction
        /// </summary>
        /// <param name="role">RoleFilter</param>
        /// <param name="ifcRole">IfcRole</param>
        /// <returns></returns>
        private IfcActorRole MapRole(RoleFilter role, IfcRoleEnum ifcRole)
        {
            if (_fedModel.IsTransacting)
            {
                IfcActorRole actorRole = _fedModel.Instances.New<IfcActorRole>();
                if (ifcRole == IfcRoleEnum.USERDEFINED)
                {
                    actorRole.Role = IfcRoleEnum.USERDEFINED;
                    actorRole.UserDefinedRole = role.ToString();
                }
                else
                {
                    actorRole.Role = ifcRole;
                }
                actorRole.Description = role.ToString();
                RoleMap.Add(role, actorRole);
                return actorRole;
            }
            else
            {
                throw new InvalidOperationException("MapRole: No transaction currently in model");
            }
        }

       


        private static RoleFilter MapActorRole(string roleName)
        {
            IfcRoleEnum role;
            if (Enum.TryParse(roleName, true, out role))
            {
                if (role == IfcRoleEnum.ARCHITECT)
                    return RoleFilter.Architectural;
                if (role == IfcRoleEnum.MECHANICALENGINEER)
                    return RoleFilter.Mechanical;
                if (role == IfcRoleEnum.ELECTRICALENGINEER)
                    return RoleFilter.Electrical;
            }
            return RoleFilter.Unknown;

        }

       
        #endregion
    }
}
