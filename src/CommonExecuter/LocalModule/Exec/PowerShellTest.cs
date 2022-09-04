using Tools;

namespace tools.Hosted.Utils
{
    public class PowerShellTest : TestingElement
    {
        protected override void OnTest()
        {
            new PowerShell().Execute("Get-Command").ToJsonOnScreen().WriteToConsole();
        }
    }
}