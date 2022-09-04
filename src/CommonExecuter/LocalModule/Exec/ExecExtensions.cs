using eckumoc.Services;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Tools;

public static class ExecExtensions
{
    public static IServiceCollection AddExecServices(this IServiceCollection services)
    {
        services.AddSingleton<IPowerShellService, PowerShell>();
        services.AddSingleton<ICmd, CMD>();
        services.AddSingleton<IDOS, DOS>();

        services.AddSingleton<IGit, Git>();
        services.AddSingleton<IDotnetEF, DotnetService>();
        return services;
    }
}