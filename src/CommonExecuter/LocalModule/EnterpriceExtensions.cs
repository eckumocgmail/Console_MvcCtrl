using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class EnterpriceExtensions
{

    public static IServiceCollection AddEnterPriceContext(this IServiceCollection services)
    {
        services.AddSingleton<HostedController>();
        return services;
    }
}