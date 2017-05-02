using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xbim.CobieLiteUk;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;


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
    class MappingIfcActorToContact : XbimMappings<IModel, List<Facility>, string, IIfcActorSelect, Contact>
    {
        protected override Contact Mapping(IIfcActorSelect actor, Contact target)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            var personAndOrganization = actor as IIfcPersonAndOrganization;
            var person = actor as IIfcPerson;
            var organisation = actor as IIfcOrganization;
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
               target.Email = string.Format("unknown{0}@undefined.email", ((IPersistEntity)actor).EntityLabel);
            }
            target.CreatedBy = helper.GetCreatedBy(actor, true);
            target.CreatedOn = helper.GetCreatedOn(actor);
            if (target.Categories == null || !target.Categories.Any())
            {
                target.Categories = CoBieLiteUkHelper.UnknownCategory;
            }
            ////Attributes
            //AttributeType[] ifcAttributes = helper.GetAttributes(actor);
            //if (ifcAttributes != null && ifcAttributes.Length > 0)
            //    ContactAttributes = new AttributeCollectionType { Attribute = ifcAttributes };
            //Documents no link available IfcActorSelect not inherited from IfcRoot


            return target;
        }


        private void ConvertOrganisation(Contact target, IIfcOrganization ifcOrganization)
        {
            if (ifcOrganization.Addresses != null && ifcOrganization.Addresses.Any())
            {
                var telecom = ifcOrganization.Addresses.OfType<IIfcTelecomAddress>().FirstOrDefault(a => a.ElectronicMailAddresses.Any(e => !string.IsNullOrWhiteSpace(e)));
                var postal = ifcOrganization.Addresses.OfType<IIfcPostalAddress>().FirstOrDefault();

                if (telecom!=null)
                {
                    target.Email = telecom.ElectronicMailAddresses.FirstOrDefault();
                    target.Phone = telecom.TelephoneNumbers.FirstOrDefault();
                }

                if (postal!=null)
                {
                    target.Department = postal.InternalLocation;
                    target.Street = postal.AddressLines != null ? postal.AddressLines.ToString() : null;
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
                    target.Categories = new List<Category>(roles.Count());
                    foreach (var role in roles)
                        target.Categories.Add(new Category { Classification = "Role", Code = role.Role.ToString(), Description = role.Description });
                }
            }

            target.Company = ifcOrganization.Name;

        }

        private void ConvertPerson(Contact target, IIfcPerson ifcPerson)
        {
            target.FamilyName = ifcPerson.FamilyName;
            target.GivenName = ifcPerson.GivenName;

            if (ifcPerson.Addresses != null)
            {
                var telecom = ifcPerson.Addresses.OfType<IIfcTelecomAddress>().FirstOrDefault();
                var postal = ifcPerson.Addresses.OfType<IIfcPostalAddress>().FirstOrDefault();
                
                if (telecom!=null)
                {
                    // todo: it looks like the Resharper ReplaceWithSingleCallToFirstOrDefault produces wrong results if accepted
                    // var ml = telecom.ElectronicMailAddresses?.Where(t => t != null && !string.IsNullOrWhiteSpace(t.ToString())).FirstOrDefault().ToString();
                    // var ml1 = telecom.ElectronicMailAddresses?.FirstOrDefault(x => x != null && !string.IsNullOrWhiteSpace(x.ToString())).ToString();

                    var ml = telecom.ElectronicMailAddresses?.Where(t => t != null && !string.IsNullOrWhiteSpace(t.ToString())).FirstOrDefault().ToString();

                    // Debug.Assert(ml == ml1);

                    if (!string.IsNullOrWhiteSpace(ml))
                        target.Email = ml;

                    // todo: it looks like the Resharper ReplaceWithSingleCallToFirstOrDefault produces wrong results if accepted
                    // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
                    var phoneNum = telecom.TelephoneNumbers?.Where(t => t!= null && !string.IsNullOrWhiteSpace(t.ToString())).FirstOrDefault().ToString();
                    if (!string.IsNullOrWhiteSpace(phoneNum))
                        target.Phone = phoneNum;
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
                    target.Categories = new List<Category>(roles.Count());
                    foreach (var role in roles)
                        target.Categories.Add(new Category { Classification = "Role", Code = role.Role.ToString(), Description = role.Description });
                }

            }

        }

        public override Contact CreateTargetObject()
        {
            return new Contact();
        }
    }
}
