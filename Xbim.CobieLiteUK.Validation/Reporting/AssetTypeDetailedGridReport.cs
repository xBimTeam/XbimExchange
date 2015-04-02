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
        private AssetTypeRequirementPointer _valideatedAssetType;

        public AssetTypeDetailedGridReport(AssetTypeRequirementPointer assetType)
        {
            _valideatedAssetType = assetType;
        }

        internal DataTable AttributesGrid { get; private set; }

        internal void PrepareReport()
        {
            setBasicGrid();

            
            foreach (var runningAssetType in _valideatedAssetType.ProvidedAssetTypes)
            {
                if (runningAssetType.Assets == null)
                    continue;
                foreach (var runningAsset in runningAssetType.Assets)
                {
                    var r = GetRow(runningAsset);
                    while (r == null) // it's still preparing the columns as appropriate.
                    {
                        r = GetRow(runningAsset);
                    }
                    r[DpowAssetTypeNameColumnName] = runningAssetType.Name;
                    r[DpowAssetTypeExternalSystemColumnName] = runningAssetType.ExternalSystem;
                    r[DpowAssetTypeExternalIdColumnName] = runningAssetType.ExternalId;
                    AttributesGrid.Rows.Add(r);
                }
            }  
        }

        private const string DpowAssetTypeNameColumnName = "DPoW_AssetTypeName";
        private const string DpowAssetTypeExternalSystemColumnName = "DPoW_AssetTypeExternalSystem";
        private const string DpowAssetTypeExternalIdColumnName = "DPoW_AssetTypeExternalID";

        private const string DpowAssetNameColumnName = "DPoW_AssetName";
        private const string DpowAssetExternalSystemColumnName = "DPoW_AssetExternalSystem";
        private const string DpowAssetExternalIdColumnName = "DPoW_AssetExternalID";
         
        private void setBasicGrid()
        {
            AttributesGrid = new DataTable();
            // Id
            var workCol = AttributesGrid.Columns.Add("DPoW_ID", typeof(Int32));
            workCol.AllowDBNull = false;
            workCol.Unique = true;
            workCol.AutoIncrement = true;
            

            AttributesGrid.Columns.Add(new DataColumn(DpowAssetTypeNameColumnName, typeof(String)) { Caption = "Asset Type Name" });
            AttributesGrid.Columns.Add(new DataColumn(DpowAssetTypeExternalSystemColumnName, typeof(String)) { Caption = "Asset Type System" });
            AttributesGrid.Columns.Add(new DataColumn(DpowAssetTypeExternalIdColumnName, typeof(String)) { Caption = "Asset Type ID" });

            AttributesGrid.Columns.Add(new DataColumn(DpowAssetNameColumnName, typeof(String)) { Caption = "Asset Name" });
            AttributesGrid.Columns.Add(new DataColumn(DpowAssetExternalSystemColumnName, typeof(String)) { Caption = "Asset System" });
            AttributesGrid.Columns.Add(new DataColumn(DpowAssetExternalIdColumnName, typeof(String)) { Caption = "Asset ID" });
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

            if (isInserting) // saves time otherwise
            {
                r[DpowAssetNameColumnName] = runningAsset.Name;
                r[DpowAssetExternalSystemColumnName] = runningAsset.ExternalSystem;
                r[DpowAssetExternalIdColumnName] = runningAsset.ExternalId;
            }

            return !isInserting 
                ? null 
                : r;
        }
    }
}
