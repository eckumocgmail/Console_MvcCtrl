using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class ObjectPrototypesExtensions
{
    public static object Get(this object p, string name){
        return p.GetType().GetProperty(name).GetValue(p);
    }
    public static T Get<T>(this object p, string name)
    {
        return (T)p.GetType().GetProperty(name).GetValue(p);
    }
    public static object Set(this object p, string name, object value)
    {
        var property = p.GetType().GetProperty(name);
        if (property == null) return null;
        property.SetValue(p, value);
        return p;
    }
}