
using COM;



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class RandomExtensions
{

    public static T GetRandom<T>(this IEnumerable<T> set) where T : class
    {
        T[] arr = set.ToArray<T>();
        int n = Randomizing.GetRandomInt(0, arr.Length);
        return arr[n];
    }

}