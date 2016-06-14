using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xbim.CobieLiteUk;
using Xbim.DPoW;
using Attribute = Xbim.CobieLiteUk.Attribute;

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    class MappingRoleToContact : MappingAttributableObjectToCOBieObject<Role, Xbim.CobieLiteUk.Contact>
    {
        protected override Xbim.CobieLiteUk.Contact Mapping(Role source, Xbim.CobieLiteUk.Contact target)
        {
            base.Mapping(source, target);

            target.ExternalId = source.Id.ToString();
            target.ExternalSystem = "DPoW";
            var code = String.Format("Role: {0}", source.Name ?? "");
            if (target.Categories == null)
                target.Categories = new List<Category>();
            target.Categories.Add(new Category { Code = code, Classification = "DPoW"});
            target.GivenName = source.Name;

            //email has to be defined because it is a key for ContactKey references
            var email = String.Format("{0}@role.com", source.Name ?? "noname").ToLower();
            email = (new Regex("(\\s|\\[|\\])", RegexOptions.IgnoreCase)).Replace(email, "_").Trim('_').Trim();
            target.Email = email;

            if (target.Attributes == null) target.Attributes = new List<Attribute>();
            target.Attributes.Add("Name", "RoleName", source.Name, "RolePropertySet");
            target.Attributes.Add("Description", "RoleDescription", source.Description, "RolePropertySet");
            
            return target;
        }
    }
}
