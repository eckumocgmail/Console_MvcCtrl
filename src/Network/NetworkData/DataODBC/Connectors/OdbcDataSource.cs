using ApplicationCore.CoreAPI;
 
using Newtonsoft.Json.Linq;
using ApplicationCore.Domain.Odbc.Metadata;
using System;
 
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using DataADO;

namespace DataODBC
{

    /**
     * 
        //System.Data.Odbc   @"Driver={MySQL ODBC 5.3 ANSI Driver};DATA SOURCE=mysql_app;Uid=root;Pwd=root;"
        //System.Data.Odbc   @"Driver={Microsoft Access Driver (*.mdb)};Dbq=C:\mydatabase.mdb;Uid=Admin;Pwd=;"
        //System.Data.Odbc   @"DRIVER={SQL SERVER};SERVER=(LocalDB)\\v11.0;AttachDbFileName=G:\projects\eckumoc\AppData\persistance.mdf;"   "
        //System.Data.OleDb  @"Provider=Microsoft.Jet.OLEDB.12.0;Data Source=a:\\master.mdb;";
     */
    public class OdbcDataSource: BaseService, APIDataSource
    {
        public DatabaseMetadata metadata { get; set; }
        public string connectionString { get; set; }
 
        public string Alias { get; set; }
        /// <summary>
        /// Вывод в консоль информации об исключении
        /// </summary>
        /// <param name="ex"></param>
        public void Log(Exception ex) {
            System.Console.WriteLine(ex.Message);
            
        }
        public OdbcDataSource() {
            SetSystemDatasource("DataContext","","");
            
        }

        /// <summary>
        /// Подключение через строку соединения
        /// </summary>
        /// <param name="connectionString"> строка соединения</param>
        public OdbcDataSource(string connectionString)
        {
            this.connectionString = connectionString;
       
            OnInit();
        }

        public void OnInit() {
            this.GetConnection().InfoMessage += (sender, args) => {
                System.Console.WriteLine("From ODBC Driver: " + sender + " " + args);
            };
            this.GetConnection().StateChange += (sender, args) => {
                System.Console.WriteLine("ODBC state changed: " + sender + " " + args);
            };
            System.Console.WriteLine("Connection state: " + this.GetConnection().State);
        }

        public ResultSet CleverExecute(string expression)
        {
         
            using (System.Data.Odbc.OdbcConnection connection = GetConnection())
            {

                connection.Open();
                DataTable dataTable = new DataTable();
                OdbcDataAdapter adapter = new OdbcDataAdapter(expression, connection);
                adapter.Fill(dataTable);

                TableMetaData tmd = new TableMetaData();
                foreach (DataColumn column in dataTable.Columns)
                {
                    ColumnMetaData cmd = new ColumnMetaData()
                    {
                        nullable = column.AllowDBNull,
                        unique = column.Unique,
                        description = column.Caption,
                        type = column.DataType.Name,
                    };
                    tmd.columns.Add(column.ColumnName, cmd);
                    
                }

                var rs = this.convert(dataTable);
                return new ResultSet()
                {

                    MetaData = tmd,
                    DataTable = dataTable,
                    DataSet = rs
                };
            }
        }


        /// <summary>
        /// Подключение к источнику зарегистрированному в системе
        /// </summary>
        /// <param name="dns"> имя источника </param>
        /// <param name="login"> логин </param>
        /// <param name="password"> пароль </param>
        public OdbcDataSource(string dns, string login, string password)
        {
            this.SetSystemDatasource(dns,login,password);
        }

        public void SetSystemDatasource(string dns, string login, string password)
        {
            this.connectionString = "dsn=" + dns + ";UID=" + login + ";PWD=" + password + ";";
        }


        /// <summary>
        /// Установка соединения 
        /// </summary>
        public virtual System.Data.Odbc.OdbcConnection GetConnection()
        {
            
            System.Data.Odbc.OdbcConnection connection = null;
            try
            {
                Info(connectionString);
                connection = new System.Data.Odbc.OdbcConnection(this.connectionString);
            }
            catch(Exception ex)
            {
                Error("При попытки установить соединение ODBC: "+this.connectionString+" возникла неожиданныя ситуация", ex);                
            }
            return connection;
        }

    
        

        /// <summary>
        /// Считывание бинарных данных, получаемых запросом
        /// </summary>
        public byte[] ReadBlob( string sqlCommand )
        {
            using ( System.Data.Odbc.OdbcConnection connection = GetConnection() )
            {
                connection.ChangeDatabase("FRMO");
                connection.Open();
                OdbcCommand command = new OdbcCommand( sqlCommand, connection );
                OdbcDataReader reader = command.ExecuteReader();
                if ( reader.Read() )
                {
                    // matching record found, read first column as string instance
                    byte[] value = ( byte[] ) reader.GetValue( 0 );
                    reader.Close();
                    command.ExecuteNonQuery();
                    return value;
                }
                return null;
            }
        }


        /// <summary>
        /// Запись бинарных данных в базу
        /// </summary>
        public int InsertBlob( string sqlCommand, string blobColumn, byte[] data )
        {
            using ( System.Data.Odbc.OdbcConnection connection = GetConnection() )
            {
                connection.Open();
                OdbcCommand command = new OdbcCommand( sqlCommand, connection );
                command.Parameters.Add( blobColumn, OdbcType.Binary );
                command.Parameters[blobColumn].Value = data;
                return command.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Получение расширенной справочной информации
        /// </summary>
        public Dictionary<string, object> GetSchemaDictionary()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            using (System.Data.Odbc.OdbcConnection connection = GetConnection())
            {
                connection.Open();
                DataTable catalogs = connection.GetSchema();
                JArray jcatalogs = this.convert(catalogs);
                foreach (JObject catalogInfo in jcatalogs)
                {
                    string collectionName = catalogInfo["CollectionName"].Value<string>();
                    if(collectionName == "Indexes")
                    {
                        Dictionary<string, object> indexes = new Dictionary<string, object>();
                        foreach ( string table in GetTables())
                        {
                            JArray catalog = this.convert(connection.GetSchema(collectionName,new string[]{ null,null,table }));
                            indexes[table] = catalog;
                        }
                        result[collectionName] = indexes;
                    }
                    else
                    {                  
                        if(collectionName != "DataTypes")
                        {
                            JArray catalog = this.convert(connection.GetSchema(collectionName));
                            result[collectionName] = catalog;
                        }                        
                    }                                              
                }
                result["catalogs"] = jcatalogs;
            }
            return result;
        }
         

        /// <summary>
        /// Вспомогательный метод преобразования данных в JSON
        /// </summary>
        public JArray convert(DataTable dataTable)
        {
            Dictionary<string, object> resultSet = new Dictionary<string, object>();
            List<Dictionary<string, object>> listRow = new List<Dictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                Dictionary<string, object> rowSet = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    rowSet[column.Caption] = row[column.Caption];                    
                }                
                listRow.Add(rowSet);
            }
            resultSet["rows"] = listRow;

            JObject jrs = JObject.FromObject(resultSet);
            return (JArray)jrs["rows"];
        }


        /// <summary>
        /// Получение списка таблиц
        /// </summary>
        public IEnumerable<string> GetTables()
        {
            List<string> tableNames = new List<string>();
            using (System.Data.Odbc.OdbcConnection connection = GetConnection())
            {
                connection.Open();
                DataTable tables = connection.GetSchema("Tables");
                foreach(JObject next in this.convert(tables))
                {
                    string tableName = next["TABLE_NAME"].Value<string>();
                    if (tableName.StartsWith("__") == false)
                    {
                        tableNames.Add(tableName);
                    }
                }
            }
            return tableNames.ToArray();
        }


        /// <summary>
        /// Запрос параметров хранимых процедур
        /// </summary>
        /// <returns></returns>
        private Dictionary< string, ProcedureMetadata > GetStoredProceduresMetadata()
        {
            Dictionary<string, ProcedureMetadata> metadata = new Dictionary<string, ProcedureMetadata>();
            // TODO:
            return metadata;
        }



        


        public virtual DatabaseMetadata GetDatabaseMetadata( )
        {           
            
 
            Info(connectionString);

            if (metadata != null)
                return metadata;
            using (var connection = new System.Data.Odbc.OdbcConnection(this.connectionString))
            {
                connection.Open();
                metadata = new DatabaseMetadata();
                metadata.driver = connection.Driver;
                metadata.database = connection.Database;
                object site = connection.Site;
            
                metadata.serverVersion = connection.ServerVersion;
                metadata.connectionString = connection.ConnectionString;
                
                DataTable columns = connection.GetSchema( "Columns" );              
                foreach ( DataRow row in columns.Rows )
                {                    
                    string table = row["TABLE_NAME"].ToString();                   
                    string column = row["COLUMN_NAME"].ToString();
                    string type = row["TYPE_NAME"] == null ? null : row["TYPE_NAME"].ToString();
                    //string catalog = row["TABLE_CAT"]  == null? null: row["TABLE_CAT"].ToString();
                    string schema = row["TABLE_SCHEM"] == null ? null : row["TABLE_SCHEM"].ToString();
                    string description = row["COLUMN_DEF"] == null ? null : row["COLUMN_DEF"].ToString();
                    string nullable = row["NULLABLE"] == null ? null : row["NULLABLE"].ToString();

                    //исколючаем системные таблицы и служебные
                    if (schema == "sys" || schema == "INFORMATION_SCHEMA" || table.ToLower().IndexOf("migration")!=-1 )
                    {
                        continue;
                    }
                  

                    
                    if ( !metadata.Tables.ContainsKey( table ) )
                    {
                        metadata.Tables[table] = new TableMetaData();
                        metadata.Tables[table].name = table;
                        metadata.Tables[table].description = "";

                        //определение наименования в множественном числе и единственном                        
                        string tableName = table;
                        if ( tableName.EndsWith( "s" ) )
                        {
                            if( tableName.EndsWith( "ies" ) )
                            {
                                metadata.Tables[table].multicount_name = tableName;
                                metadata.Tables[table].singlecount_name = tableName.Substring( 0, tableName.Length - 3 )+"y";
                            }
                            else
                            {
                                metadata.Tables[table].multicount_name = tableName;
                                metadata.Tables[table].singlecount_name = tableName.Substring( 0, tableName.Length - 1 );
                            }
                        }
                        else
                        {
                            if( tableName.EndsWith("y") )
                            {
                                metadata.Tables[table].multicount_name = tableName.Substring(0,tableName.Length-1) + "ies";
                                metadata.Tables[table].singlecount_name = tableName;

                            }
                            else
                            {
                                metadata.Tables[table].multicount_name = tableName + "s";
                                metadata.Tables[table].singlecount_name = tableName;
                            }
                        }
                    }
                    metadata.Tables[table].columns[column] = new ColumnMetaData();
                    metadata.Tables[table].columns[column].name = column;
                    metadata.Tables[table].columns[column].type = type;
                    metadata.Tables[table].columns[column].nullable = (nullable == "1") ? true : false;
                    metadata.Tables[table].columns[column].description = description;                    
                }


                //определение внешних ключей по правилам наименования
                List<TableMetaData> tables = ( from table in metadata.Tables.Values select table ).ToList<TableMetaData>();
                foreach ( var ptable in metadata.Tables )
                {

                    HashSet<string> associations = new HashSet<string>() { ptable.Value.multicount_name, ptable.Value.singlecount_name };
                    foreach ( var pcolumn in ptable.Value.columns )
                    {
                        //дополнительный анализ наименований колоной
                        string[] ids = pcolumn.Key.ToLower().Split( "_" );
                        HashSet<string> idsSet = new HashSet<string>( ids );
                        List<string> lids = ( from id in idsSet select id.ToLower() ).ToList<string>();
                        if ( idsSet.Contains("id") )
                        {
                            int count = ( from s in idsSet where associations.Contains( s ) select s ).Count();
                            if( count == 0 )
                            {
                                TableMetaData foreignKeyTable = ( from table 
                                                                  in tables 
                                                                  where lids.Contains( table.singlecount_name ) || lids.Contains( table.multicount_name ) select table )
                                                                  .FirstOrDefault<TableMetaData>();
                                if( foreignKeyTable == null )
                                {
                                    //throw new Exception("внешний ключ не найден для поля "+ ptable.Key+"."+pcolumn.Key );
                                }
                                else
                                {
                                    
                                    ptable.Value.fk[pcolumn.Key] = foreignKeyTable.singlecount_name;
                                }
                            }
                            else
                            {
                                pcolumn.Value.primary = true;
                                ptable.Value.pk = metadata.Tables[ptable.Key].pk = pcolumn.Key;

                            }                            
                        }
                    }
                }
                return metadata;
            }      
        }


        /// <summary>
        /// Выполнение запроса, возвращающего одну запись.
        /// </summary>
        public JObject GetSingleJObject( string sql )
        {
             
            using ( System.Data.Odbc.OdbcConnection connection = GetConnection() )
            {
                connection.Open();
                DataTable dataTable = new DataTable();
                OdbcDataAdapter adapter = new OdbcDataAdapter( sql, connection );
                adapter.Fill( dataTable );         
                JArray rs = this.convert( dataTable );                
                foreach ( JObject next in rs )
                {
                    return next;
                }
                throw new Exception("Запрос не вернул данные "+sql);
            }
        }



        JArray APIDataSource.GetJsonResult(string sql)
        {
            return ((OdbcDataSource)this).Execute(sql);
        }

        public DataTable CreateDataTable(string sql)
        {
            System.Console.WriteLine(sql);
            using (System.Data.Odbc.OdbcConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    DataTable dataTable = new DataTable();
                    OdbcDataAdapter adapter = new OdbcDataAdapter(sql, connection);
                    adapter.Fill(dataTable);
                    return dataTable;
                }
                catch (Exception ex)
                {
                    ex.ToString().WriteToConsole();
                    throw;
                }
            }
        }

        /// <summary>
        /// Выполнение запроса 
        /// </summary>
        public DataTable ExecuteDT( string sql )
        
        {
            System.Console.WriteLine(sql);
            using ( System.Data.Odbc.OdbcConnection connection = GetConnection() )
            {
                try
                {
                    connection.Open();
                    DataTable dataTable = new DataTable();
                    OdbcDataAdapter adapter = new OdbcDataAdapter(sql, connection);
                    adapter.Fill(dataTable);

                    return dataTable;
                }
                catch(Exception ex)
                {
                    Log(ex);
                    throw;
                }
                
            }
        }
        /// <summary>
        /// Выполнение запроса 
        /// </summary>
        public JArray Execute(string sql)

        {
            System.Console.WriteLine(sql);
            using (System.Data.Odbc.OdbcConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    DataTable dataTable = new DataTable();
                    OdbcDataAdapter adapter = new OdbcDataAdapter(sql, connection);
                    adapter.Fill(dataTable);

                    TableMetaData tmd = new TableMetaData();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        ColumnMetaData cmd = new ColumnMetaData()
                        {
                            nullable = column.AllowDBNull,
                            unique = column.Unique,
                            description = column.Caption,
                            type = column.DataType.Name,
                        };
                        tmd.columns.Add(column.ColumnName, cmd);
                    }
                    var array = this.convert(dataTable);
                    System.Console.WriteLine(array);
                    return array;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Ошибка при выполнении запроса: "+sql + " " + ex.Message);
                    throw;                    
                }

            }
        }


        public string GetConenctionString()
        {
            return this.connectionString+ "Driver={SQL Server};";
        }

        public bool canConnect()
        {
            return GetTables() != null;
        }

        public bool canReadAndWrite()
        {
            //TODO:
            return true;
        }

        public bool canCreateAlterTables()
        {
            //TODO:
            return true;
        }

        public object SingleSelect(string sql)
        {
            throw new NotImplementedException();
        }

        public object MultiSelect(string sql)
        {
            throw new NotImplementedException();
        }

        public object Exec(string sql)
        {
            throw new NotImplementedException();
        }

        public object Prepare(string sql)
        {
            throw new NotImplementedException();
        }

        void APIDataSource.InsertBlob(string sql, string v, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
