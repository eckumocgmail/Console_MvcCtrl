using System;
using System.Threading.Tasks;

namespace tools.Hosted
{
    public class HostedAppUnitTest : TestingElement
    {
        protected override void OnTest()
        {
            TestClientApp();
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task TestClientApp()
        {
            return Task.Run(() => {            
                var app = new ClientApp(@"D:\projects_ftp");
                app.InitSelf();
                app.Build();
                app.TryRun(4243);
            });
        }
    }
}