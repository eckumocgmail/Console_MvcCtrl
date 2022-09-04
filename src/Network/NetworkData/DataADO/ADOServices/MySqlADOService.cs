using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataADO;

namespace DataADO
{
    /// <summary>
    /// Сервис выполнения sql-запросов 
    /// </summary>
    public class MySqlADOService : MySqlConnectionString
    {
        public DataTable ExecuteQuery(string SQL) => ExecuteQuery(base.ToString(), SQL);
        public int PrepareQuery(string SQL) => PrepareQuery(base.ToString(), SQL);
        public string GetConnectionString() => this.ToString();

        public MySqlADOService(): base()
        {
            Info("Create");
        }

        /// <summary>
        /// Проверка доступности источника
        /// </summary>
        /// <param name="ConnectionString">строка соединения</param>
        /// <returns></returns>
        public bool CanConnect(string ConnectionString)
        {
            try
            {
                Info($"CanConnect({ConnectionString})");
                var tables = new List<string>();
                using (MySqlConnection con = new MySqlConnection(ConnectionString))
                {
                    MySqlCommand command = new MySqlCommand("SELECT " +
                                                                "TABLE_NAME, " +
                                                                "COLUMN_NAME, " +
                                                                "CONSTRAINT_NAME,  " +
                                                                "REFERENCED_TABLE_NAME, " +
                                                                "REFERENCED_COLUMN_NAME " +
                                                                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE ", con);
                    con.Open();

                    MySqlDataReader reader = command.ExecuteReader();
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
        public DataTable ExecuteQuery(string ConnectionString, string SQL)
        {
            Info($"ExecuteQuery({ConnectionString},{SQL})");
            using (MySqlConnection con = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand(SQL, con);
                con.Open();

                DataTable dataTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(SQL, con);
                adapter.Fill(dataTable);

                con.Close();
                return dataTable;
            }

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
            using (MySqlConnection con = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT " +
                                                            "TABLE_NAME, " +
                                                            "COLUMN_NAME, " +
                                                            "CONSTRAINT_NAME,  " +
                                                            "REFERENCED_TABLE_NAME, " +
                                                            "REFERENCED_COLUMN_NAME " +
                                                            "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE ", con);
                con.Open();

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string tableName = reader.GetString(0);
                    tables.Add(tableName);
                }
                reader.Close();
            }
            return tables;
        }


        /// <summary>
        /// Выполнение SQL комманды 
        /// </summary>
        /// <param name="ConnectionString">строка соединения</param>
        /// <param name="SQL">текст sql-команды</param> 
        public int PrepareQuery(string ConnectionString, string SQL)
        {
            Info($"PrepareQuery({ConnectionString},{SQL})");
            using (MySqlConnection con = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand(SQL, con);
                con.Open();

                int result = command.ExecuteNonQuery();
                return result;
            }
        }

        public DataTable ExecuteProcedure(string name, IDictionary<string, string> input, IDictionary<string, string> output)
        {
            throw new NotImplementedException();
        }
    }
}

