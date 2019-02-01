using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OfficeOpenXml;

namespace Schools.Services.Infrastructure
{
    public static class Extensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static DataTable ToDataTable(this ExcelWorksheet sheet, bool hasHeader = false)
        {
            var startRow = hasHeader ? 2 : 1;
            var table = new DataTable(sheet.Name);

            BuildTable(table, sheet);

            var lastColumnIndex = table.Columns.Count;


            for (var rowIndex = startRow; rowIndex <= sheet.Dimension.End.Row; rowIndex++)
            {
                var range = sheet.Cells[rowIndex, 1, rowIndex, lastColumnIndex];

                var row = table.Rows.Add();
                foreach (var cell in range)
                {
                    row[cell.Start.Column - 1] = cell.Value;
                }
            }

            return table;
        }

        private static void BuildTable(DataTable table, ExcelWorksheet sheet)
        {
            var address = new ExcelAddress(1,1,1, sheet.Dimension.End.Column);
            sheet.Select(address);
            var range = sheet.SelectedRange;

            var columns = from cell in range
                          select new DataColumn(cell?.Value?.ToString());

            table.Columns.AddRange(columns.ToArray());
        }

        public static void TrimLastEmptyRows(this ExcelWorksheet worksheet)
        {
            while (worksheet.IsLastRowEmpty())
            {
                worksheet.DeleteRow(worksheet.Dimension.End.Row, 1);
            }
        }

        public static bool IsLastRowEmpty(this ExcelWorksheet worksheet)
        {
            var empties = new List<bool>();

            for (var index = 1; index <= worksheet.Dimension.End.Column; index++)
            {
                var value = worksheet.Cells[worksheet.Dimension.End.Row, index].Value;
                empties.Add(value == null || string.IsNullOrWhiteSpace(value.ToString()));
            }

            return empties.All(e => e);
        }
    }
}
