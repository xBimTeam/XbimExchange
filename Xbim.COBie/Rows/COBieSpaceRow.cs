using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.XbimExtensions;
using System.Reflection;

namespace Xbim.COBie.Rows
{
    [Serializable()]
    public class COBieSpaceRow : COBieRow
    {
        public COBieSpaceRow(ICOBieSheet<COBieSpaceRow> parentSheet)
            : base(parentSheet) { ExtIdentifier = IFCGuid.ToIfcGuid(Guid.NewGuid()); }

        [COBieAttributes(0, COBieKeyType.PrimaryKey, "", COBieAttributeState.Required_PrimaryKey, "Name", 255, COBieAllowedType.AlphaNumeric)]
        public string Name { get; set; }

        [COBieAttributes(1, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_ForeignKey, "CreatedBy", 255, COBieAllowedType.Email)]
        public string CreatedBy { get; set; }

        [COBieAttributes(2, COBieKeyType.None, "", COBieAttributeState.Required_Information, "CreatedOn", 255, COBieAllowedType.ISODateTime)]
        public string CreatedOn { get; set; }

        [COBieAttributes(3, COBieKeyType.ForeignKey, "PickLists.Category-Space", COBieAttributeState.Required_Reference_PickList, "Category", 255, COBieAllowedType.AlphaNumeric)]
        public string Category { get; set; }

        [COBieAttributes(4, COBieKeyType.ForeignKey, "Floor.Name", COBieAttributeState.Required_Reference_ForeignKey, "FloorName", 255, COBieAllowedType.AlphaNumeric)]
        public string FloorName { get; set; }

        [COBieAttributes(5, COBieKeyType.None, "", COBieAttributeState.Required_Information, "Description", 255, COBieAllowedType.AlphaNumeric)]
        public string Description { get; set; }

        [COBieAttributes(6, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtSystem", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtSystem { get; set; }

        [COBieAttributes(7, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtObject { get; set; }

        [COBieAttributes(8, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtIdentifier { get; set; }

        [COBieAttributes(9, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "RoomTag", 255, COBieAllowedType.AlphaNumeric)]
        public string RoomTag { get; set; }

        [COBieAttributes(10, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "UsableHeight", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string UsableHeight { get; set; }

        [COBieAttributes(11, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "GrossArea", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string GrossArea { get; set; }

        [COBieAttributes(12, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "NetArea", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string NetArea { get; set; }
    }
}
