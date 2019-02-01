using System.IO;

namespace Schools.Console
{
    using System;
    using Schools.Services.Providers;

    internal class Program
    {
        private const string OdsFile = @"C:\Users\Dave\dev\Projects\Workbench\Holger\Resources\Schulliste_ohne AWB.xlsx";
        private const string CsvFile = @"C:\Users\Dave\dev\Projects\Workbench\Holger\Resources\test.csv";

        private static void Main(string[] args)
        {
            var e = new ExcelProvider();
            e.Read(OdsFile, "KiTa");

            var data = File.ReadAllBytes(CsvFile);


            var lines = File.ReadAllLines(CsvFile);

            using (var reader = new StreamReader(CsvFile, true))
            {
                reader.Read();
                Console.WriteLine(reader.CurrentEncoding);

            }



            var c = new CsvProvider();
            c.Read(CsvFile);
        }
    }
}
