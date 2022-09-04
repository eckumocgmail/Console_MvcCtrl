 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Tools;

using RootLaunch;
using RootLaunch.Host.ServiceEndpoints;

 
using System.Diagnostics;
using System.IO;
 
using System.Reflection;

public class TaskRuntimeService
{

}
 
public class HostedController: WinSysAPI
{

    /// <summary>
    /// Singleton нужен будет
    /// </summary>
    public static HostedController Instance { get; private set; }

    private string NetCoreAppsDir = @"D:\Projects-Workers";
    private string AngularAppsDir = @"D:\Projects-Frontend";    


    public HashSet<Task> Running = new HashSet<Task>();
    public ClientApp[] GetClientApps() => GetClientAppsForDir(AngularAppsDir);    
    public ServerApp[] GetHostedApps( ) => GetSolutionsForDir(NetCoreAppsDir);



    /// <summary>
    /// Сведения и приложения
    /// </summary>
    public string GetConsoleApplications =>
       new CMD().CmdExec(@$"d: && cd d:\Projects-Console && dir /ad /b *");
    public string GetWorkersApplications =>
       new CMD().CmdExec(@$"d: && cd d:\Projects-Workers && dir /ad /b *");
    public string GetMvcApplications =>
       new CMD().CmdExec(@$"d: && cd d:\Projects-Mvc && dir /ad /b *");
    public string GetWwwApps =>
       new CMD().CmdExec(@$"d: && cd d:\Projects-Www && dir /ad /b *");



    public ServerApp GetHostedApp(string name )
    {
        return GetHostedApps().Where(app => app.GetName().IndexOf(name) != -1).FirstOrDefault();
    }
    
    public HostedController()
    {
        if (!System.IO.Directory.Exists(NetCoreAppsDir))
            throw new Exception("Необходимо поправить пути к проектам");
        OnClientApp(Info);

        if (!System.IO.Directory.Exists(AngularAppsDir))
            throw new Exception("Необходимо поправить пути к проектам");
        OnClientApp(Info);
    }


    public void OnClientApp(Action<ClientApp> todo)
    {
        Info("Просматриваем клиентские приложения");
        GetClientApps().ToList().ForEach(todo);
    }



    public void DoAction(Action todo)
    {
        Task run = null;
        Func<Task, Task> close = (task) =>
        {
            return Task.Run(() =>
            {
                Running.Remove(run);
            });
        };
        Running.Add(Task.Run(todo).ContinueWith(close));
    }


    /// <summary>
    /// Перед выполнение нужно уедится что разделяемые резурсы не заняты
    /// </summary>
    public void RunClientApps()
    {
        
        System.Console.WriteLine("Выполняем клиентские приложения");
        int port = 4200;
        var set = new List<Task>();
        foreach (var app in GetClientApps())
        {
          

            try
            {
                app.Build();
                //app.GetProjectProperties().WriteToConsole();
                app.SetPortNumberAndSave(port);
                //app.GetProjectProperties().WriteToConsole();
                //app.GetVersion().WriteToConsole();
                DoAction(() => {
                    app.TryRun(port = port + 1);
                });
                ConnectionsContext.Manifests.Add(app.GetManifest(port));
               
            }
            catch
            {
                continue;
            } 

        }
        Task.WaitAll(set.ToArray());
        
        
    }


    /// <summary>
    /// Выполнение 
    /// </summary>
    public void RunServerApps()
    {
        var set = new HashSet<Task>();
        int port = 9000;
        foreach (var app in GetHostedApps())
        {

            Info(app.GetName());
            app.Init();
        
           
            app.Build();
            app.GetProjectProperties().WriteToConsole();
            app.SetPortNumberAndSave(port);
            app.GetProjectProperties().WriteToConsole();
            app.GetVersion().WriteToConsole();
            set.Add(app.TryRun());
            ConnectionsContext.Manifests.Add(app.GetManifest(port));
            port = port + 1;
            

        } 
    }

 
    


    public ServerApp[] GetSolutionsForDir(string dir)
    {
        var apps = new List<ServerApp>();
        var slns = SearchApps(dir);
        foreach (string path in slns.Split("\n"))
        {
            if(string.IsNullOrEmpty(path)==false)
                apps.Add(new ServerApp(path));
        }
        return apps.ToArray();
    }


    public ClientApp[] GetClientAppsForDir(string dir)
    {
        var apps = new List<ClientApp>();
        System.IO.Directory.GetDirectories(dir).ToList().ForEach((subdir)=> {
            if (System.IO.File.Exists(subdir + ResourceManager.GetPathSeparator() + "angular.json"))
            {
                try
                {
                    Info(subdir);
                    apps.Add(new ClientApp(subdir));
                }
                catch(Exception ex)
                {
                    do
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.Source + " " + ex.TargetSite + "");
                        foreach(var kv in ex.Data)
                        {
                            Console.WriteLine($"{kv.ToString()}");
                        }
                        if (ex.StackTrace!=null)
                        {

                            
                            if( ex.StackTrace.Length > 256 )
                            {
                                Console.WriteLine(ex.StackTrace.Substring(0, 256));
                            } 
                        }
                        Console.WriteLine(ex.HelpLink);
                        
                        ex = ex.InnerException;
                    }
                    while (ex != null);
                   
                    Console.WriteLine("продолжаем?");
                    Console.ReadLine();
                }
            }
        });

        return apps.ToArray();
    }

    private void Info(object subdir)
    {
        Console.WriteLine(subdir);
    }
}






