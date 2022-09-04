using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tools
{
    public class DOS : IDOS
    {
        private static readonly string DEFAULT_BAT_FILENAME = @"D:\DOS.BAT";
        private static readonly string DEFAULT_RUN_FILENAME = @"D:\TODO_DOS.BAT";
        private static readonly string DEFAULT_OUTPUT_FILENAME = @"D:\DOS.LOG";
        private string _wrk;
        private string batFilePath;
        private string outputFilePath;

        public DOS(string wrk=@"C:\")
        {
            this._wrk = wrk;
            this.batFilePath =  DEFAULT_BAT_FILENAME;
            this.outputFilePath =  DEFAULT_OUTPUT_FILENAME;
        }

        public void Set(string batFilePath, string outputFilePath)
        {
            this.batFilePath = batFilePath;
            this.outputFilePath = outputFilePath;
        }

        public string Execute(string command)
        {
            try
            {
                Thread.Sleep(2);
                System.IO.File.WriteAllText(batFilePath, command );
                string log = $@"D:\log_{DateTimeOffset.Now.Millisecond}.txt";
                System.IO.File.WriteAllText(DEFAULT_RUN_FILENAME, batFilePath + " > " + log);

                ProcessStartInfo info = new ProcessStartInfo(@"powershell.exe", "/C " + DEFAULT_RUN_FILENAME);
                System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
                process.WaitForExit();
                string result = System.IO.File.ReadAllText(log, Encoding.UTF8);
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception("Не удалось выполнить команду: "+command+ " "+ex.Message);
            }
        }

    }
}