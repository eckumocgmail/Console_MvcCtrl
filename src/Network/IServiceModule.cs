using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public interface IServiceHostingModule
{
    public void ConfigureServices( IDictionary<string, string> configuration, Microsoft.Extensions.DependencyInjection.IServiceCollection services);
}

