using CsvHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TEDExporter.DTO;

namespace TEDExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"C:\testImportTed";
            var originalPath = Path.Combine(filePath, "original.csv");
            var nouveau = Path.Combine(filePath, "nouveau.csv");
            List<ExportTED> dataOriginal = null;
            List<ExportTED> dataNew = null;
            using (var reader = new CsvReader(new StreamReader(originalPath, Encoding.Default)))
            {
                reader.Configuration.RegisterClassMap<CSvMapperTED>();
                reader.Configuration.Delimiter = ";";

                dataOriginal = reader.GetRecords<ExportTED>().ToList();
            }

            using (var reader = new CsvReader(new StreamReader(nouveau, Encoding.Default)))
            {
                reader.Configuration.RegisterClassMap<CSvMapperTED>();
                reader.Configuration.Delimiter = ";";

                dataNew = reader.GetRecords<ExportTED>().ToList();
            }


        }
    }
}
