using COM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TextConvertExtensionsTest: TestingElement
{
    protected override void OnTest()
    {
        throw new NotImplementedException("TextConvertExtensions".ToType().GetOwnPublicMethods().ToJson());
    }
}

/// <summary>
/// 
/// </summary>
public static class TextConvertExtensions
{
    public static int ToInt(this string text)
    {
        text.EnsureIsInt();
        return int.Parse(text);
    }
    public static float ToFloat(this string text)
    {
        text.EnsureIsFloat();
        return float.Parse(text.Replace(".", ","));
    }
    public static DateTime ToDate(this string s)
    {
        string text = s.Substring(0, "2020-01-01".Length);
        text.EnsureIsDate();

        char? separator = text.FirstChar(@"-.\/:");
        string[] arr = text.Split((char)separator);
        if ((arr[0].Length == 4 && arr[1].Length == 2 && arr[2].Length == 2))
        {
            int year = int.Parse(arr[0]);
            int month = int.Parse(arr[1]);
            int day = int.Parse(arr[2]);

            return new DateTime(year, month, day);
        }
        else if ((arr[0].Length == 2 && arr[1].Length == 2 && arr[2].Length == 4))
        {
            int year = int.Parse(arr[2]);
            int month = int.Parse(arr[1]);
            int day = int.Parse(arr[0]);
            return new DateTime(year, month, day);
        }
        throw new Exception("Не удалось преобразовать текст в дату");
    }
}