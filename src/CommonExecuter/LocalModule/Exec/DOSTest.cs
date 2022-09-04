using System;

using Tools;


namespace tools.Hosted.Utils
{
    public class DOSTest : TestingElement
    {
        protected override void OnTest()
        {
            var dos = new DOS();
            string output = dos.Execute("HELP");
            System.Console.WriteLine(output);
        }
    }
}