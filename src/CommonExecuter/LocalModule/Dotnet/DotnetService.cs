using DataADO;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tools;
namespace Tools

{

    /// <summary>
    /// Выполнение инструкции через командную строку
    /// </summary>
    public class DotnetService :   IDotnet, IDotnetEF
    {

        private string _wrk;
        private Tools.DOS _dos;

        public DotnetService() { }
        public void SetWrk(string wrk=@"C:\")
        {
            if (System.IO.Directory.Exists(wrk) == false)
            {
                System.IO.Directory.CreateDirectory(wrk);
            }
            _wrk = wrk;
            _dos = new Tools.DOS(_wrk);
        }

        public string AddPackage(string package)
        {
            string result = ExecCommand($"dotnet add package {package}");
            return result;
        }

        public string CreateApplication(DotnetTemplates template)
        {
            string result = ExecCommand($"dotnet new {ToString(template)} --force");
            return result;
        }

        public static string ToString(DotnetTemplates template)
        {
            switch (template)
            {
                case DotnetTemplates.nunitTest: return "nunit-test";
                case DotnetTemplates.toolManifest: return "tool-manigest";
                default: return template.ToString();
            }
        }

        public string ExecCommand(string command)
        {
            //@$"cd {_wrk} && {command}"
            string message = new CMD().CmdExec (@$"cd {_wrk} && {command}");  // _dos.Execute(command);
            return message;//
            
        }

        public string ProjectFiles( )
        {
            //@$"cd {_wrk} && {command}"
            string message = new CMD().CmdExec(@$"cd {_wrk} && dir *.csproj"); ;// _dos.Execute(command);
            return message;//

        }



        public string ListMigrations(string context)
        {
            string command = $@"dotnet ef migrations list --no-build --context {context}";
            return ExecCommand(command);
        }

        public Task Run()
        {
            string command = $@"dotnet run";
            return Task.Run(() =>
            {
                ExecCommand(command);
            });
        }

        public string CreateMigration(string context, string name)
        {
            string command = $@"dotnet ef migrations add {name} --context {context} --no-build";
            return ExecCommand(command);
        }

        public string Build()
        {
            ProjectFiles().WriteToConsole();
            string command = $@"dotnet build";
            return ExecCommand(command);
        }

        public void CreateRazorComponent(string componentNamer)
        {
            string command = $@"dotnet new razorcomponent -o {componentNamer} --no-build";
            ExecCommand(command);
        }
        public string DropDatabase(string context)
        {
            ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " +
                        @$"cd {_wrk} && dotnet ef database drop --context {context} --no-build");

            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            return process.StandardOutput.ReadToEnd();
        }
        public string UpdateDatabase(string context, string name)
        {
            ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " +
                        @$"cd {_wrk} && dotnet ef database update --context {context} --no-build");

            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            return process.StandardOutput.ReadToEnd();
        }

        public string GenerateFromDatabase(string connectionString, DotnetEFProviders provider)
        {
       
            switch(provider)
            {
                case DotnetEFProviders.SqlServer:
                    return ExecCommand($"dotnet ef dbcontext scaffold \"{connectionString}\" Microsoft.EntityFrameworkCore.SqlServer");
                default:
                    throw new Exception("Пока ещё не поддерживается обратное проектирование для поставщика "+provider);

            }
            
        }

        public string CreateMigration(string name)
        {
            string command = $@"dotnet ef migrations add {name} --no-build";
            return ExecCommand(command);
        }

        public string UpdateDatabase( )
        {
            string command = $@"dotnet ef database update --no-build";
            return ExecCommand(command);
        }
    }


}