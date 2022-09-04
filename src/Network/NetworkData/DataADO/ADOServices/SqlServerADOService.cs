 
using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace DataADO
{
    /// <summary>
    /// Сервис выполнения sql-запросов 
    /// </summary>
    public class SqlServerADOService : SqlServerConnectionString      
    {
        public string GetConnectionString() => base.ToString();
         

        public SqlServerADOService()
        {
        }
        public SqlServerADOService(string server, string database) : base(server, database)
        {
        }
        public SqlServerADOService(string server, string database, bool trustedConnection, string userID, string password) : base(server, database, trustedConnection, userID, password)
        {
        }
        


        //int Exec(string sql, IDictionary<string, string> input, IDictionary<string, string> output) { }


        /// <summary>
        /// Проверка доступности источника
        /// </summary>
        /// <param name="ConnectionString">строка соединения</param>
        /// <returns></returns>
        public bool CanConnect() => CanConnect(base.ToString());
        public bool CanConnect(string ConnectionString)
        {

            try
            {
                

                Info($"CanConnect({ConnectionString})");
                var tables = new List<string>();
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM sys.databases", con);
                    con.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string tableName = reader.GetString(0);
                        tables.Add(tableName);
                    }
                    reader.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Info("Ошибка при попытки установить соединение");
                Error(ex);
                return false;
            }
        }


        /// <summary>
        /// Выполнение запроса на выборку данных
        /// </summary>
        /// <param name="ConnectionString">строка соединения</param>
        /// <param name="SQL">текст sql-запроса</param>
        /// <returns>результирующий набор</returns>
        public DataTable ExecuteQuery(string SQL) => ExecuteQuery(ToString(), SQL);
        public DataTable ExecuteQuery(string ConnectionString, string SQL)
        {
            Info($"ExecuteQuery({ConnectionString},{SQL})");
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(SQL, con);
                con.Open();

                DataTable dataTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(SQL, con);
                adapter.Fill(dataTable);

                con.Close();
                return dataTable;
            }

        }

        /// <summary>
        /// Выполнение SQL комманды 
        /// </summary>
        /// <param name="ConnectionString">строка соединения</param>
        /// <param name="SQL">текст sql-команды</param>             
        public virtual int PrepareQuery(string SQL) => PrepareQuery(ToString(), SQL);
        public int PrepareQuery(string ConnectionString, string SQL)
        {
            int resultPrepareQuery = 0;
            Info($"PrepareQuery({ConnectionString},{SQL})");
            SQL.Split("GO").ToList().ForEach(operation => {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(operation, con);
                    con.Open();

                    int result = command.ExecuteNonQuery();
                    resultPrepareQuery += result;
                    con.Close();
                }
            });
            return resultPrepareQuery;
        }

        public DataTable ExecuteProcedure(string name, IDictionary<string, string> input, IDictionary<string, string> output)
        {
            throw new Exception();
            /*using (SqlConnection connection = new SqlConnection(base.ToString()))
            {
                // Create a SqlDataAdapter based on a SELECT query.
                SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT CategoryID, CategoryName FROM dbo.Categories", connection);

                // Create a SqlCommand to execute the stored procedure.
                adapter.InsertCommand = new SqlCommand("InsertCategory", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;

                // Create a parameter for the ReturnValue.
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@RowCount", SqlDbType.Int);
                parameter.Direction = ParameterDirection.ReturnValue;

                // Create an input parameter for the CategoryName.
                // You do not need to specify direction for input parameters.
                adapter.InsertCommand.Parameters.Add("@CategoryName", SqlDbType.NChar, 15, "CategoryName");

                // Create an output parameter for the new identity value.
                parameter = adapter.InsertCommand.Parameters.Add("@Identity", SqlDbType.Int, 0, "CategoryID");
                parameter.Direction = ParameterDirection.Output;

                // Create a DataTable and fill it.
                DataTable categories = new DataTable();
                adapter.Fill(categories);

                // Add a new row.
                DataRow categoryRow = categories.NewRow();
                categoryRow["CategoryName"] = "New Beverages";
                categories.Rows.Add(categoryRow);

                // Update the database.
                adapter.Update(categories);

                // Retrieve the ReturnValue.
                Int rowCount = (Int)adapter.InsertCommand.Parameters["@RowCount"].Value;

                System.Console.WriteLine("ReturnValue: {0}", rowCount.ToString());
                System.Console.WriteLine("All Rows:");
                foreach (DataRow row in categories.Rows)
                {
                    System.Console.WriteLine("  {0}: {1}", row[0], row[1]);
                }
            }*/
        }



        /// <summary>
        /// Получение списка таблиц базы данных
        /// </summary>
        /// <param name="ConnectionString">строка соединения ADO с сервером БД</param>
        /// <returns>список таблиц</returns>
        public IEnumerable<string> GetTables(string ConnectionString)
        {
            Info($"GetTables({ConnectionString})");
            var tables = new List<string>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT " +
                                                    "TABLE_NAME, " +
                                                    "COLUMN_NAME " +
                                                    //   "CONSTRAINT_NAME,  " +
                                                    //"REFERENCED_TABLE_NAME, " +
                                                    //  "REFERENCED_COLUMN_NAME " +
                                                    "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE ", con);
                con.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string tableName = reader.GetString(0);
                    if(tableName != "__EFMigrationsHistory")
                    {
                        tables.Add(tableName);
                    }
                    
                }
                reader.Close();
            }
            return tables;
        }
        public IEnumerable<string> GetStoredProcedures()
        {
            Info($"GetStoredProcedures({base.ToString()})");
            var proceduresList = new List<string>();
            using (SqlConnection con = new SqlConnection(base.ToString()))
            {
                SqlCommand command = new SqlCommand("exec sp_stored_procedures", con);
                con.Open();

                //"PROCEDURE_QUALIFIER","PROCEDURE_OWNER","PROCEDURE_NAME","NUM_INPUT_PARAMS","NUM_OUTPUT_PARAMS","NUM_RESULT_SETS","PROCEDURE_TYPE","REMARKS"
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader.GetString("PROCEDURE_NAME");
                    name = name.Substring(0, name.LastIndexOf(";"));
                    string owner = reader.GetString("PROCEDURE_OWNER");
                    string prefix = reader.GetString("PROCEDURE_QUALIFIER");
                    string fullname = $"{prefix}.{owner}.{name}";
                    proceduresList.Add(fullname);
                }
                reader.Close();
                return proceduresList.ToArray();
            }
        }

        /// <summary>
        /// Получение списка баз данных
        /// </summary>            
        public IEnumerable<string> GetDatabases() => GetDatabases(base.ToString());
        public IEnumerable<string> GetDatabases(string connectionString)
        {
            Info($"Databases({connectionString})");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT name FROM sys.databases", con);
                var databases = new List<string>();
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    databases.Add(reader.GetString(0));
                }
                reader.Close();
                return databases.ToArray();
            }
        }

    }

}
