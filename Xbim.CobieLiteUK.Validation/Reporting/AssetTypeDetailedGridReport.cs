using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.CobieLiteUK.Validation.Extensions;
using Attribute = Xbim.COBieLiteUK.Attribute;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    /// <summary>
    /// Data preparation layer for the production or reports that group all AssetTypes/Assets associated with an assetType requirement
    /// </summary>
    public class AssetTypeDetailedGridReport
    {
        private readonly AssetTypeRequirementPointer _valideatedAssetType;

        /// <summary>
        /// Initialise the reporting class with an AssetTypeRequirementPointer
        /// </summary>
        public AssetTypeDetailedGridReport(AssetTypeRequirementPointer assetType)
        {
            _valideatedAssetType = assetType;
        }

        /// <summary>
        /// Lists all categories that are verified for compliance with the requirement
        /// </summary>
        public IEnumerable<Category> RequirementCategories
        {
            get
            {
                return _valideatedAssetType.RequirementCategories;
            }
        }

        /// <summary>
        /// Datatable that contains all submitted assets with all available and missing properties
        /// </summary>
        public DataTable AttributesGrid { get; private set; }

        internal void PrepareReport()
        {
            SetBasicGrid();

            foreach (var runningAssetType in _valideatedAssetType.ProvidedAssetTypes)
            {
                if (runningAssetType.Assets == null)
                    continue;
                foreach (var runningAsset in runningAssetType.Assets)
                {
                    var r = GetRow(runningAssetType, runningAsset);
                    
                    while (r == null) // it's still preparing the columns as appropriate.
                    {
                        r = GetRow(runningAssetType, runningAsset);
                    }
                    
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
         
        private void SetBasicGrid()
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

        private DataRow GetRow(AssetType parentAssetType, Asset runningAsset)
        {
            var r = AttributesGrid.NewRow();

            var isInserting = FillRow(parentAssetType.Attributes, r);
            if (!isInserting)
                return null;

            isInserting = FillRow(runningAsset.Attributes, r);
            if (!isInserting)
                return null;

            r[DpowAssetNameColumnName] = runningAsset.Name;
            r[DpowAssetExternalSystemColumnName] = runningAsset.ExternalSystem;
            r[DpowAssetExternalIdColumnName] = runningAsset.ExternalId;

            r[DpowAssetTypeNameColumnName] = parentAssetType.Name;
            r[DpowAssetTypeExternalSystemColumnName] = parentAssetType.ExternalSystem;
            r[DpowAssetTypeExternalIdColumnName] = parentAssetType.ExternalId;
            return r;
        }

        private bool FillRow(List<Attribute> atts, DataRow r)
        {
            var isInserting = true;
            foreach (var attribute in atts)
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

                // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
                if (attribute.Value is StringAttributeValue)
                    r[sName] = ((StringAttributeValue) attribute.Value).Value;
                else if (attribute.Value is IntegerAttributeValue)
                    r[sName] = ((IntegerAttributeValue) attribute.Value).Value;
                else if (attribute.Value is DecimalAttributeValue)
                    r[sName] = ((DecimalAttributeValue) attribute.Value).Value;
                else if (attribute.Value is BooleanAttributeValue)
                    r[sName] = ((BooleanAttributeValue) attribute.Value).Value.ToString();
                else if (attribute.Value is DateTimeAttributeValue)
                    r[sName] = ((DateTimeAttributeValue) attribute.Value).Value;
                // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull
            }
            return isInserting;
        }
    }
}
