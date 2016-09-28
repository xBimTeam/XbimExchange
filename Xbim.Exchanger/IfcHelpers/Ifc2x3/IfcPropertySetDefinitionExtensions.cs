using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.Ifc2x3.QuantityResource;

namespace XbimExchanger.IfcHelpers.Ifc2x3
{
    public static class IfcPropertySetDefinitionExtensions
    {
        public static bool Add(this IfcPropertySetDefinition pSetDefinition, IfcSimpleProperty prop)
        {
            var propSet = pSetDefinition as IfcPropertySet;
            if(propSet!=null) propSet.HasProperties.Add(prop);
            return propSet != null;
        }

        public static bool Add(this IfcPropertySetDefinition pSetDefinition, IfcPhysicalQuantity quantity)
        {
            var quantSet = pSetDefinition as IfcElementQuantity;
            if (quantSet != null) quantSet.Quantities.Add(quantity);
            return quantSet != null;
        }
    }
}
