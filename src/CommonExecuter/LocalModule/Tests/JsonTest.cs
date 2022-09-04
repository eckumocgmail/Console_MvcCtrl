namespace tools.Hosted.Utils
{
    public class JsonTest : TestingElement
    {
        protected override void OnTest()
        {
            System.Console.WriteLine(JsonEditor.Get(@"D:\wrk\app1\appsettings.json", "Logging"));
        }
    }
}