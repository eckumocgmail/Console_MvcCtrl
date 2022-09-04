using COM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.IO.Directory;


public static class TextExecutersExtensions

{
    public static IEnumerable<int> GetIndexes(this string name, string word)
    {
        List<int> indexes = new List<int>();
        for (int i=0; i<name.Length; i++)
        {
            if (name.Substring(i).ToUpper().StartsWith(word.ToUpper())){
                indexes.Add(i);
                i += word.Length;
            }
            if (!(i < name.Length))
                break;
        }
        return indexes;


    }
    public static string Translite(this string name) => RusEngTranslite.TransliteToLatine(name);
}

public static class TextIdentitificationExtensions
{
    public static bool IsFile(this string name) => System.IO.Directory.Exists(name) == false && System.IO.File.Exists(name);
    public static bool IsDirectory(this string name) => System.IO.Directory.Exists(name) == false || System.IO.File.Exists(name);
    public static bool IsExe(this string name) => name.IsFile() && name.ToUpper().EndsWith(".EXE");
    public static bool IsDll(this string name) => name.IsFile() && name.ToUpper().EndsWith(".DLL");
    public static bool IsHttp(this string name) => name.IsFile() && name.ToUpper().StartsWith("HTTP:");
    public static bool IsHttps(this string name) => name.IsFile() && name.ToUpper().StartsWith("HTTPS:");
    public static bool IsWss(this string name) => name.IsFile() && name.ToUpper().StartsWith("WSS:");
    public static bool IsAssembly(this string name) => name.IsFile() && name.ToUpper().EndsWith(".DLL");
}









/// <summary>
/// Склонение по численному признаку
/// </summary>
public static class TextCountingExtensions
{

    /// <summary>
    /// Возвращает существительное во множественном числе
    /// </summary>   
    public static bool IsMultiCount(this string name)
    {
        return Counting.GetMultiCountName(name) == name;
    }
    public static bool IsSingleCount(this string name)
    {
        return Counting.GetSingleCountName(name) == name;
    }



    /// <summary>
    /// Возвращает существительное во множественном числе
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string ToMultiCount(this string name)
    {
        return Counting.GetMultiCountName(name);
    }


    /// <summary>
    /// Возвращает существительное в единственном
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string ToSingleCount(this string name)
    {
        return Counting.GetSingleCountName(name);
    }

}
