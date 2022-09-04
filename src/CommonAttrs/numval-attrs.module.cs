using ValidationAnnotationsNS;

namespace NumValAttrsNS{
public class IsIntNumberAttribute: BaseValidationAttribute
{
    public override string Validate(object model, string property, object value)
    {
        return (value != null && value.ToString().Trim().IsInt())? null: GetMessage(model, property, value);
    }

    public override string GetMessage(object model, string property, object value)
    {
        return $"Значение свойства {model.GetType().Name}.{property} не является целочисленным";
    }
}
    
    

} // end of {ns} 
