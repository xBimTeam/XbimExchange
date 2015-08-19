using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xbim.FilterHelper;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.IO;
using Xbim.XbimExtensions;

namespace Xbim.Client
{
    public class FederatedModel : IDisposable
    {
        #region Properties
        private XbimModel _fedModel = null;

        // <summary>
        /// Return the federated XBim Model
        /// </summary>
        public XbimModel FedModel
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
        public Dictionary<FileInfo, RoleFilter> FileRoles
        {
            get
            {
                return _fedModel != null ? GetFileRoles() : new Dictionary<FileInfo, RoleFilter>();
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
        /// <param name="file">FileInfo for the xbimf file</param>
        /// <param name="author">Author name</param>
        /// <param name="organisation">Orgsnisation Name</param>
        /// <param name="prjName">Project Name</param>
        public FederatedModel(FileInfo file, string author, string organisation, string prjName = null)
        {
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
            _fedModel = XbimModel.CreateModel(file.FullName, XbimDBAccess.ReadWrite);

            _fedModel.Initialise(author, organisation, "xBIM", "xBIM Team", ""); //"" is version, but none to grab as yet

            if (prjName != null)
            {
                using (var txn = _fedModel.BeginTransaction())
                {
                    _fedModel.IfcProject.Name = prjName;
                    txn.Commit();
                }
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
            IfcIdentifier docId = _fedModel.AddModelReference(file.FullName, organizationName, IfcRole.UserDefined).Identifier;
            
            using (var txn = _fedModel.BeginTransaction())
            {
                IfcDocumentInformation addDocId = _fedModel.ReferencedModels.Where(item => item.DocumentInformation.DocumentId == docId).Select(item => item.DocumentInformation).FirstOrDefault();
                IfcOrganization org = addDocId.DocumentOwner as IfcOrganization;
                if (org != null)
                {
                    org.Roles.Clear();
                    //Add Roles
                    foreach (var item in GetActorRoles(roles))
                    {
                        org.AddRole(item);
                    }
                }
                //add display name if required
                if (displayName != null)
                {
                    addDocId.Description = displayName;
                   
                }
                txn.Commit();
            }
        }

        /// <summary>
        /// Get the file to roles information
        /// </summary>
        /// <returns>Dictionary or FileInfo to RoleFilter</returns>
        private Dictionary<FileInfo, RoleFilter> GetFileRoles()
        {
            Dictionary<FileInfo, RoleFilter> fileRoles = new Dictionary<FileInfo, RoleFilter>();
            foreach (var refModel in _fedModel.ReferencedModels)
            {
                IfcDocumentInformation doc = refModel.DocumentInformation;
                IfcOrganization owner = doc.DocumentOwner as IfcOrganization;
                if ((owner != null) && (owner.Roles != null))
                {
                    RoleFilter docRoles = GetRoleFilters(owner.Roles.ToList());
                    try
                    {
                        FileInfo file = new FileInfo(doc.Name);
                        if (file.Exists)
                        {
                            fileRoles[file] = docRoles;
                        }
                        else
                        {
                            MessageBox.Show("File path does not exist: {0}", doc.Name);
                        }
                    }
                    catch (System.ArgumentException )
                    {
                        MessageBox.Show("File path is incorrect: {0}", doc.Name);
                    }
                }
            }
            return fileRoles;
        }

        /// <summary>
        /// Dispose method, close host model, and run through referencedModel list in the XBimModel itself and close all referenced models
        /// </summary>
        public void Dispose()
        {
            if (_fedModel != null)
            {
                _fedModel.Close();
                _fedModel.Dispose(); //might not need this, as close can call dispose above
                _fedModel = null;
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
                actorRoles.Add(RoleMap.ContainsKey(RoleFilter.Architectural) ? RoleMap[RoleFilter.Architectural] : MapRole(RoleFilter.Architectural, IfcRole.Architect));
            }
            if (role.HasFlag(RoleFilter.Mechanical))
            {
                actorRoles.Add(RoleMap.ContainsKey(RoleFilter.Mechanical) ? RoleMap[RoleFilter.Mechanical] : MapRole(RoleFilter.Mechanical, IfcRole.MechanicalEngineer));
            }
            if (role.HasFlag(RoleFilter.Electrical))
            {
                actorRoles.Add(RoleMap.ContainsKey(RoleFilter.Electrical) ? RoleMap[RoleFilter.Electrical] : MapRole(RoleFilter.Electrical, IfcRole.ElectricalEngineer));
            }
            if (role.HasFlag(RoleFilter.Plumbing))
            {
                actorRoles.Add(RoleMap.ContainsKey(RoleFilter.Plumbing) ? RoleMap[RoleFilter.Plumbing] : MapRole(RoleFilter.Plumbing, IfcRole.UserDefined));
            }
            if (role.HasFlag(RoleFilter.FireProtection))
            {
                actorRoles.Add(RoleMap.ContainsKey(RoleFilter.FireProtection) ? RoleMap[RoleFilter.FireProtection] : MapRole(RoleFilter.FireProtection, IfcRole.UserDefined));
            }
            if (role.HasFlag(RoleFilter.Unknown))
            {
                actorRoles.Add(RoleMap.ContainsKey(RoleFilter.Unknown) ? RoleMap[RoleFilter.Unknown] : MapRole(RoleFilter.Unknown, IfcRole.UserDefined));
            }
            return actorRoles;
        }

        /// <summary>
        /// Convert RoleFilter to IfcActorRole, need to be within a transaction
        /// </summary>
        /// <param name="role">RoleFilter</param>
        /// <param name="ifcRole">IfcRole</param>
        /// <returns></returns>
        private IfcActorRole MapRole(RoleFilter role, IfcRole ifcRole)
        {
            if (_fedModel.IsTransacting)
            {
                IfcActorRole actorRole = _fedModel.Instances.New<IfcActorRole>();
                if (ifcRole == IfcRole.UserDefined)
                {
                    actorRole.Role = IfcRole.UserDefined;
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

        /// <summary>
        /// Convert a list of IfcActorRole to set correct flags on the returned RoleFilter 
        /// </summary>
        /// <param name="actorRoles">list of IfcActorRole</param>
        /// <returns>RoleFilter</returns>
        private RoleFilter GetRoleFilters(List<IfcActorRole> actorRoles)
        {
            RoleFilter roles = RoleFilter.Unknown; //reset to unknown

            //set selected roles
            int idx = 0;
            foreach (var item in actorRoles)
            {
                RoleFilter role = MapActorRole(item);
                if (!roles.HasFlag(role))
                {
                    roles |= role; //add flag to RoleFilter
                    //remove the inital RoleFilter.Unknown,  set in declaration unless found role is unknown
                    if ((idx == 0) && (role != RoleFilter.Unknown))
                    {
                        roles &= ~RoleFilter.Unknown;
                    }
                    idx++;
                }
            }
            return roles;
        }

        /// <summary>
        /// Map IfcActroRole to RoleFilter, if no match the RoleFilter.Unknown returned
        /// </summary>
        /// <param name="actorRole">IfcActorRole</param>
        /// <returns>RoleFilter</returns>
        private RoleFilter MapActorRole(IfcActorRole actorRole)
        {
            var role = actorRole.Role;
            var userDefined = actorRole.UserDefinedRole;
            if (role == IfcRole.UserDefined)
            {
                RoleFilter role2;
                if (Enum.TryParse(userDefined, out role2))
                {
                    return role2;
                }
            }
            else
            {
                if (role == IfcRole.Architect)
                    return RoleFilter.Architectural;
                if (role == IfcRole.MechanicalEngineer)
                    return RoleFilter.Mechanical;
                if (role == IfcRole.ElectricalEngineer)
                    return RoleFilter.Electrical;
            }
            //Unhandled IfcRole
            //IfcRole.Supplier IfcRole.Manufacturer IfcRole.Contractor IfcRole.Subcontractor IfcRole.StructuralEngineer
            //IfcRole.CostEngineer IfcRole.Client IfcRole.BuildingOwner IfcRole.BuildingOperator IfcRole.ProjectManager
            //IfcRole.FacilitiesManager IfcRole.CivilEngineer IfcRole.ComissioningEngineer IfcRole.Engineer IfcRole.Consultant
            //IfcRole.ConstructionManager IfcRole.FieldConstructionManager  IfcRole.Owner IfcRole.Reseller
            return RoleFilter.Unknown;
        }
        #endregion
    }
}
