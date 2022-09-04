using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MessageLevel
{
    public class AppClient : MessageClient
    {
        public string Username = "admin";
        public string Password = "admin";
        public string Token = "thisIsAtest";

        public AppClient(string Host, int Port) : base(Host, Port)
        {
        }

        public void Login()
        {
            /*this.OnLoginComplete(
                this.Request(
                    new LoginMessage(this)
                )
            );*/
            
        }

        public void OnLoginComplete(MessageModel model)
        {
            this.Token = model.Response["token"].ToString();
            System.Console.WriteLine(JObject.FromObject(model.Response));
            SetInterval(() => {
                System.Console.Clear();
                this.Validate();
            }, 3000);
        }

        private void SetInterval(Action p, int v)
        {
            throw new NotImplementedException();
        }

        public void Validate()
        {
            /*this.OnValidateComplete(
                this.Request(new ValidateMessage(this))
            );*/
        }

        public void OnValidateComplete(MessageModel model)
        {
            System.Console.WriteLine(model.ToString());
      
            if (model != null && model.Response != null && model.Response["validation"]!=null && model.Response["validation"].ToString() == "true")
            {
             /*   this.OnLoginComplete(model);
                RuntimeUtils.SetTimeout(() => {
                    this.Validate();
                }, 1000);*/
            }
            else
            {
                this.Login();
            }
             
            
        }
    }
}
