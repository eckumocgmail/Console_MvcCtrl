using API;

using CommonTests.CommonCollections;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace eckumoc_common_api.CommonCollections
{
    /// <summary>
    /// Справочник реализует события изменения данных
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonDictionary<T> : CommonDictionaryEvents<T>, API.ICommonDictionary<T> where T : class
    {
              
        public T[] Items() => Memory.Values.ToArray();
        public string[] Indexes() => Memory.Keys.ToArray();
        public bool Has(string key) => Memory.ContainsKey(key);


        public void ForEachValue(Action<T> On)
        {
            foreach (var kv in Memory)
            {
                On(kv.Value);
            }
        }

        public void ForEachKeyValue(Action<string,T> On)
        {
            foreach(var kv in Memory)
            {
                On(kv.Key, kv.Value);
            }
        }





        /// <summary>
        /// Доступ к объекты с заданным ключём
        /// </summary>
        public T Get(string key)
        {
            T item = Memory.ContainsKey(key) ? Memory[key] : null;
            OnGet(key, item);
            return item;
        }


        /// <summary>
        /// Определения ключа доступа к объекту
        /// </summary>
        public T Set(string key, T value)
        {
            T before = Memory.ContainsKey(key) ? Memory[key] : null;
            Memory[key] = value;
            OnSet(key, before, value);
            return value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T RemoveByKey(string key)
        {
            if (Memory.ContainsKey(key))
            {
                T removed = this.Memory[key];
                OnRemove(key, removed);
                return removed;
            }
            OnRemove(key, null);
            return null;
        }

        public virtual int ForEach(Action<T> todo)
        {
            throw new NotImplementedException();
        }

        public virtual int ForEach(Action<IKV<string, T>> todo)
        {
            throw new NotImplementedException();
        }

        public ICollection<KeyValuePair<string, T>> AsKeyValuePairs() => Memory;

        IEnumerable<KeyValuePair<string, T>> ICommonDictionary<T>.AsKeyValuePairs()
        {
            throw new NotImplementedException();
        }
    }
}
