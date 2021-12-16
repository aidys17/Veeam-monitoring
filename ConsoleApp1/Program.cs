using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;

namespace ConsoleApp1
{
    class Program
    {
        //for timer
        private static void prockill(string pname, int ptime)
        {
            //find and kill
            File.AppendAllText("log.log", DateTime.Now.ToString() + ": Find process - " + pname + Environment.NewLine);
            Process[] procByName = Process.GetProcessesByName(pname);
            File.AppendAllText("log.log", DateTime.Now.ToString() + ":  Processes found - " + procByName.Length + Environment.NewLine);
            foreach (Process proc in procByName)
            {
                if ((DateTime.Now - proc.StartTime).TotalMinutes > ptime)
                {
                    proc.Kill();
                    proc.WaitForExit();
                    File.AppendAllText("log.log", DateTime.Now.ToString() + ": Close process - " + proc.ProcessName + Environment.NewLine);
                }
            }
        }

        static void Main(string[] args)
        {
            //log-file (check or create)
            if (File.Exists("log.log"))
            {
                try
                {
                    File.AppendAllText("log.log", DateTime.Now.ToString()+": Start application."+Environment.NewLine);
                }
                catch
                {
                    Console.WriteLine("Log-File does not overwrite");
                }
            }
            else
            {
                try
                {
                    File.AppendAllText("log.log", DateTime.Now.ToString() + ": Start application." + Environment.NewLine);
                }
                catch
                {
                    Console.WriteLine("Log-File does not create");
                }
            }
            Console.WriteLine("monitoring");
            //check argument
            if (args.Length==3)
            {
                if (!args[0].Intersect(System.IO.Path.GetInvalidFileNameChars()).Any())
                {
                    int t, p;
                    if (Int32.TryParse(args[1], out t) && Int32.TryParse(args[2], out p))
                    {
                        File.AppendAllText("log.log", DateTime.Now.ToString() + ": Arguments - ");
                        Console.WriteLine("Arguments - ");
                        foreach (string s in args)
                        {
                            File.AppendAllText("log.log", s + " ");
                            Console.WriteLine(s);
                        }
                        File.AppendAllText("log.log", Environment.NewLine);
                        //start
                        prockill(args[0], t);
                        //start period
                        System.Timers.Timer t1 = new System.Timers.Timer(p * 60 * 1000);
                        t1.Elapsed += delegate { prockill(args[0], t); };
                        t1.AutoReset = true;
                        t1.Enabled = true;
                        t1.Start();
                    }
                    else
                    {
                        File.AppendAllText("log.log", DateTime.Now.ToString() + ": Error (Time or Period): - " + args[1] + ", " + args[2] + Environment.NewLine);
                        Console.WriteLine("Error Arguments - Time or Period - "+args[1]+" "+args[2]);
                    }
                }
                else
                {
                    File.AppendAllText("log.log", DateTime.Now.ToString() + ": ProcessName - " +args[0] + Environment.NewLine);
                    Console.WriteLine("Error Argument - ProcessName - "+args[0]);
                }
            }
            else
            {
                File.AppendAllText("log.log", DateTime.Now.ToString() + ": Not Arguments"+Environment.NewLine);
                Console.WriteLine("Not 3 Arguments (ProcessName, Time, Period)");
            }
            //close
            Console.ReadLine();
            File.AppendAllText("log.log", DateTime.Now.ToString() + ": Close application." + Environment.NewLine);
        }
    }
}
