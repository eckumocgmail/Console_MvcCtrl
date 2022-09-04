
namespace tools.Hosted.Utils
{
    public class WinTest : TestingElement
    {
        protected override void OnTest()
        {
            var api = new WinSysAPI();
            var titles = api.GetWindowTitles();
            titles.ToJsonOnScreen().WriteToConsole();
        }
    }
}