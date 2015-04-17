using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Xbim.COBie.Rows
{

    [Serializable()]
    public class COBieAssemblyRow : COBieRow
    {
        public COBieAssemblyRow(ICOBieSheet<COBieAssemblyRow> parentSheet)
            : base(parentSheet)
        {
            ExtIdentifier = IFCGuid.ToIfcGuid(Guid.NewGuid());
        }

        [COBieAttributes(0, COBieKeyType.CompoundKey, "", COBieAttributeState.Required_PrimaryKey, "Name", 255, COBieAllowedType.AlphaNumeric)]
        public string Name { get; set; }

        [COBieAttributes(1, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_ForeignKey, "CreatedBy", 255, COBieAllowedType.Email)]
        public string CreatedBy { get; set; }

        [COBieAttributes(2, COBieKeyType.None, "", COBieAttributeState.Required_Information, "CreatedOn", 255, COBieAllowedType.ISODateTime)]
        public string CreatedOn { get; set; }

        [COBieAttributes(3, COBieKeyType.ForeignKey, "PickLists.AssemblyType", COBieAttributeState.Required_Reference_PickList, "AssemblyType", 255, COBieAllowedType.AlphaNumeric)]
        public string AssemblyType { get; set; }

        [COBieAttributes(4, COBieKeyType.CompoundKey_ForeignKey, "PickLists.SheetType", COBieAttributeState.Required_Reference_PickList, "SheetName", 255, COBieAllowedType.AlphaNumeric)]
        public string SheetName { get; set; }

        [COBieAttributes(5, COBieKeyType.CompoundKey_ForeignKey, "Component.Name", COBieAttributeState.Required_Reference_PrimaryKey, "ParentName", 255, COBieAllowedType.AlphaNumeric)]
        public string ParentName { get; set; }

        [COBieAttributes(6, COBieKeyType.CompoundKey_ForeignKey, "Component.Name", COBieAttributeState.Required_Reference_PrimaryKey, "ChildNames", 255, COBieAllowedType.AlphaNumeric, COBieCardinality.ManyToMany)]
        public string ChildNames { get; set; }
       
        [COBieAttributes(7, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtSystem", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtSystem { get; set; }

        [COBieAttributes(8, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtObject { get; set; }

        [COBieAttributes(9, COBieKeyType.None, "", COBieAttributeState.Required_System_IfSpecified, "ExtIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtIdentifier { get; set; }

        [COBieAttributes(10, COBieKeyType.None, "", COBieAttributeState.Required_IfSpecified, "Description", 255, COBieAllowedType.AlphaNumeric)]
        public string Description { get; set; }




    }
}
