using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IServerApp
{
    string DefaultDistDir { get; }
    AppManifest manifest { get; }
    FileResource RootController { get; }

    void AddDataSet(string EntityType);
    string AddNewtonSoft();
    string AddPackage(string package, string version);
    string Build();
    void Change();
    void copyDllsToWrk();
    string CreateMigration(string context, string name);
    string CreateMvcApp();
    void CreateRazorComponent(string componentName);
    void Destroy();
    void Error(Exception ex);
    string ExecCommand(string command);
    string GetDistrPath();
    FileResource GetDllResource();
    string GetExePath(string f = "net6.0");
    AppManifest GetManifest(int port);
    string GetName();
    string GetProjectProperties();
    string GetState();
    string GetVersion();
    string GetWrk();
    void Import();
    void Info(object v);
    void Init();
    List<string> ListSourceFiles();
    List<string> ListSourceFiles(string dir);
    string PushlishLocal();
    void RunAtNewProcess();
    void SetPortNumberAndSave(int port);
    Task TryRun();
    void Update();
    string UpdateDatabase(string context, string name);
    Dictionary<string, string> ValidateApp();
}