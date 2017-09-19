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
            Console.Clear();

            String[] nouveau = WorkingClass.DoJob();
            Console.Clear();
            if (nouveau[0] != "")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                WorkingClass.DoExportator(nouveau[0], nouveau[1]);
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
            Logo.Image(true);
        }
    }
}
