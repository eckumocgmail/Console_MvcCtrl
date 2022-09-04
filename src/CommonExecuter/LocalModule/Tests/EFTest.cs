using Tools;

namespace tools.Hosted.Utils
{
    public class EFTest : TestingElement
    {
        protected override void OnTest()
        {
            var dotnet = new DotnetService();
            dotnet.SetWrk(@"d:\wrk\app1");
            System.Console.WriteLine(dotnet.CreateApplication(DotnetTemplates.mvc));
            System.Console.WriteLine(dotnet.AddPackage(DotnetPackages.EF_CORE));
            System.Console.WriteLine(dotnet.AddPackage(DotnetPackages.EF_DESIGN));
            System.Console.WriteLine(dotnet.AddPackage(DotnetPackages.EF_INMEMORY));
            System.Console.WriteLine(dotnet.AddPackage(DotnetPackages.EF_SQLSERVER));
            System.Console.WriteLine(dotnet.GenerateFromDatabase("Server=KEST;Database=ERP;Trusted_Connection=True;",DotnetEFProviders.SqlServer));
            System.Console.WriteLine(dotnet.CreateMigration("CREATED"));
            System.Console.WriteLine(dotnet.UpdateDatabase());
        }
    }
}