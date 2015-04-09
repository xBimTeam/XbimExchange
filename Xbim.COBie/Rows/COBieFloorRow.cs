using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.XbimExtensions;
using Xbim.Ifc2x3.ProductExtension;
using System.Reflection;

namespace Xbim.COBie.Rows
{
    [Serializable()]
    public class COBieFloorRow : COBieRow
    {
        public COBieFloorRow(ICOBieSheet<COBieFloorRow> parentSheet)
            : base(parentSheet) { ExtIdentifier = IFCGuid.ToIfcGuid(Guid.NewGuid()); }

        [COBieAttributes(0, COBieKeyType.PrimaryKey, "", COBieAttributeState.Required_PrimaryKey, "Name", 255, COBieAllowedType.AlphaNumeric)]
        public string Name { get; set; }

        [COBieAttributes(1, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_ForeignKey, "CreatedBy", 255, COBieAllowedType.Email)]
        public string CreatedBy { get; set; }

        [COBieAttributes(2, COBieKeyType.None, "", COBieAttributeState.Required_Information, "CreatedOn", 255, COBieAllowedType.ISODateTime)]
        public string CreatedOn { get; set; }


        [COBieAttributes(3, COBieKeyType.ForeignKey, "PickLists.FloorType", COBieAttributeState.Required_Reference_PickList, "Category", 255, COBieAllowedType.AlphaNumeric)]
        public string Category { get; set; }

        [COBieAttributes(4, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtSystem", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtSystem { get; set; }

        [COBieAttributes(5, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtObject { get; set; }

        [COBieAttributes(6, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtIdentifier { get; set; }

        [COBieAttributes(7, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Description", 255, COBieAllowedType.AlphaNumeric)]
        public string Description { get; set; }

        [COBieAttributes(8, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Elevation", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string Elevation { get; set; }

        [COBieAttributes(9, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Height", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string Height { get; set; }
    }
}
