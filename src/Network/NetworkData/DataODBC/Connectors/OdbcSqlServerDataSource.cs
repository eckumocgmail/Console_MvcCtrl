using Newtonsoft.Json.Linq;

using ApplicationCore.Domain.Odbc.Metadata;
using System.Collections.Generic;
using COM;
using System;

namespace DataODBC
{
    public class OdbcSqlServerDataSource: OdbcDataSource
    {
        /**
         * Строка подключения к SQL Server через ODBC драйвер отличается от ADO-connectionString
         * только параметром, указывающем на драйвер 
         * Driver={SQL Server};
         * 
         * ADO-connection string
         *  Server=CCPL-1728;Database=spb-public-libs;Trusted_Connection=True;MultipleActiveResultSets=True
         * ODBC-connection string
         *  Driver={SQL Server};Server=CCPL-1728;Database=spb-public-libs;Trusted_Connection=True;MultipleActiveResultSets=True
         */
        public OdbcSqlServerDataSource() : this("Driver={SQL Server};Server=kest;Database=model;Trusted_Connection=True;MultipleActiveResultSets=True") {
            Info(base.GetConnection());
        }

        public OdbcSqlServerDataSource(string connectionString) : base("" + connectionString.Replace(@"\\", @"\")) {
            Info(base.GetConnection());
        }

        public OdbcSqlServerDataSource(string server,string database ) : base("Driver={SQL Server};"+ $"Server={server};Database={database};Trusted_Connection=True;MultipleActiveResultSets=True".Replace(@"\\", @"\"))
        {
            Info(base.GetConnection());
        }



        public static string FOREIGN_KEYS =
                "\nSELECT " +
                    "CCU.TABLE_NAME AS SOURCE_TABLE" +
                    ",CCU.COLUMN_NAME AS SOURCE_COLUMN" +
                    ",KCU.TABLE_NAME AS TARGET_TABLE" +
                    ",KCU.COLUMN_NAME AS TARGET_COLUMN" +
                "FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CCU " +
                    "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC " +
                    " ON CCU.CONSTRAINT_NAME = RC.CONSTRAINT_NAME " +
                    "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU " +
                    " ON KCU.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME " +
                "ORDER BY CCU.TABLE_NAME\n";

        /// <summary>
        /// Форматирует строку подключения ADO в ODBC для SQL Server.        
        /// Необходимо добавить параметр указывающий на драйвер ODBC для SQL Server;
        /// </summary>
        /// <param name="adoConnectionString"> строка соединения ADO </param>
        /// <returns> строка соединения ODBC </returns>
        public static string FromAdoToOdbcConnectionStringForSqlServer(string adoConnectionString)
        {
            return "Driver={SQL Server};" + adoConnectionString.Replace(@"\\", @"\");
        }

        /**
         * Структура таблиц
         *   SELECT TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS                                       
         */
        public override DatabaseMetadata GetDatabaseMetadata()
        {
            try
            {
                DatabaseMetadata dbm = base.GetDatabaseMetadata();
                /*JArray keys = this.Execute(FOREIGN_KEYS);

                foreach (JObject next in keys)
                {
                    string table = next["SOURCE_TABLE"].Value<string>();
                    string column = next["SOURCE_COLUMN"].Value<string>();
                    string targetTable = next["TARGET_TABLE"].Value<string>();
                    string targetColumn = next["TARGET_COLUMN"].Value<string>();
                    if (targetTable != null)
                    {
                        dbm.Tables[table].fk[column] = targetTable.ToSingleCount();
                    }
                    else
                    {
                        dbm.Tables[table].pk = column;
                    }
                }
                foreach (var p in dbm.Tables)
                {
                    p.Value.references = new List<string>();
                }
                foreach (var p in dbm.Tables)
                {
                    string table = p.Key;
                    foreach (var nextKey in p.Value.fk)
                    {
                        string column = nextKey.Key;
                        string refTable = nextKey.Value;
                        TableMetaData precord = null;
                        if (dbm.Tables.ContainsKey(Naming.GetMultiCountName(refTable)))
                        {
                            precord = dbm.Tables[Naming.GetMultiCountName(refTable)];
                        }
                        else
                        {
                            precord = dbm.Tables[Naming.GetSingleCountName(refTable)];
                        }
                        precord.references.Add(table);
                    }
                }*/

                return dbm;
            }
            catch(Exception ex)
            {
                Error("Не удалось получить метаданные источников ODBC: "+GetConenctionString(), ex);
                throw;
            }
        

        }
    }
}
