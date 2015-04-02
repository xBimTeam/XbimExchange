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
                var r = GetRow(runningAsset);
                while (r == null) // it's still preparing the columns as appropriate.
                {
                    r = GetRow(runningAsset);
                }
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

                if (attribute.Value is StringAttributeValue)
                    r[sName] = ((StringAttributeValue)attribute.Value).Value;
                else if (attribute.Value is IntegerAttributeValue)
                    r[sName] = ((IntegerAttributeValue)attribute.Value).Value;
                else if (attribute.Value is DecimalAttributeValue)
                    r[sName] = ((DecimalAttributeValue)attribute.Value).Value;
                else if (attribute.Value is BooleanAttributeValue)
                    r[sName] = ((BooleanAttributeValue)attribute.Value).Value.ToString();
                else if (attribute.Value is DateTimeAttributeValue)
                    r[sName] = ((DateTimeAttributeValue)attribute.Value).Value;
            }
            return !isInserting 
                ? null 
                : r;
        }
    }
}
