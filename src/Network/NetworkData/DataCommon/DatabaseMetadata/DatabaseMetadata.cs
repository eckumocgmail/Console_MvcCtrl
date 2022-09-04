using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Odbc.Metadata
{
    /// <summary>
    /// Класс определяет свойства базы данных
    /// </summary>
    public class DatabaseMetadata
    {
        public string driver;
        public string database;
        public string serverVersion;
        public string connectionString;

        public readonly Dictionary<string, TableMetaData> Tables = new Dictionary<string, TableMetaData>();
        public readonly Dictionary<string, object> Metadata = new Dictionary<string, object>();

        public IDictionary<string, string> GetStoredProcedures() => Procedures;

        public readonly IDictionary<string, string> Keys = new Dictionary<string, string>();
        private readonly IDictionary<string, string> Connections = new Dictionary<string, string>();
        private readonly IDictionary<string, string> Procedures = new Dictionary<string, string>();
        public IDictionary<string, string> GetConnections() => Connections;
        

        public DatabaseMetadata() { }

        public override string ToString()
        {
            return Newtonsoft.Json.Linq.JObject.FromObject(this).ToString();
        }

        public void Validate()
        {
            foreach( var ptable in this.Tables)
            {
                if (ptable.Value.getPrimaryKey() == null)
                {
                    throw new Exception($"Primary key undefined for table: {ptable.Key}");
                }              
            }
        }

        public void SetProcedures(IEnumerable<string> enumerable) => enumerable.ToList().ForEach(name => Procedures[name]=name);
         
    }
}
