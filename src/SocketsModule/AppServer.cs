

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
 
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MessageLevel
{
    public class AppServer : MessageServer
    {
        private static Func<string, TextMessage> JsonDecoder = (json) => {
            return JsonConvert.DeserializeObject<TextMessage>(json);
        };

        public Func<string, TextMessage> OnMessageEvent { get; }

        public AppServer() : this("127.0.0.1", 8181, JsonDecoder)
        {

        }

        public AppServer (Func<string, TextMessage> decode) :this("127.0.0.1",8181, decode)
        {
            
        }


        public AppServer (string ip, int port, 
                Func<string, TextMessage> decode) : base(ip, port)
        {
            //this.loginManager = new LoginManager();
            this.OnMessageEvent = decode;
        }

        public AppServer(string ip, int port) : base(ip, port)
        {
        }

        private object GetContoller(string token)
        {
            /*miac.ServerApp.Model.SessionScope session = this.loginManager.GetSession( token );
            if (session == null)
            {
                return this.loginManager;
            }
            else
            {
                return session;
            }        */
            throw new Exception();
        }


        /*protected override TextMessage OnMessage(TextMessage message)
        {
            return message;


        }*/

        /*public TextMessage OnRequest(TextMessage message)
        {
            string token = null;
            try
            {
                if (message == null)
                {
                    throw new Exception("Required property 'Token' is missed");
                }
                message.State = "Readed";
                token = message.Token;
                if (String.IsNullOrEmpty(token))
                {
                    throw new Exception("Required property 'Token' is missed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error while authentication proccessing: " + ex.Message + "\n" + ex.StackTrace);
            }


            object controller = GetContoller(token);
            if (message.Path == null)
            {
                throw new Exception("Required property 'Command' is missed");
            }
            else if (controller == null)
            {
                throw new Exception("api controller object has not been founded");
            }
            else
            {

                //find method
                Dictionary<string, Object> prototype = null;
                try
                {
                    prototype = Find(controller, message.Path);
                    if (prototype["method"] == null)
                    {
                        throw new Exception("[ReflectionUtil].[Find].[Exception]: " + message.Path);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("[ReflectionUtil].[Find].[Exception]: " + message.Path);
                }




                //ivoke
                try
                {
                    message.Response = (Dictionary<string, object>)Invoke(
                        (MethodInfo)prototype["method"],
                        prototype["target"],
                        message.Pars == null ? null : JObject.FromObject(message.Pars)
                    );
                    message.State = "Success";
                }
                catch (Exception ex)
                {
                    throw new Exception($"IvokationException: Error while ivoke reflected method: { ((MethodInfo)prototype["method"]).Name}. => " + ex.Message + " " + ex.StackTrace);

                }


            }
            return message;

        }*/

        private Dictionary<string, object> Invoke(MethodInfo methodInfo, object v, JObject jObject)
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, object> Find(object controller, string path)
        {
            throw new NotImplementedException();
        }




    }
}
