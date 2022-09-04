using Newtonsoft.Json.Linq;

using ApplicationCore.Domain.Odbc.Metadata;

using System.Collections.Generic;

namespace DataODBC
{
    public class OdbcMySqlDataSource: OdbcDataSource
    {
        public OdbcMySqlDataSource() : base("mysql_app", "root", "sgdf") { }
        public OdbcMySqlDataSource(string connectionString) : base(connectionString) { }
        public OdbcMySqlDataSource(string datasource, string login, string password):base(datasource,login,password) 
        { 

        }

        private JArray GetForeightKeys()
        {
            string sql = "SELECT "+
                         "TABLE_NAME, "+
                         "COLUMN_NAME, "+
                         "CONSTRAINT_NAME,  "+
                         "REFERENCED_TABLE_NAME, "+
                         "REFERENCED_COLUMN_NAME " +
                         "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE ";
             
            return this.Execute(sql);
        }

        public override DatabaseMetadata GetDatabaseMetadata()
        {
            DatabaseMetadata dbm = base.GetDatabaseMetadata();
            foreach(JObject next in this.GetForeightKeys())
            {
                string table = next["TABLE_NAME"].Value<string>();
                string column = next["COLUMN_NAME"].Value<string>();
                string refTable = next["REFERENCED_TABLE_NAME"].Value<string>();
                string refColumn = next["REFERENCED_COLUMN_NAME"].Value<string>();
                if( refTable != null)
                {
                    dbm.Tables[table].fk[column] = refTable;                    
                }
                else
                {
                    //dbm.Tables[table].pk = column;
                }
            }
            foreach (var p in dbm.Tables)
            {
                p.Value.references = new List<string>();
            }
            foreach ( var p in dbm.Tables)
            {
                string table = p.Key;                
                foreach ( var nextKey in p.Value.fk)
                {
                    string column = nextKey.Key;
                    string refTable = nextKey.Value;
                    dbm.Tables[refTable].references.Add(table);
                }
            }
                
            return dbm;
        }

    }
}
