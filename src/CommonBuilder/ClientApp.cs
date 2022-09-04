using RootLaunch;

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ValidationAnnotationsNS;
using Tools;

/// <summary>
/// 
/// </summary>
public class ClientApp: BaseComponent   
{

    /// <summary>
    /// 
    /// </summary>
    [InputText]
    [NotNullNotEmpty]
    public string Wrk { get; private set; }


    /// <summary>
    /// 
    /// </summary>
    public FileController<AppManifest> ManifestController { get; private set; }


    [InputText]
    [NotNullNotEmpty]
    private DirectoryResource rootResources;



    [InputText]
    [NotNullNotEmpty]
    private DotnetService dotnet;

    [InputText]
    [NotNullNotEmpty]
    private Git GitRepository;

    private AppManifest manifest { get => ManifestController.Get(); }
    public FileResource RootController { get; private set; }

    public ClientApp(string subdir) 
    {      
        this.Wrk = subdir;
        
        this.InitSelf();
    }

    public void InitSelf()
    {

        try
        {

            
            InitWorkDirectory();
            InitManifest();
            InitMigration();
            InitRepository();
            
        }
        catch (Exception ex)
        {
            Error("Инициаллизация не выполнена ", ex);

        }

    }


    /// <summary>
    /// Выполнение команды через CMD
    /// </summary>
    public string Execute(string command)
    {
        var thread = new Thread(new ThreadStart(() =>
        {
            ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + command);
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            
            var task = new Task(() => {
                string line;
                while ((line = process.StandardOutput.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            });
            
        }));
        thread.IsBackground = true;
        thread.Start();
        return "";
    }

    private string GetWrk()
    {
        return Wrk;
    }

   




    public void Build()
    {
        PreBuild(this);
        var dos=new CMD();
        dos.CmdExec($"{Wrk.Substring(0,2)} && cd {Wrk} && ng build");
        PostBuild(this);
    }



    public void PreBuild(ClientApp clientApp)
    {
        
    }

    public void PostBuild(ClientApp clientApp)
    {
        throw new NotImplementedException();
    }

    public FileResource GetProjectProperties()
    {
        throw new NotImplementedException();
    }

    public void SetPortNumberAndSave(int port)
    {
        
    }



    /// <summary>
    /// Выполнение приложения
    /// </summary>
    /// <param name="port"></param>
    public void TryRun(int port)
    {
        Info("Открываем порт");
        var dos = new DOS();
        ManifestController.Get().Port = (port);
        ManifestController.Set();
        dos.Execute($"{Wrk.Substring(0, 2)} && cd {Wrk} && ng serve --port {port} --open");
        
    }
    public Task ExecCommand(string command)
    {
        this.Info($"ExecCommand({command})");
        return Task.Run(() =>

        {
            ProcessStartInfo info = new ProcessStartInfo("powershell.exe", "/C " +
              @$"{GetWrk().Substring(0, 2)} && cd {GetWrk()} && {command}");
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = true;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            System.Console.WriteLine(process.StandardOutput.ReadLine());
            process.WaitForExit();
        }); 

    }


    /// <summary>
    /// Получаем описатель ресурса
    /// </summary>
    public AppManifest GetManifest(int port)
    {
        Info("Создание манифеста");
        manifest.Port = (port);
        ManifestController.Set();
        return manifest;
    }


  

    private void InitManifest()
    {

        //всё остальное
        string sep = ResourceManager.GetPathSeparator();
        this.ManifestController = this.rootResources.Bind<AppManifest>("manifest.xml");
        this.ManifestController.Set();

        this.RootController = rootResources.CreateFile(this.rootResources.GetName().ReplaceAll(".", "_") + "Controller.cs");
        this.RootController.WriteText("");
    }

    private void InitWorkDirectory()
    {
        //определяем какой путь передан в качестве аргумента в конструктор и инициаллизируем
        //файловые ресурсы
        FileResource resource = new FileResource(this.Wrk);
        if (resource.IsFile)
        {
            this.Wrk = ResourceManager.GetParentPath(this.Wrk);
            this.rootResources =   DirectoryResource.Get(this.Wrk);
        }
        else if (resource.IsDirectory)
        {
            this.Wrk = this.Wrk;
            this.rootResources = DirectoryResource.Get(this.Wrk);
        }
        else
        {
            throw new Exception("Проверьте правильность пути " + this.Wrk);
        }
      //  this.ID = this.rootResources.NameShort;
    }


    /// <summary>
    /// 
    /// </summary>
    private void InitMigration()
    {
        Info("Создаем миграцию");
        this.dotnet = new DotnetService();
        this.dotnet.SetWrk(Wrk);
        this.dotnet.CreateMigration(Wrk, "version_1.0.0.0");
        this.dotnet.UpdateDatabase(Wrk, "version_1.0.0.0");
    }


    /// <summary>
    /// 
    /// </summary>
    private void InitRepository()
    {
        Info("Создаём репозиторий");
        this.GitRepository = new Git();
        this.GitRepository.SetWrk(Wrk);
        this.GitRepository.Commit("version_1.0.0.0");
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

    public override void Init()
    {
        throw new NotImplementedException();
    }
}