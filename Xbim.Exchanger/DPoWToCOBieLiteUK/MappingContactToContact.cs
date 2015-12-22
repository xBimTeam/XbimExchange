using System;
using System.Text.RegularExpressions;

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    class MappingContactToContact : MappingAttributableObjectToCOBieObject<Xbim.DPoW.Contact, Xbim.COBieLiteUK.Contact>
    {
        protected override Xbim.COBieLiteUK.Contact Mapping(Xbim.DPoW.Contact source, Xbim.COBieLiteUK.Contact target)
        {
            base.Mapping(source, target);

            target.ExternalId = source.Id.ToString();
            target.Company = source.CompanyName;
            target.Country = source.Country;
            target.Department = source.DepartmentName;
            target.FamilyName = source.FamilyName;
            target.GivenName = source.GivenName;
            target.Phone = source.PhoneNumber;
            target.PostalBox = source.PostalBox;
            target.PostalCode = source.PostCode;
            target.StateRegion = source.Region;
            target.Street = source.Street;
            target.Town = source.TownName;

            //email has to be defined because it is a key for ContactKey references. It might be made up from available information
            var email = source.Email ?? String.Format("{0}.{1}@{2}.com", source.GivenName ?? "noname", source.FamilyName ?? "nosurname", source.CompanyName ?? "nocompany").ToLower();
            //refine the result
            email = (new Regex("(\\s|\\[|\\])", RegexOptions.IgnoreCase)).Replace(email, "_").Trim('_').Trim();
            target.Email = email;
            
            return target;
        }
    }
}
