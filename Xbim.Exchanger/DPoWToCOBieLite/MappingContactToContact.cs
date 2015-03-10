using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW;

namespace XbimExchanger.DPoWToCOBieLite
{
    class MappingContactToContact : MappingAttributableObjectToCOBieObject<Contact, ContactType>
    {
        protected override ContactType Mapping(Contact source, ContactType target)
        {
            base.Mapping(source, target);

            target.externalID = source.Id.ToString();
            target.ContactCompanyName = source.CompanyName;
            target.ContactCountryName = source.Country;
            target.ContactDepartmentName = source.DepartmentName;
            target.ContactFamilyName = source.FamilyName;
            target.ContactGivenName = source.GivenName;
            target.ContactPhoneNumber = source.PhoneNumber;
            target.ContactPostalBoxNumber = source.PostalBox;
            target.ContactPostalCode = source.PostCode;
            target.ContactRegionCode = source.Region;
            target.ContactStreet = source.Street;
            target.ContactTownName = source.TownName;
            target.ContactURL = source.URL;

            //email has to be defined because it is a key for ContactKey references. It might be made up from available information
            var email = source.Email ?? String.Format("{0}.{1}@{2}.com", source.GivenName ?? "noname", source.FamilyName ?? "nosurname", source.CompanyName ?? "nocompany").ToLower();
            //refine the result
            email = (new Regex("(\\s|\\[|\\])", RegexOptions.IgnoreCase)).Replace(email, "_").Trim('_').Trim();
            target.ContactEmail = email;
            
            return target;
        }
    }
}
