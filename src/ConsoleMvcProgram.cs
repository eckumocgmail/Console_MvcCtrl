using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NameSpaceModule;
using System.IO;
using System.Linq;
using static EcKuMoC;
using COM;
using System.Collections.Generic;

/**
 * Выполнить из рабочей директории:
    dotnet add package MySql.Data
    dotnet add package Newtonsoft.Json
    dotnet add package Npgsql
    dotnet add package System.Data.Odbc
    dotnet add package MailKit
    dotnet add package Microsoft.Data.SqlClient
 */
public class ConsoleMvcProgram: CMD
{
    
    public static int Main(string[] args)
    {
        foreach(var arg in args)
        {
            
        }
        
        return 0;
    }
   
}













    //
    //CommandBuilder.OnRun();






    /*



    TestingProgram.OnRun();
    WatchProgram.OnRun();

    NetworkProgram.OnRun();

    CommandLineApplication.OnRun();


    Test();
    TestingProgram.Run(new Compile());

    var unit = new CommonUnit();
    Console.WriteLine("\n\n");
    string text = unit.DoTest().ToDocument().Substring(3);
    text.WriteToConsole();
    */
    //TcpSamples.Sample();
    //DataProgram.ToMain(args);
    


  
  

/// <summary>
/// Базовая конфигурация
/// </summary>
public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        var xrds = services.ToList();
        services.AddSingleton(typeof(NameServiceProvider), sp => new NameServiceProvider(sp, xrds));
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler("/ExceptionHandler");
        app.UseHttpsRedirection();
        UseLocalDir(app,@"www");

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapFallbackToController("Index", "Home");
        });
    }

    private static void UseLocalDir(IApplicationBuilder app, string dir, string path="")
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), dir)),
            RequestPath = new PathString(path)
        });
    }
}
