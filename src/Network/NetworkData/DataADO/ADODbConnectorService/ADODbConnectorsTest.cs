using DataADO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public class ADODbConnectorsTest: TestingElement
    {
        public static void Run()
        {
            ADODbConnectorsTest test = new ADODbConnectorsTest();
            test.SqlServerDbConnector_Test();
            test.MySqlDbConnector_Test();
            test.PostgresDbConnector_Test();
        }

        private void PostgresDbConnector_Test()
        {
            PostgresDbConnector connector = new PostgresDbConnector();
            using(var connection = connector.CreateAndOpenConnection())
            {
                connector.Info(connection.ConnectionString);
            }
        }

        private void MySqlDbConnector_Test()
        {
            MySqlDbConnector connector = new MySqlDbConnector();
            using (var connection = connector.CreateAndOpenConnection())
            {
                connector.Info(connection.ConnectionString);
            }
        }

        private void SqlServerDbConnector_Test()
        {
            SqlServerDbConnector connector = new SqlServerDbConnector();
            using (var connection = connector.CreateAndOpenConnection())
            {
                connector.Info(connection.ConnectionString);
            }
        }

        protected override void OnTest()
        {
            Run();
        }
    }
}
