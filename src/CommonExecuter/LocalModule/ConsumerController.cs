using Microsoft.AspNetCore.Mvc.ApplicationModels;

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
/// Объект реализует управление клиентом ассоциированным с соединением
/// </summary>
public sealed class ConsumerController
{
    public string Token;
    public readonly bool _Logging = true;
    public readonly Action<string> Send;
    public readonly StartupAsyncContext AsyncContext;
    public readonly Dictionary<string, ActionModel> Actions;


    public class ActionRequest
    {
        public ActionRequest()
        {
        }

        /// <summary>
        /// Ключ доступа клиента
        /// </summary>
        //[JsonProperty("token")]
        public virtual string AccessToken { get; set; } = "";


        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        //[JsonProperty("mid")]
        public virtual string SerialKey { get; set; } = "";


        /// <summary>
        /// Имя процедуры
        /// </summary>
        //[JsonProperty("request.path")]
        public virtual string ActionName { get; set; } = "";


        /// <summary>
        /// Параметры выполнения
        /// </summary>
        //[JsonProperty("request.pars")]
        public virtual Dictionary<string, string> MessageObject { get; set; } = new Dictionary<string, string>();

    }
    public ConsumerController(Action<string> Send, StartupAsyncContext AsyncContext)
    {
        this.Send = Send;
        this.AsyncContext = AsyncContext;
        this.Actions = new Dictionary<string, ActionModel>();        
    }


    private void LogInformation(string message)
    {
        System.Console.WriteLine("\n"+message +"\n");
    }


    public Task Request( string Action, Dictionary<string, string> Args, Action<object> OnSuccess, Action<string> OnError = null)
    {
        return Task.Run(()=> {
            if (_Logging)
                LogInformation("Requesting: \n" + JObject.FromObject(Args).ToString());
            string SerialKey = this.AsyncContext.Put(new ActionHandler()
            {
                OnSuccess = OnSuccess,
                OnError = OnError == null ? (evt) => {
                    LogInformation(evt);
                }: OnError
            });
            var RequestMessage = new ActionRequest()
            {
                SerialKey = SerialKey,
                MessageObject = Args,
                ActionName = Action,
                AccessToken = Token
            };
            string RequestText = JObject.FromObject(RequestMessage).ToString();
            Send(RequestText);
        });
    }


    public void AddAction(string action)
    {
        if (_Logging)
            LogInformation($"AddAction({action})");
        /*Actions[action] = new ActionModel(action, (args) => {
            return Request(action, args,
                (resp) => { },
                (err) => { }
            );
        });*/
    }

    
}