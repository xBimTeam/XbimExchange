using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.ActorResource;
using Xbim.IO;
using Xbim.XbimExtensions.SelectTypes;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    public class ContactComparer : IEqualityComparer<Contact>
    {
        public bool Equals(Contact x, Contact y)
        {
            return x.Email.Equals(y.Email);
        }

        public int GetHashCode(Contact obj)
        {
            return obj.Email.GetHashCode();
        }
    }
    class MappingIfcActorToContact : XbimMappings<XbimModel, List<Facility>, string, IfcActorSelect, Contact>
    {
        protected override Contact Mapping(IfcActorSelect actor, Contact target)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            var personAndOrganization = actor as IfcPersonAndOrganization;
            var person = actor as IfcPerson;
            var organisation = actor as IfcOrganization;
            if (personAndOrganization != null)
            {
                ConvertOrganisation(target, personAndOrganization.TheOrganization);
                ConvertPerson(target, personAndOrganization.ThePerson);

            }
            else if (person != null)
                ConvertPerson(target, person);
            else if (organisation != null)
                ConvertOrganisation(target, organisation);

            if (string.IsNullOrWhiteSpace(target.Email))
            {
               target.Email = string.Format("unknown{0}@undefined.email", actor.EntityLabel);
            }
            target.CreatedBy = helper.GetCreatedBy(actor);
            target.CreatedOn = helper.GetCreatedOn(actor);
            ////Attributes
            //AttributeType[] ifcAttributes = helper.GetAttributes(actor);
            //if (ifcAttributes != null && ifcAttributes.Length > 0)
            //    ContactAttributes = new AttributeCollectionType { Attribute = ifcAttributes };
            return target;
        }


        private void ConvertOrganisation(Contact target, IfcOrganization ifcOrganization)
        {
            if (ifcOrganization.Addresses != null)
            {
                var telecom = ifcOrganization.Addresses.OfType<IfcTelecomAddress>().FirstOrDefault(a => a.ElectronicMailAddresses.Any(e => !string.IsNullOrWhiteSpace(e)));
                var postal = ifcOrganization.Addresses.OfType<IfcPostalAddress>().FirstOrDefault();

                if (telecom!=null)
                {
                    target.Email = telecom.ElectronicMailAddresses.FirstOrDefault();
                    target.Phone = telecom.TelephoneNumbers.FirstOrDefault();
                }

                if (postal!=null)
                {

                    target.Department = postal.InternalLocation;
                    target.Street = postal.AddressLines.ToString();
                    target.PostalBox = postal.PostalBox;
                    target.Town = postal.Town;
                    target.StateRegion = postal.Region;
                    target.PostalCode = postal.PostalCode;
                }
            }
            if (ifcOrganization.Roles != null)
            {
                var roles = ifcOrganization.Roles;
                if (roles.Any())
                {
                    target.Categories = new List<Category>(roles.Count);
                    foreach (var role in roles)
                        target.Categories.Add(new Category { Classification = "Role", Code = role.Role.ToString(), Description = role.Description });
                }
            }

            target.Company = ifcOrganization.Name;


        }

        private void ConvertPerson(Contact target, IfcPerson ifcPerson)
        {
            target.FamilyName = ifcPerson.FamilyName;
            target.GivenName = ifcPerson.GivenName;

            if (ifcPerson.Addresses != null)
            {
                var telecom = ifcPerson.Addresses.OfType<IfcTelecomAddress>().FirstOrDefault();
                var postal = ifcPerson.Addresses.OfType<IfcPostalAddress>().FirstOrDefault();
                
                if (telecom!=null)
                {
                    if (telecom.ElectronicMailAddresses != null)
                    {
                        var emailAddress =
                            telecom.ElectronicMailAddresses.FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));
                        if (!string.IsNullOrWhiteSpace(emailAddress)) //override any set if we have one at person level
                            target.Email = emailAddress;
                    }
                    if (telecom.TelephoneNumbers != null)
                    {
                        var phoneNum = telecom.TelephoneNumbers.FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));
                        if (!string.IsNullOrWhiteSpace(phoneNum))
                            target.Phone = phoneNum;
                    }
                   
                }


                if (postal!=null)
                {
                    var deptName = postal.InternalLocation;
                    if (deptName.HasValue) target.Department = deptName;
                    if (postal.AddressLines != null)
                    {
                        var streetNames = postal.AddressLines.ToString();
                        if (!string.IsNullOrWhiteSpace(streetNames))
                            target.Street = streetNames;
                    }
                   
                    if (!string.IsNullOrWhiteSpace(postal.PostalBox))
                        target.PostalBox = postal.PostalBox;
                    if (!string.IsNullOrWhiteSpace(postal.Town))
                        target.Town = postal.Town;
                    if (!string.IsNullOrWhiteSpace(postal.Region))
                        target.StateRegion = postal.Region;                 
                    if (!string.IsNullOrWhiteSpace(postal.PostalCode))
                        target.PostalCode = postal.PostalCode;
                }
            }
            if (ifcPerson.Roles != null)
            {
                var roles = ifcPerson.Roles;
                if (roles.Any())
                {
                    target.Categories = new List<Category>(roles.Count);
                    foreach (var role in roles)
                        target.Categories.Add(new Category { Classification = "Role", Code = role.Role.ToString(), Description = role.Description });
                }

            }

        }
    }
}
