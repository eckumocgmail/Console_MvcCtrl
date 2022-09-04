using AttrsReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using COM;

using StatReports.BusinessResources.DataADO.ADODataSource;

 
using System.Reflection;
using eckumoc_netcore_data;


public class DataProgram: SuperType
{
    public static string URL { get; set; } = System.IO.Directory.GetCurrentDirectory();
    public object ActionsMetaData { get; set; }

        
    public Dictionary<string, string> TypeMetaData { get; set; }

    internal static void OnRun()
    {
        ToMain(Environment.GetCommandLineArgs());
    }

    private Dictionary<string, Dictionary<string, string>> PropertiesMetaData { get; set; }


    public IDictionary<string, string> GetTypeAttributes()
        => AttrUtils.ForType(GetType());

    public IDictionary<string, IDictionary<string, string>> GetPropertiesAttributes()
        => AttrUtils.ForAllPropertiesInType(GetType());

    public IDictionary<string, string> GetMethodAttributes()
    {
        throw new NotImplementedException();
    }




    public static void ToMain(string[] args)
    {
        Run(args);
        var odbc = new OdbcDriverManager();
        odbc.GetOdbcDrivers();
        using(var sql= new SqlServerADODataSource())
        {
            sql.EnsureIsValide();                
        }
        Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args.ToList().Concat(new string[] {"URLS=https://127.0.0.1:4554" }.ToList()).ToArray())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                    
                webBuilder.UseStartup<StartupNetwork>();
            }).Build().Run();
    }


        
    static void Run(string[] args)
    {
        System.Console.WriteLine($"Вызов: {Assembly.GetCallingAssembly().GetName().Name }");
        System.Console.WriteLine($"Впроцессе: {Assembly.GetExecutingAssembly().GetName().Name }");

        foreach (var type in Assembly.GetExecutingAssembly().GetDataContexts())
        {
            var forProperties = ForAllPropertiesInType(type);
            var forMethods = ForAllMethodsInType(type);
            var forType = ForType(type);

            var superType = new DataProgram();
            superType.TypeMetaData = forType;
            superType.PropertiesMetaData = forProperties;
            superType.ActionsMetaData = forMethods;
            superType.Trace();
        }
    }

    private void Trace()
    {
        Writing.ToConsole("Type: ");
        foreach(var kv in this.TypeMetaData)
        {
            Writing.ToConsole($"\t{kv.Key}={kv.Value}");
        }
    }

    private static Dictionary<string,string> ForType(Type type)
    {
        throw new NotImplementedException();
    }

    private static IDictionary<string, string> ForAllMethodsInType(Type type)
    {
        throw new NotImplementedException();
    }

    private static Dictionary<string, Dictionary<string, string>> ForAllPropertiesInType(Type type)
    {
        throw new NotImplementedException();
    }

    
}




