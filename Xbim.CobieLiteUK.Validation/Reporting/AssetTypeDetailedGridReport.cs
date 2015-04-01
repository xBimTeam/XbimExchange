using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    public class AssetTypeDetailedGridReport
    {
        private AssetType _valideatedAssetType;

        public AssetTypeDetailedGridReport(AssetType assetType)
        {
            _valideatedAssetType = assetType;
        }

        internal DataTable AttributesGrid { get; private set; }

        internal void PrepareReport()
        {
            AttributesGrid = new DataTable();
            if (_valideatedAssetType.Assets == null)
                return;
            
            foreach (var runningAsset in _valideatedAssetType.Assets)
            {
                DataRow r;
                while ((r = GetRow(runningAsset)) == null)  { }
                AttributesGrid.Rows.Add(r);
            }  
        }

        private DataRow GetRow(Asset runningAsset)
        {
            var r = AttributesGrid.NewRow();
            var isInserting = true;
            
            foreach (var attribute in runningAsset.Attributes)
            {
                var sName = attribute.Name;
                if (!AttributesGrid.Columns.Contains(sName))
                {
                    isInserting = false;
                    AttributesGrid.Columns.Add(new DataColumn(attribute.Name, typeof (string))
                    {
                        Namespace = attribute.PropertySetName
                    });
                }
                if (!isInserting || attribute.Value == null) 
                    continue;
                var value = attribute.Value as StringAttributeValue;
                r[sName] = value.Value;
            }
            return !isInserting 
                ? null 
                : r;
        }
    }
}
