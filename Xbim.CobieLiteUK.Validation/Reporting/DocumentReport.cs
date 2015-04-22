using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.CobieLiteUK.Validation.Extensions;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    internal class DocumentsReport
    {
        private readonly List<Document> _documentsToReport;

        public DocumentsReport(List<Document> documentsToReport)
        {
            _documentsToReport = documentsToReport;
        }

        internal DataTable GetReport(string groupingAttributeName)
        {
            if (_documentsToReport == null || !_documentsToReport.Any())
                return null;
            var d = PrepareTable(groupingAttributeName);
            var dic = new Dictionary<string, List<Document>>();
            foreach (var document in _documentsToReport)
            {
                var attV = "";
                var firstAtt = document.Attributes.FirstOrDefault(att => att.Name == groupingAttributeName);
                if (firstAtt != null)
                {
                    var tmpV = firstAtt.Value.GetStringValue();
                    if (!string.IsNullOrEmpty(tmpV))
                        attV = tmpV;
                }
                if (!dic.ContainsKey(attV))
                {
                    dic.Add(attV, new List<Document> { document });
                }
                else
                {
                    dic[attV].Add(document);
                }
            }

            foreach (var key in dic.Keys)
            {
                var newrow = d.NewRow();
                newrow[GroupColName] = key;
                var iTot = 0;
                var iPass = 0;
                foreach (var doc in dic[key])
                {
                    iTot++;
                    if (doc.ValidationResult() == StatusOptions.Passed)
                    {
                        iPass++;
                    }
                }
                newrow[SubmittedColName] = iTot;

                var aStyle = VisualAttentionStyle.Green;
                if (iPass < iTot)
                    aStyle = VisualAttentionStyle.Red;
                if (iTot == 0)
                    aStyle = VisualAttentionStyle.Amber;
                // ReSharper disable once RedundantAssignment (reduces risk for future edits)
                newrow[ValidColName] = new VisualValue(iPass) { AttentionStyle = aStyle };
                d.Rows.Add(newrow);
            }
            return d;
        }

        private const string GroupColName = "DPoW_groupingAttribute";
        private const string SubmittedColName = "DPoW_Submitted";
        private const string ValidColName = "DPoW_Valid";


        private static DataTable PrepareTable(string mainClassification)
        {
            const string repName = "Documents validation report";
            var retTable = new DataTable(repName, "DPoW");
            var workCol = retTable.Columns.Add("DPoW_ID", typeof(Int32));
            workCol.AllowDBNull = false;
            workCol.Unique = true;
            workCol.AutoIncrement = true;

            retTable.Columns.Add(new DataColumn(GroupColName, typeof(String)) { Caption = mainClassification });
            retTable.Columns.Add(new DataColumn(SubmittedColName, typeof(int)) { Caption = "No. required" });
            retTable.Columns.Add(new DataColumn(ValidColName, typeof(VisualValue)) { Caption = "No. Valid" });
            return retTable;
        }
    }
}
