namespace MessageLevel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;
   

 
    public class MessageModel
    {
        public long Created = Now();
        public int Rating = 1;
        public string Token { get; set; }
        public string Path { get; set; }
        public Dictionary<string, object> Pars { get; set; }

        public string From { get; set; }
        public string State { get; set; }
        public Dictionary<string, object> Response { get; set; }
        public string Message { get; set; }

        public MessageModel()
        {
            this.State = "Created";
            this.Pars = new Dictionary<string, object>();
        }

        public static long Now()
        {
            TimeSpan uTimeSpan = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)uTimeSpan.TotalMilliseconds;
        }

        public override string ToString()
        {
            return JObject.FromObject(this).ToString();
        }
    }
}
