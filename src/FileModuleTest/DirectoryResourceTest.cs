using RootLaunch;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DirectoryTest : TestingElement
{
    protected override void OnTest()
    {
        var wrk = DirectoryResource.Get( );
        Info(wrk.GetFiles().ToJsonOnScreen());

    }
}
