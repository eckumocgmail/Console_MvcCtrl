using COM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;



public static class TextExtensions
{

    public static IEnumerable<Type> GetTypes(this string @namespace)
    {            
        return Assembly.GetCallingAssembly().GetTypes().Where(t => t.Namespace == @namespace).Where(t => t.Name.Contains("<") == false ).ToList();        
    }

    public static IEnumerable<Type> GetTypesAll(this string @namespace)
    {
        return Assembly.GetCallingAssembly().GetTypes().Where(t => t.Namespace!=null && t.Namespace.StartsWith(@namespace)).Where(t => t.Name!=null && t.Name.Contains("<") == false).ToList();
    }

    public static char? FirstChar(this string text, string charset)
    {
        foreach (var character in text)
        {
            if (charset.Contains(character))
            {
                return character;
            }
        }
        return null;
    }
    public static int CountOfChar(this string text, char ch)
    {
        int counter = 0;
        foreach (var character in text)
        {
            if (character == ch)
            {
                counter++;
            }
        }
        return counter;
    }
    public static string ReplaceAll(this string text, string s1, string s2)
    {
        while (text.IndexOf(s1) != -1)
        {
            text = text.Replace(s1, s2);
        }
        return text;
    }
}