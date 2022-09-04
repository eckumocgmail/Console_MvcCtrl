using ApplicationCore.CoreAPI;
using ApplicationCore.Domain.Odbc.Metadata;

using COM;

using DataADO;

using DataCommon;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StatReports.BusinessResources.DataADO.ADODataSource
{
    public class SqlServerADODataSource: SqlServerADOService, APIDataSource
    {

        public static string FOREIGN_KEYS =
                "\nSELECT " +
                    "CCU.TABLE_NAME AS SOURCE_TABLE " +
                    ",CCU.COLUMN_NAME AS SOURCE_COLUMN " +
                    ",KCU.TABLE_NAME AS TARGET_TABLE " +
                    ",KCU.COLUMN_NAME AS TARGET_COLUMN " +
                "FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CCU " +
                    "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC " +
                    " ON CCU.CONSTRAINT_NAME = RC.CONSTRAINT_NAME " +
                    "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU " +
                    " ON KCU.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME " +
                "ORDER BY CCU.TABLE_NAME\n";
        private IDataTableService DataTableService = new DataTableService();
        public IEnumerable<string> GetTables() => base.GetTables(ToString());
        public SqlServerADODataSource(string server, string database): base(server, database)
        {
        }

        public SqlServerADODataSource()
        {
            //ID = GetType().Name;
        }

        public DatabaseMetadata GetDatabaseMetadata()
        {
            var metadata = new DatabaseMetadata();
            metadata.SetProcedures(GetStoredProcedures());
            foreach(string table in GetTables())
            {
                metadata.Tables[table] = GetTableMetaData(table);                
            }
            DataTable foreignKeysDataTable = this.ExecuteQuery(FOREIGN_KEYS);
            foreach (DataRow row in foreignKeysDataTable.Rows)
            {
                string SourceTable = row["SOURCE_TABLE"].ToString();
                string SourceColumn = row["SOURCE_COLUMN"].ToString();
                string TargetTable = row["TARGET_TABLE"].ToString();
                string TargetColumn = row["TARGET_COLUMN"].ToString();
                metadata.Keys[SourceTable + "." + SourceColumn] = TargetTable + "." + TargetColumn;
                if (TargetTable != null)
                {
                    metadata.Tables[SourceTable].fk[SourceColumn] = Naming.GetSingleCountName(TargetTable);
                }
                else
                {
                    metadata.Tables[SourceTable].pk = SourceColumn;
                }
            }
            return metadata;
        }

        private TableMetaData GetTableMetaData(string table)
        {
            TableMetaData metadata = new TableMetaData();
            metadata.name = table;
            DataTable columnDataTable = this.ExecuteQuery(base.ToString(), @"select * from INFORMATION_SCHEMA.COLUMNS");
            foreach(DataRow row in columnDataTable.Rows)
            {
                foreach (DataColumn column in columnDataTable.Columns)
                {
                    var columnMetaData = new ColumnMetaData();
                    string columnName = row["COLUMN_NAME"].ToString();
                    columnMetaData.name = columnName;
                    columnMetaData.type = row["DATA_TYPE"].ToString();
                    columnMetaData.nullable = row["IS_NULLABLE"].ToString().Trim().ToUpper() == "NO" ? false : true;
                    metadata.columns[columnName] = columnMetaData;                     
                }                
            }
            
            
            return metadata;
        }

        

        public bool canConnect()        
            => base.CanConnect(this.GetConenctionString());
        

        public bool canReadAndWrite()
        {
            throw new NotImplementedException();
        }

        public bool canCreateAlterTables()
        {
            throw new NotImplementedException();
        }

        public string GetConenctionString() =>
            base.ToString();

        public object SingleSelect(string sql)
        {
            throw new NotImplementedException();
        }

        public object MultiSelect(string sql)
        {
            throw new NotImplementedException();
        }

        public object Exec(string sql)
        {
            throw new NotImplementedException();
        }

        public object Prepare(string sql)
        {
            throw new NotImplementedException();
        }

        public JArray GetJsonResult(string sql)
        {
            DataTable ResultDataTable = this.ExecuteQuery(sql);
            JArray JResult = DataTableService.GetJArray(ResultDataTable);
            return JResult;
        }

        public JObject GetSingleJObject(string sql)
        {
            DataTable ResultDataTable = this.ExecuteQuery(sql);
            JArray JResult = DataTableService.GetJArray(ResultDataTable);
            JToken token = JResult.FirstOrDefault();
            return token != null ? (JObject)token : null;
        }

        

        public void InsertBlob(string sql, string v, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
