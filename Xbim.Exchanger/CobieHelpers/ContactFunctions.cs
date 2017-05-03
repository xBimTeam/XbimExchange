using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.CobieHelpers
{
    public static class ContactFunctions
    {
        public static int GetPostal(IEnumerable<IIfcAddress> addresses, out string department, out string street, 
            out string postalBox, out string town, out string stateRegion, out string postalCode )
        {
            department = null;
            street = null;
            postalBox = null;
            town = null;
            stateRegion = null;
            postalCode = null;

            var filledCount = 0;

            var postals = addresses?.OfType<IIfcPostalAddress>();
            if (postals == null)
                return 0;
            foreach (var postal in postals)
            {
                
                if (department == null && postal.InternalLocation.HasValue)
                {
                    var tmp = postal.InternalLocation;
                    if (!string.IsNullOrWhiteSpace(tmp))
                    {
                        department = tmp;
                        filledCount++;
                    }
                }
                
                if (street == null && postal.AddressLines != null)
                {
                    var address = string.Join(", ", postal.AddressLines.ToArray());
                    if (!string.IsNullOrWhiteSpace(address))
                    {
                        street = address;
                        filledCount++;
                    }
                }

                if (postalBox == null && !string.IsNullOrWhiteSpace(postal.PostalBox))
                {
                    postalBox = postal.PostalBox;
                    filledCount++;
                }
                if (town == null && !string.IsNullOrWhiteSpace(postal.Town))
                {
                    town = postal.Town;
                }
                if (stateRegion == null && !string.IsNullOrWhiteSpace(postal.Region))
                {
                    stateRegion = postal.Region;
                }
                if (postalCode == null && !string.IsNullOrWhiteSpace(postal.PostalCode))
                {
                    postalCode = postal.PostalCode;
                }

                // exit enum if no more fields needed.
                if (filledCount == 6)
                    return filledCount;
            }
            return filledCount;
        }

        public static string EmailAddressOf(IIfcActorSelect personOrg)
        {
            IIfcPerson person = null;
            IIfcOrganization organisation = null;
            if (personOrg is IIfcPerson)
            {
                person = personOrg as IIfcPerson;
            }
            if (personOrg is IIfcOrganization)
            {
                organisation = personOrg as IIfcOrganization;
            }
            if (personOrg is IIfcPersonAndOrganization)
            {
                person = (personOrg as IIfcPersonAndOrganization).ThePerson;
                organisation = (personOrg as IIfcPersonAndOrganization).TheOrganization;
            }

            string email;
            string phone; // not used

            // priority goes to person
            if (person != null)
            {
                GetTelecom(person.Addresses, out email, out phone);
                if (email != null)
                    return email;
            }
            if (organisation != null)
            {
                GetTelecom(organisation.Addresses, out email, out phone);
                if (email != null)
                    return email;
            }
            return DefaultUniqueEmail(personOrg);
        }

        public static int GetTelecom(IEnumerable<IIfcAddress> addresses, out string email, out string phone)
        {
            email = null;
            phone = null;
            var filledCount = 0;
            var telecoms = addresses?.OfType<IIfcTelecomAddress>();
            if (telecoms == null)
                return 0;
            foreach (var telecom in telecoms)
            {

                if (email == null && telecom.ElectronicMailAddresses != null)
                {
                    // todo: it looks like the Resharper ReplaceWithSingleCallToFirstOrDefault produces wrong results if accepted
                    // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
                    var ml = telecom.ElectronicMailAddresses.Where(
                            t => t != null && !string.IsNullOrWhiteSpace(t.ToString()))
                            .FirstOrDefault().ToString();
                    if (!string.IsNullOrWhiteSpace(ml))
                    {
                        email = ml;
                        filledCount++;
                    }
                }

                if (phone == null && telecom.TelephoneNumbers != null)
                {
                    // todo: it looks like the Resharper ReplaceWithSingleCallToFirstOrDefault produces wrong results if accepted
                    // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
                    var phoneNum = telecom.TelephoneNumbers.Where(
                            t => t != null && !string.IsNullOrWhiteSpace(t.ToString()))
                            .FirstOrDefault()
                            .ToString();
                    if (!string.IsNullOrWhiteSpace(phoneNum))
                    {
                        phone = phoneNum;
                        filledCount++;
                    }
                }
                // exit enum if no more fields needed.
                if (filledCount == 2)
                    return filledCount;
            }
            return filledCount;
        }

        public static string DefaultUniqueEmail(IIfcActorSelect actor)
        {
            return string.Format("unknown{0}@undefined.email", actor.EntityLabel);
        }
    }
}
