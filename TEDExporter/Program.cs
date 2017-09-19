using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TEDExporter.DTO;

namespace TEDExporter
{
    class Program
    {
        static void Main(string[] args)
        {

            if (!Directory.Exists(@"C:\tedExportator"))
            {
                Directory.CreateDirectory(@"C:\tedExportator");
                Directory.CreateDirectory(Path.Combine(@"C:\tedExportator", ".cache"));
            }

            Welcome();
            Thread.Sleep(300);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Appuyez sur Entrée ...");

            Console.ReadKey();

            String[] nouveau = DoJob();

            Console.Clear();
            if (nouveau[0] != "")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                DoExportator(nouveau[0], nouveau[1]);
                Console.Read();
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < 5; i++)
                Console.WriteLine();
            Console.WriteLine("                 FERMETURE DE TED_EXPORTATOR");
            Thread.Sleep(1000);
        }

        private static void Welcome()
        {

            Console.ForegroundColor = ConsoleColor.White;

            Image(true);
        }
        public static string[] DoJob()
        {
            bool done = false;
            while (!done)
            {
                Image(false);
                String filePath = @"C:\tedExportator";
                String fileName = "";
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Veuillez placer votre fichier dans C:\\tedExportator,");
                Console.WriteLine("puis renseigner ci-après le nom de votre fichier : ");
                fileName = Console.ReadLine();
                if (File.Exists(Path.Combine(filePath, fileName)))
                {
                    done = true;
                    return new string[] { filePath, fileName };
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
            using (var writer = File.CreateText(Path.Combine(filePath, ".cache", "numerosTEDonLeankit.csv")))
            {
                writer.WriteLine("Numero;");
                foreach (var x in idNouveaux)
                {
                    writer.WriteLine(x.ToString() + ";");
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
        public static void Image(bool firstShow)
        {
            if (!firstShow)
            {
                Console.Clear();
            }
            if (File.Exists("./Res/Dessin.txt"))
            {
                using (StreamReader sr = new StreamReader(@"Res\Dessin.txt"))
                {
                    int i = 1;
                    while (sr.Peek() >= 0)
                    {

                        string line = sr.ReadLine();
                        if ((i >= 3 && i <= 5) || (i >= 11 && i <= 15))
                        {
                            for (int j = 0; j < line.Length; j++)
                            {
                                if (j >= 16 && j <= 60)
                                {
                                    Console.BackgroundColor = ConsoleColor.White;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.Write(line[j]);
                                }
                                else
                                {
                                    Console.ResetColor();
                                    Console.Write(line[j]);
                                }
                            }
                            Console.Write("\n");
                        }
                        else if (i >= 6 && i <= 10)
                        {
                            for (int j = 0; j < line.Length; j++)
                            {
                                if (j >= 16 && j <= 60)
                                {
                                    Console.BackgroundColor = ConsoleColor.White;
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write(line[j]);
                                }
                                else
                                {
                                    Console.ResetColor();
                                    Console.Write(line[j]);
                                }
                            }
                            Console.Write("\n");
                        }
                        else
                        {
                            Console.ResetColor();
                            Console.WriteLine(line);
                        }
                        if (firstShow)
                            Thread.Sleep(100);
                        i++;
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
    }
}
