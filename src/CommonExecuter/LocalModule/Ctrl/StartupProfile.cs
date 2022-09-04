using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootLaunch.Host.ServiceEndpoints
{

    /// <summary>
    /// Профиль выполнения приложения NetCore
    /// </summary>
    public class StartupProfile
    {
        public string commandName { get; set; } = "Project";
        public string dotnetRunMessages { get; set; } = "true";
        public bool launchBrowser { get; set; } = true;
        public string applicationUrl { get; set; } = "http://localhost:";

        public StartupProfile(int port)
        {
            this.applicationUrl = $"http://localhost:{port}";
        }
    }
}
