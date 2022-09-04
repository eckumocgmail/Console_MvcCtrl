using Tools;

namespace tools.Hosted.Utils
{
    public class GITTest : TestingElement
    {
        protected override void OnTest()
        {
            var git = new Git();
            git.SetWrk(@"D:\wrk\app1");
            git.Init();
            git.Commit("This is a test");            
        }
    }
}