  
using RootLaunch;
using RootLaunch.Host.ServiceEndpoints;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Tools;

public class ServerAppTest : TestingElement
{
    protected override void OnTest()
    {
        var app = new ServerApp(@"D:\Projects-MVC\Movie-Poster");
        app.ToJsonOnScreen().WriteToConsole();
        app.Build();
        var resource = app.GetDllResource();
        var assemblyImage = resource.ReadBytes();
        var distr = Assembly.Load(assemblyImage);
        distr.GetTypes().Select(t => t.Name).ToList().ForEach(app.Info);
    }


}



public class ServerApp : BaseComponent, IServerApp
{

    protected string RN = ResourceManager.GetPathSeparator();

    protected FileController<AppManifest> ManifestController { get; private set; }
    public FileResource RootController { get; private set; }
    public AppManifest manifest { get => this.ManifestController.Get(); }




    protected IDotnet dotnet;
    protected string _wrk;
    protected Git git;

    /// <summary>
    /// Корневая директория проекта, должна содержать файл *.csproj
    /// </summary>
    protected DirectoryResource rootResources;
    private readonly string _path;

    public void copyDllsToWrk()
    {
        var app = new ServerApp(@"D:\Project-MVC\Movie-Poster");
        var files = app.GetDllResource().GetParent().GetAllFiles().Where(f => f.NameShort.EndsWith("*.dll"));
        string dir = System.IO.Directory.GetCurrentDirectory();
        files.ToList().ForEach(r => r.Copy(dir));
    }

    public string GetName() => this.manifest.Name;
    public string GetVersion() => this.manifest.Version;



    public ServerApp(string path)
    {
        this._path = path;
        this._wrk = path;
        this.Init();
    }

    public ServerApp() : this(System.IO.Directory.GetCurrentDirectory())
    {

    }

    public void Import()
    {
        Assembly loaded = Assembly.Load(GetDllResource().ReadBytes());
        loaded.GetName().Name.WriteToConsole();
    }


    DirectoryResource distrResources;
    public string DefaultDistDir { get; } = @"D:\Applications";

    public FileResource GetDllResource()
    {

        this.distrResources = DirectoryResource.Get(GetDistrPath());
        var dlls = this.distrResources.GetAllFiles();
        var name = this.GetName();

        var dll = dlls
            .Where(f => f.NameShort.Equals(this.GetName() + ".dll")).FirstOrDefault();
        if (dll == null)
        {
            throw new Exception("Не удалось найти файл " + this.GetName() + ".dll" + "в списке " + dlls.Select(d => d.NameShort).ToJsonOnScreen());
        }
        return dll;
    }


    /// <summary>
    /// Проверка данных проекта:
    /// -рабочая директория,
    /// -путь к exe
    /// -и т.д.
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> ValidateApp()
    {

        var dic = new Dictionary<string, string>();
        while (true)
        {
            try
            {
                GetDistrPath();
                break;
            }
            catch (Exception ex)
            {
                ex.Message.ToString().WriteToConsole();
                continue;
            }
        }

        return dic;
    }

    public string GetExePath(string f = "net6.0")
    {
        try
        {
            ValidateApp();
            var dll = GetDistrPath() + "\\" + f + ResourceManager.GetPathSeparator() + (this.GetName().Replace("-", "")) + ".exe";
            return dll;
        }
        catch (Exception ex)
        {
            throw new Exception("Не удалось определить путь к исполняемому файлу проекта", ex);
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetDistrPath()
    {
        try
        {
            var csprojFile = this.rootResources.GetFiles().Where(f => f.Path.EndsWith(".csproj")).Single();
            var ProjectFileXml = csprojFile.ReadText();
            int i = ProjectFileXml.IndexOf("OutputPath");
            string res = null;
            if (i == -1)
            {
                res = this.GetWrk() + @$"{ResourceManager.GetPathSeparator()}bin{ResourceManager.GetPathSeparator()}Debug";
            }
            else
            {

                res = GetWrk() + "\\bin\\Debug\\net5.0";

            }

            if (System.IO.Directory.Exists(res) == false)
            {
                string mes =
                "Не существует директории\n " + res + "\n" +
                    "Исправьте путь к скомпилированным ресурсам проекта";
                throw new Exception(mes);
            }
            return res;


        }
        catch (Exception ex)
        {
            throw new Exception("Не удалось прочтать файл проекта: " + ex.Message, ex);
        }



    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string Build()
    {
        this.Info("Build() ... ");
        this.manifest.Build++;
        this.manifest.Version = "1.0.0.0." + this.manifest.Build;
        this.ManifestController.Set();

        Console.WriteLine(this.GetDistrPath());

        string log = this.dotnet.Build();

        this.Info(log);

        //упаковка исходных файлов
        this.Pack($"{DefaultDistDir}{RN}{GetName()}_{GetVersion().ReplaceAll(".", "_")}");


        return log;
    }

    /// <summary>
    /// /
    /// </summary>
    /// <param name="dir"></param>
    private void Pack(string dir)
    {

        this.Info("Pack() ... ");
        if (System.IO.Directory.Exists(dir))
        {
            throw new Exception($"Директория {dir} уже существует. Предполагается её нету ещё");
        }
        System.IO.Directory.CreateDirectory(dir);
        ListSourceFiles().ToList().ForEach(src =>
        {

            try
            {
                Info(src);
                string catalog = src;
                while (true)
                {
                    catalog = ResourceManager.GetParentPath(catalog);
                    string pdir = catalog.Replace(_wrk, dir);
                    if (System.IO.Directory.Exists(pdir.Trim()) == false)
                    {
                        System.IO.Directory.CreateDirectory(pdir.Trim());
                    }
                    else
                    {
                        break;
                    }
                }

                string to = src.Replace(_wrk, dir);


                Info(src + " " + to);
                System.IO.File.Copy(src, to.Trim());
            }
            catch (Exception ex)
            {
                Info("Ошибка при копировании файла: " + src);
                Error(ex);
            }



        });
    }

    public void Error(Exception ex)
    {
        throw new NotImplementedException();
    }

    public void Info(object v)
    {
        Console.WriteLine(v);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /*public CancellationToken Run()
    {
        Info("Run()");
        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;
        ThreadPool.QueueUserWorkItem(new WaitCallback(Startup), token);            
        return token;                       
    }*/


    void Startup(object obj)
    {
        CancellationToken token = (CancellationToken)obj;
        for (int i = 0; i < 100000; i++)
        {
            if (token.IsCancellationRequested)
            {
                System.Console.WriteLine("In iteration {0}, cancellation has been requested...", i + 1);
                break;
            }
            Thread.SpinWait(500000);
        }
    }

    public void SetPortNumberAndSave(int port)
    {
        DirectoryResource dir = this.rootResources.GetOrCreateDirectory("Properties");
        string filename = dir.Path + ResourceManager.GetPathSeparator() + "launchSettings.json";
        if (System.IO.File.Exists(filename))
        {
            System.IO.File.WriteAllText(filename + ".temp", System.IO.File.ReadAllText(filename));
        }
        JsonEditor.Post(filename, new
        {
            profiles = new Dictionary<string, object>() {
            { GetName(),new StartupProfile(port)}
        }
        }.ToJsonOnScreen());

    }

    public AppManifest GetManifest(int port)
    {
        manifest.Port = port;
        return manifest;
    }

    public Task TryRun()
    {
        this.Init();
        return this.dotnet.Run();
    }

    public string GetState()
    {
        throw new NotImplementedException();
    }



    public string GetWrk()
    {
        return _wrk;
    }



    public override void Init()
    {

        //определяем какой путь передан в качестве аргумента в конструктор и инициаллизируем
        //файловые ресурсы
        this.rootResources = DirectoryResource.Get(this._path);
        /*FileResource resource = new FileResource(this._path);
        if (resource.IsFile)
        {
            this._wrk = ResourceManager.GetParentPath(this._path);
            this.rootResources = DirectoryResource.Get(this._wrk);
        }
        else if (resource.IsDirectory)
        {
            this._wrk = this._path;
            this.rootResources = DirectoryResource.Get(this._wrk);

        }
        else
        {
            throw new Exception("Проверьте правильность пути " + this._path);
        }*/


        this.rootResources.OnInit();
        this.rootResources.GetFiles().ToJsonOnScreen().WriteToConsole();
        if (this.rootResources.GetFiles().Where(f => f.Path.EndsWith(".csproj")).FirstOrDefault() == null)
        {
            throw new Exception("Не удалось найти файл проеекта " + this._wrk);
        }

        //всё остальное
        this.ManifestController = this.rootResources.Bind<AppManifest>("manifest.xml");
        this.manifest.Name = this._wrk.Split(ResourceManager.GetPathSeparator()).ToList().Last();
        this.ManifestController.Set();

        this.RootController = rootResources.CreateFile(GetName().ReplaceAll(".", "_") + "Controller.cs");
        this.RootController.WriteText("");

        this.dotnet = new DotnetService();
        this.dotnet.SetWrk(_wrk);
        //this.dotnet.CreateMigration(_wrk, "version_1.0.0.0");
        //this.dotnet.UpdateDatabase(_wrk, "version_1.0.0.0");
        this.git = new Git();
        this.git.SetWrk(_wrk);
        //this.git.Commit("version_1.0.0.0");

    }

    public string GetProjectProperties()
    {
        DirectoryResource dir = this.rootResources.GetOrCreateDirectory("Properties");
        string filename = dir.Path + ResourceManager.GetPathSeparator() + "launchSettings.json";
        return System.IO.File.ReadAllText(filename);
    }



    public void CreateRazorComponent(string componentName)
    {
        this.dotnet.CreateRazorComponent(componentName);
    }

    private string GetDbContext()
    {
        throw new NotImplementedException();
    }

    public string CreateMigration(string context, string name)
    {
        return this.dotnet.CreateMigration(context, name);
    }
    public string UpdateDatabase(string context, string name)
    {
        return this.dotnet.UpdateDatabase(context, name);

    }
    public void RunAtNewProcess()
    {
        Info(this.GetExePath("net6.0"));
        if (System.IO.File.Exists(this.GetExePath("net6.0")) == false)
            throw new Exception("Не найден исполняемый файл " + GetExePath());
        var t = new Thread(() =>
        {

            ProcessStartInfo info = new ProcessStartInfo("cmd.exe", $" /C  d: && cd {_wrk} &&  { this.GetExePath()} ");
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;

            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            System.IO.StreamReader reader = process.StandardOutput;
            while (process.HasExited == false)
            {
                string line;
                while ((line = process.StandardOutput.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        });
        t.IsBackground = true;
        t.Start();
    }

    private bool IsValidateWrk(string wrk)
    {
        Info(wrk);
        var files = System.IO.Directory.GetFiles(wrk);
        return files.Where(f => f.EndsWith(".csproj.user") || f.EndsWith(".csproj")).Count() > 0;

    }

    public string CreateMvcApp()
    {
        return ExecCommand("dotnet new mvc");
    }

    public string PushlishLocal()
    {
        return ExecCommand("dotnet pack --no-build --output nupkgs");
    }
    public string AddNewtonSoft()
    {
        return ExecCommand(@"dotnet add package newtonsoft.json");
    }

    public string AddPackage(string package, string version)
    {
        return ExecCommand(@$"dotnet add package {package} --version {version}");
    }

    /*public string ScaffoldSqlServerDatabase(string server, string database, bool trusted, string username = "", string password = "")
    {
        string connectionString = new SqlServerADOConnectionStringModel() { Server = "", DataBase = "AppDesign", TrustedConnection = true }.ToString();
        return ExecCommand($"dotnet ef database scaffold \"" + $"{connectionString}\"" + $"Microsoft.EntityFrameworkCore.SqlServer" +
            $" -OutputDir {database}Entities -Context {database} -DataAnnotations");
    }*/

    public string ExecCommand(string command)
    {
        ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " +
                    @$"cd {_wrk} && {command}");
        info.RedirectStandardError = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
        return process.StandardOutput.ReadToEnd();
    }
    public List<string> ListSourceFiles()
    {
        return ListSourceFiles(_wrk);
    }
    public List<string> ListSourceFiles(string dir)
    {
        var list = System.IO.Directory.GetFiles(dir).Where(f => f.EndsWith(".cs")).ToList();

        System.IO.Directory.GetDirectories(dir).ToList().ForEach(p =>
        {
            list.AddRange(ListSourceFiles(p));
        });
        return list;
    }

    public void AddDataSet(string EntityType)
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

    public override void Destroy()
    {
        throw new NotImplementedException();
    }

    public override void Change()
    {
        throw new NotImplementedException();
    }

}