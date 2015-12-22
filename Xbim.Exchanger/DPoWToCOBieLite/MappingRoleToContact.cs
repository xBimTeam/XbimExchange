using System;
using System.Text.RegularExpressions;
using Xbim.COBieLite;
using Xbim.DPoW;

namespace XbimExchanger.DPoWToCOBieLite
{
    class MappingRoleToContact : MappingAttributableObjectToCOBieObject<Role, ContactType>
    {
        protected override ContactType Mapping(Role source, ContactType target)
        {
            base.Mapping(source, target);

            target.externalID = source.Id.ToString();
            target.ContactCategory = "Role";
            target.ContactGivenName = source.Name;

            //email has to be defined because it is a key for ContactKey references
            var email = String.Format("{0}@role.com", source.Name ?? "noname").ToLower();
            email = (new Regex("(\\s|\\[|\\])", RegexOptions.IgnoreCase)).Replace(email, "_").Trim('_').Trim();
            target.ContactEmail = email;

            if (target.ContactAttributes == null) target.ContactAttributes = new AttributeCollectionType();
            target.ContactAttributes.Add("Name", "RoleName", source.Name, "RolePropertySet");
            target.ContactAttributes.Add("Description", "RoleDescription", source.Description, "RolePropertySet");
            
            return target;
        }
    }
}
