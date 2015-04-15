using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Xbim.COBie.Rows
{
    [Serializable()]
    public class COBieDocumentRow : COBieRow
    {
        public COBieDocumentRow(ICOBieSheet<COBieDocumentRow> parentSheet)
            : base(parentSheet) { ExtIdentifier = IFCGuid.ToIfcGuid(Guid.NewGuid()); }

        [COBieAttributes(0, COBieKeyType.CompoundKey, "", COBieAttributeState.Required_PrimaryKey, "Name", 255, COBieAllowedType.AlphaNumeric)]
        public string Name { get; set; }

        [COBieAttributes(1, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_ForeignKey, "CreatedBy", 255, COBieAllowedType.Email)]
        public string CreatedBy { get; set; }

        [COBieAttributes(2, COBieKeyType.None, "", COBieAttributeState.Required_Information, "CreatedOn", 255, COBieAllowedType.ISODateTime)]
        public string CreatedOn { get; set; }

        [COBieAttributes(3, COBieKeyType.ForeignKey, "PickLists.DocumentType", COBieAttributeState.Required_Reference_PickList, "Category", 255, COBieAllowedType.Text)]
        public string Category { get; set; }

        [COBieAttributes(4, COBieKeyType.ForeignKey, "PickLists.ApprovalBy", COBieAttributeState.Required_Reference_PickList, "ApprovalBy", 255, COBieAllowedType.Text)]
        public string ApprovalBy { get; set; }

        [COBieAttributes(5, COBieKeyType.CompoundKey_ForeignKey, "PickLists.StageType", COBieAttributeState.Required_Reference_PickList, "Stage", 255, COBieAllowedType.Text)]
        public string Stage { get; set; }

        [COBieAttributes(6, COBieKeyType.CompoundKey_ForeignKey, "PickLists.SheetType", COBieAttributeState.Required_Reference_PickList, "SheetName", 255, COBieAllowedType.Text)]
        public string SheetName { get; set; }

        [COBieAttributes(7, COBieKeyType.CompoundKey, "", COBieAttributeState.Required_Reference_PrimaryKey, "RowName", 255, COBieAllowedType.AlphaNumeric)]
        public string RowName { get; set; }

        [COBieAttributes(8, COBieKeyType.None, "", COBieAttributeState.Required_Information, "Directory", 255, COBieAllowedType.AlphaNumeric)]
        public string Directory { get; set; }

        [COBieAttributes(9, COBieKeyType.None, "", COBieAttributeState.Required_Information, "File", 255, COBieAllowedType.AlphaNumeric)]
        public string File { get; set; }

        [COBieAttributes(10, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtSystem", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtSystem { get; set; }

        [COBieAttributes(11, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtObject { get; set; }

        [COBieAttributes(12, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtIdentifier { get; set; }

        [COBieAttributes(13, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Description", 255, COBieAllowedType.AlphaNumeric)]
        public string Description { get; set; }

        [COBieAttributes(14, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Reference", 255, COBieAllowedType.AlphaNumeric)]
        public string Reference { get; set; }
    }
}
