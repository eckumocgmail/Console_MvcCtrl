using StartupModule;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using tools.Hosted.Utils;

namespace tools.Hosted
{
    public class HostUtilsUnitTest: TestingUnit
    {
        public HostUtilsUnitTest()
        {
            Push(new HostedAppFilesTest());
            Push(new NetworkMonitorTest());
            Push(new HostedAppUnitTest());
            Push(new HostedControllerUnitTest());
            Push(new HostedErpUnitTest());
            Push(new UtilsUnit());
        }
    }
}
