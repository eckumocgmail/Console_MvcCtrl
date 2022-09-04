using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class Extensions
{



    public static string RemoveTokens(this string text, params string[] chars)
    {
        foreach (var ch in chars)
        {
            text = text.ReplaceAll(ch,"");
        }
        return text;
    }

    public static KeyValuePair<string,string> SplitSingle(this string text, string separator)
    {
        string[] arr = text.Split(separator);
        if (arr.Length != 2)
            throw new Exception($"Некорректные данные [{text}] для разбиения строки на две подстроки через разделитель "+separator);
        return new KeyValuePair<string, string>(arr[0], arr[1]);
    }
}
