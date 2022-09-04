using ApplicationCore.Domain;
 

using System;

namespace CommonTests
{
    public class CommonExecuterUnit : TestingElement
    {
        protected override void OnTest()
        {
            Warn("Проекстируй программу выполнения запросов");
  
            var app = new AppExecuter();
            if (app.Execute("HELP").Length > 10)
            {
                this.Messages.Add("Умеем выполнять команды CMD");
            }
            else
            {
                throw new Exception("Не удалось выполнять команду CMD");
            }
        }
    }
}