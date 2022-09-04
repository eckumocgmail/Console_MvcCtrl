using System;





namespace AttributeEntityNS
{
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeEntity\EntityNavigationAttribute.cs **************/
    public class EntityNavigationAttribute: Attribute
    {
        private readonly string _propertyName;

        public EntityNavigationAttribute(string propertyName )
        {
            _propertyName = propertyName;
        }
    }
 

    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeEntity\ManyToMany.cs **************/
    public class ManyToMany: ModelCreatingAttribute
    {
    
        /// <param name="includeToProperty">
        /// Имя свойства коллекцией связанных обьектов
        /// </param>
        public ManyToMany( string includeToProperty )
        {

        }
    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeEntity\ModelCreatingAttribute.cs **************/
    public class ModelCreatingAttribute: Attribute
    {
        public ModelCreatingAttribute()
        {
        }

    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeEntity\SearchTermsAttribute.cs **************/
    public class SearchTermsAttribute: ModelCreatingAttribute
    {

        public SearchTermsAttribute(string terms) : base()
        {
            
        }
    }
 
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeEntity\SystemEntityAttribute.cs **************/
    public class SystemEntityAttribute : Attribute
    {
        private readonly string _message;

        public  SystemEntityAttribute(string message = "")
        {
            _message = message;
        }
    }
    
    /******* * D:\gitlab\auth\Data\Resources\Common\AttributeEntity\UniqueConstraintAttribute.cs **************/
    public class UniqueConstraintAttribute: ModelCreatingAttribute
    {
        private readonly string[] _columns;

        public UniqueConstraintAttribute( string sequence )
        {
            _columns = sequence.Split(",");            
             
        }
    }
 

    

} // end of {ns} 
