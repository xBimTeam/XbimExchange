using System;
using System.Linq;
using Xbim.CobieLiteUk;
using Xbim.Ifc2x3.ActorResource;

namespace XbimExchanger.COBieLiteUkToIfc
{
    internal class MappingContactToIfcPersonAndOrganization : CoBieLiteUkIfcMappings<string, Contact, IfcPersonAndOrganization>
    {
        protected override IfcPersonAndOrganization Mapping(Contact contact, IfcPersonAndOrganization ifcPersonAndOrganization)
        {
            IfcPerson ifcPerson = Exchanger.TargetRepository.Instances.New<IfcPerson>();
            IfcOrganization ifcOrganization = Exchanger.TargetRepository.Instances.New<IfcOrganization>();
            //add Organization code
            if (Exchanger.StringHasValue(contact.OrganizationCode))
                ifcOrganization.Id = contact.OrganizationCode;
            if (Exchanger.StringHasValue(contact.Company))
            {
                ifcOrganization.Name = contact.Company;
            }
           
            //add GivenName
            if (Exchanger.StringHasValue(contact.GivenName))
                ifcPerson.GivenName = contact.GivenName;
            //add Family Name
            if (Exchanger.StringHasValue(contact.FamilyName))
                ifcPerson.FamilyName = contact.FamilyName;

            if (contact.Categories != null && contact.Categories.Any())
            {
                SetRoles(contact, ifcPerson);
            }
            
            //add addresses into IfcPerson object
            //add Telecom Address
            IfcTelecomAddress ifcTelecomAddress = SetTelecomAddress(contact);
            ifcPerson.Addresses.Add(ifcTelecomAddress);//add to existing collection
            
            // Add postal address
            IfcPostalAddress ifcPostalAddress = SetAddress(contact);
            ifcPerson.Addresses.Add(ifcPostalAddress);//add to existing collection

            //add the person and the organization objects 
            ifcPersonAndOrganization.ThePerson = ifcPerson;
            ifcPersonAndOrganization.TheOrganization = ifcOrganization;
            return ifcPersonAndOrganization;
        }

        private IfcPostalAddress SetAddress(Contact contact)
        {
            IfcPostalAddress ifcPostalAddress = Exchanger.TargetRepository.Instances.New<IfcPostalAddress>();
            //add Department
            if (Exchanger.StringHasValue(contact.Department))
                ifcPostalAddress.InternalLocation = contact.Department;
            //add Street
            if (Exchanger.StringHasValue(contact.Street))
            {
                foreach (var line in contact.Street.Split(new char[] { ',', ':' }))
                {
                    ifcPostalAddress.AddressLines.Add(line);
                }
            }

            //add PostalBox
            if (Exchanger.StringHasValue(contact.PostalBox))
                ifcPostalAddress.PostalBox = contact.PostalBox;
            //add Town
            if (Exchanger.StringHasValue(contact.Town))
                ifcPostalAddress.Town = contact.Town;
            //add StateRegion
            if (Exchanger.StringHasValue(contact.StateRegion))
                ifcPostalAddress.Region = contact.StateRegion;
            //add PostalCode
            if (Exchanger.StringHasValue(contact.PostalCode))
                ifcPostalAddress.PostalCode = contact.PostalCode;
            //add Country
            if (Exchanger.StringHasValue(contact.Country))
                ifcPostalAddress.Country = contact.Country;

            return ifcPostalAddress;
        }

        private IfcTelecomAddress SetTelecomAddress(Contact contact)
        {
            IfcTelecomAddress ifcTelecomAddress = Exchanger.TargetRepository.Instances.New<IfcTelecomAddress>();
            //add email
            if (Exchanger.StringHasValue(contact.Email))
            {
                    ifcTelecomAddress.ElectronicMailAddresses.Add(contact.Email); //add to existing collection
            }

            //add Phone
            if (Exchanger.StringHasValue(contact.Phone))
            {
               ifcTelecomAddress.TelephoneNumbers.Add(contact.Phone);//add to existing collection
            }
            return ifcTelecomAddress;
        }

        private void SetRoles(Contact contact, IfcActorSelect actor)
        {
            IfcActorRole ifcActorRole = null;
            IfcPerson ifcPerson = (actor is IfcPerson) ? actor as IfcPerson : null;
            IfcOrganization ifcOrganization = (actor is IfcOrganization) ? actor as IfcOrganization : null;
            //IfcPersonAndOrganization ifcPersonAndOrganization = (actor is IfcPersonAndOrganization) ? actor as IfcPersonAndOrganization : null;
            //does not handle IfcPersonAndOrganization although it is a IfcActorSelect
            if (actor is IfcPersonAndOrganization)
            {
                throw new ArgumentException("Expecting IfcPerson or IfcOrganization");
            }
            //swap categories into Roles
            foreach (var item in contact.Categories)
            {
                if (Exchanger.StringHasValue(item.Code))
                {
                    ifcActorRole = Exchanger.TargetRepository.Instances.New<IfcActorRole>();
                    ifcActorRole.RoleString = item.Code;
                    ifcActorRole.Description = item.Description;
                    if (ifcPerson != null)
                    {
                       ifcPerson.Roles.Add(ifcActorRole);//add to existing collection                     
                    }
                    if (ifcOrganization != null)
                    {
                        ifcOrganization.Roles.Add(ifcActorRole);//add to existing collection                        
                    }
                    //if (ifcPersonAndOrganization != null)
                    //{
                    //    if (ifcPersonAndOrganization.Roles == null)
                    //    {
                    //        ifcPersonAndOrganization.SetRoles(ifcActorRole);//create the ActorRoleCollection and set to Roles field
                    //    }
                    //    else
                    //    {
                    //        ifcPersonAndOrganization.Roles.Add(ifcActorRole);//add to existing collection
                    //    }
                    //}
                }
               
            }
        }
    }
}