using System;
using System.Collections.Generic;
using System.Linq;
using static EcKuMoC;
using System.Threading.Tasks;
using eckumoc_common_api.CommonCollections;
using API;

using static COM.Typing;
using static Newtonsoft.Json.JsonConvert;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using System.Text;

using static System.Diagnostics.Process;

public interface IBinarySeriallizer
{
    ICommonDictionary<IDictionary<string, string>> DeserializeBinnary(byte[] data);
    byte[] SerializeBinnary(ICommonDictionary<IDictionary<string, string>> target);
}
public partial class EcKuMoC : WinSysAPI
{


    /// <summary>
    /// Валидаторы
    /// </summary>
    public static Dictionary<string, Func<object, string>> Validators
        = new Dictionary<string, Func<object, string>>();

    /// <summary>
    /// Записи в реестре
    /// </summary>
    public static ICommonDictionary<string> Configuration
        = new CommonDictionary<string>();


    public static string[] args { get; set; }





    /// <summary>
    /// Уничтожаем предудущие запуски
    /// </summary>
    private static void PreRun()
    {
        ForShadowProcesses(p => p.Kill());
    }



    /// <summary>
    /// Выполнение операции по отношению к процессам представляющим угрозу
    /// </summary>    
    private static void ForShadowProcesses(Action<Process> todo)
    {
        string executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        WriteLine(executingAssemblyName);
        GetProcesses().ToList().ForEach(proc =>
        {

            //
            if (proc.Id != GetCurrentProcess().Id && proc.MainWindowTitle.IndexOf("Консоль") != -1)
            {
                todo(proc);
            }

            /// процесс среды разработки попадает сюда
            /* else if (proc.Id != GetCurrentProcess().Id && proc.MainWindowTitle.IndexOf(executingAssemblyName) != -1)
            {
                Console.WriteLine(proc.MainWindowTitle);
                foreach (var module in ((ICollection<dynamic>)proc.Modules))
                {
                   ((ProcessModule)module).ToJsonOnScreen().WriteToConsole();
                }
                //todo(proc);
            }*/
            else if (proc.Id != GetCurrentProcess().Id && proc.MainWindowTitle.IndexOf("cmd.exe") != -1)
            {
                todo(proc);
            }
        });

    }






    /// <summary>
    /// Вывод сообщений
    /// </summary>
    public static void WriteLine(params object[] messages)
    {
        if ((IsCollectionType(messages.GetType())) == false)
        {
            foreach (var message in messages)
            {
                if (message is String && message.ToString().Contains("\n"))
                {
                    WriteLine(((String)message).Split("\n"));
                }
                 
                //Thread.Sleep(1000);
            }
        }
        else
        {
            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
                
            //Thread.Sleep(1000);
        }
 
    }


    /// <summary>
    /// Сериализация с парамтерами по-умолчанию.
    /// </summary>    
    public static void ToJsonOnScreen(object target)
    {
        WriteLine(SerializeObject(target));
    }


    /// <summary>
    /// Вызывается динамически подключаемым модулем
    /// </summary>
    public static void OnRun(params string[] args)
    {


        var executer = Assembly.GetExecutingAssembly();
        var caller = Assembly.GetCallingAssembly();
        var module = Assembly.GetEntryAssembly();
        Console.WriteLine($"executer={executer.GetName().Name}");
        Console.WriteLine($"caller={caller.GetName().Name}");
        Console.WriteLine($"module={module.GetName().Name}");
        var build = new Compile(executer, caller, module);

        TestingProgram.Run(build);
        //CommonProgram.Run(build);
    }




    /// <summary>
    /// /////
    /// </summary>
    public static void OnRunNetwork()
    {
        int max = 65535;
        int min = 49152;
        var sb = new StringBuilder();
        var hosts = new CommonDictionary<IHost>();

        string urlss = "http://localhost:" + (min + 8080) + "";
        sb.Append($"http://{127}.{0}.{0}.{1}:" + 8080 + ";http://localhost:" + 8080 + ";https://127.0.0.1:" + NetworkMonitor.GetFreeTcpPort());
        WriteLine(urlss);
        hosts.Set(urlss,
            Host.CreateDefaultBuilder()
                    /*.ConfigureWebHostDefaults(webBuilder =>
                    {
                        Console.WriteLine(urlss);
                        webBuilder.UseUrls(urlss);
                        webBuilder.UseStartup<Startup>();
                    })*/.Build());
        hosts[urlss].RunAsync();
        /* // исп. 255 портов
         for (int j1 = 0; j1 <= 255; j1--)
             for (int j2 = 0; j2 <= 255; j2--)
                 for (int j3 = 0; j3 <= 255; j3--)
                     for (int j4 = 0; j4 <= 255; j4--)
                         for (int i = max - min; i >= 0; i--)
                         {

                             string urls = "http://localhost:" + (min + i) + "";
                             sb.Append($"http://{j1}.{j2}.{j3}.{j4}:" + i + ";http://localhost:" + i + ";https://127.0.0.1:" + new NetworkMonitor().GetFreeTcpPort());
                             WriteLine(urls);
                             hosts.Set(urls,
                                 Host.CreateDefaultBuilder()
                                         .ConfigureWebHostDefaults(webBuilder =>
                                         {
                                             Console.WriteLine(urls);
                                             webBuilder.UseUrls(urls);
                                             webBuilder.UseStartup<Startup>();
                                         }).Build());
                             hosts[urls].RunAsync();

                         }*/
    }








    /// <summary>
    /// Сборка
    /// </summary>
    public class Compile
    {
        Assembly Caller;
        Assembly Executer;
        Assembly Entry;

        /// <summary>
        /// Методические указатели
        /// </summary>
        public ICommonDictionary<EcKuMoC> Associations = new CommonDictionary<EcKuMoC>();

        public Compile(Assembly caller, Assembly executer, Assembly entry)
        {
            Caller = caller;
            Executer = executer;
            Entry = entry;
        }

        public Compile() : this(
            Assembly.GetCallingAssembly(),
            Assembly.GetExecutingAssembly(),
            Assembly.GetEntryAssembly())
        {
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="module"></param>
    public static void RunProgram(Assembly module)
    {

        var program = module.GetTypes().Where(t => t.GetMethod("Main") != null).First();
        program.GetMethod("Main").Invoke(module, new string[0]);

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="filepath"></param>
    public static void ImportFile(string filepath)
    {
        var module = Assembly.LoadFile(filepath);
        System.Console.WriteLine(
            $"{module.GetName().Name} =< {module.Location}"
        );


    }







    /// <summary>
    /// Выводит состояние в стандартный поток ввода-вывода
    /// </summary>
    private void Trace()
    {

    }

    /// <summary>
    /// Бинарный вывод
    /// </summary>
    public byte[] SerializeBinnary(API.ICommonDictionary<IDictionary<string, string>> target)
    {
        var text = target.ToJson();
        return System.Text.Encoding.Default.GetBytes(text);
    }



    /// <summary>
    /// Бинарный ввод
    /// </summary>
    public API.ICommonDictionary<IDictionary<string, string>> DeserializeBinnary(byte[] data)
    {
        var text = System.Text.Encoding.Default.GetString(data);
        return (text.FromJson<API.ICommonDictionary<IDictionary<string, string>>>());
    }






}


public class Seriallizer
{
    /// <summary>
    /// Бинарный вывод
    /// </summary>
    public byte[] SerializeBinnary(API.ICommonDictionary<IDictionary<string, string>> target)
    {
        var text = target.ToJson();
        return System.Text.Encoding.Default.GetBytes(text);
    }



    /// <summary>
    /// Бинарный ввод
    /// </summary>
    public API.ICommonDictionary<IDictionary<string, string>> DeserializeBinnary(byte[] data)
    {
        var text = System.Text.Encoding.Default.GetString(data);
        return (text.FromJson<API.ICommonDictionary<IDictionary<string, string>>>());
    }

}






/// <summary>
/// Исключение выполнения процедуры
/// </summary>
public abstract class CallMessage : Exception
{
    public string MethodName { get; }
    public object[] InvokeArgs { get; }


    public CallMessage(string method, params object[] args)
    {
        this.MethodName = method;
        this.InvokeArgs = args;
    }
}