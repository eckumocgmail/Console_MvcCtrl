using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class Serializer
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public interface ISerializeBinnary<TObject>
    {
        byte[] SerializeBinnary(TObject target);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public interface IDeserializeBinnary<TObject>
    {
        TObject DeserializeBinnary(byte[] data);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBinnarySerializer<T> : ISerializeBinnary<T>, IDeserializeBinnary<T>
    {
    }
}