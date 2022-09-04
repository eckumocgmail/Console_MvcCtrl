using API;

using COM;

using DetailsAnnotationsNS;

using eckumoc_common_api.CommonCollections;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



public class CustomAttrs
{


    private static bool LogCustomization  = false;
    private static int CustomTypeAttributesCount = 0;

    /// <summary>
    /// Ключ -имя типа
    /// значение - карта 
    ///     имя атрибута-значение атрибута
    /// </summary>
    private static IDictionary<string, IDictionary<string, string>> CustomTypeAttriburtes =
        new ConcurrentDictionary<string, IDictionary<string, string>>();

    /// <summary>
    /// Ключ -имя типа
    /// значение - карта 
    ///     имя свойства( метода ) - значение карта
    ///                 имя атрибута-значение атрибута
    /// </summary>
    private static int CustomMemberAttributesCount = 0;
    private static IDictionary<string, IDictionary<string, IDictionary<string, string>>> CustomMemberAttriburtes =
        new Dictionary<string, IDictionary<string, IDictionary<string, string>>>();


    /// <summary>
    /// Добавление настраиваемого атрибута для типа
    /// </summary>
    /// <param name="name"></param>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static int AddTypeAttr(Type type, string Name, string Value)
    {
        //проверяем наличие обьявления атрибута заданного именем
        Type atrType = Name.ToType();
        if (atrType == null)
        {
            throw new Exception($"Атрибут {Name} не зарегистрирован ");
        }
        if (atrType.IsExtendsFrom("Attribute")==false)
        {
            throw new Exception($"Тип {Name} не является атрибутом");
        }
        if (CustomTypeAttriburtes.ContainsKey(type.Name) == false)
        {
            CustomTypeAttriburtes[type.Name] = ForType(type);
        }
        if (CustomTypeAttriburtes[type.Name].ContainsKey(Name))
        {
            throw new Exception($"Атрибут {Name} уже определён для типа {type.Name}");
        }
        CustomTypeAttriburtes[type.Name][Name] = Value;
        CustomTypeAttributesCount++;
        return CustomTypeAttributesCount;
    }

    private static IDictionary<string, string> ForType(Type type)
    {
        return CustomTypeAttriburtes.ContainsKey(type.GetTypeName()) ?
            CustomTypeAttriburtes[type.GetTypeName()] : AttrUtils.ForType(type);
    }

    /// <summary>
    /// Добавление настраиваемого атрибута для типа
    /// </summary>
    /// <param name="name"></param>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static int AddMemberAttr(Type type, string Member, string Name, string Value)
    {
        //проверяем наличие обьявления атрибута заданного именем
        Type atrType = Name.ToType();
        if (atrType == null)
        {
            throw new Exception($"Атрибут {Name} не зарегистрирован ");
        }
        if (atrType.IsExtendsFrom("Attribute")==false)
        {
            throw new Exception($"Тип {Name} не является атрибутом");
        }
        if (CustomMemberAttriburtes.ContainsKey(type.Name) == false)
        {
            CustomMemberAttriburtes[type.Name] = GetAttributesByMemberForType(type);
        }
        if (type.GetProperty(Member) == null && type.GetMethod(Member) == null)
        {
            throw new Exception($"Тип {type.Name} не определяет ни свойства ни метода с именем {Member}");
        }
        if (CustomMemberAttriburtes[type.Name].ContainsKey(Member) == false)
        {
            if (type.GetProperty(Member) != null)
            {
                CustomMemberAttriburtes[type.Name][Member] = ForProperty(type, Member);
            }
            else
            {
                CustomMemberAttriburtes[type.Name][Member] = ForMethod(type, Member);
            }
        }
        if (CustomMemberAttriburtes[type.Name][Member].ContainsKey(Name))
        {
            throw new Exception($"Элемент типа {type.Name} уже обьявляет атрибут {Name} для свойства (или метода) {Member}");
        }
        CustomMemberAttriburtes[type.Name][Member][Name] = Value;
        CustomMemberAttributesCount++;
        return CustomMemberAttributesCount;
    }

    private static IDictionary<string, string> ForMethod(Type type, string member)
        => AttrUtils.ForMethod(type, member);

    private static IDictionary<string, string> ForProperty(Type type, string member)
        => AttrUtils.ForProperty(type, member);

    private static IDictionary<string, IDictionary<string, string>> GetAttributesByMemberForType(Type type)
    {
        return AttrUtils.ForAllPropertiesInType(type);
    }


    /// <summary>
    /// Добавление настраиваемого атрибута Label для типа.
    /// Исп. краткого текстного наименования для контекнта
    /// </summary>
    public static int AddLabelForType(Type type, string Value)
    {
        try
        {
            if(LogCustomization)
                Console.WriteLine("Добавлен настраиваемый атрибут " +
                    nameof(EntityLabelAttribute) + " со значением [" + Value + "] для типа " + type.Name);
            int res = AddTypeAttr(type, nameof(EntityLabelAttribute), Value);
            if (LabelFor(type) != Value)
            {
                throw new Exception("Атрибут установлен не корректно");
            }
            return CustomMemberAttributesCount++;

        }
        catch(Exception ex)
        {
            throw new Exception($"СustomAttributes.AddLabelForType({type}) => {ex.Message} \n {ex.ToString()}");
        }
    }

    private static string LabelFor(Type type)
     => AttrUtils.LabelFor(type);




    /// <summary>
    /// Добавление настраиваемого атрибута Label для типа.
    /// Исп. краткого текстного наименования для контекнта
    /// </summary>
    public static int AddIconForType(Type type, string Value)
    {
        return AddTypeAttr(type, nameof(EntityIconAttribute), Value);
    }

}