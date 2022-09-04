using COM;

using System;

 
public static class TypeAttrsExtensions
{
    public static string SetLabel(this Type type, string text)
    {
        CustomAttrs.AddLabelForType(type, text);
        return text; 
    }
}
 