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
using Xbim.Ifc4.Interfaces;
using IfcRoleEnum = Xbim.Ifc2x3.ActorResource.IfcRoleEnum;

namespace Xbim.COBieLiteUK.Client
{
    public class FederatedModel : IDisposable
    {
        #region Properties
        private IfcStore _fedModel;

        // <summary>
        /// Return the federated XBim Model
        /// 
        public IfcStore Model
        { get { return _fedModel; } }

        
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
                var project = Model.ReferencingModel.Instances.OfType<IIfcProject>().FirstOrDefault();
                return project!=null?project.Name.ToString():string.Empty;
            }
        }

        /// <summary>
        /// Project author
        /// </summary>
        public string Author
        {
            get
            {
                var project = Model.ReferencingModel.Instances.OfType<IIfcProject>().FirstOrDefault();
                return project != null ? project.OwnerHistory.OwningUser.ThePerson.FamilyName.ToString() : string.Empty;        
            }
        }

        /// <summary>
        /// Project organisation
        /// </summary>
        public string Organisation
        {
            get
            {
                var project = Model.ReferencingModel.Instances.OfType<IIfcProject>().FirstOrDefault();
                return project != null ? project.OwnerHistory.OwningUser.TheOrganization.Name.ToString() : string.Empty;                
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
            Open(file);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="author">Author name</param>
        /// <param name="organisation">Orgsnisation Name</param>
        /// <param name="prjName">Project Name</param>
        public FederatedModel(string author, string organisation, string prjName = null)
        {
           
            Create(author, organisation, prjName);            
        }

        /// <summary>
        /// Creat the federated file
        /// </summary>
        /// <param name="author">Author name</param>
        /// <param name="organisation">Orgsnisation Name</param>
        /// <param name="prjName">Project Name</param>
        /// <param name="ifcVersion">Ifc schema version</param>
        /// <param name="storage">Type of xbim file store</param>
        public void Create(string author, string organisation, string prjName = null, IfcSchemaVersion ifcVersion=IfcSchemaVersion.Ifc2X3,XbimStoreType storage=XbimStoreType.InMemoryModel )
        {         
            var creds = new XbimEditorCredentials
            {
                ApplicationIdentifier = "xBIM",
                ApplicationDevelopersName = "xBIM Team",
                EditorsFamilyName = author,
                EditorsOrganisationName = organisation,
                ApplicationVersion = "1.0"
            };
            _fedModel = IfcStore.Create(creds, ifcVersion, storage); //create in memory
            using (var txn = _fedModel.BeginTransaction())
            {
                var project = _fedModel.Instances.New<Ifc2x3.Kernel.IfcProject>();
                project.Name = prjName ?? "Undefined";
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
                var creds = new XbimEditorCredentials
                {
                    ApplicationIdentifier = "xBIM",
                    ApplicationDevelopersName = "xBIM Team",
                    EditorsFamilyName = "undefined",
                    EditorsOrganisationName = "undefined",
                    ApplicationVersion = "1.0"
                };
                try
                {
                    _fedModel = IfcStore.Open(file.FullName, creds, 0);
                    return true;
                }
                catch (Exception)
                {
                    //just return false;
                }                
            }
            return false;
        }

        /// <summary>
        /// Add the refrence models
        /// </summary>
        /// <param name="file">FileInfo for the xbimf file</param>
        /// <param name="organizationName">Organisation Name</param>
        /// <param name="roles"></param>
        /// <param name="displayName"></param>
        public void AddRefModel(FileInfo file, string organizationName, RoleFilter roles = RoleFilter.Unknown, string displayName = null)
        {
            
            if (!file.Exists)
                throw new FileNotFoundException("Cannot find reference model file", file.FullName);
            var actorRoles = GetActorRoles(roles);
            var roleString = string.Join(",", actorRoles.Select(r => r.RoleString));
            //add model to list of referenced models
            _fedModel.AddModelReference(file.FullName, organizationName, roleString);
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
                catch (ArgumentException)
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
                //dispose of fed xbim databse
                _fedModel.Close(); //will close referenced models as well
                _fedModel = null;

            }
        }

        /// <summary>
        /// Close Host and Referenced models
        /// </summary>
        public void Close()
        {
            Dispose();
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
            if (_fedModel.CurrentTransaction!=null)
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
