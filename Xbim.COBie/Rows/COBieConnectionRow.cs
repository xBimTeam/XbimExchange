using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Xbim.COBie.Rows
{
    [Serializable()]
    public class COBieConnectionRow : COBieRow
    {
        public COBieConnectionRow(ICOBieSheet<COBieConnectionRow> parentSheet)
            : base(parentSheet) { ExtIdentifier = IFCGuid.ToIfcGuid(Guid.NewGuid()); }

        [COBieAttributes(0, COBieKeyType.CompoundKey, "", COBieAttributeState.Required_PrimaryKey, "Name", 255, COBieAllowedType.AlphaNumeric)]
        public string Name { get; set; }

        [COBieAttributes(1, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_ForeignKey, "CreatedBy", 255, COBieAllowedType.Email)]
        public string CreatedBy { get; set; }

        [COBieAttributes(2, COBieKeyType.None, "", COBieAttributeState.Required_Information, "CreatedOn", 255, COBieAllowedType.ISODateTime)]
        public string CreatedOn { get; set; }

        [COBieAttributes(3, COBieKeyType.CompoundKey_ForeignKey, "PickLists.ConnectionType", COBieAttributeState.Required_Reference_PickList, "ConnectionType", 255, COBieAllowedType.Text)]
        public string ConnectionType { get; set; }

        [COBieAttributes(4, COBieKeyType.CompoundKey_ForeignKey, "PickLists.SheetType", COBieAttributeState.Required_Reference_PickList, "SheetName", 255, COBieAllowedType.Text)]
        public string SheetName { get; set; }

        [COBieAttributes(5, COBieKeyType.CompoundKey_ForeignKey, "Component.Name", COBieAttributeState.Required_Reference_PrimaryKey, "RowName1", 255, COBieAllowedType.Text)]
        public string RowName1 { get; set; }

        [COBieAttributes(6, COBieKeyType.CompoundKey_ForeignKey, "Component.Name", COBieAttributeState.Required_Reference_PrimaryKey, "RowName2", 255, COBieAllowedType.Text)]
        public string RowName2 { get; set; }

        [COBieAttributes(7, COBieKeyType.None, "", COBieAttributeState.Required_Reference_PrimaryKey, "RealizingElement", 255, COBieAllowedType.Text)]
        public string RealizingElement { get; set; }

        [COBieAttributes(8, COBieKeyType.None, "", COBieAttributeState.Required_Reference_ForeignKey, "PortName1", 255, COBieAllowedType.Text)]
        public string PortName1 { get; set; }

        [COBieAttributes(9, COBieKeyType.None, "", COBieAttributeState.Required_Reference_ForeignKey, "PortName2", 255, COBieAllowedType.Text)]
        public string PortName2 { get; set; }

        [COBieAttributes(10, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtSystem", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtSystem { get; set; }

        [COBieAttributes(11, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtObject { get; set; }

        [COBieAttributes(12, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtIdentifier { get; set; }

        [COBieAttributes(13, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Description", 255, COBieAllowedType.AlphaNumeric)]
        public string Description { get; set; }
    }
}
