
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ApplicationCore.Converter.Models;
using CommonHttp.CommandLine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApplicationUnit.CommonUnit
{
  
  
    
    [Route("[controller]/[action]")]
    public class CommonApiTestsController: Controller
    {

        public async Task Index(){
            /*var cmd = new CommandLineAssembly(Assembly.GetExecutingAssembly());
            HttpContext.Response.ContentType=("text/html;charset=UTF-8;");
            cmd.InitExecutable();*/
            var app=new MyApplicationModel();
            app.LoadFile(@"D:\1.json");
            await HttpContext.Response.WriteAsync(app.ToJson());
            
        }
        public CommonApiTestsController(ILogger<CommonApiTestsController> logger) : base()
        {
                              
            logger.LogInformation(this.GetHashCode().ToString());
                 //1
           /* var cmd = new MyApplicationModel();
            cmd.Configure(
                System.IO.File.ReadAllText(@"D:\Projects-Console\eckumoc-netcore-xml-cmd\"+
                @"movie-poster-api.json"));

            //2
            var startup = new FileController<MyApplicationModel>(
                 @"D:\Projects-Console\eckumoc-netcore-xml-cmd\movie-poster-api.json");
            var console = startup.Get();
            console.Run(new string[] { });*/

            //3
            /*var imported = new CommandLineAssembly(Assembly.GetCallingAssembly());
            imported.InitExecutable();
            foreach (KeyValuePair<string,MyControllerModel> ctrl in imported)
            {
                Console.WriteLine(ctrl.Value.Path);
            } */             
         
        }


    }
}
