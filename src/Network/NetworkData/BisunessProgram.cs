using DataODBC;
using System;
using System.Linq;

namespace COM { }
namespace odbc
{
    class BusinessProgram
    {
        static void Run(string[] args)
        {
            var odbc = new OdbcDataSource("ASpbMarketPlace", "root", "sgdf1423");
            odbc.EnsureIsValide();
            var dm = new OdbcDatabaseManager(odbc);
            dm.Discovery();

            System.Console.Clear();
            dm.GetKeywords().ToJsonOnScreen().WriteToConsole();
            //var tm = dm.memory.First().Value;

            //tm.SelectAll().ToJsonOnScreen().WriteToConsole();
        }
    }
}
