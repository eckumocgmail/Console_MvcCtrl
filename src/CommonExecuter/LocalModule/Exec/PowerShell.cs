using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ConsumerController;

namespace Tools
{
    public interface IActionExecuter
    {
        public string Execute(string action, IDictionary<string, object> args);
    }
    public class PowerShellExecuter : PowerShell, IActionExecuter
    {
        public class ActionRequest
        {
            public string ActionName { get; set; }
            public string ArgsJson { get; set; }
            public override string ToString() => this.ToJson();
        }

        public string FileExe { get; set; }
        public PowerShellExecuter(Type type) : base(type.Namespace.ReplaceAll(".",@"\\"))
        {
            FileExe = type.Name + ".exe";
        }

        public string Execute(string action, IDictionary<string, object> args)
        {

            return Execute(new ActionRequest() { ActionName= action, ArgsJson = args.ToJson() }.ToString());
        }
        private string Execute(ActionRequest request) => this.Run($"{FileExe} {request.ToJson()}", $"{FileExe}.txt");
    }

    public class PowerShell : IPowerShellService
    {
        
        public PowerShell(string wrk="C:\\")
        {
            _wrk = wrk;
        }

        public string _wrk { get; set; }

        public void SetWrk(string wrk)
        {
            _wrk = wrk;
        }

        

        public string Run(string command, string log)
        {
            return Execute(command+" > "+log);
        }
        public string Execute(string command)
        {
            System.Console.WriteLine("Execute command: " + command);
            //new PowerShell().Execute("Get-Command").ToJsonOnScreen().WriteToConsole();
            var t = new Thread(() =>
            {
                ProcessStartInfo info = new ProcessStartInfo("PowerShell.exe", $"d: && cd {_wrk} && " + command);
                info.RedirectStandardError = true;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;

                System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
                System.IO.StreamReader reader = process.StandardOutput;
                while(process.HasExited==false)
                {
                    string line;
                    while((line=process.StandardOutput.ReadLine())!=null)
                    {
                        Console.WriteLine(line);
                    }
                }
            });
            t.IsBackground = true;
            t.Start();
           
            return "Process Has Been Started";
        }
    }
}