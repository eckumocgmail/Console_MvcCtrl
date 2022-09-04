


using ApplicationCore.Converter.Models;
using CommonTests;
using CommonModule;
using DataADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
 
 

/// <summary>
/// Common-модель связывания по HTTP
/// </summary>
namespace CommonHttp
{


    /// <summary>
    /// Программа выполняет связывание поставщиков сетевых служб.
    /// </summary>
    public class HttpProgram
    {
        
    



        /// <summary>
        /// 
        /// </summary>        
        public static void Run(string[] args)
        {
            try
            {
                new CommonProgram(new SqlServerDDLFactory()).Run();
            }
            catch(Exception ex)
            {
                foreach(var line in ex.StackTrace.Split("   at"))
                {
                    Console.WriteLine(line.Trim());
                }
                Console.WriteLine("Нажмите любую клавишу для продолжения ... ");
                Console.ReadLine();
            }
            


            var unit = new CommonUnit();
            unit.DoTest().ToDocument().WriteToConsole();

            string origin = "http://localhost:5000";
            string apiFile = @"D:\Projects-MVC\Movie-Poster\movie-poster-api.json";            
            string json = apiFile.ReadText();
            var cmd = json.FromJson<MyApplicationModel>();
            cmd.Ping(origin).Wait();
        }

        private static void BindHttpService()
        {
            var app = new ServerApp(@"D:\Projects-MVC\Movie-Poster");
            app.GetWrk().WriteToConsole();
            app.Build();
            Console.WriteLine(app.GetExePath());
            app.RunAtNewProcess();
        }
    }
}
