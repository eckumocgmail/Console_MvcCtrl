
using ApplicationCore.Converter.Models;
using ApplicationCore.Domain;

using CommonHttp.CommandLine;

using Newtonsoft.Json;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Tools;

using Transport;

namespace CommonHttp
{

    public class CommandLineApplication: MyApplicationModel
    {
        internal static void OnRun()
        {
            OnRun(new string[] {
                @"D:\Projects-MVC\MoviePoster"
            });
        }
        public static void OnRun(string[] args)
        {

            var test = new ServerAppTest();
            test.DoTest();


            var controller = new HostedController();
            if (Environment.UserInteractive)
            {

                //int port = 8081;
                var cmd = new CommandLineApplication();

                var app = new ServerApp();
                app.GetWrk().WriteToConsole();
                app.Build();


                cmd.Configure(
                    System.IO.File.ReadAllText(@"D:\1.json"));
                app.RunAtNewProcess();
                var commands = new CommandLineApplication();
                commands.LoadFile(@"D:\movie-poster-api.json");
                cmd.Run(new string[] {});

                var files = app.GetDllResource().GetParent().GetAllFiles().Where(f => f.NameShort.EndsWith("*.dll"));
                string dir = System.IO.Directory.GetCurrentDirectory();
                files.ToList().ForEach(r => r.Copy(dir));


                
                Assembly loaded = Assembly.Load(app.GetDllResource().ReadBytes()); 
                loaded.GetName().Name.WriteToConsole();
                var imported = new CommandLineAssembly(loaded);
                imported.InitChannels();
                foreach (var ctrl in imported)
                {
                    Console.WriteLine(ctrl.Value.Path);
                }

                app.TryRun(); 
                cmd.Configure(HttpService.Request($"{cmd.url}/ControllerGenerator/ShowModel"));
                cmd.Run(new string[] {@"D:\movie-poster-api.json"});
            }

        }

        
    }
}