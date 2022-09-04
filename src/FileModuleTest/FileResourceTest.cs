using RootLaunch;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FileResourceTest: TestingElement
{
    protected override void OnTest()
    {
        var file = new FileResource("Program.cs");
        Info(file.ReadText());

    }
}
