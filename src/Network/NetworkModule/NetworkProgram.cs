
using MessageLevel;

using ServiceEndpoint.ServiceEndpoint;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TestTask;
public class NetworkProgram : TextMessageHandler
{


    public static void Run(string[] args)
    {

        /*var network = new NetworkAdapter();
        network.Exchange("Check \n",
            (text) =>
            {
                if (string.IsNullOrEmpty(text) == false)
                {
                    Console.WriteLine(text);
                }
            },
            (ex) =>
            {
                Console.WriteLine(ex);
            }
        );*/
        //ReadLine("127.0.0.1", 8383);
        NetworkService.Main();
        var Client = new TcpClient("127.0.0.1", 13000);
        using (var stream = Client.GetStream())
        {
            
            var thread = new Thread(() => {
                byte next;
                while (true)
                {
                    next = (byte)stream.ReadByte();
                    Console.Write(next);
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

    }

    internal static void OnRun()
    {
        throw new NotImplementedException();
    }




    /// <summary>
    /// Подключение текстового канала управления 
    /// </summary>
    /// <param name="endpoint">IP-адрес</param>
    /// <param name="port">порт</param>
    static void ReadLine(string endpoint, int port)
    {
        var client = new AppClient(endpoint, port);
        while (true)
        {
            var message = new TextMessage();
            message.Enqueue(
                "data: \n".ToCharArray().Select(ch => (byte)ch).ToArray()
            );
            foreach(var line in message)
                Console.WriteLine(line);
            client.Request(message);

            Thread.Sleep(100);
        }
    }



    /// <summary>
    /// Функция конвертирования текстового сообщения в терминальную последовательность 
    /// </summary>
    public override Func<string, MessageModel> GetConnection()
    {
        return (text) =>
        {
            Console.WriteLine($"Program Readed Text: {text}");
            var message = new TextMessage();

            message.Enqueue("ENG:   task 1 => decode text please".ToCharArray().Select(ch => (byte)ch).ToArray());
            message.Enqueue("RU:    Задача 1 => Прочитать текст задания".ToCharArray().Select(ch => (byte)ch).ToArray());
            return new MessageModel()
            {
                Message = "ENG: task 1 => decode text please" + "RU: Задача 1 => Прочитать текст задания"
            };
        };
    }






}
