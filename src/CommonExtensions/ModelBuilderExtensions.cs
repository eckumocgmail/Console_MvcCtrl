 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttributeEntityNS;
using DetailsAnnotationsNS;
using COM;

public static class ModelBuilderExtensions
{
    public static void ApplyAnnotations( this object builder, object context )
    {
   
        foreach (var type in context.GetEntitiesTypes())
        {
            Dictionary<string, string> dictionary = GetEntityContrainsts(type);
            foreach (var p in dictionary)
            {
                switch (p.Key)
                {
                    case nameof(EntityLabelAttribute):
                        Invoke(builder,"Entity", type);//.HasComment(p.Value);
                        break;

                    case nameof(UniqueConstraintAttribute):
                        //builder.Invoke("Entity", type).HasIndex(p.Value.Split(",")).IsUnique();
                        HasIndex(Invoke(builder,"Entity", type),p.Value.Split(","));
                        break;
                     
                }

            }
        }
       
    }

    private static object Invoke(object builder, string name, Type type)
    {
        throw new NotImplementedException();
    }

    private static void HasIndex(object builder, string[] vs)
    {
        throw new NotImplementedException();
    }

    public static Dictionary<string, string> GetEntityContrainsts(Type type)
    {
        Dictionary<string, string> attrs = new Dictionary<string, string>();
        foreach (var data in type.GetCustomAttributesData())
        {
            if (data.AttributeType.IsExtendsFrom(typeof(ModelCreatingAttribute).Name))
            {
                foreach (var arg in data.ConstructorArguments)
                {
                    string value = arg.Value.ToString();
                    attrs[data.AttributeType.Name] = value;
                }
            }

        }
        return attrs;
    }
}
 