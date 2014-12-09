using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.Ifc2x3.ActorResource;
using Xbim.COBie.Rows;
using Xbim.XbimExtensions.Transactions;
using Xbim.IO;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimContact : COBieXBim
    {
        public COBieXBimContact(COBieXBimContext xBimContext)
            : base(xBimContext)
        {

        }

        /// <summary>
        /// Add the IfcPersonAndOrganizations to the Model object
        /// </summary>
        /// <param name="cOBieSheet"></param>
        public void SerialiseContacts(COBieSheet<COBieContactRow> cOBieSheet)
        {

            
                    
            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Contacts"))
            {
                try
                {
                    int count = 1;
                    SetEmailUser(Constants.DEFAULT_EMAIL); //add Unknown.Unknown@Unknown.com PersonAndOrganization to use for nulls
                    SetDefaultUser();
                    ProgressIndicator.ReportMessage("Starting Contacts...");
                    ProgressIndicator.Initialise("Creating Contacts", cOBieSheet.RowCount);
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        BumpTransaction(trans, count);
                        count++;
                        ProgressIndicator.IncrementAndUpdate();
                        COBieContactRow row = cOBieSheet[i];
                        CreatePersonAndOrganization(row);

                    }
                    ProgressIndicator.Finalise();
                    trans.Commit();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            //IfcPersonAndOrganization thisContact = new IfcPersonAndOrganization();
            //XbimMemoryModel model = _model as XbimMemoryModel;
            //if (model != null)
            //{
            //    model.Instances.Add(thisContact);
            //}
            //IEnumerable<IfcPersonAndOrganization> ifcPersonAndOrganizations = Model.Instances.OfType<IfcPersonAndOrganization>();
            //string xxx = ifcPersonAndOrganizations.First().ThePerson.GivenName;
        }


        public void CreatePersonAndOrganization(COBieContactRow row, IfcPersonAndOrganization ifcPersonAndOrganization = null)
        {
            if (!Contacts.ContainsKey(row.Email)) //should filter on merge also unless Contacts is reset
            {
                IfcPerson ifcPerson = Model.Instances.New<IfcPerson>();
                IfcOrganization ifcOrganization = Model.Instances.New<IfcOrganization>();
                if (ifcPersonAndOrganization == null)
                    ifcPersonAndOrganization = Model.Instances.New<IfcPersonAndOrganization>();
                Contacts.Add(row.Email, ifcPersonAndOrganization); //build a list to reference for History objects

                //add email
                IfcTelecomAddress ifcTelecomAddress = Model.Instances.New<IfcTelecomAddress>();
                if (ValidateString(row.Email))
                {
                    if (ifcTelecomAddress.ElectronicMailAddresses == null)
                        ifcTelecomAddress.SetElectronicMailAddress(row.Email); //create the LabelCollection and set to ElectronicMailAddresses field
                    else
                        ifcTelecomAddress.ElectronicMailAddresses.Add(row.Email); //add to existing collection
                }

                //IfcPersonAndOrganization has no OwnerHistory so our COBie is extracting this from IfcProject so do nothing here

                //add Role from Category
                if (ValidateString(row.Category))
                {
                    IfcActorRole ifcActorRole = Model.Instances.New<IfcActorRole>();
                    ifcActorRole.RoleString = row.Category;
                    if (ifcPerson.Roles == null)
                        ifcPerson.SetRoles(ifcActorRole);//create the ActorRoleCollection and set to Roles field
                    else
                        ifcPerson.Roles.Add(ifcActorRole);//add to existing collection
                }
                //add Company
                if (ValidateString(row.Company))
                {
                    ifcOrganization.Name = row.Company;
                }
                else
                {
                    ifcOrganization.Name = "Unknown"; //is not an optional field so fill with unknown value
                }
                //add Phone
                if (ValidateString(row.Phone))
                {
                    if (ifcTelecomAddress.TelephoneNumbers == null)
                        ifcTelecomAddress.SetTelephoneNumbers(row.Phone);//create the LabelCollection and set to TelephoneNumbers field
                    else
                        ifcTelecomAddress.TelephoneNumbers.Add(row.Phone);//add to existing collection
                }

                //External System, as no history object we have to allow this to default to DEFAUL_STRING, so do nothing here
                //External Object is retrieved from object type IfcPersonAndOrganization so do nothing here

                //add External Identifier
                if (ValidateString(row.ExtIdentifier)) ifcPerson.Id = row.ExtIdentifier;
                //add Department
                IfcPostalAddress ifcPostalAddress = Model.Instances.New<IfcPostalAddress>();
                if (ValidateString(row.Department)) ifcPostalAddress.InternalLocation = row.Department;
                //add Organization code
                if (ValidateString(row.OrganizationCode)) ifcOrganization.Id = row.OrganizationCode;
                //add GivenName
                if (ValidateString(row.GivenName)) ifcPerson.GivenName = row.GivenName;
                //add Family Name
                if (ValidateString(row.FamilyName)) ifcPerson.FamilyName = row.FamilyName;
                //add Street
                if (ValidateString(row.Street)) ifcPostalAddress.SetAddressLines(row.Street.Split(','));
                //add PostalBox
                if (ValidateString(row.PostalBox)) ifcPostalAddress.PostalBox = row.PostalBox;
                //add Town
                if (ValidateString(row.Town)) ifcPostalAddress.Town = row.Town;
                //add StateRegion
                if (ValidateString(row.StateRegion)) ifcPostalAddress.Region = row.StateRegion;
                //add PostalCode
                if (ValidateString(row.PostalCode)) ifcPostalAddress.PostalCode = row.PostalCode;
                //add Country
                if (ValidateString(row.Country)) ifcPostalAddress.Country = row.Country;

                //add addresses into IfcPerson object
                //add Telecom Address
                if (ifcPerson.Addresses == null)
                    ifcPerson.SetTelecomAddresss(ifcTelecomAddress);//create the AddressCollection and set to Addresses field
                else
                    ifcPerson.Addresses.Add(ifcTelecomAddress);//add to existing collection
                // Add postal address
                if (ifcPerson.Addresses == null)
                    ifcPerson.SetPostalAddresss(ifcPostalAddress);//create the AddressCollection and set to Addresses field
                else
                    ifcPerson.Addresses.Add(ifcPostalAddress);//add to existing collection

                //add the person and the organization objects 
                ifcPersonAndOrganization.ThePerson = ifcPerson;
                ifcPersonAndOrganization.TheOrganization = ifcOrganization;
            }
        }

        /// <summary>
        /// set the default IfcPersonAndOrganization in the Model
        /// </summary>
        public void SetDefaultUser()
        {
            COBieSheet<COBieFacilityRow> facilityRow = (COBieSheet<COBieFacilityRow>)WorkBook[Constants.WORKSHEET_FACILITY];
            if (facilityRow.RowCount > 0)
            {
                COBieFacilityRow row = facilityRow[0]; //use first row supported on facility sheet

                string defaultUser = "";
                if ((!string.IsNullOrEmpty(row.CreatedBy)) || (row.CreatedBy != Constants.DEFAULT_STRING)) defaultUser = row.CreatedBy;
                COBieSheet<COBieContactRow> contacts = (COBieSheet<COBieContactRow>)WorkBook[Constants.WORKSHEET_CONTACT];
                for (int i = 0; i < contacts.RowCount; i++)
                {
                    if (contacts[i].Email == defaultUser)
                    {
                        CreatePersonAndOrganization(contacts[i], Model.DefaultOwningUser);
                        break;
                    }
                }

            }
        }

        
    }
}
