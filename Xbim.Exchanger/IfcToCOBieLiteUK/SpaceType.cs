using System.Collections.Generic;
using System.Xml.Serialization;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    public partial class SpaceType: Space
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcSpace"></param>
        /// <param name="helper"></param>
        public SpaceType(IfcSpace ifcSpace, CoBieLiteUkHelper helper)
            
        {
           // _ifcSpace = ifcSpace;
            ExternalEntity = helper.ExternalEntityName(ifcSpace);
            ExternalId = helper.ExternalEntityIdentity(ifcSpace);
            ExternalSystem = helper.ExternalSystemName(ifcSpace);
            Name = ifcSpace.Name;
            Categories = helper.GetCategories(ifcSpace);
            Description = ifcSpace.Description;
            RoomTag = helper.GetCoBieAttribute<StringAttributeValue>("SpaceSignageName", ifcSpace).Value;
            UsableHeight = helper.GetCoBieAttribute<DecimalAttributeValue>("SpaceUsableHeightValue", ifcSpace).Value;
            GrossArea = helper.GetCoBieAttribute<DecimalAttributeValue>("SpaceGrossAreaValue", ifcSpace).Value;
            NetArea = helper.GetCoBieAttribute<DecimalAttributeValue>("SpaceNetAreaValue", ifcSpace).Value;


            //Attributes
            Attributes = helper.GetAttributes(ifcSpace);
    
            //TODO:
            //Space Issues
            //Space Documents
        }

    }
}
