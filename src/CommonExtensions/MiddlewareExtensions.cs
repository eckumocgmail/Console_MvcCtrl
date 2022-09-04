using COM;

//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class MiddlewareExtensions
{
    /*
      public static void UseHttp<DCOM>( this IApplicationBuilder builder, string uri ) where DCOM: IMiddleware 
      {
          System.Console.WriteLine("Регистрация HTTP-контекста");
          builder.UseWhen(
              http => http.Request.Path.ToString().StartsWith(uri),
              rebuild =>
              {

                  rebuild.UseMiddleware<DCOM>();
              }
          );;
      }*/
}
