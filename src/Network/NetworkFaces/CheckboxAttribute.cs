using System;

using static EcKuMoC;

public class CheckboxAttribute : ValidationAnnotationsNS.BaseValidationAttribute
{
    public string OnChangedMessage { get; set; }
    public string Expression { get; set; }


    public CheckboxAttribute()
    {
    }

    public override string Validate(object model, string property, object value)
    {
        var before = model.Get(property).Compile(Expression);
        throw new NotImplementedException();

    }

    public override string GetMessage(object model, string property, object value)
    {
        throw new NotImplementedException();
    }
}