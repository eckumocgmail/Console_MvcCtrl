using COM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Расширения коллекций
/// </summary>
public static class CollectionsIQuerableExtensions
{
    public static IEnumerable<T> GetPage<T>(this IEnumerable<T> set, int page, int size = 10)
    {
        return set.Skip((page - 1) * size).Take(size);
    }
  
    

}