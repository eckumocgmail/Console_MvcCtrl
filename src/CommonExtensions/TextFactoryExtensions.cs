using COM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;





/// <summary>
/// Расширения к строкового типа
/// </summary>
public static class TextFactoryExtensions
{

    /// <summary>
    /// Поиск типа по имени
    /// </summary>
    public static Type ToType(this string text)
    {
         
        //System.Console.WriteLine($"[Info][{typeof(TextFactoryExtensions).Name}]: ToType({text})");
    
        return text.Contains(".")? ReflectionService.TypeForName(text): ReflectionService.TypeForShortName(text);
    }




    /// <summary>
    /// Поиск типа по имени
    /// </summary>
    public static object New(this string text)
    {
        return ReflectionService.CreateWithDefaultConstructor<object>(text.ToType());
    }

    /// <summary>
    /// Поиск типа по имени
    /// </summary>
    public static object New(this Type type)
    {
        return ReflectionService.CreateWithDefaultConstructor<object>(type);
    }

    /// <summary>
    /// Поиск типа по имени
    /// </summary>
    public static object New(this Type type, object[] parameters)
    {
        return ReflectionService.Create<object>(type, parameters);
    }

    /// <summary>
    /// Поиск типа по имени
    /// </summary>
    public static object New(this string text, string deps)
    {
        var constructor = text.ToType().GetConstructors()[0];
        return constructor.Invoke(new object[] { deps });
    }

}