
using ApplicationCore.Domain.Odbc.Metadata;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.CoreAPI
{
    public interface APIDataSource
    {
        DatabaseMetadata GetDatabaseMetadata();
        JArray GetJsonResult(string sql);
        JObject GetSingleJObject(string command);

        bool canConnect();
        bool canReadAndWrite();
        bool canCreateAlterTables();

        string GetConenctionString();
        object SingleSelect(string sql);
        object MultiSelect(string sql);

        



        object Prepare(string sql);
        
        IEnumerable<string> GetTables();
        void InsertBlob(string sql, string v, byte[] data);
    }
}
