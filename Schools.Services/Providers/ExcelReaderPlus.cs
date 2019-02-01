using System;
using System.Data;
using System.IO;
using OfficeOpenXml;
using Schools.Services.Infrastructure;

namespace Schools.Services.Providers
{
    public class ExcelReaderPlus
    {
        public ExcelReaderPlus(string filePath)
        {
            this.File = new FileInfo(filePath);
        }

        public FileInfo File { get; }

        public DataTable GetDataTable(string sheetName)
        {
            using (var package = new ExcelPackage(this.File))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    if (worksheet.Name.Equals(sheetName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return worksheet.ToDataTable(true);
                    }
                }

                return new DataTable("EmptyTable");
            }
        }

        public DataTable GetSchema()
        {
            using (var package = new ExcelPackage(this.File))
            {
                var table = new DataTable();
                table.Columns.Add(new DataColumn("Name", typeof(string)));
                table.Columns.Add(new DataColumn("Index", typeof(int)));

                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    var row = table.NewRow();
                    row[0] = worksheet.Name;
                    row[1] = worksheet.Index;

                    table.Rows.Add(row);
                }

                return table;
            }
        }
    }
}
