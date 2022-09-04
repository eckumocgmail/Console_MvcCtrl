using eckumoc.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eckumoc
{
    public class JsonProgramTest
    {
        public static void Test()
        {
            Console.WriteLine(
                JsonFileEditor.Get(
                    @"A:\Applications\netcoreapp3.1\odbc.json",
                    "Tables.accounts")
            );
            JsonFileEditor.Set(
                @"A:\Applications\netcoreapp3.1\odbc.json",
                "Tables.accounts", "this is a test");
            Console.WriteLine(
                JsonFileEditor.Get(
                    @"A:\Applications\netcoreapp3.1\odbc.json",
                    "Tables.accounts")
            );
        }
    }
}
