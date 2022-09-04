using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreConstructorAngular.Data.DataConverter.Models
{
    public class MyNavigationOptions
    {      
        public string ForeignProperty { get; set; }
        public bool IsCollection { get; set; }

    }
}
