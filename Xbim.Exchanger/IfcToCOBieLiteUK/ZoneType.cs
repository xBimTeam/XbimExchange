using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLite;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    public class ZoneType : Zone
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcZone"></param>
        /// <param name="helper"></param>
        public ZoneType(IfcZone ifcZone, CoBieLiteUkHelper helper)
        {
            ExternalEntity = helper.ExternalEntityName(ifcZone);
            ExternalId = helper.ExternalEntityIdentity(ifcZone);
            ExternalSystem = helper.ExternalSystemName(ifcZone);
            Description = ifcZone.Description;
            Categories = helper.GetCategories(ifcZone);
            Name = ifcZone.Name;
            //Attributes
            Attributes = helper.GetAttributes(ifcZone);

            //get spaces in zones

            var spaces = helper.GetSpaces(ifcZone);
            if (spaces.Any())
            {
                Spaces = new List<SpaceKey>();

                foreach (var space in spaces)
                {
                    var spaceKey = new SpaceKey {KeyType = EntityType.Space, Name = space.Name};
                    Spaces.Add(spaceKey);
                }
            }
            //TODO:
            //Space Issues
            //Space Documents
        }
    }
}
