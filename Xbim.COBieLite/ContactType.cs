using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xbim.Ifc4.Interfaces;

namespace Xbim.COBieLite
{
    public partial class ContactType: ICOBieObject
    {
    


        /// <summary>
        /// Writes out a contact, 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="helper"></param>
        public ContactType(IIfcActorSelect actor, CoBieLiteHelper helper)
            : this()
        {
            var personAndOrganization = actor as IIfcPersonAndOrganization;
            var person = actor as IIfcPerson;
            var organisation = actor as IIfcOrganization;
            if (personAndOrganization != null)
            {
                ConvertOrganisation(personAndOrganization.TheOrganization, helper);
                ConvertPerson(personAndOrganization.ThePerson, helper);
              
            }
            else if(person!=null)
                ConvertPerson(person, helper);
            else if(organisation!=null)
                ConvertOrganisation(organisation, helper);

            ////Attributes
            //AttributeType[] ifcAttributes = helper.GetAttributes(actor);
            //if (ifcAttributes != null && ifcAttributes.Length > 0)
            //    ContactAttributes = new AttributeCollectionType { Attribute = ifcAttributes };
            
          
        }

        private void ConvertOrganisation(IIfcOrganization ifcOrganization, CoBieLiteHelper helper)
        {
            if (ifcOrganization.Addresses != null)
            {
                var telecom = ifcOrganization.Addresses.OfType<IIfcTelecomAddress>();
                var postal = ifcOrganization.Addresses.OfType<IIfcPostalAddress>();
                var ifcTelecomAddresses = telecom as IIfcTelecomAddress[] ?? telecom.ToArray();
                if (ifcTelecomAddresses.Any())
                {
                    var emailAddresses = string.Join(";",ifcTelecomAddresses.SelectMany(t => t.ElectronicMailAddresses));
                    if (!string.IsNullOrWhiteSpace(emailAddresses))
                        ContactEmail =  emailAddresses;
                    var phoneNums = string.Join(";", ifcTelecomAddresses.SelectMany(t => t.TelephoneNumbers));
                    if (!string.IsNullOrWhiteSpace(phoneNums))
                        ContactPhoneNumber = phoneNums;
                    var url = string.Join(";", ifcTelecomAddresses.Where(p => p.WWWHomePageURL.HasValue).SelectMany(p => p.WWWHomePageURL.ToString()));
                    if (!string.IsNullOrWhiteSpace(url))
                        ContactURL = url;

                }

                var ifcPostalAddresses = postal as IIfcPostalAddress[] ?? postal.ToArray();
                if (ifcPostalAddresses.Any())
                {
                    var deptNames = string.Join(";",ifcPostalAddresses.Where(p=>p.InternalLocation.HasValue).SelectMany(p => p.InternalLocation.ToString()));
                    if (!string.IsNullOrWhiteSpace(deptNames))
                        ContactDepartmentName = deptNames;
                    var streetNames = string.Join(";",ifcPostalAddresses.Where(p=>p.AddressLines!=null).SelectMany(p => p.AddressLines.ToString()));
                    if (!string.IsNullOrWhiteSpace(streetNames))
                        ContactStreet = streetNames;
                    var postalBox = string.Join(";", ifcPostalAddresses.Where(p => p.PostalBox.HasValue).SelectMany(p => p.PostalBox.ToString()));
                    if (!string.IsNullOrWhiteSpace(postalBox))
                        ContactPostalBoxNumber = postalBox;
                    var town = string.Join(";", ifcPostalAddresses.Where(p => p.Town.HasValue).SelectMany(p => p.Town.ToString()));
                    if (!string.IsNullOrWhiteSpace(town))
                        ContactTownName = town;
                    var region = string.Join(";", ifcPostalAddresses.Where(p => p.Region.HasValue).SelectMany(p => p.Region.ToString()));
                    if (!string.IsNullOrWhiteSpace(region))
                        ContactRegionCode = region;
                    var postalCode = string.Join(";", ifcPostalAddresses.Where(p => p.PostalCode.HasValue).SelectMany(p => p.PostalCode.ToString()));
                    if (!string.IsNullOrWhiteSpace(postalCode))
                        ContactPostalCode = postalCode;
                }
               
                
            }
            if (ifcOrganization.Roles != null)
            {
                var roles = string.Join(";",ifcOrganization.Roles.Select(r => r.RoleString)); //deals with User defined roles
                if (!string.IsNullOrWhiteSpace(roles))
                    ContactCategory =  roles;
            }

            ContactCompanyName = ifcOrganization.Name;

          
        }

        private void ConvertPerson(IIfcPerson ifcPerson, CoBieLiteHelper helper)
        {
            ContactFamilyName = ifcPerson.FamilyName;
            ContactGivenName = ifcPerson.GivenName;
           
            if (ifcPerson.Addresses != null)
            {
                var telecom = ifcPerson.Addresses.OfType<IIfcTelecomAddress>();
                var postal = ifcPerson.Addresses.OfType<IIfcPostalAddress>();
                var ifcTelecomAddresses = telecom as IIfcTelecomAddress[] ?? telecom.ToArray();
                if (ifcTelecomAddresses.Any())
                {
                    var emailAddresses = string.Join(";", ifcTelecomAddresses.Where(t => t.ElectronicMailAddresses!=null).SelectMany(t => t.ElectronicMailAddresses));
                    if (!string.IsNullOrWhiteSpace(emailAddresses))
                        ContactEmail = string.Join(";", emailAddresses, ContactEmail ?? "");

                    var phoneNums = string.Join(";", ifcTelecomAddresses.Where(t => t.TelephoneNumbers!=null).SelectMany(t => t.TelephoneNumbers));
                    if (!string.IsNullOrWhiteSpace(phoneNums))
                        ContactPhoneNumber = string.Join(";", phoneNums, ContactPhoneNumber ?? "");
                    var url = string.Join(";", ifcTelecomAddresses.Where(p => p.WWWHomePageURL.HasValue).SelectMany(p => p.WWWHomePageURL.ToString()));
                    if (!string.IsNullOrWhiteSpace(url))
                        ContactURL = string.Join(";", url, ContactURL ?? "");
                }

                var ifcPostalAddresses = postal as IIfcPostalAddress[] ?? postal.ToArray();
                if (ifcPostalAddresses.Any())
                {
                    var deptNames = string.Join(";", ifcPostalAddresses.Where(p => p.InternalLocation.HasValue).SelectMany(p => p.InternalLocation.ToString()));
                    if (!string.IsNullOrWhiteSpace(deptNames))
                        ContactDepartmentName =  string.Join(";", deptNames,ContactDepartmentName??"");
                    var streetNames = string.Join(";", ifcPostalAddresses.Where(p => p.AddressLines != null).SelectMany(p => p.AddressLines.ToString()));
                    if (!string.IsNullOrWhiteSpace(streetNames))
                        ContactStreet = string.Join(";", streetNames, ContactStreet ?? "");
                    var postalBox = string.Join(";", ifcPostalAddresses.Where(p => p.PostalBox.HasValue).SelectMany(p => p.PostalBox.ToString()));
                    if (!string.IsNullOrWhiteSpace(postalBox))
                        ContactPostalBoxNumber = string.Join(";", postalBox, ContactPostalBoxNumber ?? "");
                    var town = string.Join(";", ifcPostalAddresses.Where(p => p.Town.HasValue).SelectMany(p => p.Town.ToString()));
                    if (!string.IsNullOrWhiteSpace(town))
                        ContactTownName = string.Join(";", town, ContactTownName ?? "");
                    var region = string.Join(";", ifcPostalAddresses.Where(p => p.Region.HasValue).SelectMany(p => p.Region.ToString()));
                    if (!string.IsNullOrWhiteSpace(region))
                        ContactRegionCode = string.Join(";", region, ContactRegionCode ?? "");
                    var postalCode = string.Join(";", ifcPostalAddresses.Where(p => p.PostalCode.HasValue).SelectMany(p => p.PostalCode.ToString()));
                    if (!string.IsNullOrWhiteSpace(postalCode))
                        ContactPostalCode = string.Join(";", postalCode, ContactPostalCode ?? "");
                }
            }
            if (ifcPerson.Roles != null)
            {
                var roles = string.Join(";", ifcPerson.Roles.SelectMany(r => r.RoleString)); //deals with User defined roles
                if (!string.IsNullOrWhiteSpace(roles))
                    ContactCategory = string.Join(";", roles, ContactCategory ?? "");
            }

        }
        [XmlIgnore, JsonIgnore]
        DocumentCollectionType ICOBieObject.Documents
        {
            get { return ContactDocuments; }
            set { ContactDocuments = value; }
        }

        [XmlIgnore, JsonIgnore]
        IssueCollectionType ICOBieObject.Issues
        {
            get { return ContactIssues; }
            set { ContactIssues = value; }
        }

        [XmlIgnore, JsonIgnore]
        AttributeCollectionType ICOBieObject.Attributes
        {
            get { return ContactAttributes; }
            set { ContactAttributes = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Name
        {
            get { return ContactGivenName; }
            set { ContactGivenName = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Description
        {
            get { return ContactFamilyName; }
            set { ContactFamilyName = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Category
        {
            get { return ContactCategory; }
            set { ContactCategory = value; }
        }


        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Id
        {
            get { return externalID; }
            set { externalID = value; }
        }
    }
}
