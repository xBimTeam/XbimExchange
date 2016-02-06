using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    /// <summary>
    /// 
    /// </summary>
    public class ContactConverter: Contact
    {


        /// <summary>
        /// 
        /// </summary>
        public ContactConverter()
        {
            
        }
        /// <summary>
        /// Writes out a contact, 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="helper"></param>
        public ContactConverter(IIfcActorSelect actor, COBieExpressHelper helper)
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

        private void ConvertOrganisation(IIfcOrganization ifcOrganization, COBieExpressHelper helper)
        {
            if (ifcOrganization.Addresses != null)
            {
                var telecom = ifcOrganization.Addresses.OfType<IIfcTelecomAddress>();
                var postal = ifcOrganization.Addresses.OfType<IIfcPostalAddress>();
                var ifcTelecomAddresses = telecom.ToArray();
                if (ifcTelecomAddresses.Any())
                {
                    var emailAddresses = string.Join(";",ifcTelecomAddresses.SelectMany(t => t.ElectronicMailAddresses));
                    if (!string.IsNullOrWhiteSpace(emailAddresses))
                       Email =  emailAddresses;
                    var phoneNums = string.Join(";", ifcTelecomAddresses.SelectMany(t => t.TelephoneNumbers));
                    if (!string.IsNullOrWhiteSpace(phoneNums))
                        Phone = phoneNums;
                    //var url = string.Join(";", ifcTelecomAddresses.Where(p => p.WWWHomePageURL.HasValue).SelectMany(p => p.WWWHomePageURL.ToString()));
                    //if (!string.IsNullOrWhiteSpace(url))
                    //    URL= url;

                }

                var ifcPostalAddresses =  postal.ToArray();
                if (ifcPostalAddresses.Any())
                {
                    var deptNames = string.Join(";",ifcPostalAddresses.Where(p=>p.InternalLocation.HasValue).SelectMany(p => p.InternalLocation.ToString()));
                    if (!string.IsNullOrWhiteSpace(deptNames))
                        Department = deptNames;
                    var streetNames = string.Join(";",ifcPostalAddresses.Where(p=>p.AddressLines!=null).SelectMany(p => p.AddressLines.ToString()));
                    if (!string.IsNullOrWhiteSpace(streetNames))
                        Street = streetNames;
                    var postalBox = string.Join(";", ifcPostalAddresses.Where(p => p.PostalBox.HasValue).SelectMany(p => p.PostalBox.ToString()));
                    if (!string.IsNullOrWhiteSpace(postalBox))
                        PostalBox = postalBox;
                    var town = string.Join(";", ifcPostalAddresses.Where(p => p.Town.HasValue).SelectMany(p => p.Town.ToString()));
                    if (!string.IsNullOrWhiteSpace(town))
                        Town = town;
                    var region = string.Join(";", ifcPostalAddresses.Where(p => p.Region.HasValue).SelectMany(p => p.Region.ToString()));
                    if (!string.IsNullOrWhiteSpace(region))
                       StateRegion = region;
                    var postalCode = string.Join(";", ifcPostalAddresses.Where(p => p.PostalCode.HasValue).SelectMany(p => p.PostalCode.ToString()));
                    if (!string.IsNullOrWhiteSpace(postalCode))
                        PostalCode = postalCode;
                }
               
                
            }
            if (ifcOrganization.Roles != null)
            {
                var roles = ifcOrganization.Roles;
                if (roles.Any())
                {
                    Categories = new List<Category>(roles.Count());
                    foreach (var role in roles)
                        Categories.Add(new Category {Classification = "Role", Code=role.Role.ToString(), Description = role.Description});
                }
            }

            Company = ifcOrganization.Name;

          
        }

        private void ConvertPerson(IIfcPerson ifcPerson, COBieExpressHelper helper)
        {
            FamilyName = ifcPerson.FamilyName;
            GivenName = ifcPerson.GivenName;
           
            if (ifcPerson.Addresses != null)
            {
                var telecom = ifcPerson.Addresses.OfType<IIfcTelecomAddress>();
                var postal = ifcPerson.Addresses.OfType<IIfcPostalAddress>();
                var ifcTelecomAddresses = telecom.ToArray();
                if (ifcTelecomAddresses.Any())
                {
                    var emailAddresses = string.Join(";", ifcTelecomAddresses.Where(t => t.ElectronicMailAddresses != null).SelectMany(t => t.ElectronicMailAddresses));
                    if (!string.IsNullOrWhiteSpace(emailAddresses))
                        Email = string.Join(";", emailAddresses, Email ?? "");
                    var phoneNums = string.Join(";", ifcTelecomAddresses.Where(t => t.TelephoneNumbers!=null).SelectMany(t => t.TelephoneNumbers));
                    if (!string.IsNullOrWhiteSpace(phoneNums))
                        Phone = string.Join(";", phoneNums, Phone ?? "");
                    //var url = string.Join(";", ifcTelecomAddresses.Where(p => p.WWWHomePageURL.HasValue).SelectMany(p => p.WWWHomePageURL.ToString()));
                    //if (!string.IsNullOrWhiteSpace(url))
                    //    ContactURL = string.Join(";", url, ContactURL ?? ""); ;
                }

                var ifcPostalAddresses = postal.ToArray();
                if (ifcPostalAddresses.Any())
                {
                    var deptNames = string.Join(";", ifcPostalAddresses.Where(p => p.InternalLocation.HasValue).SelectMany(p => p.InternalLocation.ToString()));
                    if (!string.IsNullOrWhiteSpace(deptNames))
                        Department =  string.Join(";", deptNames,Department??"");
                    var streetNames = string.Join(";", ifcPostalAddresses.Where(p => p.AddressLines != null).SelectMany(p => p.AddressLines.ToString()));
                    if (!string.IsNullOrWhiteSpace(streetNames))
                        Street = string.Join(";", streetNames,Street ?? "");
                    var postalBox = string.Join(";", ifcPostalAddresses.Where(p => p.PostalBox.HasValue).SelectMany(p => p.PostalBox.ToString()));
                    if (!string.IsNullOrWhiteSpace(postalBox))
                        PostalBox = string.Join(";", postalBox, PostalBox ?? "");
                    var town = string.Join(";", ifcPostalAddresses.Where(p => p.Town.HasValue).SelectMany(p => p.Town.ToString()));
                    if (!string.IsNullOrWhiteSpace(town))
                        Town = string.Join(";", town, Town ?? "");
                    var region = string.Join(";", ifcPostalAddresses.Where(p => p.Region.HasValue).SelectMany(p => p.Region.ToString()));
                    if (!string.IsNullOrWhiteSpace(region))
                        StateRegion = string.Join(";", region, StateRegion ?? "");
                    var postalCode = string.Join(";", ifcPostalAddresses.Where(p => p.PostalCode.HasValue).SelectMany(p => p.PostalCode.ToString()));
                    if (!string.IsNullOrWhiteSpace(postalCode))
                        PostalCode = string.Join(";", postalCode, PostalCode ?? "");
                }
            }
            if (ifcPerson.Roles != null)
            {
                var roles = ifcPerson.Roles;
                if (roles.Any())
                {
                    Categories = new List<Category>(roles.Count());
                    foreach (var role in roles)
                        Categories.Add(new Category { Classification = "Role", Code = role.Role.ToString(), Description = role.Description });
                } 
               
            }

        }
      
    }
}
