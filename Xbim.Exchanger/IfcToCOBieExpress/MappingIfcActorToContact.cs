using System;
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
        private COBieExpressHelper _helper;

        public COBieExpressHelper Helper
        {
            get { return _helper ?? (_helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper); }
        }

        protected override CobieContact Mapping(IIfcActorSelect actor, CobieContact target)
        {
            
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
               target.Email = string.Format("unknown{0}@undefined.email", actor.EntityLabel);
            }
            target.Created = Helper.GetCreatedInfo(actor, true);
            if (target.Category == null)
                target.Category = Helper.UnknownRole;
            
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
                    target.Street = postal.AddressLines != null && postal.AddressLines.Any()? string.Join(", ", postal.AddressLines) : null;
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

            if (string.IsNullOrWhiteSpace(target.Email))
                target.Email = string.Format("unknown{0}@undefined.email", ifcOrganization.EntityLabel);

            target.Company = ifcOrganization.Name;


        }

        private void ConvertPerson(CobieContact target, IIfcPerson ifcPerson)
        {
            target.FamilyName = ifcPerson.FamilyName;
            target.GivenName = ifcPerson.GivenName;

            if (ifcPerson.Addresses != null)
            {
                var telecom = ifcPerson.Addresses.OfType<IIfcTelecomAddress>().FirstOrDefault(a => a.ElectronicMailAddresses.Any(e => !string.IsNullOrWhiteSpace(e)));
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
                    target.Street = postal.AddressLines != null && postal.AddressLines.Any() ? string.Join(", ", postal.AddressLines) : null;
                   
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

            if (string.IsNullOrWhiteSpace(target.Email))
                target.Email = string.Format("unknown{0}@undefined.email", ifcPerson.EntityLabel);

            if (ifcPerson.Roles == null) return;
            var role = ifcPerson.Roles.FirstOrDefault();
            if (role == null) return;
            var catString = role.UserDefinedRole ?? Enum.GetName(typeof (IfcRoleEnum), role.Role);
            target.Category = Helper.GetPickValue<CobieRole>(catString);
        }

        public override CobieContact CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieContact>();
        }
    }
}
