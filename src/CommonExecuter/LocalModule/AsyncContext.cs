using System;
using System.Collections.Concurrent;

public class StartupAsyncContext
{
    /// <summary>
    /// Динамическая область памяти для регистрации ссылок на функции обрабатывающии ответное сообщение
    /// </summary>
    private ConcurrentDictionary<string, ActionHandler> Pool = new ConcurrentDictionary<string, ActionHandler>();

    /// <summary>
    /// Длина ключа доступа к функции обрабатывающей ответное сообщение
    /// </summary>
    private int SerialKeyLength = 32;
    private Random Random = new Random();




    /// <summary>
    /// Регистрация функции обработчика
    /// </summary>
    /// <param name="Handle">ссылка на функцию</param>
    /// <returns>ключ доступа</returns>
    public string Put(ActionHandler Handle)
    {
        lock (this.Pool)
        {
            string SerialKey = GenerateSerialKey();
            Pool[SerialKey] = Handle;
            return SerialKey;
        }
    }


    /// <summary>
    /// Извлечение функции обработчика
    /// </summary>
    /// <param name="SerialKey"> ключ доступа</param>
    /// <returns>функция обработчика</returns>
    public ActionHandler Take(string SerialKey)
    {
        lock (this.Pool)
        {
            ActionHandler Handle = null;
            Pool.TryRemove(SerialKey, out Handle);
            return Handle;
        }
    }


    /// <summary>
    /// Генерация уникального ключа доступа
    /// </summary>
    /// <returns></returns>
    private string GenerateSerialKey()
    {
        string key;
        do
        {
            key = "";
            for (int i = 0; i < SerialKeyLength; i++)
            {
                key += (Math.Floor(Random.NextDouble() * 10)).ToString();
            }
        } while (Pool.ContainsKey(key));
        return key;
    }
    /* public async Task Request(
                     string Action,
                     Dictionary<string, object> Args,
                     Action<object> OnSuccess,
                     Action<string> OnError = null)
     {
         if (_Logging)
             System.Console.WriteLine("Requesting: \n" + JObject.FromObject(Args).ToString());



         string SerialKey = Put(new ActionHandler()
         {
             OnSuccess = OnSuccess,
             OnError = OnError == null ? (evt) => {
             }
             : OnError
         });
         var RequestMessage = new ActionRequest()
         {
             SerialKey = SerialKey,
             MessageObject = Args,
             ActionName = Action,
             AccessToken = Token
         };
         string RequestText = JObject.FromObject(RequestMessage).ToString();
         if (Connection != null)
         {
             await Connection.InvokeAsync("Request", RequestText);
             if (_Logging)
                 System.Console.WriteLine("Requested: \n" + RequestText);

         }
     }*/
}