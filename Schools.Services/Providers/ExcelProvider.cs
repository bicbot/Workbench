using System.Collections.Generic;
using System.Data;
using Schools.Services.Models;

namespace Schools.Services.Providers
{
    public class ExcelProvider
    {
        public List<Sheet> ReadSchema(string filePath)
        {
            var reader = new ExcelReaderPlus(filePath);
            var table = reader.GetSchema();
            var sheets = new List<Sheet>();

            foreach (DataRow row in table.Rows)
            {
                sheets.Add(new Sheet
                {
                    Name = row.Field<string>(0).Replace("'", "").Replace("$", ""),
                    Index = row.Field<int>(1)
                });
            }

            return sheets;
        }

        public SchoolList Read(string filePath, string sheetName)
        {
            var reader = new ExcelReaderPlus(filePath);
            var table = reader.GetDataTable(sheetName);

            return ReadTable(table, sheetName);
        }

        private static SchoolList ReadTable(DataTable table, string sheetName)
        {
            var schools = new SchoolList();
            foreach (DataRow row in table.Rows)
            {
                if (sheetName == "KiTa")
                {
                    var school = GetSchool(row, 5, 4);
                    if (school != null)
                    {
                        schools.Add(school);
                    }
                }
                else
                {
                    var school = GetSchool(row, 13, 12);
                    if (school != null)
                    {
                        schools.Add(school);
                    }
                }
            }

            return schools;
        }

        private static School GetSchool(DataRow row, int nameColumnIndex, int idColumnIndex)
        {
            if (row.ItemArray.Length <= nameColumnIndex
                || string.IsNullOrEmpty(row[nameColumnIndex].ToString()))
            {
                return null;
            }

            return new School()
            {
                Uid = row[idColumnIndex].ToString(),
                SchoolFullName = row[nameColumnIndex].ToString().Replace("$", "")
            };
        }
    }
}
