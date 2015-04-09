using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.XbimExtensions;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.UtilityResource;
using Xbim.XbimExtensions.SelectTypes;
using System.Reflection;

namespace Xbim.COBie.Rows
{
    [Serializable()]
    public class COBieFacilityRow : COBieRow
    {
        public COBieFacilityRow(ICOBieSheet<COBieFacilityRow> parentSheet)
            : base(parentSheet) { ExternalFacilityIdentifier = IFCGuid.ToIfcGuid(Guid.NewGuid()); ExternalProjectIdentifier = IFCGuid.ToIfcGuid(Guid.NewGuid()); ExternalSiteIdentifier = IFCGuid.ToIfcGuid(Guid.NewGuid()); }

        [COBieAttributes(0, COBieKeyType.PrimaryKey, "", COBieAttributeState.Required_PrimaryKey, "Name", 255, COBieAllowedType.AlphaNumeric)]
        public string Name { get; set; }

        [COBieAttributes(1, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_ForeignKey, "CreatedBy", 255, COBieAllowedType.Email)]
        public string CreatedBy { get; set; }

        [COBieAttributes(2, COBieKeyType.None, "", COBieAttributeState.Required_Information, "CreatedOn", 255, COBieAllowedType.ISODateTime)]
        public string CreatedOn { get; set; }

        [COBieAttributes(3, COBieKeyType.ForeignKey, "PickLists.Category-Facility", COBieAttributeState.Required_Reference_PickList, "Category", 255, COBieAllowedType.Text)]
        public string Category { get; set; }

        [COBieAttributes(4, COBieKeyType.None, "", COBieAttributeState.Required_Information, "ProjectName", 255, COBieAllowedType.AlphaNumeric)]
        public string ProjectName { get; set; }

        [COBieAttributes(5, COBieKeyType.None, "", COBieAttributeState.Required_Information, "SiteName", 255, COBieAllowedType.AlphaNumeric)]
        public string SiteName { get; set; }

        [COBieAttributes(6, COBieKeyType.ForeignKey, "PickLists.LinearUnit", COBieAttributeState.Required_Reference_PickList, "LinearUnits", 255, COBieAllowedType.Text)]
        public string LinearUnits { get; set; }

        [COBieAttributes(7, COBieKeyType.ForeignKey, "PickLists.AreaUnit", COBieAttributeState.Required_Reference_PickList, "AreaUnits", 255, COBieAllowedType.Text)]
        public string AreaUnits { get; set; }

        [COBieAttributes(8, COBieKeyType.ForeignKey, "PickLists.VolumeUnit", COBieAttributeState.Required_Reference_PickList, "VolumeUnits", 255, COBieAllowedType.Text)]
        public string VolumeUnits { get; set; }

        [COBieAttributes(9, COBieKeyType.ForeignKey, "PickLists.CostUnit", COBieAttributeState.Required_Reference_PickList, "CurrencyUnit", 255, COBieAllowedType.Text)]
        public string CurrencyUnit { get; set; }

        [COBieAttributes(10, COBieKeyType.None, "", COBieAttributeState.Required_Information, "AreaMeasurement", 255, COBieAllowedType.AlphaNumeric)]
        public string AreaMeasurement { get; set; }

        [COBieAttributes(11, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExternalSystem", 255, COBieAllowedType.AlphaNumeric)]
        public string ExternalSystem { get; set; }

        [COBieAttributes(12, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExternalProjectObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExternalProjectObject { get; set; }

        [COBieAttributes(13, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExternalProjectIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExternalProjectIdentifier { get; set; }

        [COBieAttributes(14, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExternalSiteObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExternalSiteObject { get; set; }

        [COBieAttributes(15, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExternalSiteIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExternalSiteIdentifier { get; set; }

        [COBieAttributes(16, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExternalFacilityObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExternalFacilityObject { get; set; }

        [COBieAttributes(17, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExternalFacilityIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExternalFacilityIdentifier { get; set; }

        [COBieAttributes(18, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Description", 255, COBieAllowedType.AlphaNumeric)]
        public string Description { get; set; }

        [COBieAttributes(19, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "ProjectDescription", 255, COBieAllowedType.AlphaNumeric)]
        public string ProjectDescription { get; set; }

        [COBieAttributes(20, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "SiteDescription", 255, COBieAllowedType.AlphaNumeric)]
        public string SiteDescription { get; set; }

        [COBieAttributes(21, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Phase", 255, COBieAllowedType.AlphaNumeric)]
        public string Phase { get; set; }

        
    }

    
}
