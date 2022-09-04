using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Console;
using CommonTests;

public class TestingProgram
{
    
    public static EcKuMoC.Compile Run(EcKuMoC.Compile build)
    {

        
        try
        {
            
            // executing        
            Assembly calling = Assembly.GetExecutingAssembly();
            var callingUnit = new AssemblyTest(calling);
            Console.WriteLine(callingUnit.ConsoleLoggerName);
            
            foreach(var line in callingUnit.DoTest().ToDocument().Split("\n"))
            {
                WriteLine(line);
            }
        }
        catch(Exception ex)
        {
            
            WriteLine($"{build.ToString()} => {ex.Message}");
        }
        
        return build;
    }

    internal static void OnRun()
    {
        System.IO.Directory.GetDirectories(@"D:\Projects-Mvc").ToJsonOnScreen().WriteToConsole();
        new CommonUnit().DoTest().ToDocument().WriteToConsole();
    }

    public static string[] Call(string[] args)
    {

        // executing        
        Assembly calling = Assembly.GetCallingAssembly();
        var callingUnit = new AssemblyTest(calling);


        var report = callingUnit.DoTest();
        var result = report.ToDocument().Split("\n");
        result.ToJsonOnScreen().WriteToConsole();
        return result;
    }

     
}
 