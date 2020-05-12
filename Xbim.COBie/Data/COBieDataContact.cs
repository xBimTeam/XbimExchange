using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.Ifc4.Interfaces;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Contact tab.
    /// </summary>
    public class COBieDataContact : COBieData<COBieContactRow>
    {
        /// <summary>
        /// Data Contact constructor
        /// </summary>
        public COBieDataContact(COBieContext context) : base(context)
        { }


        #region Methods

        /// <summary>
        /// Fill sheet rows for Contact sheet
        /// </summary>
        /// <returns>COBieSheet</returns>
        public override COBieSheet<COBieContactRow> Fill()
        {
            ProgressIndicator.ReportMessage("Starting Contacts...");



            ClearEMails(); //clear the email dictionary for a new file conversion

            //create new sheet
            var contacts = new COBieSheet<COBieContactRow>(Constants.WORKSHEET_CONTACT);
            var cobieContacts = Model.FederatedInstances.OfType<IfcPropertySingleValue>().Where(psv => psv.Name == "COBieCreatedBy" || psv.Name == "COBieTypeCreatedBy").GroupBy(psv => psv.NominalValue).Select(g => g.First().NominalValue.ToString());
            var ifcPersonAndOrganizations = Model.FederatedInstances.OfType<IfcPersonAndOrganization>();
            ProgressIndicator.Initialise("Creating Contacts", ifcPersonAndOrganizations.Count() + cobieContacts.Count());

            var ifcProject = Model.Instances.FirstOrDefault<IIfcProject>();
            Debug.Assert(ifcProject!=null);

            List<IfcOrganizationRelationship> ifcOrganizationRelationships = null;

            foreach (var ifcPersonAndOrganization in ifcPersonAndOrganizations)
            {
                ProgressIndicator.IncrementAndUpdate();
                
                //check we do not have a default email address, if skip it as we want the validation warning
                var email = GetTelecomEmailAddress(ifcPersonAndOrganization);
                if (email == Constants.DEFAULT_EMAIL)
                {
                    continue;
                }
                 
                var contact = new COBieContactRow(contacts);
                // get person and organization
                var ifcOrganization = ifcPersonAndOrganization.TheOrganization;
                var ifcPerson = ifcPersonAndOrganization.ThePerson;
                contact.Email = email;

                //lets default the creator to that user who created the project for now, no direct link to OwnerHistory on IfcPersonAndOrganization, IfcPerson or IfcOrganization
                contact.CreatedBy = GetTelecomEmailAddress(ifcProject.OwnerHistory);
                contact.CreatedOn = GetCreatedOnDateAsFmtString(ifcProject.OwnerHistory);

                //Conract.Category
                //according Responsibility Matrix v17: 
                //IfcActorRole.UserDefinedRole
                //Constructs a comma delimitted list of the unique strings found in 
                //IfcPersonAndOrganization.Roles, IfcPersonAndOrganization.ThePerson.Roles, and IfcPersonAndOrganization.TheOrganization.Roles
                var roles = new SortedSet<string>();
                CollectRoles(roles, ifcPersonAndOrganization.Roles);
                CollectRoles(roles, ifcPerson.Roles);
                CollectRoles(roles, ifcOrganization.Roles);

                string category = DEFAULT_STRING;
                foreach ( var role in roles )
                    {
                    if ( category.Length > 0 )
                        category += ",";
                    category += role;
                    }

                contact.Category = category;
                
                contact.Company = (string.IsNullOrEmpty(ifcOrganization.Name)) ? DEFAULT_STRING : ifcOrganization.Name.ToString();
                contact.Phone = GetTelecomTelephoneNumber(ifcPersonAndOrganization);
                contact.ExtSystem = DEFAULT_STRING;   // TODO: Person is not a Root object so has no Owner. What should this be?
                
                contact.ExtObject = "IfcPersonAndOrganization";
                if (!string.IsNullOrEmpty(ifcPerson.Id))
                {
                    contact.ExtIdentifier = ifcPerson.Id;
                }
                //get department
                var department = "";
                if (ifcPerson.Addresses != null)
                {
                    department = ifcPerson.Addresses.OfType<IfcPostalAddress>().Select(dept => dept.InternalLocation).FirstOrDefault(dept => !string.IsNullOrEmpty(dept));
                }
                if (string.IsNullOrEmpty(department))
                {
                    if (ifcOrganizationRelationships == null)
                    {
                        ifcOrganizationRelationships = Model.FederatedInstances.OfType<IfcOrganizationRelationship>().ToList();
                    }
                    var ifcRelOrganization = ifcOrganizationRelationships
                                                        .Where(Or => Or.RelatingOrganization.EntityLabel == ifcOrganization.EntityLabel && Or.RelatedOrganizations.Last() != null)
                                                        .Select(Or => Or.RelatedOrganizations.Last())
                                                        .LastOrDefault();
                    if (ifcRelOrganization != null)
                        department = ifcRelOrganization.Name.ToString();
                }
                if (string.IsNullOrEmpty(department)) 
                    department = ifcOrganization.Description.ToString(); //only place to match example files
                contact.Department = (string.IsNullOrEmpty(department)) ? contact.Company : department;

                contact.OrganizationCode = (string.IsNullOrEmpty(ifcOrganization.Id)) ? DEFAULT_STRING : ifcOrganization.Id.ToString();
                contact.GivenName = (string.IsNullOrEmpty(ifcPerson.GivenName)) ? DEFAULT_STRING : ifcPerson.GivenName.ToString();
                contact.FamilyName = (string.IsNullOrEmpty(ifcPerson.FamilyName)) ? DEFAULT_STRING : ifcPerson.FamilyName.ToString();
                if (ifcPerson.Addresses != null)
                    GetContactAddress(contact, ifcPerson.Addresses);
                else
                    GetContactAddress(contact, ifcOrganization.Addresses);

                contacts.AddRow(contact);
            }

            foreach (var email in cobieContacts)
            {
                ProgressIndicator.IncrementAndUpdate();
                var contact = new COBieContactRow(contacts);
                contact.Email = email;

                //lets default the creator to that user who created the project for now, no direct link to OwnerHistory on IfcPersonAndOrganization, IfcPerson or IfcOrganization
                contact.CreatedBy = GetTelecomEmailAddress(ifcProject.OwnerHistory);
                contact.CreatedOn = GetCreatedOnDateAsFmtString(ifcProject.OwnerHistory);
                contact.Category = DEFAULT_STRING;
                contact.Company = DEFAULT_STRING;
                contact.Phone = DEFAULT_STRING;
                contact.ExtSystem = DEFAULT_STRING;

                contact.ExtObject = "IfcPropertySingleValue";
                contact.Department = DEFAULT_STRING;

                contact.OrganizationCode = DEFAULT_STRING;
                contact.GivenName = DEFAULT_STRING;
                contact.FamilyName = DEFAULT_STRING;
                contact.Street = DEFAULT_STRING;
                contact.PostalBox = DEFAULT_STRING;
                contact.Town = DEFAULT_STRING;
                contact.StateRegion = DEFAULT_STRING;
                contact.PostalCode = DEFAULT_STRING;
                contact.Country = DEFAULT_STRING;
                
                contacts.AddRow(contact);
            }
            ProgressIndicator.Finalise();

            contacts.OrderBy(s => s.Email);

            return contacts;
        }

        private static void CollectRoles (SortedSet<string> roles, IEnumerable<IfcActorRole> ifcRoles)
        {
            foreach ( var ifcRole in ifcRoles )
            {
                if (ifcRole != null && ifcRole.UserDefinedRole != null)
                {
                    var role = ifcRole.UserDefinedRole.ToString();
                    if (!string.IsNullOrEmpty(role))
                    {
                        roles.Add(role);
                    }
                }
            }
        }

        private static void GetContactAddress(COBieContactRow contact, IEnumerable<IfcAddress> addresses)
        {
            var ifcAddresses = addresses as IfcAddress[] ?? addresses.ToArray();
            if ((addresses != null) && ifcAddresses.Any())
            {
                var ifcPostalAddress = ifcAddresses.OfType<IfcPostalAddress>().FirstOrDefault();
                if (ifcPostalAddress != null) 
                {
                    var street = new List<string>();
                    if (ifcPostalAddress.AddressLines != null)
                        street = ifcPostalAddress.AddressLines.Select(st => st.Value.ToString()).ToList();
                    
                    contact.Street = (street.Count > 0) ? string.Join(", ", street) : DEFAULT_STRING;
                    contact.PostalBox = (string.IsNullOrEmpty(ifcPostalAddress.PostalBox)) ? DEFAULT_STRING : ifcPostalAddress.PostalBox.ToString();
                    contact.Town = (string.IsNullOrEmpty(ifcPostalAddress.Town)) ? DEFAULT_STRING : ifcPostalAddress.Town.ToString();
                    contact.StateRegion = (string.IsNullOrEmpty(ifcPostalAddress.Region)) ? DEFAULT_STRING : ifcPostalAddress.Region.ToString();
                    contact.PostalCode = (string.IsNullOrEmpty(ifcPostalAddress.PostalCode)) ? DEFAULT_STRING : ifcPostalAddress.PostalCode.ToString();
                    contact.Country = (string.IsNullOrEmpty(ifcPostalAddress.Country)) ? DEFAULT_STRING : ifcPostalAddress.Country.ToString();
                    return;
                }
            }
            contact.Street = DEFAULT_STRING;
            contact.PostalBox = DEFAULT_STRING;
            contact.Town = DEFAULT_STRING;
            contact.StateRegion = DEFAULT_STRING;
            contact.PostalCode = DEFAULT_STRING;
            contact.Country = DEFAULT_STRING;

           
        }

        #endregion
    }
}
