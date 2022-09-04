using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

 

namespace Data
{
    public class AppServiceModule : IServiceHostingModule
    {
        //private AuthServiceModule AuthServiceModule = new AuthServiceModule();
        
        private DataServiceModule DataServiceModule = new DataServiceModule();
        //private ReCaptchaModule ReCaptchaModule = new ReCaptchaModule();
        //private ViewsServiceModule ViewsServiceModule = new ViewsServiceModule();
        public void ConfigureServices(IDictionary<string, string> configuration, IServiceCollection services)
        {
            //AuthServiceModule.ConfigureServices(configuration, services);
            //ConsoleServiceModule.ConfigureServices(configuration, services);
            //DataServiceModule.ConfigureServices(configuration, services);
            //ViewsServiceModule.ConfigureServices(configuration, services);
            //ReCaptchaModule.ConfigureServices(configuration, services);
            
        }
    }
}
 
