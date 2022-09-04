using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ValidationAnnotationsNS;

public class BaseEntity: MyValidatableObject
{



    public bool Is<TType>() => GetType().IsExtendsFrom(typeof(TType).Name);
    public TType Cast<TType>() =>
        Is<TType>() ? ((TType)this.MemberwiseClone()) :
        throw new System.Exception($"Объект типа {GetType().Name} не возможно привети к типу {typeof(TType).Name}");
    public TType Convert<TType>() =>
        Is<TType>() ? ((TType)this.MemberwiseClone()) :
        this.ToJson().FromJson<TType>();
}