using System;
using System.Collections.Generic;
using System.Data;
using Xbim.CobieLiteUK.Validation.Extensions;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    internal class DocumentsDetailedReport
    {
        private readonly List<Document> _documentsToReport;

        public DocumentsDetailedReport(List<Document> documentsToReport)
        {
            _documentsToReport = documentsToReport;
        }

        private void MakeReport()
        {
            SetBasicGrid();

            foreach (var runningParent in _documentsToReport)
            {
                var r = GetRow(runningParent);
                while (r == null) // it's still preparing the columns as appropriate.
                {
                    r = GetRow(runningParent);
                }

                AttributesGrid.Rows.Add(r);
            }  
        }

        private bool FillRow(Document doc, DataRow r)
        {
            var isInserting = true;
            foreach (var attribute in doc.Attributes)
            {
                var vA = new ValidatedAttribute(attribute);
                if (attribute.ExternalEntity != "DPoW Attributes")
                    continue;

                // check for table columns
                var sName = attribute.Name;
                if (!AttributesGrid.Columns.Contains(sName))
                {
                    isInserting = false;
                    AttributesGrid.Columns.Add(new DataColumn(attribute.Name, typeof(ValidatedAttribute))
                    {
                        Namespace = attribute.PropertySetName
                    });
                }
                if (!isInserting || attribute.Value == null)
                    continue;

                r[sName] = vA;
            }
            if (!AttributesGrid.Columns.Contains("DPoW_Directory"))
            {
                AttributesGrid.Columns.Add(new DataColumn("DPoW_Directory", typeof(VisualValue)));
                AttributesGrid.Columns.Add(new DataColumn("DPoW_File", typeof(VisualValue)));
                return false;
            }

            var status = doc.ValidationResult() == StatusOptions.Passed
                ? VisualAttentionStyle.Green
                : VisualAttentionStyle.Red;

            var vDir = new VisualValue(doc.Directory) {AttentionStyle = status};
            var vFile = new VisualValue(doc.File) { AttentionStyle = status };

            r["DPoW_Directory"] = vDir;
            r["DPoW_File"] = vFile;
            return isInserting;
        }

        private DataRow GetRow(Document runningAsset)
        {
            var r = AttributesGrid.NewRow();

            var isInserting = FillRow(runningAsset, r);
            if (!isInserting)
                return null;

            r[DpowDocName] = runningAsset.Name;
            return r;
        }

        private DataTable _attributesGrid;
        public DataTable AttributesGrid {
            get
            {
                if (_attributesGrid == null)
                {
                    MakeReport();
                }
                return _attributesGrid;
            }
        }

        private const string DpowDocName = "DPoW_DocName";

        private void SetBasicGrid()
        {
            _attributesGrid = new DataTable {TableName = "Detailed Documents report"};
            // Id
            var workCol = AttributesGrid.Columns.Add("DPoW_ID", typeof(Int32));
            workCol.AllowDBNull = false;
            workCol.Unique = true;
            workCol.AutoIncrement = true;

            AttributesGrid.Columns.Add(new DataColumn(DpowDocName, typeof(String)) { Caption = "Document name" });
            
        }
    }
}
