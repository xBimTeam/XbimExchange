using Xbim.Ifc2x3.ProductExtension;

namespace Xbim.COBieLite
{
    public partial class SystemType
    {
        public SystemType()
        {

        }
        public SystemType(IfcSystem ifcSystem, CoBieLiteHelper helper)
            : this()
        {
            externalEntityName = helper.ExternalEntityName(ifcSystem);
            externalID = helper.ExternalEntityIdentity(ifcSystem);
            externalSystemName = helper.ExternalSystemName(ifcSystem);
            SystemName = ifcSystem.Name;
            SystemDescription = ifcSystem.Description;
            SystemCategory = helper.GetClassification(ifcSystem);
            
            //Attributes
            AttributeType[] ifcAttributes = helper.GetAttributes(ifcSystem);
            if (ifcAttributes != null && ifcAttributes.Length > 0)
                SystemAttributes = new AttributeCollectionType { Attribute = ifcAttributes };

             //TODO:
            //System Issues
            //System Documents
        }
    }
}
