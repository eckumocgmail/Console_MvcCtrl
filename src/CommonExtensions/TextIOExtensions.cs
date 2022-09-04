using COM;

using ConsoleApp.console_app_src.Streaming;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TextIOExtensions
{

    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static List<string> GetDirectories(this string path)
    {
        return new List<string>(System.IO.Directory.GetDirectories(path));
    }

    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static List<string> GetResources(this string path)
    {
        return path.GetDirectories().Concat(path.GetFiles()).ToList();
        
    }

    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static List<string> GetFiles(this string path)
    {
        return new List<string>(System.IO.Directory.GetFiles(path));
    }

    /// <summary>
    /// Вывод в консоль
    /// </summary>    
    public static string WriteTo(this string text, IStreaming agent )
    {
        agent.WriteLine(text);
        return text;
    }

    /// <summary>
    /// Вывод в консоль
    /// </summary>    
    public static string WriteToConsole(this string path)
    {        
        return path.WriteToConsole(1);
    }


    /// <summary>
    /// Вывод в консоль
    /// </summary>    
    public static string WriteToConsole(this string path, int level )
    {
        string preLine = "";
        for (int i = 0; i < level; i++) preLine += "   ";
        Writing.ToConsole(preLine+path);
        return path;
    }

    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static string WriteToFile(this string text, string path)
    {
        System.IO.File.WriteAllText(path,text);
        return text;
    }

    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static string ReadText(this string path)
    {
        return System.IO.File.ReadAllText(path);
    }
}