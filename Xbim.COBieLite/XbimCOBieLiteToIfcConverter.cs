using System;
using Xbim.Common.Step21;
using Xbim.Ifc;


namespace Xbim.COBieLite
{
    class XbimCoBieLiteToIIfcConverter : IDisposable
    {
        private IfcStore _model;
       
        public bool Convert(FacilityType facility, IfcStore model=null)
        {
            if (model == null)
            {
                var credentials = new XbimEditorCredentials();
                _model = IfcStore.Create(credentials, IfcSchemaVersion.Ifc4, XbimStoreType.EsentDatabase);               
            }
            else
            {
                _model = model;
                
            }
            return  WriteFacility(facility);
           
        }

        private bool WriteFacility(FacilityType facility)
        {
            return true;
        }

        public void Dispose()
        {
            if (_model != null)
            {
                _model.Close();
                ((IDisposable) _model).Dispose();
            }
        }
    }
}
