using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

 
public class JavaScriptExecute
{
    private static string JAVA_SCRIPT_APPLICATION_FILE = 
        System.IO.Directory.GetCurrentDirectory()+"\\index.js";

      
    public static string Execute(string code)
    {
        return Execute(new string[1] { code });
    }

    public static string ExecuteFile(string file)
    {
        string output = CommandExecute("node " + file);

        return output.TrimEnd();
    }

    public static string Execute( string[] args )
    {
        WriteToFile(args);
        string output = CommandExecute("node " + JAVA_SCRIPT_APPLICATION_FILE);

        return output.TrimEnd();
    }

    private static void WriteToFile(string[] args)
    {
        string code = "";
        foreach (string arg in args)
        {
            code += arg + " \n";
        }
        System.IO.File.WriteAllText(JAVA_SCRIPT_APPLICATION_FILE, code);
    }

    private static string CommandExecute(string command)
    {
        ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + command);

        info.RedirectStandardError = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
        string response = process.StandardOutput.ReadToEnd();
        return response;
    }
}
 
