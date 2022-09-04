
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    internal class DataServiceModule: IServiceModule
    {
        public void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            string DefaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(DefaultConnectionString));
            //services.AddScoped<INewsRepository, NewsRepository>();
        }
    }
}
