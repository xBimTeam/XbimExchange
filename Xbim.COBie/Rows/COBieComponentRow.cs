using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Xbim.Ifc2x3.ProductExtension;

namespace Xbim.COBie.Rows
{
    [Serializable()]
    public class COBieComponentRow : COBieRow
    {
        public COBieComponentRow(ICOBieSheet<COBieComponentRow> parentSheet)
            : base(parentSheet) { ExtIdentifier = IFCGuid.ToIfcGuid(Guid.NewGuid()); }

        

        #region Attributes Properties

        [COBieAttributes(0, COBieKeyType.PrimaryKey, "", COBieAttributeState.Required_PrimaryKey, "Name", 255, COBieAllowedType.AlphaNumeric)]
        public string Name { get; set; }

        [COBieAttributes(1, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_ForeignKey, "CreatedBy", 255, COBieAllowedType.Email)]
        public string CreatedBy { get; set; }

        [COBieAttributes(2, COBieKeyType.None, "", COBieAttributeState.Required_Information, "CreatedOn", 255, COBieAllowedType.ISODateTime)]
        public string CreatedOn { get; set; }

        [COBieAttributes(3, COBieKeyType.ForeignKey, "Type.Name", COBieAttributeState.Required_Reference_PrimaryKey, "TypeName", 255, COBieAllowedType.AlphaNumeric)]
        public string TypeName { get; set; }

        [COBieAttributes(4, COBieKeyType.ForeignKey, "Space.Name", COBieAttributeState.Required_Reference_PrimaryKey, "Space", 255, COBieAllowedType.AlphaNumeric, COBieCardinality.ManyToMany)]
        public string Space { get; set; }

        [COBieAttributes(5, COBieKeyType.None, "", COBieAttributeState.Required_Information, "Description", 255, COBieAllowedType.AlphaNumeric)]
        public string Description { get; set; }

        [COBieAttributes(6, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtSystem", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtSystem { get; set; }

        [COBieAttributes(7, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtObject { get; set; }

        [COBieAttributes(8, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtIdentifier { get; set; }

        [COBieAttributes(9, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "SerialNumber", 255, COBieAllowedType.AlphaNumeric)]
        public string SerialNumber { get; set; }

        [COBieAttributes(10, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "InstallationDate", 19, COBieAllowedType.ISODate)]
        public string InstallationDate { get; set; }

        [COBieAttributes(11, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "WarrantyStartDate", 19, COBieAllowedType.ISODate)]
        public string WarrantyStartDate { get; set; }

        [COBieAttributes(12, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "TagNumber", 255, COBieAllowedType.AlphaNumeric)]
        public string TagNumber { get; set; }

        [COBieAttributes(13, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "BarCode", 255, COBieAllowedType.AlphaNumeric)]
        public string BarCode { get; set; }

        [COBieAttributes(14, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "AssetIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string AssetIdentifier { get; set; }

        #endregion
    }
}
