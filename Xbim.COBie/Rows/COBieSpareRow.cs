using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Xbim.COBie.Rows
{
    [Serializable()]
    public class COBieSpareRow : COBieRow
    {
        public COBieSpareRow(ICOBieSheet<COBieSpareRow> parentSheet)
            : base(parentSheet) { ExtIdentifier = IFCGuid.ToIfcGuid(Guid.NewGuid()); }

        [COBieAttributes(0, COBieKeyType.PrimaryKey, "", COBieAttributeState.Required_PrimaryKey, "Name", 255, COBieAllowedType.AlphaNumeric)]
        public string Name { get; set; }

        [COBieAttributes(1, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_ForeignKey, "CreatedBy", 255, COBieAllowedType.Email)]
        public string CreatedBy { get; set; }

        [COBieAttributes(2, COBieKeyType.None, "", COBieAttributeState.Required_Information, "CreatedOn", 255, COBieAllowedType.ISODateTime)]
        public string CreatedOn { get; set; }

        [COBieAttributes(3, COBieKeyType.ForeignKey, "PickLists.SpareType", COBieAttributeState.Required_Reference_PickList, "Category", 255, COBieAllowedType.Text)]
        public string Category { get; set; }

        [COBieAttributes(4, COBieKeyType.ForeignKey, "Type.Name", COBieAttributeState.Required_Reference_PrimaryKey, "TypeName", 255, COBieAllowedType.Text)]
        public string TypeName { get; set; }

        [COBieAttributes(5, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_ForeignKey, "Suppliers", 255, COBieAllowedType.Email, COBieCardinality.ManyToMany)]
        public string Suppliers { get; set; }

        [COBieAttributes(6, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtSystem", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtSystem { get; set; }

        [COBieAttributes(7, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtObject { get; set; }

        [COBieAttributes(8, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtIdentifier { get; set; }

        [COBieAttributes(9, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Description", 255, COBieAllowedType.AlphaNumeric)]
        public string Description { get; set; }

        [COBieAttributes(10, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "SetNumber", 255, COBieAllowedType.AlphaNumeric)]
        public string SetNumber { get; set; }

        [COBieAttributes(11, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "PartNumber", 255, COBieAllowedType.AlphaNumeric)]
        public string PartNumber { get; set; }
    }
}
