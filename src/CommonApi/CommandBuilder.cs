using API;
using eckumoc_common_api.CommonCollections;
using System.Collections.Generic;
using static System.Console;


/// <summary>
/// Отвечает за формирование коммандной строки
/// </summary>
public interface ICommandBuilder
{
    /// <summary>
    /// Задаёт режим выполнения комманды
    /// </summary>
    /// <param name="name">ключ</param>
    /// <returns>добавленное значение</returns>
    public string SetMode(string name);

    /// <summary>
    /// Анализирует текст и выполняет правильное решение о том что это такое
    /// </summary>
    /// <param name="arg"></param>
    public void AddArg(string arg);

    /// <summary>
    /// Добавляет ключ в комманду
    /// </summary>
    /// <param name="name">наименование ключа</param>
    /// <returns>добавленное значение</returns>
    public string AddKey(string name);
    public string RemoveKey(string name);
    public IEnumerable<string> GetKeys();

    /// <summary>
    /// Добавляет параметр в комманду
    /// </summary>
    /// <param name="key">ключ</param>
    /// <param name="value">значение</param>
    /// <returns>добавленное значение</returns>
    public string AddParameter(string key, string value);
    public string RemoveParameter(string key);
    public IEnumerable<KeyValuePair<string,string>> GetParameters();
}


/// <summary>
/// Отвечает за формирование коммандной строки
/// </summary>
public class CommandBuilder: ICommandBuilder
{
    private static IDictionary<string, string> Mapping = new Dictionary<string, string>()
    {
        { "\n","\\n" },
        { "\r","\\r" },
        { "\t","\\t" }
    };

    public static void OnRun()
    {
        var builder = new CommandBuilder("echo");
        builder.AddKey("c");
        builder.AddParameter("IP","192.168.0.1");            
        builder.AddParameter("Port", "80");
        WriteLine(builder);
    }

    public static ICommandBuilder Get(string app, string[] args)
    {
        var builder = new CommandBuilder(app);
        foreach(string arg in args)
        {
            builder.AddArg(arg);
        }
        return builder;
    }


    public string App = null;
    public string Mode = null;
    public IList<string> Keys;
    public ICommonDictionary<string> Parameters;

    public CommandBuilder(string app)
    {
        this.Mode = "/c";
        this.App = app;
        Parameters = new CommonDictionary<string>();
        Keys = new List<string>();
    }

    private string Resolve(string command)
    {
        foreach(var kv in Mapping)
        {
            command = command.ReplaceAll(kv.Key, kv.Value);
        }
        return command.ToUpper();
    }
        
    public string AddParameter(string key, string value) => Parameters.Set(Resolve(key), Resolve(value));
    public string RemoveParameter(string key)=> Parameters.RemoveByKey(key);

    public string SetMode(string name)
        => Mode = this.Resolve(name.ToUpper());
    public string AddKey(string name)
    {
        Keys.Add(name = this.Resolve(name.ToUpper()));
        return name;
    }
    public string RemoveKey(string key)
    {
        this.Keys.Remove(key=this.Resolve(key));
        return key;
    }

    public void AddArg(string arg)
    {
        if (arg.StartsWith("/"))
        {
            this.AddKey(arg.Substring(1));
        }else if (arg.StartsWith("-"))
        {
            arg = arg.Substring(1);
            int sep = arg.IndexOf("=");
            if(sep == -1)
            {
                throw new System.Exception($"Аргумент {arg} должен содержать знак =");
            }
            var key = arg.Substring(0, sep);
            arg = arg.Substring(sep + 2);
            arg = arg.Substring(0, arg.Length - 1);
            if( IsValidAsKey(key))
            {
                this.AddParameter(key, arg);
            }
            else
            {
                throw new System.Exception($"Ключ {key} не пригодный");
            }
                
        }
        else
        {
            throw new System.Exception("Ошибка при разборе аргументов коммандной строки: ["+arg+"]");
        }
    }

    public override string ToString() => $"{App}{KeysStr}{ParamStr}";

    public string KeysStr
    {
        get
        {
            string str = "";
            foreach (var next in Keys)
            {
                string key = Resolve(next);
                if (IsValidAsKey(key) == false)
                {
                    throw new System.Exception($"Ключ {key} записан в некорректной форме");
                }
                str += $" -{key}";
            }
            return str;
        }
    }

    private bool IsValidAsKey(string key)
    {
        return key.IsRus() || key.IsEng() || key.IsNumber();
    }

    /// <summary>
    /// 
    /// </summary>
    public string ParamStr
    {
        get
        {
            string str = "";
            Parameters.ForEachKeyValue((k, v) => {
                str += $" /{k}=" + '"' + v + '"';
            });
            return str;
        }
    }

    public IEnumerable<string> GetKeys() => Keys;

    public IEnumerable<KeyValuePair<string, string>> GetParameters() => Parameters.AsKeyValuePairs();
}
