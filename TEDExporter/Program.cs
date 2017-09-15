using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using TEDExporter.DTO;

namespace TEDExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Image(true);
            String nouveau = Welcome();

            Console.Clear();
            if (nouveau != "")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                DoExportator(nouveau);
                Console.Read();
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < 5; i++)
                Console.WriteLine();
            Console.WriteLine("                 FERMETURE DE TED_EXPORTATOR");
            Thread.Sleep(1000);
        }

        private static string Welcome()
        {
            bool done = false;
            while (!done)
            {
                Image(false);
                String filePath = "";
                Console.WriteLine(
                    "Veuillez renseigner ci-après le chemin menant à votre fichier .csv d'exportation de TED : ");
                filePath = Console.ReadLine();
                if (File.Exists(filePath))
                {
                    done = true;
                    return filePath;
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Le fichier n'existe pas");
                    Console.WriteLine("Tapez : ");
                    Console.WriteLine("         '1' - pour réessayer");
                    Console.WriteLine("         '2' - pour arrêter");
                    bool correctKey = false;
                    while (!correctKey)
                    {
                        switch (Console.ReadKey().KeyChar)
                        {
                            case '1':
                                correctKey = true;
                                break;
                            case '2':
                                correctKey = true;
                                done = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return "";
        }

        public static void DoExportator(String nouveau)
        {
            var filePath = @"C:\testImportTed";
            var originalPath = Path.Combine(filePath, "empty.csv");
            List<ExportTED> dataOriginal;
            List<ExportTED> dataNew;
            using (CsvReader reader = new CsvReader(new StreamReader(originalPath, Encoding.Default)))
            {
                reader.Configuration.RegisterClassMap<CSvMapperTED>();
                reader.Configuration.Delimiter = ";";

                dataOriginal = reader.GetRecords<ExportTED>().ToList();
            }

            using (CsvReader reader = new CsvReader(new StreamReader(nouveau, Encoding.Default)))
            {
                reader.Configuration.RegisterClassMap<CSvMapperTED>();
                reader.Configuration.Delimiter = ";";

                dataNew = reader.GetRecords<ExportTED>().ToList();
            }

            IEnumerable<int> idNouveaux = dataNew.Select(x => x.Numero);
            IEnumerable<int> idAnciens = dataOriginal.Select(x => x.Numero);
            IEnumerable<int> diff = idNouveaux.Except(idAnciens);
            IEnumerable<ExportTED> nouveauxTed = dataNew.Where(x => diff.Contains(x.Numero));
            Console.WriteLine("Il y a {0} nouveaux ted", nouveauxTed.Count());

            IEnumerable<ImportLeanKit> resFSS = nouveauxTed.Where(x => x.SousSysteme == "Prestations FSS" && !x.Intitule.ToUpper().StartsWith("ESRC")).Select(x => TedToLeaKitMapper.GetDTOForTED(x));
            if (resFSS.Any())
                ExportWriter(filePath, "FSS", resFSS);
            IEnumerable<ImportLeanKit> resESrc = nouveauxTed.Where(x => x.SousSysteme == "eSRC" || x.Intitule.ToUpper().StartsWith("ESRC")).Select(x => TedToLeaKitMapper.GetDTOForTED(x));
            if (resESrc.Any())
                ExportWriter(filePath, "ESrc", resESrc);
            IEnumerable<ImportLeanKit> resTauri = nouveauxTed.Where(x => x.SousSysteme.StartsWith("Actuaria") && !x.Intitule.ToUpper().StartsWith("ESRC")).Select(x => TedToLeaKitMapper.GetDTOForTED(x));
            if (resTauri.Any())
                ExportWriter(filePath, "Tauri", resTauri);
            IEnumerable<ImportLeanKit> resSCommun = nouveauxTed.Where(x => x.SousSysteme == "ServicesCommuns" && !x.Intitule.ToUpper().StartsWith("ESRC")).Select(x => TedToLeaKitMapper.GetDTOForTED(x));
            if (resSCommun.Any())
                ExportWriter(filePath, "SCommun", resSCommun);
            IEnumerable<ImportLeanKit> resIndetermine = nouveauxTed.Where(x =>
                                                                            !x.SousSysteme.StartsWith("Actuaria") &&
                                                                            x.SousSysteme != "Prestations FSS" &&
                                                                            x.SousSysteme != "eSRC" &&
                                                                            !x.Intitule.ToUpper().StartsWith("ESRC") &&
                                                                            x.SousSysteme != "ServicesCommuns"
                                                                         ).Select(x => TedToLeaKitMapper.GetDTOForTED(x));
            if (resIndetermine.Any())
                ExportWriter(filePath, "Indetermine", resIndetermine);
        }
        public static void ExportWriter(string filePath, string domaine, IEnumerable<ImportLeanKit> res)
        {
            string nameFile = string.Format(domaine + "{0}.csv", DateTime.Now.ToString("ddMMMMyyyy_h'h'm'm's"));
            using (var writer = File.CreateText(Path.Combine(filePath, nameFile)))
            {
                var csvWriter = new CsvWriter(writer);
                csvWriter.Configuration.Delimiter = ",";
                csvWriter.Configuration.QuoteAllFields = true;
                csvWriter.WriteRecords(res);
            }
            Console.WriteLine("Fichier créé : " + filePath + "/" + nameFile + " \n avec " + res.Count() + " lignes");
        }
        public static void Image(bool firstShow)
        {
            if (!firstShow)
            {
                Console.Clear();
                Console.ResetColor();
            }
            else
            {
                String filepath = @"C:\testImportTed";
                string path = filepath + "\\Dessin.txt";
                try
                {
                    if (File.Exists(path))
                    {
                        using (StreamReader sr = new StreamReader(path))
                        {
                            while (sr.Peek() >= 0)
                            {
                                Console.WriteLine(sr.ReadLine());
                                if (firstShow)
                                    Thread.Sleep(100);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("------------------------------------");
                        Console.WriteLine("---CGI Rennes-Orléans Cds Humanis---");
                        Console.WriteLine("------------------------------------");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
            }
        }
    }
}
