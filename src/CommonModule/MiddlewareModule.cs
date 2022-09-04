//using MessageBroker.Data.ServiceConsumerModule;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using static System.Console;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Builder.Extensions;
using UniversalShare.ShareApp;
using Microsoft.AspNetCore.Http;

namespace UniversalShare.ShareApp

{
    public class MiddlewareModule<TComponent>: BaseService, IServiceModule where TComponent : IMiddleware
    {
        public void SetupServices(IConfiguration configuration, IServiceCollection services)
        {
           
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Use(async (http, ctx) => {
                WriteLine("1");
                await ctx.Invoke();
            });
            app.Use(async (http, ctx) => {
                WriteLine("2");
                await ctx.Invoke();
            });            
        }
    }
}
