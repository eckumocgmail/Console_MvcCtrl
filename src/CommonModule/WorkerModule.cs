using UniversalShare.ShareApp;
//using MessageBroker.Data.ServiceConsumerModule;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalShare.ShareApp

{
    public abstract class WorkerModule: BaseService, IServiceModule
    {
        public abstract void SetupServices(IConfiguration configuration, IServiceCollection services);
        public void Configure(IApplicationBuilder app)        
            => throw new NotImplementedException();
        
    }
}
