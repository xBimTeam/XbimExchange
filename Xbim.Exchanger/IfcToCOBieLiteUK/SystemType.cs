using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    /// <summary>
    /// 
    /// </summary>
    public  class SystemType: Xbim.COBieLiteUK.System
    {
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcSystem"></param>
        /// <param name="helper"></param>
        public SystemType(IfcSystem ifcSystem, CoBieLiteUkHelper helper)  
        {
            ExternalEntity = helper.ExternalEntityName(ifcSystem);
            ExternalId = helper.ExternalEntityIdentity(ifcSystem);
            ExternalSystem = helper.ExternalSystemName(ifcSystem);
            Name = ifcSystem.Name;
            Description = ifcSystem.Description;
            Categories = helper.GetCategories(ifcSystem);
            //Add Assets
            var systemAssignments = helper.GetSystemAssignments(ifcSystem);
            var ifcObjectDefinitions = systemAssignments as IList<IfcObjectDefinition> ?? systemAssignments.ToList();
            if (ifcObjectDefinitions.Any())
            {
                Components = new List<AssetKey>();
                foreach (var ifcObjectDefinition in ifcObjectDefinitions)
                {
                    var assetKey = new AssetKey {KeyType = EntityType.Asset, Name = ifcObjectDefinition.Name};
                    Components.Add(assetKey);
                }
            }

            //Attributes
            Attributes = helper.GetAttributes(ifcSystem);
          
             //TODO:
            //System Issues
            //System Documents
        }

    }
}
