using COM;

using System;
using System.Collections.Generic;
using System.Linq;




public class FromAttributes
{
    public void Init(Type type)
    {
        var attrs = type.GetAttrs();
        GetType().GetOwnPropertyNames().ToList().ForEach((name) => {
            string key = name + "Attribute";
            if ( attrs.ContainsKey(key))
            {
                Setter.SetValue(this, name, attrs[key]);
            }
        });
    }

    public void Init(Type type,string prop)
    {
        var attrs = ForProperty(type,prop);
        GetType().GetOwnPropertyNames().ToList().ForEach((name) => {
            string key = name + "Attribute";
            if (attrs.ContainsKey(key))
            {
                Setter.SetValue(this, name, attrs[key]);
            }
        });
        
    }

    private Dictionary<string, string> ForProperty(Type type, string prop)
    {
        throw new NotImplementedException();
    }
}