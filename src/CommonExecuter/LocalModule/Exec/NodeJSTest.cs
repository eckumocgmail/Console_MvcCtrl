namespace tools.Hosted.Utils
{
    public class NodeJSTest : TestingElement
    {
        protected override void OnTest()
        {
            var node = new NodeJS();
            NodeJS.Execute("console.log(1);");
        }
    }
}