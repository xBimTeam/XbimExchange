using System;
using Xbim.Ifc2x3.IO;
using Xbim.Ifc2x3.UtilityResource;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    class COBieXBimEditScope : IDisposable
    {
        private IfcOwnerHistory IfcOwnerHistory {  get;  set; }
        private XbimModel Model {  get;  set; }

        public COBieXBimEditScope(XbimModel model, IfcOwnerHistory owner)
        {
            Model = model;
            IfcOwnerHistory = Model.OwnerHistoryAddObject;
            Model.OwnerHistoryAddObject = owner;
            
        }

        public void Dispose()
        {
            Model.OwnerHistoryAddObject = IfcOwnerHistory;
        }
    }
}
