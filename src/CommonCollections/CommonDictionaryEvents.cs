using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTests.CommonCollections
{
    public class CommonDictionaryEvents<T>: EcKuMoC, IDictionary<string, T> where T: class
    {
        protected IDictionary<string, T> Memory = new Dictionary<string, T>();

        public CommonDictionaryEvents( )
        {            
            this.OnGet += DoGet;
            this.OnSet += DoSet;
        }

        

        private void Deseriallize<T1>(string state)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// Обработчик события считывания
        /// </summary>
        public Action<string, T> OnGet { get; set; } = (name,item)=> { };
        public void DoGet( string name, T item )
        {
            //Counter++;
            
        }

        /// <summary>
        /// Обработчик изменения данных
        /// </summary>
        public Action<string, T, T> OnSet { get; set; } =(name, before, after) => {};
        public void DoSet(string name, T before, T after)
        {
            //this.State = this.Seriallize();
        }


        public Action<string, T> OnRemove { get; set; } = (name,  after) => { };

       

        public void Add(string key, T value)
        {
            Memory.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return Memory.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return Memory.Remove(key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out T value)
        {
            return Memory.TryGetValue(key, out value);
        }

        public T this[string key] { get =>
                Memory.ContainsKey(key) ? Memory[key] : null;
            set => Memory[key] = value; }
      

        public ICollection<string> Keys => Memory.Keys;

        public ICollection<T> Values => Memory.Values;

        public void Add(KeyValuePair<string, T> item)
        {
            Memory.Add(item);
        }

        public void Clear()
        {
            Memory.Clear();
        }

        public bool Contains(KeyValuePair<string, T> item)
        {
            return Memory.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            Memory.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, T> item)
        {
            return Memory.Remove(item);
        }

        public int Count => Memory.Count;

        public bool IsReadOnly => Memory.IsReadOnly;

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return Memory.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Memory).GetEnumerator();
        }
    }
}
