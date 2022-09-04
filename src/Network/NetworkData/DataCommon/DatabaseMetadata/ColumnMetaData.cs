using System;
using System.Collections.Generic;

using ValidationAnnotationsNS;

namespace ApplicationCore.Domain.Odbc.Metadata
{
    /// <summary>
    /// Класс определяет свойства атрибута сущности.
    /// </summary>
    public class ColumnMetaData: MyValidatableObject
    {
        public string name = "";
        public string description = "";
        public string caption = "";

        
        public string provider = "SqlServer";

        /// <summary>
        /// Тип данных Sql
        /// </summary>
        [NotNullNotEmpty]        
        public string type;

        public bool primary = false;
        public bool incremental = false;
        public bool unique = false;
        public bool nullable = false;

        public ColumnMetaData()
        {
        }

        public ColumnMetaData(ColumnMetaData columnMetaData)
        {
             
        }

        public string cstype { get; set; }
        public string input_type { get; set; }
    }
}
