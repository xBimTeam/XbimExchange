using System.Collections.Generic;
using System.Linq;
using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.COBieLiteUK;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
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

    internal class MappingIfcActorToContact : XbimMappings<IfcStore, IModel, string, IIfcActorSelect, CobieContact>
    {
        protected override CobieContact Mapping(IIfcActorSelect actor, CobieContact target)
        {
            var helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper;
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
            target.Created = helper.GetCreatedInfo(actor, true);
            if (target.Category == null)
                target.Category = COBieExpressHelper.UnknownRole;
            
            ////Attributes
            //AttributeType[] ifcAttributes = helper.GetAttributes(actor);
            //if (ifcAttributes != null && ifcAttributes.Length > 0)
            //    ContactAttributes = new AttributeCollectionType { Attribute = ifcAttributes };
            //Documents no link available IfcActorSelect not inherited from IfcRoot


            return target;
        }


        private void ConvertOrganisation(CobieContact target, IIfcOrganization ifcOrganization)
        {
            if (ifcOrganization.Addresses != null)
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
                var role = ifcOrganization.Roles.FirstOrDefault();
                if (role != null)
                {
                    var cls = role.Role.ToString();
                    target.Category = Exchanger.TargetRepository.Instances.New<CobieRole>(r => r.Value = cls);
                }
            }

            target.Company = ifcOrganization.Name;


        }

        private void ConvertPerson(CobieContact target, IIfcPerson ifcPerson)
        {
            target.FamilyName = ifcPerson.FamilyName;
            target.GivenName = ifcPerson.GivenName;

            if (ifcPerson.Addresses != null)
            {
                var telecom = ifcPerson.Addresses.OfType<IIfcTelecomAddress>().FirstOrDefault();
                var postal = ifcPerson.Addresses.OfType<IIfcPostalAddress>().FirstOrDefault();
                
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
                var roles = ifcPerson.Roles.ToList();
                if (roles.Any())
                {
                    target.Categories = new List<Category>(roles.Count());
                    foreach (var role in roles)
                        target.Categories.Add(new Category { Classification = "Role", Code = role.Role.ToString(), Description = role.Description });
                }

            }

        }

        public override CobieContact CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieContact>();
        }
    }
}
