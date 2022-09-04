using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public interface IKV<K,V>
    {
        public K Key { get; }
        public V Value { get; }
    }
    public interface TodoWithSet<T>
    {
        int ForEach(Action<T> todo);
      
    }
    public interface ICommonDictionary<T>: TodoWithSet<T>, TodoWithSet<IKV<string, T>>
    {
        public string[] Indexes();
        public T[] Items();
        public bool Has(string key);
        public T RemoveByKey(string key);
        public T Get(string key);
        public T Set(string key, T value);


        public void ForEachValue(Action<T> On);
        public void ForEachKeyValue(Action<string, T> On);
        IEnumerable<KeyValuePair<string, T>> AsKeyValuePairs();
    }
}
