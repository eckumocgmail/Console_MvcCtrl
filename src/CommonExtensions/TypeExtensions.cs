using COM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public static class TypeExtensions
{

    //TextIOExtensionsTest,TextLangExtensionsTest,TypeAttributesExtensionTest,ThrowableExtensionsTest,TextParserExtensionsTest,TextIntExtensionsTest
    public static IEnumerable<object> News(this IEnumerable<Type> types)
    {
        return types.Select(t => (object)t.GetTypeName().ToType().New()).ToList();
    }
    public static Type[] GetParamTypes(this Type type)
    {
       
        return type.GenericTypeArguments;
    }
    public static bool IsExtendsFrom(this Type type, Type baseType)
    {
        return Typing.IsExtendedFrom(type, baseType.Name);
    }
    public static bool IsExtendsFrom(this Type type, string baseType)
    {
        return Typing.IsExtendedFrom(type, baseType);
    }


    public static MethodInfo GetPublicMethod( this Type type, string name)
    {
        var ptype = type;
        while (ptype != null)
        {
            var result = ptype.GetMethod(name);
            if (result != null && result.IsPublic )
                return result;
            ptype = ptype.BaseType;
        }
        return null;
    }
}