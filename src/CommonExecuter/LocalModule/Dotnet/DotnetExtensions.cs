using eckumoc.Services;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Tools;

public static class DotnetExtensions
{
    public static IServiceCollection AddDotnetServices(this IServiceCollection services)
    {
        services.AddSingleton<IDotnet, DotnetService>();
        services.AddSingleton<IDll, Dll>();
        services.AddSingleton<IGit, Git>();
        services.AddSingleton<IDotnetEF, DotnetService>();
        return services;
    }
}