using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TEDExporter
{
    static class Logo
    {
        public static void Image(bool firstShow)
        {
            if (!firstShow)
            {
                Console.Clear();
            }
            if (File.Exists("./Res/Dessin.txt"))
            {
                CancellationTokenSource sourceCancel = new CancellationTokenSource();
                Action<object> actionLogo = (object obj) =>
                {
                    using (StreamReader sr = new StreamReader(@"Res\Dessin.txt"))
                    {
                        int i = 1;
                        while (sr.Peek() >= 0)
                        {
                            if (sourceCancel.Token.IsCancellationRequested)
                                break;
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
                    if (firstShow && !sourceCancel.Token.IsCancellationRequested)
                    {
                        Thread.Sleep(300);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Appuyez sur Entrée ...");
                    }
                };
                Task t1 = new Task(actionLogo, "?");
                t1.Start();
                if (firstShow)
                {
                    Action<object> actionCancel = (object obj) =>
                    {
                        Console.ReadKey();
                        if (!t1.IsCompleted)
                        {
                            sourceCancel.Cancel();
                        }
                    };
                    Task t2 = new Task(actionCancel, "?");
                    t2.Start();
                    t2.Wait();
                }
                else
                {
                    t1.Wait();
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
