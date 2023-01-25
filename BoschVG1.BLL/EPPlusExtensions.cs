using OfficeOpenXml;
using BoschVG1.Models.InExcelSheet;
using System.Reflection;

namespace BoschVG1.BLL
{
    public static class EPPlusExtensions
    {
        public static string GetText(this ExcelWorksheet worksheet, int row, int col)
        {
            return worksheet.Cells[row, col].Value?.ToString();
        }

        public static List<T> ConvertSheetToObjects<T>(this ExcelWorksheet worksheet, string fileName, int rowHeader, bool replaceDashInDecimalAsNUll) where T : new()
        {
            var columnAttributes = typeof(T).GetProperties()
                 .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(Column)))
                 .Select(p => new HeaderColumn
                 {
                     Property = p,
                     ColumnName = p.GetCustomAttributes<Column>().First().ColumnName,
                     ColumnIndex = p.GetCustomAttributes<Column>().First().ColumnIndex,
                     Columntype = p.GetCustomAttributes<Column>().First().ColumnType
                 }).ToList();

            var start = worksheet?.Dimension.Start;
            var end = worksheet?.Dimension.End;
            var lastEmptyRow = worksheet.Cells.Last(c => c.Start.Column == 1).Start.Row;

            if (start == null || end == null)
                throw new Exception($"No valid matching name columns are found in Excel file {fileName}");

            var headersTexts = worksheet.Cells[rowHeader, start.Column, rowHeader, end.Column]
                              .Select(c => new
                              {
                                  Text = c.Value?.ToString().Replace("\r\n", " ").Trim(),
                                  ColumnIndex = c.Start.Column
                              }).Where(t => t.Text != null)
                              .ToList();

            foreach (var column in columnAttributes.Where(c => c.ColumnIndex == 0 && !string.IsNullOrWhiteSpace(c.ColumnName)))
            {
                var headercount = headersTexts.Where(h => h.Text.Equals(column.ColumnName, StringComparison.OrdinalIgnoreCase)).ToList();
                if (headercount.Count > 1)
                {
                    throw new Exception($"header column [{column.ColumnName}] appears more than 1 time in the Excel. Please correct and try again");
                }

                var header = headersTexts.SingleOrDefault(h => h.Text.Equals(column.ColumnName, StringComparison.OrdinalIgnoreCase));

                if (header == null)
                    continue;

                column.ColumnIndex = header.ColumnIndex;
            }

            for (int row = start.Row; row <= end.Row; row++)
            {
                for (int col = start.Column; col <= end.Column; col++)
                {
                    if (!string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Value?.ToString()))
                    {
                        lastEmptyRow = row;
                        continue;
                    }
                }
            }

            var missingMandatoryColumnsInExcel = columnAttributes
                .Where(c => c.ColumnIndex == 0 && !string.IsNullOrWhiteSpace(c.ColumnName))
                .Where(item => !headersTexts
                .Any(item2 => item2.Text
                .Equals(item.ColumnName, StringComparison.OrdinalIgnoreCase)))
                .Where(item => item.Columntype == Column.Columntype.Mandatory)
                .ToList();

            if (missingMandatoryColumnsInExcel.Any())
            {
                throw new Exception($@"Missing mandatory columns: [{string.Join(",", missingMandatoryColumnsInExcel.Select(x => x.ColumnName).ToList())}] " +
                                       "in the Excel. Please correct and try again");
            }

            var collection = new List<T>();

            for (int irow = start.Row + rowHeader; irow <= lastEmptyRow; irow++)
            {
                var row = worksheet.Row(irow);
                var tnew = new T();

                columnAttributes.Where(c => c.ColumnIndex > 0 && !string.IsNullOrWhiteSpace(c.ColumnName)).ToList()
               .ForEach(col =>
               {
                   var cell = worksheet.Cells[irow, col.ColumnIndex];

                   if (cell == null)
                       col.Property.SetValue(tnew, null);
                   else if (string.IsNullOrEmpty(cell.Text))
                       col.Property.SetValue(tnew, null);
                   else if (cell.Text == "#REF!")
                       col.Property.SetValue(tnew, null);
                   else if (cell.Text == "#N/A")
                       col.Property.SetValue(tnew, null);
                   else if (replaceDashInDecimalAsNUll && cell.Text.Replace("\r\n", " ").Trim() == "-" && Nullable.GetUnderlyingType(col.Property.PropertyType) == typeof(decimal))
                       col.Property.SetValue(tnew, (decimal)0);
                   else
                   {
                       var type = Nullable.GetUnderlyingType(col.Property.PropertyType) ?? col.Property.PropertyType;
                       var typename = type.Name;
                       if (type == typeof(int))
                       {
                           typename = "Whole number";
                       }
                       else if (type == typeof(DateTime))
                       {
                           typename = "Date or Date + time";
                       }
                       else if (type == typeof(Double))
                       {
                           typename = "Double or Decimal";
                       }

                       try
                       {
                           col.Property.SetValue(tnew, Convert.ChangeType(cell.Value, type));
                       }
                       catch (InvalidCastException)
                       {
                           throw new Exception($"Invalid type in column [{col.ColumnName}] should be {typename}. Please check this column in excel by filtering the data. Example Row {irow}");
                       }
                       catch (FormatException)
                       {
                           throw new Exception($"Invalid type in column [{col.ColumnName}] should be {typename}. Please check this column in excel by filtering the data. Example Row {irow}");
                       }
                   }
               });
                collection.Add(tnew);
            }
            return collection;
        }
    }
}
