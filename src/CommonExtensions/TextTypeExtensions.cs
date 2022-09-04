using COM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/// <summary>
/// Признаки типизирующие символы
/// </summary>
public static class TextTypeExtensions
{

    /// <summary>
    /// Признак имени собственного объявленного как Класс-Тип  
    /// </summary>
    public static bool IsType(this string name)
    {
        return name != null && name.ToType() != null;
    }


    /// <summary>
    /// Признак арифметического оператора
    /// </summary>
    public static bool IsOpenOperator(this char ch)
    {
        return "[{'\"".Contains(ch);
    }

    /// <summary>
    /// Признак арифметического оператора
    /// </summary>
    public static bool IsLinearOperation(this char ch)
    {
        return "+-*/^%".Contains(ch);
    }

    /// <summary>
    /// Признак арифметического оператора
    /// </summary>
    public static bool IsLinearOperation(this string ch)
    {
        return ch.Length == 1 && ch[0].IsLinearOperation();
    }

    /// <summary>
    /// Признак цифрового символа
    /// </summary>
    public static bool IsNumber(this char ch)
    {
        return "0123456789".Contains(ch);
    }

    /// <summary>
    /// Признак цифрового символа
    /// </summary>
    public static bool IsNumber(this string ch)
    {
        return ch.Length == 1 && ch[0].IsNumber();
    }
 








    /// <summary>
    /// date
    /// </summary>
    public static bool IsDate(this string text)
    {
        try
        {
            text.EnsureIsDate();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public static void EnsureIsDate(this string text)
    {
        string message = text.ValidateIsDate();
        if (message != null)
        {
            throw new Exception(message);
        }
    }
    public static string ValidateIsDate(this string text)
    {
        char? separator = text.FirstChar(@"-.\/:");
        if (separator == null)
        {
            return @"Дата должна содержать один из разделителей [-.\/]";
        }
        if (text.CountOfChar((char)separator) != 2)
        {
            return @"Дата должна содержать 2 разделителя";
        }
        string numeric = text.ReplaceAll("" + (char)separator, "");
        if (numeric.Length != "12341212".Length)
        {
            return "Длина текста не корректна";
        }
        string message = numeric.ValidateIsPositiveInt();
        string[] arr = text.Split((char)separator);
        if (!(arr[0].Length == 4 && arr[1].Length == 2 && arr[2].Length == 2) &&
           !(arr[0].Length == 2 && arr[1].Length == 2 && arr[2].Length == 4))
        {
            return "Разделители установлены некорректно";
        }
        if ((arr[0].Length == 4 && arr[1].Length == 2 && arr[2].Length == 2))
        {
            int year = int.Parse(arr[0]);
            if (year.IsYear() == false)
            {
                return "Год задан неверно";
            }
            int month = int.Parse(arr[1]);
            if (month.IsMonth() == false)
            {
                return "Месяц задан неверно";
            }
            int day = int.Parse(arr[2]);
            if (day.IsDayOfMonth(year, month) == false)
            {
                return "Месяц задан неверно";
            }

        }
        else if ((arr[0].Length == 2 && arr[1].Length == 2 && arr[2].Length == 4))
        {
            int year = int.Parse(arr[2]);
            if (year.IsYear() == false)
            {
                return "Год задан неверно";
            }
            int month = int.Parse(arr[1]);
            if (month.IsMonth() == false)
            {
                return "Месяц задан неверно";
            }
            int day = int.Parse(arr[0]);
            if (day.IsDayOfMonth(year, month) == false)
            {
                return "Месяц задан неверно";
            }
        }
        return message;
    }

    /// <summary>
    /// float
    /// </summary>
    /// <param name="text"></param>
    public static bool IsFloat(this string text)
    {
        try
        {
            text.EnsureIsFloat();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public static void EnsureIsFloat(this string text)
    {
        string message = text.ValidateIsFloat();
        if (message != null)
        {
            throw new Exception(message);
        }
    }
    public static string ValidateIsFloat(this string text)
    {
        string str = text.ToString();
        if (str.Length == 0)
        {
            return "Числовое значение не может быть записано в 0 сиволов";
        }
        if ((str.Substring(0, 1) == "-" || str.Substring(0, 1).IsNumber()) == false)
        {
            return "Числовое значение должно начинаться либо с цифры либо со знака '-'";
        }
        int ctn = str.CountOfChar(',') + str.CountOfChar('.');
        if (ctn > 1)
        {
            return "Числовое значение может содержать единственный разделитель";
        }
        foreach (var ch in text)
        {
            if ((ch.IsNumber() || ch == '-' || ch == '.' || ch == ',') == false)
            {
                return "Числовое значение не может содержать символ " + ch;
            }
        }
        return null;
    }


    /// <summary>
    /// int
    /// </summary>
    /// <param name="text"></param>
    public static bool IsInt(this string text)
    {
        try
        {
            text.EnsureIsInt();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public static void EnsureIsInt(this string text)
    {
        string message = text.ValidateIsInt();
        if (message != null)
        {
            throw new Exception(message);
        }
    }
    public static string ValidateIsInt(this string text)
    {
        string str = text.ToString();
        if (str.Length == 0)
        {
            return "Числовое значение не может быть записано в 0 сиволов";
        }
        if ((str.Substring(0, 1) == "-" || str.Substring(0, 1).IsNumber()) == false)
        {
            return "Числовое значение должно начинаться либо с цифры либо со знака '-'";
        }
        if (text.Substring(1).CountOfChar('-') != 0)
        {
            return "Числовое значение может только начинаться со знака '-'";
        }
        foreach (var ch in text)
        {
            if ((ch.IsNumber() || ch == '-') == false)
            {
                return "Числовое значение не может содержать символ " + ch;
            }
        }
        return null;
    }
    public static string ValidateIsPositiveInt(this string text)
    {
        string str = text.ToString();
        if (str.Length == 0)
        {
            return "Числовое значение не может быть записано в 0 сиволов";
        }
        foreach (var ch in text)
        {
            if ((ch.IsNumber() || ch == '-') == false)
            {
                return "Числовое значение не может содержать символ " + ch;
            }
        }
        return null;
    }


    public static string GetTypeName(this Type propertyType)
    {
        string name = propertyType.Name;
        if (name.Contains("`"))
        {
            string text = propertyType.AssemblyQualifiedName;
            text = text.Substring(text.IndexOf("[[") + 2);
            text = text.Substring(0, text.IndexOf(","));
            name = name.Substring(0, name.IndexOf("`")) + "<" + text + ">";
        }
        return name;
    }

}