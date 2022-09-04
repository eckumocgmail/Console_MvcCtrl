using COM;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public static class ISuperSerExtensions
{
    /// <summary>
    /// Нехороший способ извеления наименований сущностей
    /// </summary>
    /// <param name="dbContext"> контекст данных </param>
    /// <returns> множество наименований сущностей </returns>
    public static HashSet<Type> GetEntitiesTypes(this object dbContext)
    {
        Type type = dbContext.GetType();
        HashSet<Type> entities = new HashSet<Type>();
        foreach (MethodInfo info in type.GetMethods())
        {
            if (info.Name.StartsWith("get_") == true && info.ReturnType.Name.StartsWith("ISuperSer"))
            {
                if (info.Name.IndexOf("MigrationHistory") == -1)
                {
                    entities.Add(info.ReturnType);
                }
            }
        }
        return entities;
    }


    public static HashSet<string> GetEntityTypeNames(this Type type)
    {       
        HashSet<string> entities = new HashSet<string>();
        foreach (MethodInfo info in type.GetMethods())
        {
            if (info.Name.StartsWith("get_") == true && info.ReturnType.Name.StartsWith("ISuperSer"))
            {
                if (info.Name.IndexOf("MigrationHistory") == -1)
                {
                    entities.Add(Typing.ParseCollectionType(info.ReturnType));
                }
            }
        }
        return entities;
    }







    public static Dictionary<string, object> GetOperations(this object _context)
    {
        var res = new Dictionary<string, object>();
        foreach (MethodInfo info in _context.GetType().GetMethods())
        {
            if (info.Name.StartsWith("get_") == true && info.ReturnType.Name.StartsWith("ISuperSer"))
            {
                if (info.Name.IndexOf("MigrationHistory") == -1)
                {
                    string displayName = GetShortDisplayName(info.ReturnType);
                    string entityTypeName = displayName.Substring(displayName.IndexOf("<") + 1);
                    entityTypeName = entityTypeName.Substring(0, entityTypeName.IndexOf(">"));
                    res[entityTypeName] = (dynamic)info.Invoke(_context, new object[0]);
                }

            }
        }
        return res;
    }

    private static string GetShortDisplayName(Type returnType)
    {
        throw new NotImplementedException();
    }

    public static dynamic GetISuperSer(this object _context, string entityTypeShortName)
    {
        try
        {
            foreach (MethodInfo info in _context.GetType().GetMethods())
            {
                if (info.Name.StartsWith("get_") == true && info.ReturnType.Name.StartsWith("ISuperSer"))
                {
                    if (info.Name.IndexOf("MigrationHistory") == -1)
                    {
                        string displayName = GetShortDisplayName(info.ReturnType);
                        string entityTypeName = displayName.Substring(displayName.IndexOf("<") + 1);
                        entityTypeName = entityTypeName.Substring(0, entityTypeName.IndexOf(">"));
                        if (entityTypeShortName == entityTypeName)
                        {
                            return (dynamic)info.Invoke(_context, new object[0]);
                        }
                    }

                }
            }
        }
        catch (Exception)
        {
            return (dynamic)null;
        }
        

        throw new Exception($"Сущность [{entityTypeShortName}] не определена в контексте базы данных "+_context.GetType().Name);
    }

 
    public static IQueryable<T> GetISuperSer<T>(this object _context) where T: class
    {
        return (IQueryable<T>)_context.GetISuperSer(typeof(T).Name);
    }
}
 