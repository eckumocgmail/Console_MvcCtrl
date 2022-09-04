using DataADO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.DataModels.MessageModel
{
    public class MessageDbContext : SqlServerMigBuilder
    {
        public MessageDbContext()
        {
            AddEntityType(typeof(MessageAttribute));
            AddEntityType(typeof(MessageProperty));
            AddEntityType(typeof(MessageProtocol ));
            AddEntityType(typeof(ValidationModel));
        }
    }
}
