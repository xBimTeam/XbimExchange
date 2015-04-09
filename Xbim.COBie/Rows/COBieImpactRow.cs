using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Xbim.COBie.Rows
{
    [Serializable()]
    public class COBieImpactRow : COBieRow
    {
        public COBieImpactRow(ICOBieSheet<COBieImpactRow> parentSheet)
            : base(parentSheet) { ExtIdentifier = IFCGuid.ToIfcGuid(Guid.NewGuid()); }

        [COBieAttributes(0, COBieKeyType.CompoundKey, "", COBieAttributeState.Required_PrimaryKey, "Name", 255, COBieAllowedType.AlphaNumeric)]
        public string Name { get; set; }

        [COBieAttributes(1, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_ForeignKey, "CreatedBy", 255, COBieAllowedType.Email)]
        public string CreatedBy { get; set; }

        [COBieAttributes(2, COBieKeyType.None, "", COBieAttributeState.Required_Information, "CreatedOn", 255, COBieAllowedType.ISODateTime)]
        public string CreatedOn { get; set; }

        [COBieAttributes(3, COBieKeyType.CompoundKey_ForeignKey, "PickLists.ImpactType", COBieAttributeState.Required_Reference_PickList, "ImpactType", 255, COBieAllowedType.Text)]
        public string ImpactType { get; set; }

        [COBieAttributes(4, COBieKeyType.CompoundKey_ForeignKey, "PickLists.ImpactStage", COBieAttributeState.Required_Reference_PickList, "ImpactStage", 255, COBieAllowedType.Text)]
        public string ImpactStage { get; set; }

        [COBieAttributes(5, COBieKeyType.CompoundKey_ForeignKey, "PickLists.SheetType", COBieAttributeState.Required_Reference_PickList, "SheetName", 255, COBieAllowedType.Text)]
        public string SheetName { get; set; }

        [COBieAttributes(6, COBieKeyType.CompoundKey, "", COBieAttributeState.Required_Reference_PrimaryKey, "RowName", 255, COBieAllowedType.AlphaNumeric)]
        public string RowName { get; set; }

        [COBieAttributes(7, COBieKeyType.None, "", COBieAttributeState.Required_Information, "Value", 255, COBieAllowedType.Numeric)]
        public string Value { get; set; }

        [COBieAttributes(8, COBieKeyType.ForeignKey, "PickLists.ImpactUnit", COBieAttributeState.Required_Reference_PickList, "ImpactUnit", 255, COBieAllowedType.Text)]
        public string ImpactUnit { get; set; }

        [COBieAttributes(9, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "LeadInTime", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string LeadInTime { get; set; }

        [COBieAttributes(10, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Duration", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string Duration { get; set; }

        [COBieAttributes(11, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "LeadOutTime", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string LeadOutTime { get; set; }

        [COBieAttributes(12, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtSystem", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtSystem { get; set; }

        [COBieAttributes(13, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtObject { get; set; }

        [COBieAttributes(14, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtIdentifier { get; set; }

        [COBieAttributes(15, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Description", 255, COBieAllowedType.AlphaNumeric)]
        public string Description { get; set; }
    }
}
