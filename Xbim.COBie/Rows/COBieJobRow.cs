using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Xbim.COBie.Rows
{
    [Serializable()]
    public class COBieJobRow : COBieRow
    {
        public COBieJobRow(ICOBieSheet<COBieJobRow> parentSheet)
            : base(parentSheet) { ExtIdentifier = IFCGuid.ToIfcGuid(Guid.NewGuid()); }

        [COBieAttributes(0, COBieKeyType.CompoundKey, "", COBieAttributeState.Required_PrimaryKey, "Name", 255, COBieAllowedType.AlphaNumeric)]
        public string Name { get; set; }

        [COBieAttributes(1, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_ForeignKey, "CreatedBy", 255, COBieAllowedType.Email)]
        public string CreatedBy { get; set; }

        [COBieAttributes(2, COBieKeyType.None, "", COBieAttributeState.Required_Information, "CreatedOn", 255, COBieAllowedType.ISODateTime)]
        public string CreatedOn { get; set; }

        [COBieAttributes(3, COBieKeyType.ForeignKey, "PickLists.JobType", COBieAttributeState.Required_Reference_PickList, "Category", 255, COBieAllowedType.AlphaNumeric)]
        public string Category { get; set; }

        [COBieAttributes(4, COBieKeyType.ForeignKey, "PickLists.JobStatusType", COBieAttributeState.Required_Reference_PickList, "Status", 255, COBieAllowedType.AlphaNumeric)]
        public string Status { get; set; }

        [COBieAttributes(5, COBieKeyType.CompoundKey_ForeignKey, "Type.Name", COBieAttributeState.Required_Reference_PrimaryKey, "TypeName", 255, COBieAllowedType.AlphaNumeric)]
        public string TypeName { get; set; }

        [COBieAttributes(6, COBieKeyType.None, "", COBieAttributeState.Required_Information, "Description", 255, COBieAllowedType.AlphaNumeric)]
        public string Description { get; set; }

        [COBieAttributes(7, COBieKeyType.None, "", COBieAttributeState.Required_Information, "Duration", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string Duration { get; set; }

        [COBieAttributes(8, COBieKeyType.ForeignKey, "PickLists.DurationUnit", COBieAttributeState.Required_Reference_PickList, "DurationUnit", 255, COBieAllowedType.Text)]
        public string DurationUnit { get; set; }

        [COBieAttributes(9, COBieKeyType.None, "", COBieAttributeState.Required_Information, "Start", Constants.DOUBLE_MAXSIZE, COBieAllowedType.ISODate)]
        public string Start { get; set; }

        [COBieAttributes(10, COBieKeyType.ForeignKey, "PickLists.DurationUnit", COBieAttributeState.Required_Reference_PickList, "TaskStartUnit", 255, COBieAllowedType.Text)]
        public string TaskStartUnit { get; set; }

        [COBieAttributes(11, COBieKeyType.None, "", COBieAttributeState.Required_Information, "Frequency", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string Frequency { get; set; }

        [COBieAttributes(12, COBieKeyType.ForeignKey, "PickLists.DurationUnit", COBieAttributeState.Required_Reference_PickList, "FrequencyUnit", 255, COBieAllowedType.Text)]
        public string FrequencyUnit { get; set; }

        [COBieAttributes(13, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtSystem", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtSystem { get; set; }

        [COBieAttributes(14, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtObject { get; set; }

        [COBieAttributes(15, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtIdentifier { get; set; }

        [COBieAttributes(16, COBieKeyType.CompoundKey, "", COBieAttributeState.Required_IfSpecified, "TaskNumber", 255, COBieAllowedType.AlphaNumeric)]
        public string TaskNumber { get; set; }

        [COBieAttributes(17, COBieKeyType.ForeignKey, "Job.TaskNumber", COBieAttributeState.Required_IfSpecified, "Priors", 255, COBieAllowedType.Text, COBieCardinality.ManyToMany)]
        public string Priors { get; set; }

        [COBieAttributes(18, COBieKeyType.ForeignKey, "Resource.Name", COBieAttributeState.Required_IfSpecified, "ResourceNames", 255, COBieAllowedType.Text, COBieCardinality.ManyToMany)]
        public string ResourceNames { get; set; }
    }
}
