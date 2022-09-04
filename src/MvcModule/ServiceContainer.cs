 

using DataADO;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



public class ServiceContainer: BackgroundService
{
    private readonly ConcurrentQueue<Action> Incomming = new ConcurrentQueue<Action>();
    //private readonly ActionsConsumer consumer;
    //private readonly HashSet<HostedService> HostedServices;
    private readonly ILogger<ServiceContainer> _logger;
    private readonly bool _logging = false;

    public ServiceContainer( ILogger<ServiceContainer> logger ):base()
    {       
        _logger = logger;
        //this.consumer = new ActionsConsumer("http://localhost:8000/hubs/ActionsHub");
        
        //HostedServices = new HashSet<HostedService>();
 
        /*Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsExtendsFrom(nameof(HostedService))).ToList().ForEach((type) => {
            System.Console.WriteLine("Регистрация службы удалённого вызова процедур: " + type.Name);
            HostedServices.Add((HostedService)type.New());
        });*/
    }


    public void AddToQueue(Action todo)
    {
        Incomming.Enqueue(todo);
    }
  


    protected override Task ExecuteAsync([NotInput]CancellationToken stoppingToken)
    {
        
        return Task.Run(() => {
            if(_logging) _logger.LogInformation("ExecuteAsync()");
            object mdb = null;// new TheMovieDatabaseService();                                  
            while (stoppingToken.IsCancellationRequested == false)
            {

                //подключение к маршрутизатору событий
                /*while(this.consumer.Connection == null ||
                        this.consumer.Connection.State!=Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected )
                {
                    if(_logging) _logger.LogInformation("ExecuteAsync()");
                    this.consumer.Connect().Wait();
                    foreach (var service in HostedServices)
                    {
                        object serviceItem = service;
                        var actions = ReflectionService.GetSkeleton(service);
                        foreach (var action in actions)
                        {
                            if (stoppingToken.IsCancellationRequested == false)
                            {
                                if(_logging) _logger.LogInformation($"Регистрация операции {action.Key}()");
                                this.consumer.Publish(action.Key, (args) => {

                                    if(_logging)  _logger.LogInformation($"{action.Key}()");
                                    //var todos = ReflectionService.Find(service, action.Key);                        
                                    //object result = ReflectionService.Execute((MethodInfo)todos["method"], todos["target"], args);
                                    return new
                                    {
                                        message = "test"
                                    };
                                }).Wait();
                            }
                        }
                    }
                }*/


                /*Task.Run(async()=> {
                    await this.consumer.Request("movieSearch", new Dictionary<string, string>() {
                            { "query", "kill"},
                            { "page", "1"},
                        },
                       (resp) =>
                       {
                           _logger.LogInformation($"Успех выполнения операции {resp.ToJsonOnScreen()}()");
                       },
                       (err) =>
                       {
                           _logger.LogInformation($"Провал выполнения операции {err}()");
                       }
                   );
                    Func<Dictionary<string, string>, object> todo = (args) =>
                    {
                        var task = mdb.movieSearch(args["query"], args["page"].ToInt());
                        task.Wait();

                        object res = task.Result;
                        return res;


                    };
                    var result = todo(new Dictionary<string, string>() {
                                    { "query", "kill"},
                                    { "page", "1"},
                                });
                    this.consumer.Publish("movieSearch", todo).Wait();
                });*/
                Thread.Sleep(1000);
            }
            //this.consumer.Disconnect().Wait();
        });
      
        
    }
 
    
  
   
}
 
