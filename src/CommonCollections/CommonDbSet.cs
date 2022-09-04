using API;

using CommonTests.CommonCollections;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using static EcKuMoC;

namespace eckumoc_common_api.CommonModule.CommonCollections
{
    public class CommonDbSet<T> : CommonDictionaryStatefull<T>, ISuperDbSet<T> where T : BaseEntity
    {
        string code { get; }
        public CommonDbSet(string code) : base(code)
        {
            this.code = code;
        }

        public override void Init(string state)
        {

            if (string.IsNullOrEmpty(state) == false)
            {
                ICommonDictionary<IDictionary<string, string>> data = DeserializeBinnary(state.GetBytes());

                data.ForEach(AppendKeyValuePairCallback);
            }

        }


        //OnDatasetInit
        private void AppendKeyValuePairCallback(IDictionary<string, string> obj)
        {

            obj.ToList().ForEach((kv) =>
            {
                this[this.code + "/" + kv.Key] = kv.Value.FromJson<T>();
            });
        }

     
    }
}
