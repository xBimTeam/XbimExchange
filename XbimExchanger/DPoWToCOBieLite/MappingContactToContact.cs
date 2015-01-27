using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.DPoWToCOBieLite
{
    public class MappingContactToContact : DPoWToCOBieLiteMapping<Contact, ContactType>
    {
        public override ContactType CreateTargetObject()
        {
            var obj = base.CreateTargetObject();
            obj.externalID = Exchanger.GetStringIdentifier();
            return obj;
        }
        protected override ContactType Mapping(Contact source, ContactType target)
        {
            target.externalID = Exchanger.GetStringIdentifier();
            target.ContactCompanyName = source.ContactCompanyName;
            target.ContactCountryName = source.ContactCountry;
            target.ContactDepartmentName = source.ContactDepartmentName;
            target.ContactEmail = source.ContactEmail;
            target.ContactFamilyName = source.ContactFamilyName;
            target.ContactGivenName = source.ContactGivenName;
            target.ContactPhoneNumber = source.ContactPhoneNumber;
            target.ContactPostalBoxNumber = source.ContactPostalBox;
            target.ContactPostalCode = source.ContactPostCode;
            target.ContactRegionCode = source.ContactRegion;
            target.ContactStreet = source.ContactStreet;
            target.ContactTownName = source.ContactTownName;
            target.ContactURL = source.ContactURL;

            //prepare attributes collection for the case it is necessary.
            target.ContactAttributes = new AttributeCollectionType();

            return target;
        }

        /// <summary>
        /// Creates string identifier of the contact from email, name and surname of contact. This should be used as a key for mappings
        /// </summary>
        /// <param name="contact">Input contact</param>
        /// <returns>Identifier to be used as a key for mappings</returns>
        public static string GetKey(Contact contact)
        {
            return contact.GetHashCode().ToString();
            //return String.Format("{0} {1} {2} {3}", contact.ContactEmail, contact.ContactFamilyName, contact.ContactGivenName, contact.ContactCompanyName);
        }
    }
}
