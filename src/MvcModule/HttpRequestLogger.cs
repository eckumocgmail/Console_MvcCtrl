using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_Npm.Services
{





    /// <summary>
    /// 
    /// </summary>
    public class HttpRequestLoggerMiddleware: IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var logger = (HttpRequestLogger)
                context.RequestServices.GetService(typeof(HttpRequestLogger));
            logger.Log();

            switch(context.Request.Method.ToUpper())
            {
                case "PUT":
                    logger.LogBody();
                    break;
                case "PATCH":
                    logger.LogBody();
                    break;
                case "OPTIONS":
                    logger.LogBody();
                    break;
                case "DELETE":
                    logger.LogBody();
                    break;
            }
            return next.Invoke(context);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class HttpRequestLogger  
    {
        private readonly ILogger<HttpRequestLogger> _logger;
        private readonly IHttpContextAccessor _accessor;

        public HttpRequestLogger(ILogger<HttpRequestLogger> logger, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _accessor = accessor;
        }

        public void Log()
        {
            var request = _accessor.HttpContext.Request;
            
            string message = $"{request.Method.ToUpper()}: {request.Path.ToString()}\n";
            foreach(var header in request.Headers)
            {
                string valuesString = "[";
                foreach(var headerValue in header.Value)
                {
                    valuesString += $"\"{headerValue}\",";
                }
                if (valuesString.EndsWith(",")) valuesString = valuesString.Substring(0, valuesString.Length - 1);
                valuesString += "]";
                message+=$"\t{header.Key}={valuesString}\n";
            }
            _logger.LogInformation($"{message}");
        }

        public void LogBody()
        {
            var request = _accessor.HttpContext.Request;
            _logger.LogInformation($"Length: {request.Body.Length}");
            var buffer = new byte[request.Body.Length];
            int readed = request.Body.Read(buffer, 0, (int)request.Body.Length);
            _logger.LogInformation($"readed: {readed}");

        }
    }
}
