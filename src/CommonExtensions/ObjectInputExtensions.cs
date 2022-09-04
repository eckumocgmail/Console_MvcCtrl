using COM;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
 
public static class ObjectInputExtensions
{
    
    public static string[] GetUserInputPropertyNames(this Type type)
    {
        var navs = GetRefTypPropertiesNames(type);        
        return ReflectionService
              .GetPropertyNames(type).
               Where(n => 
                    IsInput(type, n) &&
                   IsVisible(type, n) &&
                   navs.Contains(n) == false && 
                   navs.Contains(n+"ID") == false).ToArray();
    }

    private static IEnumerable<string> GetRefTypPropertiesNames(Type type)
    {
        throw new NotImplementedException();
    }

    private static bool IsInput(Type type, string n)
    {
        throw new NotImplementedException();
    }

    private static bool IsVisible(Type type, string n)
    {
        throw new NotImplementedException();
    }

    public static bool IsCollectionType(this Type type)
    {
        return Typing.IsCollectionType(type);
    }

    /// <summary>
    /// Преобразование к формату JSON
    /// </summary>
    public static string ToXML(this object target)
    {
        return target.ToXML();
    }
    public static Dictionary<string, string> GetAttrs(this object p)
    {
        if(p is Type)return ForType((Type)p);
        return ForType(p.GetType());
    }

    private static Dictionary<string, string> ForType(Type p)
    {
        throw new NotImplementedException();
    }

    public static Dictionary<string, object> ToDictionary(this object p )
    {
        return p.ToJson().FromJson<Dictionary<string, object>>();
    }

    public static Dictionary<string,object> ToDictionary(this object p, ViewItem style, string[] options)
    {
        return p.ToJson().FromJson<Dictionary<string, object>>();
    }

    public static Dictionary<string, string> ToDictionaryOfText(this object p)
    {
        return p.ToJson().FromJson<Dictionary<string, string>>();
    }

    public static string[] GetOwnPublicMethods(this Type type)
    {
        return (from p in new List<MethodInfo>((type).GetMethods())
                where p.DeclaringType == type
                    && p.IsPublic
                    && p.IsStatic == false
                select p.Name).ToArray();
    }
    public static string[] GetOwnStaticMethods(this Type type)
    {
        return (from p in new List<MethodInfo>((type).GetMethods())
                where p.DeclaringType == type
                    && p.IsPublic
                    && p.IsStatic == true
                select p.Name).ToArray();
    }
    
    public static string[] GetOwnMethodNames(this Type type)
    {
        return (from p in new List<MethodInfo>((type).GetMethods()) where p.DeclaringType == type select p.Name).ToArray();
    }
    public static string[] GetOwnPropertyNames(this Type type)
    {
        return (from p in new List<PropertyInfo>((type).GetProperties()) where p.DeclaringType == type select p.Name).ToArray();
    }

    /// <summary>
    /// Возвращает значение свойства
    /// </summary> 
    public static object GetProperty(this object target, string property )
    {
        return target.GetType().GetProperty(property).GetValue(target);
    }

    /// <summary>
    /// Возвращает значение свойства
    /// </summary> 
    public static void SetProperty(this object target, string property, object value)
    {
        target.GetType().GetProperty(property).SetValue(target,value);
    }

    /// <summary>
    /// Преобразование к формату JSON
    /// </summary>
    public static string ToJson(this object target )
    {
        return JsonConvert.SerializeObject(target,Formatting.None);
    }

    /// <summary>
    /// Преобразование к формату JSON
    /// </summary>
    public static void Log(this object target)
    {
        Console.WriteLine(JsonConvert.SerializeObject(target));
        Console.WriteLine(Environment.StackTrace);

    }

    /// <summary>
    /// Преобразование к формату JSON
    /// </summary>
    public static string ToJsonOnScreen(this object target)
    {        
        return JsonConvert.SerializeObject(target,new JsonSerializerSettings() { Formatting=Formatting.Indented});
    }
   
}