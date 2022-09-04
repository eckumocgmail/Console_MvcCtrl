using RootLaunch;

namespace tools.Hosted.Utils
{
    public class FilesTest : TestingElement
    {
        protected override void OnTest()
        {
            var dir = DirectoryResource.Get(@"D:\commands");
            dir.Trace();

        }
    }
}