
using DataADO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
public class OdbcDataConnector   
{
    private readonly SqlServerADOService _sqlserver;
    private readonly string _connectionString;

    public OdbcDataConnector( IConfiguration config, SqlServerADOService sqlserver )
    {
        _sqlserver = sqlserver;
        _connectionString = config.GetConnectionString("Default");
    }

    public OdbcDatabaseManager GetDatabaseManager(int id)
    {
        if (_connectionString == null) throw new Exception("Строка подключения Default не зарегистрирована в конфигурации приложения.");
        var resultset = 
            _sqlserver.ExecuteQuery(_connectionString,
                $"select * from BusinessDatasources where ID={id}");
        string remotecConnectionString = resultset.Rows[0]["ConnectionString"].ToString();    
        return OdbcDatabaseManager.GetOdbcDatabaseManager(remotecConnectionString);
    }

} 