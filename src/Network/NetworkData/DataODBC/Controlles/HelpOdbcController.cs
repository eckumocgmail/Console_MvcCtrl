using DataODBC;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;

namespace eckumoc_common_api.NetworkDatabases.DataODBC.Controlles
{

     

    [Route("/api/odbc/help/[action]")]
    public class HelpOdbcController: Controller
    {
        private readonly ILogger<HelpOdbcController> _logger;

        public HelpOdbcController( ILogger<HelpOdbcController> logger )
        {
            _logger = logger;
        }



        /// <summary>
        /// Сведения о структурах данных
        /// </summary>     
        /// 
        public IDictionary<string, string> Json()
        {
            var @res = new Dictionary<string, string>();
            new OdbcDriverManager().GetOdbcDatasourcesNames().ToList().ForEach(dsn => {
                try
                {
                    using (var ods = new OdbcDataSource(dsn, "sa", "Gye*34FRtw"))
                    {
                        res[$"/api/odbc/{ods.ConsoleLoggerName}/$metadata"] =
                        "Получение сведений о структурах истоника данных. ";
                        res[$"/api/odbc/{ods.ConsoleLoggerName}/$stats"] =
                            "Получение сведений о объёмах данных. ";
                        res[$"/api/odbc/{ods.ConsoleLoggerName}/$keywords"] =
                            "Получение сведений о плотности распределния терминов";
                        res[$"/api/odbc/{ods.ConsoleLoggerName}/$top"] =
                            "Получение сведений о лексической категории ресурса";
                        ods.GetDatabaseMetadata().Tables.Keys.ToList().ForEach(table =>
                        {
                            res[$"/api/odbc/{ods.ConsoleLoggerName}/{table}/$metadata"] =
                                $"Получение сведений о структурe \"{table}\". ";
                            res[$"/api/odbc/{ods.ConsoleLoggerName}/{table}/$search"] =
                                $"Выполнение поисковых запросов по \"{table}\". ";
                            res[$"/api/odbc/{ods.ConsoleLoggerName}/{table}/$keywords"] =
                                $"Получение сведений лексической плотности данных \"{table}\". ";
                            res[$"/api/odbc/{ods.ConsoleLoggerName}/{table}/$list"] =
                                $"Просмотр в виде списка \"{table}\". ";
                            res[$"/api/odbc/{ods.ConsoleLoggerName}/{table}/$editor"] =
                                $"Переход в редактор объектов  \"{table}\". \"{table}\". ";
                            res[$"/api/odbc/{ods.ConsoleLoggerName}/{table}/$nav"] =
                                $"Получение сведений по связанным структурам \"{table}\". ";
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("", ex);
                }
                

            });
            return @res;
        }


    }
}
