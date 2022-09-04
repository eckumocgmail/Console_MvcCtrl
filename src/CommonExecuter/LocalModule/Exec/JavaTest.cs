namespace tools.Hosted.Utils
{
    public class JavaTest : TestingElement
    {
        protected override void OnTest()
        {
            var jar = new JavaExe();
            jar.SetPath(@"");
            jar.runtimeExecute("");
        }
    }
}