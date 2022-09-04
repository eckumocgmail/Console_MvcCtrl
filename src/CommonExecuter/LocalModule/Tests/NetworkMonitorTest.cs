
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupModule
{
    class NetworkMonitorTest : TestingElement
    {
        protected override void OnTest()
        {
            var monitor = new NetworkMonitor();
            monitor.Init();
            monitor.ToJsonOnScreen().WriteToConsole();
        }
    }
}
