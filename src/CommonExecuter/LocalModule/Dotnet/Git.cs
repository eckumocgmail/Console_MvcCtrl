using RootLaunch;

using System;
using static EcKuMoC;
using System.Diagnostics;
using System.IO;



public class GitUnit: TestingElement
{
    protected override void OnTest()
    {
        var git = new Git(@"D:\System-Config");
        canInit(git);
        canInit(git);
    }

    private void canInit(Git git)
    {
        try
        {
            string cmd = git.Init();
        }
        catch(Exception ex)
        {
            this.Messages.Add("Инициаллизация репозиторий завершилась с ошибкой: "+ex.Message);
            WriteLine(ex);
        }

    }
}


public interface IGit : ConsoleMvcApi.IRepository
{

    void SetWrk(string wrk);
    string UpdateDatabase(string context, string name);
}
public class Git : IGit
{
    private CommonNode<DirectoryResource> _root;
    private DirectoryResource _resource;
    private CMD _cmd = new CMD();
    private string _wrk;

    public Git() : this(System.IO.Directory.GetCurrentDirectory()) { }
    public Git( string wrk )
    {
        SetWrk(wrk);
    }




    public void SetWrk(string wrk)
    {
        if (System.IO.Directory.Exists(wrk) == false)
        {
            throw new Exception($"Не существует директории: {wrk}");
        }
        this._resource = new DirectoryResource(_wrk = wrk);
        _root = new CommonNode<DirectoryResource>("Root", this._resource, null);
        _cmd = new CMD();
    }


    public string Init()
    {
        ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " +
                    @$"cd {_wrk} && git init");

        info.RedirectStandardError = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
        return process.StandardOutput.ReadToEnd();
    }





    public string Commit(string message)
    {
        ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " +
                    @$"cd {_wrk} && git add * && git commit -m \'{message}'");

        info.RedirectStandardError = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);

        return process.StandardOutput.ReadToEnd();
    }








    public string UpdateDatabase(string context, string name)
    {
        ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " +
                    @$"cd {_wrk} && dotnet ef database update --context {context} --no-build");

        info.RedirectStandardError = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
        return process.StandardOutput.ReadToEnd();
    }




    public string Add(string path)
    {
        new FileResource(path);
        throw new NotImplementedException();
    }

    public string Pull()
    {
        throw new NotImplementedException();
    }

    public string Pull(string remote)
    {
        throw new NotImplementedException();
    }

    public string Rebase()
    {
        throw new NotImplementedException();
    }

    public string Rebase(string remote)
    {
        throw new NotImplementedException();
    }

    public string Push()
    {
        throw new NotImplementedException();
    }

    public string Push(string remote)
    {
        throw new NotImplementedException();
    }

    public string Checkout()
    {
        throw new NotImplementedException();
    }

    public string Checkout(string branch)
    {
        throw new NotImplementedException();
    }

    public string Branch()
    {
        throw new NotImplementedException();
    }

    public string Branch(string branch)
    {
        throw new NotImplementedException();
    }

    public string Version()
    {
        throw new NotImplementedException();
    }
}
