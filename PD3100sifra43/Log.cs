using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PD3100sifra43
{
    class Log
    {
        public static void Write(string message)
        {
            Console.WriteLine(message);
        }
        public static void Write(string message, object p1)
        {
            message = String.Format(message, p1);
            Console.WriteLine(message);
            writeLog(message, "");
        }
        public static void Write(string message, object p1, object p2)
        {
            message = String.Format(message, p1, p2);
            Console.WriteLine(message);
            writeLog(message, "");
        }
        public static void Write(string message, object p1, object p2, object p3)
        {
            message = String.Format(message, p1, p2, p3);
            Console.WriteLine(message);
            writeLog(message, "");
        }
        public static void toLogFile(string txt)
        {
            writeLog(txt, "");
        }
        private static void writeLog(string txt, string sufix)
        {
        //    if (!Directory.Exists("log"))
        //        Directory.CreateDirectory("log");
            try
            {
               // string fname = System.IO.Path.GetFullPath("log/" + DateTime.Now.ToShortDateString() + ".log");
                
                string pathLog = "C:\\PD3100RazmjenaPU\\Log\\";
                if (!Directory.Exists(pathLog))
                    Directory.CreateDirectory(pathLog);
                string fname = pathLog + DateTime.Now.ToShortDateString() + ".log";

                if (!File.Exists(fname))
                {
                    FileStream fs = File.Create(fname);
                    fs.Close();
                }
                StreamWriter writer = File.AppendText(fname);
                writer.WriteLine(DateTime.Now.ToString("dd-MM-yyyy @ HH:mm:ss  ") + ": " + sufix + txt);
                writer.Close();
            }
            catch (Exception ex)
            {
                //
            }
        }
    }
}
