using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Xbim.COBie.Rows
{
    [Serializable()]
    public class COBieZoneRow : COBieRow
    {
        public COBieZoneRow(ICOBieSheet<COBieZoneRow> parentSheet)
            : base(parentSheet) { ExtIdentifier = IFCGuid.ToIfcGuid(Guid.NewGuid()); }

        [COBieAttributes(0, COBieKeyType.CompoundKey, "", COBieAttributeState.Required_PrimaryKey, "Name", 255, COBieAllowedType.AlphaNumeric)]
        public string Name { get; set; }

        [COBieAttributes(1, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_ForeignKey, "CreatedBy", 255, COBieAllowedType.Email)]
        public string CreatedBy { get; set; }

        [COBieAttributes(2, COBieKeyType.None, "", COBieAttributeState.Required_Information, "CreatedOn", 255, COBieAllowedType.ISODateTime)]
        public string CreatedOn { get; set; }

        [COBieAttributes(3, COBieKeyType.CompoundKey_ForeignKey, "PickLists.ZoneType", COBieAttributeState.Required_Reference_PickList, "Category", 255, COBieAllowedType.Text)]
        public string Category { get; set; }

        [COBieAttributes(4, COBieKeyType.CompoundKey_ForeignKey, "Space.Name", COBieAttributeState.Required_Multi_PrimaryKey, "SpaceNames", 255, COBieAllowedType.Text, COBieCardinality.ManyToMany)]
        public string SpaceNames { get; set; }

        [COBieAttributes(5, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtSystem", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtSystem { get; set; }

        [COBieAttributes(6, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtObject { get; set; }

        [COBieAttributes(7, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtIdentifier { get; set; }

        [COBieAttributes(8, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Description", 255, COBieAllowedType.AlphaNumeric)]
        public string Description { get; set; }
    }
}
