
using DataADO;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ServiceCollectionExtension
{
   
    public static IServiceCollection AddSqlServer<TDBContext>(this IServiceCollection services,
        string Server, bool TrustedConnection, string Username = null, string Password = null)
        where TDBContext : DataContext
        
    {
        System.Console.WriteLine($"[Info][{typeof(TDBContext).Name}]: AddSqlServer({Server},{typeof(TDBContext).Name},{TrustedConnection},{Username},{Password})");
        AddDataContext<TDBContext>(
            services,
            options => {
            var connectionString = new SqlServerConnectionString()
            {
                Server = Server,
                Database = typeof(TDBContext).Name,
                TrustedConnection = TrustedConnection,
                UserID = Username,
                Password = Password
            };
            var validator = new SqlServerADOService();
            if (validator.CanConnect(connectionString.ToString()))
            {
                UseSqlServer(options,connectionString.ToString());
            }
            else
            {
                connectionString.Database = "Model";
                if (validator.CanConnect(connectionString.ToString()))
                {
                    validator.PrepareQuery(
                        connectionString.ToString(),
                        $"Create database {typeof(TDBContext).Name}");
                    connectionString.Database = typeof(TDBContext).Name;
                    UseSqlServer(options,connectionString.ToString()); ;
                }
                else
                {
                    throw new System.Exception(
                        "Не удалось подключиться к источнику медицинских данных. " +
                        "Проверьте строку подключения: " + connectionString.ToString());
                }
                
            }
        });
        
        return services;
    }

    private static void UseSqlServer(object options, string v)
    {
        throw new NotImplementedException();
    }

    private static void AddDataContext<TDBContext>(IServiceCollection services, Action<object> p) where TDBContext : DataContext
    {
        throw new NotImplementedException();
    }
}