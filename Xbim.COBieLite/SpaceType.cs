using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite.CollectionTypes;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.QuantityResource;

namespace Xbim.COBieLite
{
    public partial class SpaceType
    {
        
        //private IfcSpace _ifcSpace;

        public SpaceType(IfcSpace ifcSpace, CoBieLiteHelper helper)
            : this()
        {
           // _ifcSpace = ifcSpace;
            externalEntityName = helper.ExternalEntityName(ifcSpace);
            externalID = helper.ExternalEntityIdentity(ifcSpace);
            externalSystemName = helper.ExternalSystemName(ifcSpace);
            SpaceName = ifcSpace.Name;
            SpaceCategory = helper.GetClassification(ifcSpace);
            SpaceDescription = ifcSpace.Description;
            SpaceSignageName = helper.GetCoBieAttribute<StringValueType>("SpaceSignageName", ifcSpace).StringValue;
            SpaceUsableHeightValue = helper.GetCoBieAttribute<DecimalValueType>("SpaceUsableHeight", ifcSpace);
            SpaceGrossAreaValue = helper.GetCoBieAttribute<DecimalValueType>("SpaceGrossFloorArea", ifcSpace);
            SpaceNetAreaValue = helper.GetCoBieAttribute<DecimalValueType>("SpaceNetFloorArea", ifcSpace);

            //Zone Assignment
            var zones = helper.GetZones(ifcSpace);
            if (zones != null)
            {
                var ifcZones = zones.ToArray();
                SpaceZoneAssignments = new ZoneAssignmentCollectionType { ZoneAssignment = new List<ZoneKeyType>(ifcZones.Length) };
                for (int i = 0; i < ifcZones.Length; i++)
                {
                    var zoneAssignment = new ZoneKeyType();
                    zoneAssignment.ZoneCategory = helper.GetClassification(ifcZones[i]);
                    zoneAssignment.ZoneName = ifcZones[i].Name;
                    zoneAssignment.externalIDReference = helper.ExternalEntityIdentity(ifcZones[i]);
                    SpaceZoneAssignments.Add(zoneAssignment);
                }
            }
            //Attributes
            var ifcAttributes = helper.GetAttributes(ifcSpace);
            if (ifcAttributes != null && ifcAttributes.Any())
                SpaceAttributes = new AttributeCollectionType { Attribute = ifcAttributes };
    
            //TODO:
            //Space Issues
            //Space Documents
        }
    }
}
