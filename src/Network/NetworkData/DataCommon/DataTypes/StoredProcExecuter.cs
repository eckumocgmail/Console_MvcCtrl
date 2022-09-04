 
using ApplicationCore.Domain.Odbc.Metadata;
using System.Collections.Generic;
using System.Data.Odbc;

namespace DataODBC
{
    public class StoredProcExecuter
    {
        private readonly OdbcDataSource _dataSource;


        public StoredProcExecuter(OdbcDataSource dataSource)
        {
            _dataSource = dataSource;
        }


        public Dictionary<string, object> Execute(
            ProcedureMetadata metadata, 
            Dictionary<string, object> inputs)
        {
            Dictionary<string, object> outputs = new Dictionary<string, object>();            
            using (System.Data.Odbc.OdbcConnection connection = _dataSource.GetConnection())
            {
                connection.Open();
                OdbcCommand command = new OdbcCommand(metadata.ToSql(), connection);                
                int result = command.ExecuteNonQuery();
            }
            return outputs;
        }


        private static void AddParameter(OdbcCommand command, string key, object value )
        {
            command.Parameters.Add(key, OdbcType.Binary);
            command.Parameters[key].Value = value;
        }
    }
}