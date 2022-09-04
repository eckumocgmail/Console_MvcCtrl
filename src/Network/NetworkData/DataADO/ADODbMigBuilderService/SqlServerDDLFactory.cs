using ApplicationCore.Domain.Odbc.Metadata;

using COM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public class SqlServerDDLFactory : IDDLFactory
    {

        public TableMetaData CreateTableMetaData(Type typeofEntity)
        {
            TableMetaData tmd = new TableMetaData();
            tmd.name = typeofEntity.Name;
            tmd.columns = new Dictionary<string, ColumnMetaData>();
            foreach (var key in typeofEntity.GetProperties().Select(p=>p.Name))
            {


                try
                {
                    var p = typeofEntity.GetProperty(key);
                    var propertyType = typeofEntity.GetProperty(p.Name).PropertyType;
                    var meta = new ColumnMetaData()
                    {
                        primary = p.Name == "ID",
                        incremental = p.Name == "ID",
                        nullable = false,
                        caption = Utils.LabelFor(typeofEntity, p.Name),
                        description = Utils.DescriptionFor(typeofEntity, p.Name),
                        name = p.Name.ToTSQLStyle(),
                        unique = Utils.IsUniq(typeofEntity, p.Name),
                        cstype = p.PropertyType.GetName(),
                        input_type = Utils.GetInputType(typeofEntity, p.Name)
                    };


                    meta.EnsureIsValide();
                    meta.type =
                        p.Name == "ID" ? "int" :
                            GetDataTypeFor(propertyType);
                    tmd.columns[p.Name] = meta;
                }
                catch( Exception )
                {
                    continue;
                }
            }
            tmd.EnsureIsValide();
            return tmd;
        }

        private bool IsNullable(Type typeofEntity, string name)
        {
            throw new NotImplementedException();
        }

        private string GetDataTypeFor(Type propertyType)
        {
            switch(propertyType.GetName().Trim())
            {
                
                case "Int32": 
                case "Int64": 
                case "Int": 
                    return "int";
                case "DateTime": return "date";
                case "String": return "nvarchar(max)";
                default: throw new Exception("Не поддерживаемый тип данных "+propertyType.GetName());
            }
        }




        /// <summary>
        /// Создать внешний ключ
        /// </summary>
        /// <returns></returns>
        public string CreateForeignkey(string relativeTable, string table, string column, bool? onDeleteCascade = false, bool? onUpdateCascade = null)
        {
            return $"ALTER TABLE {table} ADD CONSTRAINT {table + relativeTable} FOREIGN KEY {column} REFERENCES ({relativeTable})" + " ON DELETE " + (onDeleteCascade == true ? "CASCADE" : "NO ACTION ") + " ON UPDATE " + (onUpdateCascade == true ? "CASCADE" : "NO ACTION ");
        }


        /// <summary>
        /// Создать таблицу
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public string CreateTable(TableMetaData metadata)
        {
            string sql = $"CREATE TABLE [{metadata.schema}].[{metadata.name}]\n";
            sql += "(";
            Action<KeyValuePair<string, ColumnMetaData>> AddColumnDefinition = (p) => {
                sql += $"\n  {p.Key.ToTSQLStyle()} {p.Value.type} ";
                if (p.Key.ToLower() == metadata.pk.ToLower())
                {
                    if (p.Value.incremental)
                    {
                        sql += " IDENTITY(1,1) ";
                    }
                    sql += ((p.Value.nullable) ? " NULL " : "NOT NULL ");
                    sql += " PRIMARY KEY,";
                }
                else
                {
                    sql += ((p.Value.nullable) ? " NULL " : "NOT NULL ");
                    sql += ",";
                }
            };
            foreach (var p in metadata.columns)
            {
                if (p.Key == "ID")
                    AddColumnDefinition(p);
            }
            foreach (var p in metadata.columns)
            {
                if (p.Key != "ID")
                    AddColumnDefinition(p);
            }
            if (metadata.columns.Count > 0)
            {
                sql = sql.Substring(0, sql.Length - 1);
            }
            sql += "\n)\n";
            return sql;
        }

        public string CreateTable(Type metadata)
        {
            var tableMetaData = CreateTableMetaData(metadata);
            return CreateTable(tableMetaData);
        }

        
    }
}
