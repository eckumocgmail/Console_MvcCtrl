using COM;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Расширения коллекций
/// </summary>
public static class CollectionsExtensions
{
    public static void OnEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
    {
        source.ToList().ForEach(action);
    }
 

    public static void Print(this IEnumerable items)
    {
        foreach (var item in items)
        {
            Writing.ToConsole(item.ToString());
        }
    }

    public static void ForEach<T>(IEnumerable<T> items,Action<T> todo)
    {
        foreach (var item in new List<T>(items))
        {

            todo(item);
        }     
    }
    public static HashSet<T> AddRange<T>(this HashSet<T> set, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            set.Add(item);
        }
        return set;
    }
    public static Dictionary<string, T> AddRange<T>(this Dictionary<string,T> set, Dictionary<string, T> items)
    {
        foreach (var item in items)
        {

            set[item.Key] = item.Value;
        }
        return set;
    }
}



public static class IDictionaryExtensions
{

    public static Dictionary<string, string> Expect(
        this IDictionary<string, string> data, params string[] keys)
    {
        Dictionary<string, string> res = new Dictionary<string, string>();
        var expectedKeySet = new HashSet<string>(keys);
        foreach (var p in data)
        {
            if (expectedKeySet.Contains(p.Key) == false)
            {
                res[p.Key] = p.Value;
            }
        }
        return res;
    }
}
