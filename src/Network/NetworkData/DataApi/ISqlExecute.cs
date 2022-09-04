using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataADO
{
    public interface ISqlExecute
    {
        public DataTable ExecuteProcedure(string name, IDictionary<string, string> input, IDictionary<string, string> output);
        public DataTable ExecuteQuery(string SQL);
        public int PrepareQuery(string SQL);
    }
    public interface ISqlExecuteService
    {
        public DataTable ExecuteQuery(string ConnectionString, string SQL);
        public int PrepareQuery(string ConnectionString, string SQL);
    }
}
