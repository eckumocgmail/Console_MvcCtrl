


using COM;



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

public interface ICmd
{
    public void StartProcess(string command);
    public string CmdExec(string command);

    public string Search(string path, string pattern);
}

public class CMD : ConsoleLogger, ICmd, ICMD
{
    public CMD()
    {
    }



    public string Find(string regularExpression, string filename)
    {
        return CmdExec("findstr /n /r " + regularExpression + " " + filename);
    }


    public Dictionary<string, object> Search(string regularExpression)
    {
        Dictionary<string, object> searchResults = new Dictionary<string, object>();
        foreach( string file in System.IO.Directory.GetFiles("documentation"))
        {
            //string result = this.Execute("findstr /n /r " + regularExpression + " "+file,(line)=> { return 1; });
            string[] results = null;// result.Split("\n");
            searchResults[file] = new HashSet<string>(results);
        }
        return searchResults;
    }


    /// <summary>
    /// Выполнение инструкции через командную строку
    /// </summary>
    /// <param name="command"> команда </param>
    /// <returns></returns>
    public string CmdExec(string command)
    {
        command = command.ReplaceAll("\n", "").ReplaceAll("\r", "").ReplaceAll(@"\\", @"\").ReplaceAll(@"//", @"/");

        ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + command);

        info.RedirectStandardError = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
        string response = process.StandardOutput.ReadToEnd();
        string result = response.ReplaceAll("\r", "\n");
        result = result.ReplaceAll("\n\n", "\n");
        while (result.EndsWith("\n"))
        {
            result = result.Substring(0, result.Length - 1);
        }
        return result;
    }


    public string Search(string path, string pattern)
    {
        return CmdExec(@$"{path.Substring(0, 2)} && cd {path} && dir /b /s *.sln");
    }


    /// <summary>
    /// Выполнение инструкции через командную строку
    /// </summary>
    /// <param name="command"> команда </param>
    /// <returns></returns>
    public void StartProcess(string command)
    {
        Thread work = new Thread(new ThreadStart(() =>
        {
            ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + command);

            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            string response = process.StandardOutput.ReadToEnd();

        }));
        work.IsBackground = true;
        work.Start();
    }
}

public class CMDTest : TestingElement
{
    protected override void OnTest()
    {
        var cmd = new CMD();
        string output = cmd.Search(@"d:\commands", "*.bat");
        System.Console.WriteLine(output);
        Messages.Add("Умеем выполнять поиск по содержимому текстовых файлов исп. командную утилиту");
    }
}