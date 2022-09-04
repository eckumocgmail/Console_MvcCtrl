using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public abstract class ServiceWorkerHostingModule: IServiceHostingModule
{
    /// <summary>
    /// Конфигурация служб авторизации работающих в фоновом режиме
    /// </summary>
    /// <param name="context"></param>
    /// <param name="services"></param>
    public void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        this.ConfigureServices(GetSettings(context.Configuration).ToDictionaryOfText(), services);
    }

    private HostBuilderContext GetSettings(IConfiguration configuration)
    {
        throw new NotImplementedException();
    }

    public abstract void ConfigureServices(IDictionary<string, string> configuration, IServiceCollection services);
}
