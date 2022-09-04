using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


internal interface IServiceModule
{
    public void ConfigureServices(IConfiguration configuration, IServiceCollection services);
}



public abstract class ServiceWorkerModule: IServiceModule
{
    /// <summary>
    /// Конфигурация служб авторизации работающих в фоновом режиме
    /// </summary>
    /// <param name="context"></param>
    /// <param name="services"></param>
    public void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        this.ConfigureServices(context.Configuration, services);
    }
    public abstract void ConfigureServices(IConfiguration configuration, IServiceCollection services);
}
