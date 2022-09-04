using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


public interface IWinSysAPI
{
    void CloseWindow(string title);
    string GetUsername();
    string GetUsernameProfile();
    IEnumerable<string> GetWindowTitles();
    void OpenCodeEditor(string filepath);
    string SearchApps(string path);
    string SearchAppsInUserDir();
    string SearchSPAs(string dir);
}
public class WinSysAPI : IWinSysAPI
{
    public string GetUsername()=> new CMD().CmdExec("set USERNAME");




    public string GetUsernameProfile() => new CMD().CmdExec("set USERPROFILE");
    

    public string SearchAppsInUserDir()
    {
        string userProfile = GetUsernameProfile();
        userProfile = userProfile.ReplaceAll("\n", "").ReplaceAll("\r", "").ReplaceAll(@"\\", @"\").ReplaceAll(@"//", @"/");
        return new CMD().CmdExec(@$"cd {userProfile.Substring("USERPROFILE=".Length)} && dir /b /s *.sln");
    }


    public static void EditTextFile(string filepath)
    {
        filepath = filepath.Trim();
        if (System.IO.File.Exists(filepath))
        {
            var p = Process.Start("notepad", filepath);
            p.WaitForExit();
            p.Kill();
        }

    }


    public void CloseWindow(string title)
    {
        Process.GetProcesses().ToList().ForEach(p =>
        {
            if (p.MainWindowTitle.IndexOf(title) != -1)
            {
                p.CloseMainWindow();
            }
        });
    }

    public string SearchApps(string path)
    {
        string userProfile = GetUsernameProfile();
        userProfile = userProfile.ReplaceAll("\n", "").ReplaceAll("\r", "").ReplaceAll(@"\\", @"\").ReplaceAll(@"//", @"/");
        string result = new CMD().CmdExec(@$"{path.Substring(0, 2)} && cd {path} && dir /b /s *.sln");
        return result;
    }

    public string SearchSPAs(string dir)
    {
        string userProfile = GetUsernameProfile();
        userProfile = userProfile.ReplaceAll("\n", "").ReplaceAll("\r", "").ReplaceAll(@"\\", @"\").ReplaceAll(@"//", @"/");
        return new CMD().CmdExec(@$"{dir.Substring(0, 2)} && cd {dir} && dir /b angular.json");
    }
    public IEnumerable<string> GetWindowTitles()
    {

        return Process.GetProcesses().Select(p => p.MainWindowTitle);
    }

    public void OpenCodeEditor(string filepath)
    {
        new CMD().StartProcess($"vscode {filepath}");
    }

    [DllImport("User32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr h, string m, string c, int type);


    public static void InfoDialog(string title, string text)    
        => MessageBox((IntPtr)0, text, title, 0);

    
}

