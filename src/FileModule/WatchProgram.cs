using Newtonsoft.Json;

using System;
using System.Reflection;

using WatchModule;


public class WatchProgram
{
    public static void OnRun() => OnRun(new string[] { 
        System.IO.Directory.GetCurrentDirectory(),
        "dotnet build"
    });
    public static void OnRun(string[] args) => Run(args);
       
    public static void Run(string[] args)
    {

        Console.WriteLine($"Аргументы " + $"{Assembly.GetExecutingAssembly().GetName().Name}: ");
        foreach (string arg in args)
        {
            Console.WriteLine(arg);
        }

        Console.WriteLine($"\nПуть: ");
        string wrk = (args.Length != 3) ? Console.ReadLine() : args[0];
            
        Console.WriteLine($"\nКоманда: ");
        string command = (args.Length != 3) ? Console.ReadLine() : args[2];
        Console.WriteLine($"{command}");
        string filter = (args.Length != 3) ? "*.*" : args[1];
        var watcher = new FileWatcher(wrk);
        Run(command, filter, watcher);
    }

    public static void Run(string command, string filter, FileWatcher watcher)
    {
        Console.WriteLine($"\nПуть: {watcher.Path}");
        Console.WriteLine($"\t{filter}");
        var cmd = new Cmd();
        watcher.Watch((evt) =>
        {
            cmd.Execute($"{command} {JsonConvert.SerializeObject(evt)}");
        }, filter);

        Console.WriteLine("Нажмите 'q' для завершения.");
        while (Console.Read() != 'q') ;
    }
}
