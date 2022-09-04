using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ValidationAnnotationsNS;

public class Methods
{
    public class BaseMethodAttribute: BaseValidationAttribute
    {

        public BaseMethodAttribute(): base()
        {

        }

        public override string Validate(object model, string property, object value)
        {
            throw new NotImplementedException();
        }

        public override string GetMessage(object model, string property, object value)
        {
            throw new NotImplementedException();
        }
    }
}
