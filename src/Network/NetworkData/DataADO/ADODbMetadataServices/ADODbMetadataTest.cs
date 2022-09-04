using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public class ADODbMetadataTest: TestingElement
    {
        public static void Run()
        {
            ADODbMetadataTest test = new ADODbMetadataTest();
            test.SqlServerDatabase_GetTableMetadata_Test();            
            test.SqlServerDatabase_GetProcedureMetadata_Test();
        }

        public void SqlServerDatabase_GetTableMetadata_Test()
        {
            using (var database = new SqlServerDbMetadata())
            {
                database.GetTablesMetadata().ToJsonOnScreen().WriteToConsole();
            }
        }

        public void SqlServerDatabase_GetProcedureMetadata_Test()
        {
            using (var database = new SqlServerDbMetadata())
            {
                foreach(var KeyValuePair in database.GetProceduresMetadata("dbo"))
                {
                    KeyValuePair.Value.ToJsonOnScreen().WriteToConsole();
                }
            }
        }

        protected override void OnTest()
        {
            Run();
        }
    }
}
