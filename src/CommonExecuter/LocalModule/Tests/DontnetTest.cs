using Tools;

namespace tools.Hosted.Utils
{
    public class DontnetTest : TestingElement
    {
        protected override void OnTest()
        {
            var dotnet = new DotnetService();
            dotnet.SetWrk(@"D:\WRK\TestDotnetTool");
            dotnet.CreateApplication(DotnetTemplates.console);
        }
    }
}