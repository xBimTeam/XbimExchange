using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLite;
using Xbim.DPoW;

namespace XbimExchanger.DPoW2ToCOBieLite
{
    class MappingRoleToContact: DPoWToCOBieLiteMapping<Role, ContactType>
    {
        protected override ContactType Mapping(Role source, ContactType target)
        {
            target.externalID = source.Id.ToString();
            target.ContactCategory = "Role";
            //email has to be defined because it is a key for ContactKey references
            target.ContactEmail = String.Format("{0}@role.com", source.Name ?? "noname").ToLower();
            target.ContactGivenName = source.Name;

            if (target.ContactAttributes == null) target.ContactAttributes = new AttributeCollectionType();
            target.ContactAttributes.Add("Name", "RoleName", source.Name, "RolePropertySet");
            target.ContactAttributes.Add("Description", "RoleDescription", source.Description, "RolePropertySet");
            
            return target;
        }
    }
}
