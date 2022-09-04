
using CsNamespaceHelper;

using Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NameSpaceModule;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using tools;
using tools.Hosted;
//using static Data.Microsoft.Extensions;
//using IHostBuilder = Data.Microsoft.Extensions.IHostBuilder;

namespace Data
{
    /*
    public class Microsoft
    {
        public class AspNetCore
        {
            public interface IServiceCollection
            {
                
            }

            public class Builder
            {
            }
        }
        public static class Extensions
        {
            public class Configuration
            {
            }

            public interface IHostBuilder 
            {
                void ConfigureWebHostDefaults(Action<IWebHostBuilder> configureWebHost);
                void ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureHostServices);
                IHost Build();
            }

            public interface IHostExt: IHost
            {
                System.IServiceProvider Services { get; } 
            }
            public class HostExt: IHost
            {
                public IServiceProvider Services => throw new NotImplementedException();

                internal static IHostBuilder CreateDefaultBuilder(string[] vs)
                {
                    throw new NotImplementedException();
                }
            }


            public class Hosting
            {
                //
                // Сводка:
                //     The Microsoft.Extensions.Hosting.IHostEnvironment initialized by the Microsoft.Extensions.Hosting.IHost.
                public IHostEnvironment HostingEnvironment
                {
                    get;
                    set;
                }

                //
                // Сводка:
                //     The Microsoft.Extensions.Configuration.IConfiguration containing the merged configuration
                //     of the application and the Microsoft.Extensions.Hosting.IHost.
                public IConfiguration Configuration
                {
                    get;
                    set;
                }

                //
                // Сводка:
                //     A central location for sharing state between components during the host building
                //     process.
                public IDictionary<object, object> Properties
                {
                    get;
                }
                
                public Hosting(IDictionary<object, object> properties)
                {
                    Properties = (properties ?? throw new ArgumentNullException("properties"));
                }
            }
        }
        */
    }



    public class HostedProgram
    {
        private static string[] args;
        private static IServiceProvider provider;
        private static IHost host;
        //private static ServiceWorkerModule service = new ServiceWorkerModule();
        //private static IServiceScope scope;

        public static void Run(string[] args)
        {
             
            var process = Process.GetCurrentProcess();
            var builder = Host.CreateDefaultBuilder(HostedProgram.args = args);
            //builder.ConfigureWebHostDefaults(ConfigureWebHost);
            builder.ConfigureServices(ConfigureHostServices);
            host = builder.Build();
            provider = host.Services;             
            host.Run(); 
        }



        private static void ConfigureHostServices(HostBuilderContext hostBuilderContext, IServiceCollection serviceDescriptors)
        {
            //service.ConfigureServices(hostBuilderContext,serviceDescriptors);
        }

        private static void ConfigureWebHost(IWebHostBuilder webBuilder)
        {
            //webBuilder.UseStartup<HostStartup>();
        }




        /// <summary>
        /// 
        /// </summary>
        public class HostStartup
        {
            
            public IConfiguration Configuration { get; }
            public HostStartup(IConfiguration configuration)
            {
                int version = 1;
                Configuration = configuration;
                configuration.GetReloadToken().RegisterChangeCallback((evt) => 
                {
                    version++;
                    Console.WriteLine("version "+version);
                    Console.WriteLine(configuration);
                }, this);
            }



            // This method gets called by the runtime. Use this method to add services to the container.
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddDotnetServices();
                services.AddExecServices();
                services.AddHttpContextAccessor();

                services.AddSingleton<HostedController>();


                services.AddHttpContextAccessor();
                services.AddSingleton<ConnectionsContext>();
                //services.AddSingleton<MacroModelComponent>();
                services.AddSingleton<StartupAsyncContext>();
                //services.AddSingleton<ActionsContext>();     

                services.AddSingleton<HostedController>();
                services.AddSingleton<ServiceContainer>();
                //services.AddControllersWithViews();

                // сервисы 
                var disc = services.ToList();
                var serviceNames = services.Select(descr => descr.ServiceType.Name).Where(name => name.IsEng()).ToList();
                services.AddSingleton("NameServiceProvider".ToType(), sp => {
                    return "NameServiceProvider".ToType().New();
                    /*{
                        _disc = disc,
                        _services = serviceNames
                    };*/
                });
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IHostEnvironment env)
            {
            
                //app.UseDeveloperExceptionPage();
                //app.UseStaticFiles();
                //app.UseRouting();

                /*app.UseEndpoints(endpoints =>
                {
                    //endpoints.MapHub<EventsHub>("/hubs/EventsHub");
                    //endpoints.MapHub<global::ActionsHub>("/hubs/ActionsHub");
                    endpoints.MapControllers();
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });*/
            }
        }
    }
//}