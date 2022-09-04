using COM;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class HaffmanAlgExtensions
{

    public static string HaffSeriallize(this object target, Dictionary<string, object> valuesMap, object v)
    {
        var json = target.ToJson();
        var enc = new CharacterEncoder();
        return enc.Encode(json);
    }
    public static TTable HaffDeseriallize<TTable>(this string target)
    {
        var enc = new CharacterEncoder();
        return enc.Decode(target).FromJson<TTable>();
    }
    public static TTable FromJson<TTable>(this string target)
    {
        try
        {
            return JsonConvert.DeserializeObject<TTable>(target);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message+"\nНе удалось десериализовать следующий JSON: \n"+ target);

 
        }
        //if (target.Length < 5) 
            //return (TTable)typeof(TTable).New();
        
        
    }
}