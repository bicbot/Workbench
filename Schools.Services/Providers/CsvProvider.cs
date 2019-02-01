using FileHelpers;
using Schools.Services.Models;

namespace Schools.Services.Providers
{
    public class CsvProvider
    {
        public School[] Read(string fileName)
        {
            var engine = new DelimitedFileEngine<School>();
            var list = engine.ReadFile(fileName);
            return list;
        }
    }
}
