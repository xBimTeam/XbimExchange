using System;
using System.Collections.Generic;
using Xbim.Ifc4.Interfaces;


namespace XbimExchanger.IfcToCOBieLiteUK.EqCompare
{

    /// <summary>
    /// Compare IfcRelAssignsToGroup.RelatedGroup obj references
    /// </summary>
    public class IfcRelAssignsToGroupRelatedGroupObjCompare : IEqualityComparer<IIfcRelAssignsToGroup>
    {
        public bool Equals(IIfcRelAssignsToGroup x, IIfcRelAssignsToGroup y)
        {
            if (Object.ReferenceEquals(x, y)) //same instance
                return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null)) //one is null
                return false;

            if (Object.ReferenceEquals(x.RelatingGroup, y.RelatingGroup)) //same instance of RelatingGroup
                return true;

            if (Object.ReferenceEquals(x.RelatingGroup, null) || Object.ReferenceEquals(x.RelatingGroup, null)) //one RelatingGroup is null
                return false;

            //compare objects
            return x.RelatingGroup.Equals(y.RelatingGroup);
        }

        public int GetHashCode(IIfcRelAssignsToGroup obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return 0;
            if (Object.ReferenceEquals(obj.RelatingGroup, null))
                return 0;


            return obj.RelatingGroup.GetHashCode();
        }
    }
}
