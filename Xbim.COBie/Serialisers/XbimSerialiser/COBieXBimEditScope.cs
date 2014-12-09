using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.Ifc2x3.UtilityResource;
using Xbim.XbimExtensions;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.XbimExtensions.Interfaces;
using Xbim.IO;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    class COBieXBimEditScope : IDisposable
    {
        private IfcOwnerHistory ifcOwnerHistory {  get;  set; }
        private XbimModel Model {  get;  set; }

        public COBieXBimEditScope(XbimModel model, IfcOwnerHistory owner)
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
}
