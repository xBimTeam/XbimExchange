using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.IO;

namespace Xbim.COBieLite
{
    class XbimCoBieLiteToIfcConverter : IDisposable
    {
        private XbimModel _model;
        private bool _merging;
        public bool Convert(FacilityType facility, XbimModel model=null)
        {
            if (model == null)
            {
                _model = XbimModel.CreateTemporaryModel();
                _merging = false;
            }
            else
            {
                _model = model;
                _merging = true;
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
