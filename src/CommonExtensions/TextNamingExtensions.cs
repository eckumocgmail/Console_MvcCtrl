using COM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static COM.Naming;

/// <summary>
/// Стилизация идентификаторов
/// </summary>
public static class TextNamingExtensions
{

    


    /// <summary>
    /// Метод определния стиля записи идентификатора
    /// </summary>
    /// <param name="name"> идентификатор </param>
    /// <returns> стиль записи </returns>
    public static NamingStyles ParseStyle(this string name)
    {
        return Naming.ParseStyle(name);
    }


    /// <summary>
    /// Запись идентификатора в CamelStyle
    /// </summary> 
    public static string ToCamelStyle(this string name)
    {
        return Naming.ToCamelStyle(name);
    }

    public static bool IsTsqlStyled(this string name) => Naming.IsTSQLStyle(name);
    public static bool IsCapitalStyled(this string name) => Naming.IsCapitalStyle(name);
    public static bool IsCamelStyle(this string name) => Naming.IsCamelStyle(name);
    public static bool IsSnakeStyle(this string name) => Naming.IsSnakeStyle(name);


    /// <summary>
    /// Запись идентификатора в CapitalStyle
    /// </summary> 
    public static string ToCapitalStyle(this string name)
    {
        return Naming.ToCapitalStyle(name);
    }


    /// <summary>
    /// Запись идентификатора в TSQL
    /// </summary> 
    public static string ToTSQLStyle(this string name)
    {
        return Naming.ToTSQLStyle(name);
    }
    



    /// <summary>
    /// Запись идентификатора в KebabStyle
    /// </summary> 
    public static string ToKebabStyle(this string lastname)
    {
        return Naming.ToKebabStyle(lastname);
    }





    /// <summary>
    /// Запись идентификатора в SnakeStyle
    /// </summary>
 
    public static string ToSnakeStyle(this string lastname)
    {
        return Naming.ToSnakeStyle(lastname);
    }

}
