using ApplicationCore.Domain.Odbc.Metadata;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ValidationAnnotationsNS;

namespace DataCommon.DatabaseMetadata
{
    public class ColumnMetadata: MyValidatableObject
    {

        public ColumnMetadata() { }
        public ColumnMetadata(ColumnMetaData columnMetaData)
        {
            DataType = columnMetaData.type;
            ColumnName = columnMetaData.name;
        }
 

        //[NotNullNotEmpty]
        public string TableCatalog { get; set; }

        //[NotNullNotEmpty]
        public string TableSchema { get; set; }
        
        //[NotNullNotEmpty]
        public string TableName { get; set; }

        //[NotNullNotEmpty]
        public string ColumnName { get; set; }

        //[InputNumber]
        //[IsPositiveNumber]
        //[NotNullNotEmpty]
        public int OrdinalPosition { get; set; }
        public bool IsComputed { get; set; } = false;
        public string IsNullable { get; set; }
        public string DataType { get; set; }

        //[Label("Параметр сортировки")]
        public string CollationName { get; set; }

        
        public string CharacterSetName { get; set; }
    
    }
}
