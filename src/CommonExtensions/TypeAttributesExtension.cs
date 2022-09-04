using COM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TypeAttributesExtension
{

    public static string Label(this Type type) => AttrUtils.LabelFor(type);
    public static string Label(this Type type, string property) => AttrUtils.LabelFor(type, property);
    public static string Description(this Type type) => AttrUtils.DescriptionFor(type);
    public static string Icon(this Type type) => AttrUtils.IconFor(type);
    public static bool IsEntity(this Type type) => type.IsExtendsFrom(nameof(BaseEntity));
}