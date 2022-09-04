using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UniversalShare.ShareApp

{
    public interface IServiceModule
    {
        public void SetupServices(IConfiguration configuration, IServiceCollection services);
        public void Configure(IApplicationBuilder app);

    }
}