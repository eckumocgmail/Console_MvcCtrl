using System;
using System.ComponentModel;

namespace DetailsAnnotationsNS{
/******* * D:\gitlab\auth\Data\Resources\Common\DetailsAnnotations\ClassAttribute.cs **************/
public class ClassAttribute : Attribute
{
}
    
/******* * D:\gitlab\auth\Data\Resources\Common\DetailsAnnotations\ClassDescriptionAttribute.cs **************/
public class ClassDescriptionAttribute : Attribute
{    
    public ClassDescriptionAttribute(string message="")
    {
    }
}
public class DetailsAttribute : Attribute
{
    public DetailsAttribute(string message = "")
    {
    }
}
    
/******* * D:\gitlab\auth\Data\Resources\Common\DetailsAnnotations\DescriptionAttribute.cs **************/
public class DescriptionAttribute : Attribute
{
    private readonly string _message;

    public  DescriptionAttribute(string message)
    {
        _message = message;
    }
}
    
/******* * D:\gitlab\auth\Data\Resources\Common\DetailsAnnotations\EntityIconAttribute.cs **************/
public class EntityIconAttribute : Attribute
{
    public EntityIconAttribute(string text)  
    {
    }
}
    
/******* * D:\gitlab\auth\Data\Resources\Common\DetailsAnnotations\EntityLabelAttribute.cs **************/
public class EntityLabelAttribute: DisplayNameAttribute
{
    public EntityLabelAttribute(string text) : base(text)
    {
    }
}
    
/******* * D:\gitlab\auth\Data\Resources\Common\DetailsAnnotations\HelpMessageAttribute.cs **************/
public class HelpMessageAttribute : Attribute
{    
    public HelpMessageAttribute(string message)
    {
    }
}
    
/******* * D:\gitlab\auth\Data\Resources\Common\DetailsAnnotations\IconAttribute.cs **************/
public class IconAttribute : Attribute
{
    public IconAttribute(string icon)
    {
    }
}
    
/******* * D:\gitlab\auth\Data\Resources\Common\DetailsAnnotations\LabelAttribute.cs **************/
public class LabelAttribute : DisplayNameAttribute
{
    public LabelAttribute(string text): base( text )
    {
    }
}
    
/******* * D:\gitlab\auth\Data\Resources\Common\DetailsAnnotations\UnitsAttribute.cs **************/
public class UnitsAttribute : Attribute
{
    private readonly string _postfix;

    public UnitsAttribute( string postfix ) {
        _postfix = postfix;
    }
}
    

} // end of {ns} 
