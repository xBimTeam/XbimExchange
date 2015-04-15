using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.UtilityResource;
using Xbim.XbimExtensions;
using Xbim.Ifc2x3.Kernel;
using Xbim.XbimExtensions.SelectTypes;
using Xbim.Ifc2x3.PropertyResource;

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
        /// <param name="model">The context of the model being generated</param>
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
            COBieSheet<COBieContactRow> contacts = new COBieSheet<COBieContactRow>(Constants.WORKSHEET_CONTACT);
            IEnumerable<string> cobieContacts = Model.Instances.OfType<IfcPropertySingleValue>().Where(psv => psv.Name == "COBieCreatedBy" || psv.Name == "COBieTypeCreatedBy").GroupBy(psv => psv.NominalValue).Select(g => g.First().NominalValue.ToString());
            IEnumerable<IfcPersonAndOrganization> ifcPersonAndOrganizations = Model.Instances.OfType<IfcPersonAndOrganization>();
            ProgressIndicator.Initialise("Creating Contacts", ifcPersonAndOrganizations.Count() + cobieContacts.Count());

            List<IfcOrganizationRelationship> ifcOrganizationRelationships = null;

            foreach (IfcPersonAndOrganization ifcPersonAndOrganization in ifcPersonAndOrganizations)
            {
                ProgressIndicator.IncrementAndUpdate();
                
                //check we do not have a default email address, if skip it as we want the validation warning
                string email = GetTelecomEmailAddress(ifcPersonAndOrganization);
                if (email == Constants.DEFAULT_EMAIL)
                {
                    continue;
                }

                COBieContactRow contact = new COBieContactRow(contacts);
                // get person and organization
                IfcOrganization ifcOrganization = ifcPersonAndOrganization.TheOrganization;
                IfcPerson ifcPerson = ifcPersonAndOrganization.ThePerson;
                contact.Email = email;

                //lets default the creator to that user who created the project for now, no direct link to OwnerHistory on IfcPersonAndOrganization, IfcPerson or IfcOrganization
                contact.CreatedBy = GetTelecomEmailAddress(Model.IfcProject.OwnerHistory);
                contact.CreatedOn = GetCreatedOnDateAsFmtString(Model.IfcProject.OwnerHistory);
                
                IfcActorRole ifcActorRole = null;
                if (ifcPerson.Roles != null)
                    ifcActorRole = ifcPerson.Roles.FirstOrDefault();
                if (ifcOrganization.Roles != null)
                    ifcActorRole = ifcOrganization.Roles.FirstOrDefault();
                if ((ifcActorRole != null) && (!string.IsNullOrEmpty(ifcActorRole.UserDefinedRole)))
                {
                    contact.Category = ifcActorRole.UserDefinedRole.ToString();
                }
                else
                    contact.Category = DEFAULT_STRING;
                
                contact.Company = (string.IsNullOrEmpty(ifcOrganization.Name)) ? DEFAULT_STRING : ifcOrganization.Name.ToString();
                contact.Phone = GetTelecomTelephoneNumber(ifcPersonAndOrganization);
                contact.ExtSystem = DEFAULT_STRING;   // TODO: Person is not a Root object so has no Owner. What should this be?
                
                contact.ExtObject = "IfcPersonAndOrganization";
                if (!string.IsNullOrEmpty(ifcPerson.Id))
                {
                    contact.ExtIdentifier = ifcPerson.Id;
                }
                //get department
                string department = "";
                if (ifcPerson.Addresses != null)
                {
                    department = ifcPerson.Addresses.PostalAddresses.Select(dept => dept.InternalLocation).Where(dept => !string.IsNullOrEmpty(dept)).FirstOrDefault();
                }
                if (string.IsNullOrEmpty(department))
                {
                    if (ifcOrganizationRelationships == null)
                    {
                        ifcOrganizationRelationships = Model.Instances.OfType<IfcOrganizationRelationship>().ToList();
                    }
                    IfcOrganization ifcRelOrganization = ifcOrganizationRelationships
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

            foreach (string email in cobieContacts)
            {
                ProgressIndicator.IncrementAndUpdate();
                COBieContactRow contact = new COBieContactRow(contacts);
                contact.Email = email;

                //lets default the creator to that user who created the project for now, no direct link to OwnerHistory on IfcPersonAndOrganization, IfcPerson or IfcOrganization
                contact.CreatedBy = GetTelecomEmailAddress(Model.IfcProject.OwnerHistory);
                contact.CreatedOn = GetCreatedOnDateAsFmtString(Model.IfcProject.OwnerHistory);
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

        private static void GetContactAddress(COBieContactRow contact, AddressCollection addresses)
        {
            if ((addresses != null) && (addresses.PostalAddresses  != null))
            {
                IfcPostalAddress ifcPostalAddress = addresses.PostalAddresses.FirstOrDefault();
                if (ifcPostalAddress != null) 
                {
                    List<string> street = new List<string>();
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
