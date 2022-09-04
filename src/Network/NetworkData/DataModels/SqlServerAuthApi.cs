using DataADO;
 

using DataCommon.DatabaseMetadata;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataModels
{
    public class SqlServerAuthApi : SqlServerWebApi
    {

        public static bool Created = false;
         
        public SqlServerAuthApi()
        {
            Init();
        }

        public void CreateDatabaseFile(string database)
        {
            TryPrepareQuery(@$"CREATE DATABASE {database}");
        }

        public SqlServerAuthApi(string server, string database) : base(server, database)
        {
            Init();
        }

        public SqlServerAuthApi(string server, string database, bool trustedConnection, string userID, string password) : base(server, database, trustedConnection, userID, password)
        {
            Init();
        }

        private void Init()
        {
            if(Created==false)
            {
                DropTables();
                //AddEntityType(typeof(Auth.Data.Account));
                //AddEntityType(typeof(Auth.Data.Login));
                //AddEntityType(typeof(Auth.Data.Person));
                //AddEntityType(typeof(Auth.Data.User));
                UpdateDatabase();
                CreateServices();
                Created = true;
            }
             
        }

        public void DropTables()
        {
            GetTablesMetadata().ToList().ForEach(md => {
                PrepareQuery($"DROP TABLE [{md.Value.TableSchema}].[{md.Value.TableName}]");
            });
           
        }

        private void CreateServices()
        {
            foreach(var EntityType in EntityTypes)
            {
                var metadata = DDLFactory.CreateTableMetaData(EntityType);
                var fasade = new EntityFasade(this, new TableMetadata(metadata), EntityType);
                this.Services.Add(fasade);
            }
        }

        private TableMetadata FindMetaData(Type entityType)
        {
            GetTablesMetadata().ToJsonOnScreen().WriteToConsole();
            return GetTablesMetadata().Where(meta => meta.Value.TableName == entityType.Name).Select(kv=>kv.Value).FirstOrDefault();
        }
    }
}
