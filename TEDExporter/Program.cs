using CsvHelper;
using System;
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



            var idNouveaux = dataNew.Select(x => x.Numero);
            var idAnciens = dataOriginal.Select(x => x.Numero);
            var diff = idNouveaux.Except(idAnciens);
            var nouveauxTed = dataNew.Where(x => diff.Contains(x.Numero));
            Console.WriteLine("Il y a {0} nouveaux ted", nouveauxTed.Count());
            Console.Read();

            var res = nouveauxTed.Select(x => TedToLeaKitMapper.GetDTOForTED(x));


            using (var writer = File.CreateText(Path.Combine(filePath, string.Format("{0}.csv", DateTime.Now.Ticks))) ){
                var csvWriter = new CsvWriter(writer);
                csvWriter.Configuration.Delimiter = ",";
                csvWriter.Configuration.QuoteAllFields = true;
                csvWriter.WriteRecords(res);
            }
        }
    }
}
