using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Xbim.XbimExtensions;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ActorResource;
using System.Reflection;


namespace Xbim.COBie.Rows
{
    [Serializable()]
    public class COBieContactRow : COBieRow
    {
        public COBieContactRow(ICOBieSheet<COBieContactRow> parentSheet)
            : base(parentSheet) { ExtIdentifier = IFCGuid.ToIfcGuid(Guid.NewGuid()); }    

        [COBieAttributes(0, COBieKeyType.PrimaryKey, "", COBieAttributeState.Required_PrimaryKey, "Email", 255, COBieAllowedType.AlphaNumeric)]
        public string Email { get; set; }

        [COBieAttributes(1, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_PrimaryKey, "CreatedBy", 255, COBieAllowedType.Email)]
        public string CreatedBy { get; set; }

        [COBieAttributes(2, COBieKeyType.None, "", COBieAttributeState.Required_Information, "CreatedOn", 255, COBieAllowedType.ISODateTime)]
        public string CreatedOn { get; set; }

        [COBieAttributes(3, COBieKeyType.ForeignKey, "PickLists.Category-Role", COBieAttributeState.Required_Reference_PickList, "Category", 255, COBieAllowedType.AlphaNumeric)]
        public string Category { get; set; }

        [COBieAttributes(4, COBieKeyType.None, "", COBieAttributeState.Required_Information, "Company", 255, COBieAllowedType.AlphaNumeric)]
        public string Company { get; set; }

        [COBieAttributes(5, COBieKeyType.None, "", COBieAttributeState.Required_Information, "Phone", 255, COBieAllowedType.AlphaNumeric)]
        public string Phone { get; set; }

        [COBieAttributes(6, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtSystem", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtSystem { get; set; }

        [COBieAttributes(7, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtObject { get; set; }

        [COBieAttributes(8, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtIdentifier { get; set; }

        [COBieAttributes(9, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Department", 255, COBieAllowedType.AlphaNumeric)]
        public string Department { get; set; }

        [COBieAttributes(10, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "OrganizationCode", 255, COBieAllowedType.AlphaNumeric)]
        public string OrganizationCode { get; set; }

        [COBieAttributes(11, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "GivenName", 255, COBieAllowedType.AlphaNumeric)]
        public string GivenName { get; set; }

        [COBieAttributes(12, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "FamilyName", 255, COBieAllowedType.AlphaNumeric)]
        public string FamilyName { get; set; }

        [COBieAttributes(13, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Street", 255, COBieAllowedType.AlphaNumeric)]
        public string Street { get; set; }

        [COBieAttributes(14, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "PostalBox", 255, COBieAllowedType.AlphaNumeric)]
        public string PostalBox { get; set; }

        [COBieAttributes(15, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Town", 255, COBieAllowedType.AlphaNumeric)]
        public string Town { get; set; }

        [COBieAttributes(16, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "StateRegion", 255, COBieAllowedType.AlphaNumeric)]
        public string StateRegion { get; set; }

        [COBieAttributes(17, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "PostalCode", 255, COBieAllowedType.AlphaNumeric)]
        public string PostalCode { get; set; }

        [COBieAttributes(18, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Country", 255, COBieAllowedType.AlphaNumeric)]
        public string Country { get; set; }

        
    }
}
