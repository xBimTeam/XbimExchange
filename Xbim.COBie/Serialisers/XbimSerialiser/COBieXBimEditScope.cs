using System;
using Xbim.Ifc2x3.IO;
using Xbim.Ifc2x3.UtilityResource;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    class COBieXBimEditScope : IDisposable
    {
        private IfcOwnerHistory _owner;
        private XbimModel Model {  get;  set; }

        public COBieXBimEditScope(XbimModel model, IfcOwnerHistory owner)
        {
            Model = model;
            _owner = owner;
            Model.EntityNew += Model_EntityNew;
            
        }

        void Model_EntityNew(Common.IPersistEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Model.EntityNew -= Model_EntityNew;
        }
    }
}
