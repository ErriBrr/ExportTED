using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeanKit.API.Client.Library.TransferObjects;
using TEDExporter.DTO;
using TEDExporter.LeanKitAPI;
using TEDExporter.Map;

namespace TEDExporter
{
    static class WorkingClass
    {
        public static string[] DoJob()
        {
            bool done = false;
            while (!done)
            {
                Logo.Image(false);
                String filePath = @"C:\tedExportator";
                String fileName = "";
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Veuillez placer votre fichier dans C:\\tedExportator,");
                Console.WriteLine("puis renseigner ci-après le nom de votre fichier : ");
                fileName = Console.ReadLine();
                if (File.Exists(Path.Combine(filePath, fileName)))
                {
                    if (ControlColumns(Path.Combine(filePath, fileName)))
                    {
                        done = true;
                        return new string[] { filePath, fileName };
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Votre fichier ne respecte pas la norme des noms de colonne \n");
                        Console.WriteLine("Veuillez réessayer \n");
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("NORME :\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        foreach (string column in TED.Columns)
                        {
                            Console.Write(column + ";     ");
                        }
                        Console.ReadKey(false);
                    }
                }
                else
                {
                    bool correctKey = false;
                    while (!correctKey)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Le fichier n'existe pas");
                        Console.WriteLine("Tapez : ");
                        Console.WriteLine("         '1' - pour réessayer");
                        Console.WriteLine("         '2' - pour arrêter");

                        switch (Console.ReadKey().KeyChar)
                        {
                            case '1':
                                correctKey = true;
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case '2':
                                correctKey = true;
                                done = true;
                                break;
                            default:
                                Console.Clear();
                                break;
                        }
                    }
                }
            }
            return new string[] { "", "" };
        }

        public static bool ControlColumns(string filePath)
        {
            bool correct = true;
            string testColumns;
            using (StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding("iso-8859-1")))
            {
                testColumns = reader.ReadLine();
            }
            int i = 1;
            while (correct && i <= TED.Columns.Length)
            {
                foreach (string column in TED.Columns)
                {
                    if (!testColumns.Contains(";" + column + ";") && !testColumns.Contains(TED.Columns[0] + ";"))
                        correct = false;
                }
                i++;
            }
            return correct;
        }
        public static void DoExportator(String filePath, String nouveau)
        {
            String originalPath = @"Res\numerosTEDonLeankit.csv";
            if (File.Exists(Path.Combine(filePath, ".cache", "numerosTEDonLeankit.csv")))
                originalPath = Path.Combine(filePath, ".cache", "numerosTEDonLeankit.csv");

            List<ExportInit> dataOriginal;
            List<ExportTED> dataNew;

            using (CsvReader reader = new CsvReader(new StreamReader(originalPath, Encoding.Default)))
            {
                reader.Configuration.RegisterClassMap<CSvMapperInit>();
                reader.Configuration.Delimiter = ";";
                dataOriginal = reader.GetRecords<ExportInit>().ToList();
            }

            using (CsvReader reader = new CsvReader(new StreamReader(Path.Combine(filePath, nouveau), Encoding.Default)))
            {
                reader.Configuration.RegisterClassMap<CSvMapperTED>();
                reader.Configuration.Delimiter = ";";
                dataNew = reader.GetRecords<ExportTED>().ToList();
            }
            
            Helper lkWorker = new Helper();
            
            List<ExportTED> nouveauxTed = new List<ExportTED>();
            Console.WriteLine("en attente du service web ...");
            Action<object> action = (object obj) =>
            {
                //pour accelerer et eviter un appel serveur a chq appel a 'nouveauxTed'
                foreach (var x in dataNew.Where(x => lkWorker.DoesntExistCard(x)))
                {
                    nouveauxTed.Add(x);
                }
            };
            Task t2 = new Task(action, "?");
            //t2.RunSynchronously();
            t2.Start();
            t2.Wait();
            int numTedsNvx = nouveauxTed.Count();

            Console.Clear();
            Console.WriteLine("Il y a {0} nouveaux ted", numTedsNvx);

            IEnumerable<ImportLeanKit> resFSS = nouveauxTed.Where(x =>
                                                                    x.SousSysteme == "Prestations FSS" &&
                                                                    !x.Intitule.ToUpper().StartsWith("ESRC")
                                                                  ).Select(x => TedToLeaKitMapper.GetDTOForTED(x));
            if (resFSS.Any())
                ExportWriter(filePath, "FSS", resFSS);

            IEnumerable<ImportLeanKit> resESrc = nouveauxTed.Where(x =>
                                                                    x.SousSysteme == "eSRC" ||
                                                                    x.Intitule.ToUpper().StartsWith("ESRC")
                                                                  ).Select(x => TedToLeaKitMapper.GetDTOForTED(x));
            if (resESrc.Any())
                ExportWriter(filePath, "ESrc", resESrc);

            IEnumerable<ImportLeanKit> resTauri = nouveauxTed.Where(x =>
                                                                        x.SousSysteme.StartsWith("Actuaria") &&
                                                                        !x.Intitule.ToUpper().StartsWith("ESRC")
                                                                    ).Select(x => TedToLeaKitMapper.GetDTOForTED(x));
            if (resTauri.Any())
                ExportWriter(filePath, "Tauri", resTauri);

            IEnumerable<ImportLeanKit> resSCommun = nouveauxTed.Where(x =>
                                                                        x.SousSysteme == "ServicesCommuns" &&
                                                                        !x.Intitule.ToUpper().StartsWith("ESRC")
                                                                     ).Select(x => TedToLeaKitMapper.GetDTOForTED(x));
            if (resSCommun.Any())
                ExportWriter(filePath, "SCommun", resSCommun);

            //Normalement aucun conflit avec ESrc
            IEnumerable<ImportLeanKit> resJOH1 = nouveauxTed.Where(x =>
                                                                        x.SousSysteme == "Joachim"
                                                                     ).Select(x => TedToLeaKitMapper.GetDTOForTED(x));
            if (resJOH1.Any())
                ExportWriter(filePath, "JOH1", resJOH1);

            IEnumerable<ImportLeanKit> resIndetermine = nouveauxTed.Where(x =>
                                                                            !x.SousSysteme.StartsWith("Actuaria") &&
                                                                            x.SousSysteme != "Prestations FSS" &&
                                                                            x.SousSysteme != "eSRC" &&
                                                                            !x.Intitule.ToUpper().StartsWith("ESRC") &&
                                                                            x.SousSysteme != "Joachim" &&
                                                                            x.SousSysteme != "ServicesCommuns"
                                                                         ).Select(x => TedToLeaKitMapper.GetDTOForTED(x));
            if (resIndetermine.Any())
                ExportWriter(filePath, "Indetermine", resIndetermine);

            using (var writer = new StreamWriter(Path.Combine(filePath, ".cache", "numerosTEDonLeankit.csv")))
            {
                writer.WriteLine("Numero;");
                foreach (var x in dataOriginal.Select(x => x.Numero))
                {
                    writer.WriteLine(x + ";");
                }
                foreach (var x in nouveauxTed.Select(x => x.Numero))
                {
                    writer.WriteLine(x + ";");
                }
            }
        }
        private static void ExportWriter(string filePath, string domaine, IEnumerable<ImportLeanKit> res)
        {
            string nameFile = string.Format("{0}.csv", domaine);
            if (File.Exists(Path.Combine(filePath, nameFile)))
                File.Delete(Path.Combine(filePath, nameFile));
            using (var writer = File.CreateText(Path.Combine(filePath, nameFile)))
            {
                var csvWriter = new CsvWriter(writer);
                csvWriter.Configuration.Delimiter = ",";
                csvWriter.Configuration.QuoteAllFields = true;
                csvWriter.WriteRecords(res);
            }
            Console.WriteLine("Fichier créé : " + filePath + @"\" + nameFile + " \n avec " + res.Count() + " lignes");
        }
    }
}
