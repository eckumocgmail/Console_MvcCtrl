using System.Threading.Tasks;

namespace Tools
{
    public interface IDotnet
    {
        string CreateMigration(string context, string name);
        string DropDatabase(string context);
        string ExecCommand(string command);
        string ListMigrations(string context);
        string UpdateDatabase(string context, string name);
        string Build();
        Task Run();
        void SetWrk(string wrk);
        void CreateRazorComponent(string componentName);
    }
}