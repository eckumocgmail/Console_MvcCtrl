using Newtonsoft.Json;

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using static EcKuMoC;


/// <summary>
/// Вывод сообщений в консоль
/// </summary>
public interface IConsoleLogger
{
    public void Warn(params object[] args);
    public void Error(string name, Exception ex);
    public void Error(params object[] args);
    public void Debug(params object[] args);
    public void Info(params object[] args);
}


/// <summary>
/// Вывод сообщений в консоль
/// </summary>
public class ConsoleLogger : IConsoleLogger
{
    [NotMapped]
    [JsonIgnore]
    public string ConsoleLoggerName { get; set; }
    public ConsoleLogger()
    {
        ConsoleLoggerName = GetType().GetTypeName();
    }
    public virtual void Warn(params object[] args)
    {
        args.ToList().ForEach(message => {
            WriteLine($"[{ConsoleLoggerName}][WARN]: {message}");
        });
    }
    public virtual void Error(params object[] args)
    {
        args.ToList().ForEach(message => {
            WriteLine($"[{ConsoleLoggerName}][ERROR]: {message}");
        });
    }
    public virtual void Debug(params object[] args)
    {
        var name = GetType().GetTypeName();
        args.ToList().ForEach(message => {
            WriteLine($"[{ConsoleLoggerName}][DEBUG]: {message}");
        });
    }
    public virtual void Info(params object[] args)
    {
        System.Console.ForegroundColor = System.ConsoleColor.DarkGreen;
        args.ToList().ForEach(message => {
            WriteLine($"[{ConsoleLoggerName}][INFO]: {message}");
        });
    }
    public void Error(string name, System.Exception ex)
    {
        this.Error(new string[] { name, ex.Message, ex.StackTrace });
    }
}