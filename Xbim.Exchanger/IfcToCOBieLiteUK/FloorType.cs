using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    /// <summary>
    /// 
    /// </summary>
    public class FloorType: Floor
    {
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcBuildingStorey"></param>
        /// <param name="helper"></param>
        public FloorType(IfcBuildingStorey ifcBuildingStorey, CoBieLiteUkHelper helper)
            
        {
         //   _ifcBuildingStorey = ifcBuildingStorey;
            ExternalEntity = helper.ExternalEntityName(ifcBuildingStorey);
            ExternalId = helper.ExternalEntityIdentity(ifcBuildingStorey);
            ExternalSystem = helper.ExternalSystemName(ifcBuildingStorey);
            Name = ifcBuildingStorey.Name;
            Categories = helper.GetCategories(ifcBuildingStorey);
            Description = ifcBuildingStorey.Description;
            //set the fall backs
            
            if (ifcBuildingStorey.Elevation.HasValue)
            {
                Elevation = ifcBuildingStorey.Elevation.Value;
            }
            Height = helper.GetCoBieAttribute<DecimalAttributeValue>("FloorHeightValue", ifcBuildingStorey).Value;
            
           
            var spaces = ifcBuildingStorey.GetSpaces();
            var ifcSpaces = spaces as IList<IfcSpace> ?? spaces.ToList();
            if (ifcSpaces.Any())
            {
                Spaces =  new List<Space>(ifcSpaces.Count);
                for (var i = 0; i < ifcSpaces.Count; i++)
                {
                    Spaces.Add(new SpaceType(ifcSpaces[i], helper));
                }
            }

            //Attributes
            Attributes = helper.GetAttributes(ifcBuildingStorey);
           
            //TODO:
            //Space Issues
            //Space Documents
        }

    }
}
