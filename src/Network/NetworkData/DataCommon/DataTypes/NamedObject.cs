using DetailsAnnotationsNS;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Utils;

using ValidationAnnotationsNS;

 

namespace Data.Resources.DataCommon.DataTypes
{

    [EntityLabel("Именованый ресурс")]
    public class NamedObject : BaseEntity, INamedObject
    {
        public string Method { get; set; }
        public string Path { get; set; }

        [Label("Наименование")]
        [NotNullNotEmpty(ErrorMessage = "Введите наименование")]
        [UniqValidation]
        public virtual string Name { get; set; } = "";
        [Label("Описание")]
        [NotNullNotEmpty]
        [DataType(DataType.MultilineText)]
        public virtual string Description { get; set; } = "";
    }
}
