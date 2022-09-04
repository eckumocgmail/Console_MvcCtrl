using COM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks;

public static class ObjectCompileExpExtensions
{


    public static object FromText( this Type type, string text )
        => Setter.FromText(text, type.Name);
   
    
    public static object Compile( this object context, string expression )
    {
        try
        {
            var res = Expression.Compile(expression, context);
            return res;
        }
        catch( Exception ex )
        {
            throw new Exception($"Не удалось выполнить символьное выражение: {expression} \n {ex.ToString()}");
        }
    }


}