using NPOI.SS.UserModel;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    internal class ExcelCellVisualValue
    {
        public ExcelCellVisualValue()
        { }

        private readonly ICellStyle _orange;
        private readonly ICellStyle _green;
        private readonly ICellStyle _red;
        private readonly ICellStyle _neutral;

        public ExcelCellVisualValue(IWorkbook workbook)
        {
            _orange = GetBaseStyle(workbook);
            _orange.FillForegroundColor = IndexedColors.Orange.Index;

            _green = GetBaseStyle(workbook);
            _green.FillForegroundColor = IndexedColors.Green.Index;

            _red = GetBaseStyle(workbook);
            _red.FillForegroundColor = IndexedColors.Red.Index;

            _neutral = GetBaseStyle(workbook);
        }

        private ICellStyle GetBaseStyle(IWorkbook workbook)
        {
            var style = workbook.CreateCellStyle();
            style.BorderBottom = style.BorderLeft = style.BorderRight = style.BorderTop = BorderStyle.Thin;
            style.FillPattern = FillPattern.SolidForeground;
            return style;
        }

        internal void SetCell(ICell excelCell, IVisualValue visualValue )
        {
            if (visualValue.AttentionStyle == VisualAttentionStyle.None)
                excelCell.CellStyle = _neutral;
            switch (visualValue.AttentionStyle)
            {
                case VisualAttentionStyle.Amber:
                    excelCell.CellStyle = _orange;
                    break;
                case VisualAttentionStyle.Green:
                    excelCell.CellStyle = _green;
                    break;
                case VisualAttentionStyle.Red:
                    excelCell.CellStyle = _red;
                    break;
            }

            var attribute = visualValue.VisualValue;
            if (attribute is StringAttributeValue)
            {
                excelCell.SetCellType(CellType.String);
                excelCell.SetCellValue(((StringAttributeValue) (attribute)).Value);
                // todo: can we set here ? cellStyle.Alignment = HorizontalAlignment.Fill;
            }
            else if (attribute is IntegerAttributeValue)
            {
                excelCell.SetCellType(CellType.Numeric);
                var v = ((IntegerAttributeValue) (attribute)).Value;
                if (v.HasValue)
                {
                    // ReSharper disable once RedundantCast
                    excelCell.SetCellValue((double) v.Value);
                }
            }
            else if (attribute is DecimalAttributeValue)
            {
                excelCell.SetCellType(CellType.Numeric);
                var v = ((DecimalAttributeValue) (attribute)).Value;
                if (v.HasValue)
                {
                    // ReSharper disable once RedundantCast
                    excelCell.SetCellValue((double) v.Value);
                }
            }
            else if (attribute is BooleanAttributeValue)
            {
                excelCell.SetCellType(CellType.Boolean);
                var v = ((BooleanAttributeValue) (attribute)).Value;
                if (v.HasValue)
                {
                    excelCell.SetCellValue(v.Value);
                }
            }
            else if (attribute is DateTimeAttributeValue)
            {
                
                // var dataFormatStyle = excelCell.Sheet.Workbook.CreateDataFormat();
                excelCell.CellStyle.DataFormat = (short)0x16; //  dataFormatStyle.GetFormat("yyyy/MM/dd HH:mm:ss");
                var v = ((DateTimeAttributeValue)(attribute)).Value;
                if (v.HasValue)
                {
                    // dataformats from: https://poi.apache.org/apidocs/org/apache/poi/ss/usermodel/BuiltinFormats.html
                    excelCell.CellStyle.DataFormat = 0x16;
                    excelCell.SetCellValue(v.Value);
                }
            }
            
        }
    }
}
