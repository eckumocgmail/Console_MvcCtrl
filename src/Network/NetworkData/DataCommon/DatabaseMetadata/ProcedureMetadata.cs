using System.Collections.Generic;

namespace ApplicationCore.Domain.Odbc.Metadata
{
    public class ProcedureMetadata
    {
        public string name { get; set; }
        public string description { get; set; }

        public Dictionary<string, ColumnMetaData> input = new Dictionary<string, ColumnMetaData>();
        public Dictionary<string, ColumnMetaData> output = new Dictionary<string, ColumnMetaData>();

        public string ToSql()
        {
            string paramsstr = "";
            foreach(var p in this.input)
            {
                paramsstr += p.Key + "=?,";
            }
            if (paramsstr.Length > 0) paramsstr = paramsstr.Substring(0, paramsstr.Length - 1);
            return $"exec {name} " + paramsstr;
        }
    }
}
