using ApplicationCore.Domain.Odbc.Metadata;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using ValidationAnnotationsNS;

namespace DataCommon.DatabaseMetadata
{
    public class TableMetadata: MyValidatableObject
    {
        public TableMetadata()
        {
        }

        public TableMetadata(TableMetaData metadata)
        {
            TableName = metadata.name;
            TableSchema = metadata.schema;
            ColumnsMetadata = new Dictionary<string, ColumnMetadata>();
            foreach(var kv in metadata.columns)
            {
                ColumnsMetadata[kv.Key] = new ColumnMetadata(metadata.columns[kv.Key]);
            }
        }

 
        
        public string TableCatalog { get; set; }
        public string TableSchema { get; set; }

        [NotNullNotEmpty]
        public string TableName { get; set; }
        public string TableType { get; set; }

        [NotNullNotEmpty]
        [NotMapped]
        public IDictionary<string, ColumnMetadata> ColumnsMetadata { get; set; }

        [NotNullNotEmpty]
        public string PrimaryKey { get; set; } = "ID";

        /// <summary>
        /// Внешние ключи
        /// Ключ = [ColumnName] 
        /// Значение = [TableName]
        /// </summary>
        public IDictionary<string, string> ForeignKeys { get; set; } = new Dictionary<string, string>();
    }
}
