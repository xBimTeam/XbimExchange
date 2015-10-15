using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.Ifc2x3.UtilityResource;
using Xbim.IO;

namespace XbimExchanger.COBieLiteUkToIfc
{
    /// <summary>
    /// Class to allow setting of a given OwnerHistory
    /// Note: Class used in Using statement, will set the Model.OwnerHistoryAddObject to IfcConstructionProductResource.OwnerHistory then reset OwnerHistoryAddObject back in the dispose. Needed because history is set to OwnerHistoryAddObject on any property changes which loses the currently set owner history, 
    /// OK on new files, but on round trip not so good.So set any properties within a using statement to keep user history set on the object without reset to default history (OwnerHistoryAddObject)
    /// </summary>
    internal class OwnerHistoryEditScope : IDisposable
    {
        private IfcOwnerHistory ifcOwnerHistory
        { get; set; }
        private XbimModel Model
        { get; set; }

        public OwnerHistoryEditScope(XbimModel model, IfcOwnerHistory owner)
        {
            Model = model;
            ifcOwnerHistory = Model.OwnerHistoryAddObject;
            Model.OwnerHistoryAddObject = owner;

        }

        public void Dispose()
        {
            Model.OwnerHistoryAddObject = ifcOwnerHistory;
        }
    }

    /// <summary>
    /// Class used as key to reference IfcOwnerHistory Objects
    /// </summary>
    internal class OwnerHistoryKey : IEquatable<OwnerHistoryKey>
    {
        private string createdBy;
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = (value == null) ? string.Empty : value; }
        }

        private string extSystem;
        public string ExtSystem
        {
            get { return extSystem; }
            set { extSystem = (value == null) ? string.Empty : value; }
        }

        private string createdOn;
        public string CreatedOn
        {
            get { return createdOn; }
            set { createdOn = (value == null) ? string.Empty : value; }
        }


        public bool Equals(OwnerHistoryKey other)
        {
            // First two lines are just optimizations
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return CreatedBy.Equals(other.CreatedBy, StringComparison.OrdinalIgnoreCase)
                && ExtSystem.Equals(other.ExtSystem, StringComparison.OrdinalIgnoreCase)
                && CreatedOn.ToString().Equals(other.CreatedOn.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            // check instances
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            // check the type
            if (obj.GetType() != this.GetType())
                return false;

            return Equals((OwnerHistoryKey)obj);
        }

        public override int GetHashCode()
        {
            
            string CreatedByHashstr = string.IsNullOrEmpty(CreatedBy) ? string.Empty : CreatedBy;
            string ExtSystemHashstr = string.IsNullOrEmpty(ExtSystem) ? string.Empty : ExtSystem;
            int primeNo = 16777619;
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = (hash * primeNo) + CreatedByHashstr.ToUpper().GetHashCode();
                hash = (hash * primeNo) + ExtSystemHashstr.ToUpper().GetHashCode();
                hash = (hash * primeNo) + CreatedOn.GetHashCode();
                return hash;
            }
        }

        

    }
}
