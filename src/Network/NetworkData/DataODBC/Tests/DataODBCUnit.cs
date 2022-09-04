namespace eckumoc_common_api.Network.NetworkDatabases.DataODBC.Tests
{

    public class DataODBCUnit : TestingUnit
    {
        public DataODBCUnit()
        {

        }
    }

    public class ODBCDataSourceTest: TestingElement
    {
        protected override void OnTest()
        {
            var dbm = OdbcDatabaseManager.GetOdbc("SpbPublicLibs");
            dbm.Discovery();

            dbm.GetKeywords().ToJsonOnScreen().WriteToConsole();

            dbm.Statistics.ToJsonOnScreen();


            var tm = dbm.GetTableManager("News");
            tm.GetKeywordsStatistics().ToJsonOnScreen().WriteToConsole();


            dbm.GetKeywords().ToJsonOnScreen().WriteToConsole();
        }
    }
}
