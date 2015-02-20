using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW;

namespace XbimExchanger.DPoW2ToCOBieLite
{
    class MappingContactToContact : DPoWToCOBieLiteMapping<Contact, ContactType>
    {
        public override ContactType CreateTargetObject()
        {
            var obj = base.CreateTargetObject();
            obj.externalID = Exchanger.GetStringIdentifier();
            return obj;
        }
        protected override ContactType Mapping(Contact source, ContactType target)
        {
            target.externalID = source.Id.ToString();
            target.ContactCompanyName = source.CompanyName;
            target.ContactCountryName = source.Country;
            target.ContactDepartmentName = source.DepartmentName;
            //email has to be defined because it is a key for ContactKey references. It might be made up from available information
            target.ContactEmail = source.Email ?? String.Format("{0}.{1}@{2}.com", source.GivenName ?? "noname", source.FamilyName ?? "nosurname", source.CompanyName ?? "nocompany").ToLower();
            target.ContactFamilyName = source.FamilyName;
            target.ContactGivenName = source.GivenName;
            target.ContactPhoneNumber = source.PhoneNumber;
            target.ContactPostalBoxNumber = source.PostalBox;
            target.ContactPostalCode = source.PostCode;
            target.ContactRegionCode = source.Region;
            target.ContactStreet = source.Street;
            target.ContactTownName = source.TownName;
            target.ContactURL = source.URL;

            return target;
        }
    }
}
