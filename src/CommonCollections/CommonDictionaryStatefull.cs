using API;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static EcKuMoC;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace CommonTests.CommonCollections
{

    /// <summary>
    /// Справочник реализует события изменения данных
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonDictionaryStatefull<T>: CommonDictionaryEvents<T>, ISuperDbSet<T>, ICommonDictionary<T> where T: BaseEntity
    {
      

        public string[] Indexes() => Memory.Keys.ToArray();        
        public bool Has(string key) => Memory.ContainsKey(key);
        public T[] Items() => Memory.Values.ToArray();



        public CommonDictionaryStatefull( string code ): base( )
        {
             
        }




        /// <summary>
        /// Доступ к объекты с заданным ключём
        /// </summary>
        public T Get(string key)
        {
            T item = Memory.ContainsKey(key) ? Memory[key] : null;
            OnGet(key,item);
            return item;
        }


        /// <summary>
        /// Определения ключа доступа к объекту
        /// </summary>
        public T Set(string key, T value)
        {
            T before = Memory.ContainsKey(key)? Memory[key]: null;
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
            if(Memory.ContainsKey(key))
            {
                T removed = this.Memory[key];
                OnRemove(key, removed);
                return removed;
            }
            OnRemove(key, null);
            return null;
        }

        public virtual void Init(string state)
        {

        }

        public int ForEach(Action<T> todo)
        {
            throw new NotImplementedException();
        }

        public int ForEach(Action<IKV<string, T>> todo)
        {
            throw new NotImplementedException();
        }

        public Task Update(T targetData)
        {
            throw new NotImplementedException();
        }

        public IKV<string, string> Find(int accountId)
        {
            throw new NotImplementedException();
        }

        public bool Add(T item)
        {
            throw new NotImplementedException();
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Type ElementType => throw new NotImplementedException();

        public Expression Expression => throw new NotImplementedException();

        public IQueryProvider Provider => throw new NotImplementedException();

        public void ForEachValue(Action<T> On)
        {
            throw new NotImplementedException();
        }

        public void ForEachKeyValue(Action<string, T> On)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, T>> AsKeyValuePairs()
        {
            throw new NotImplementedException();
        }
    }

    
}
