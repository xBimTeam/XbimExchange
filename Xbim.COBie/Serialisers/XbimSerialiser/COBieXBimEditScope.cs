using System;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;


namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    class COBieXBimEditScope : IDisposable
    {
        private IIfcOwnerHistory IfcOwnerHistory {  get;  set; }
        private IfcStore Model { get; set; }

        public COBieXBimEditScope(IfcStore model, IIfcOwnerHistory owner)
        {
            Model = model;
            IIfcOwnerHistory = Model.OwnerHistoryAddObject;
            Model.OwnerHistoryAddObject = owner;
            
        }

        public void Dispose()
        {
            Model.OwnerHistoryAddObject = IIfcOwnerHistory;
        }
    }
}
