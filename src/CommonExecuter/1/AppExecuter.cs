

using System;
using System.Diagnostics;
using System.Threading;

namespace ApplicationCore.Domain
{
    public class AppExecuter 
    {
        /// <summary>
        /// Выполнение инструкции через командную строку
        /// </summary>
        /// <param name="command"> команда </param>
        /// <returns></returns>
        public void ExecuteBackground( string command, Func<string, int> listener )
        {
            Thread work = new Thread(new ThreadStart(() => {
                listener("starting execute ... ");
                ProcessStartInfo info = new ProcessStartInfo("powershell", "" + command);

                System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
                info.RedirectStandardError = true;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;

                listener("read stdout ... ");
                string line;
                while((line=process.StandardOutput.ReadLine())!=null)
                {
                    if (string.IsNullOrEmpty(line) == false)
                    {
                        listener(line);
                    }
                }

                
                
            }));
            work.IsBackground = true;
            work.Start();
        }




        public string Execute(string command)
        {
            ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + command);

            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            string response = process.StandardOutput.ReadToEnd();
            return response;
        }

        
    }
}
