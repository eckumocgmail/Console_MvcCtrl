using COM;

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Расширения для выполнения HTTP-запросов
/// </summary>
public static class TextHttpExtensions
{

    /// <summary>
    /// Выполнение HTTP-запроса методом GET
    /// </summary>
    public static string Get(this string query)
    {
     
        var result = new HttpClient().GetAsync(query).Result;
        return result.Content.ReadAsStringAsync().Result;
    }


    public static async Task Select(this HttpContext httpContext, IEnumerable<Type> messages)
    {
        httpContext.Response.StatusCode = 200;
        httpContext.Response.ContentType = "text/html; charset=utf-8";
        
        string inner = "";
        messages.ToList().ForEach(m => inner += $@"<a class='btn w-100 alert alert-danger' href='/$forms/{m.GetName().ToKebabStyle()}'>{m.GetName().ToKebabStyle()}</a>" );
        
        await httpContext.Response.WriteAsync(@"<!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='utf-8' />
                    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css' rel='stylesheet'>

                </ head>
                <body>
                    <header>
                        <nav class='navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3'>
                            <div class='container'>
                                <a class='navbar-brand' asp-area='' asp-controller='Home' asp-action='Index'>RootConsumer</a>
                                <button class='navbar-toggler' type='button' data-toggle='collapse' data-target='.navbar-collapse' aria-controls='navbarSupportedContent'
                                        aria-expanded='false' aria-label='Toggle navigation'>
                                    <span class='navbar-toggler-icon'></span>
                                </button>
                                <div class='navbar-collapse collapse d-sm-inline-flex justify-content-between'>
                                    <ul class='navbar-nav flex-grow-1'>
                        
                                    </ul>
                                </div>
                            </div>
                        </nav>
                    </header>
                    <div class='container'>
                        <main role='main' class='pb-3'>
                           " +
                       inner
                       + @"
                        </main>
                    </div>

                    <footer class='border-top footer text-muted'>
                        <div class='container'>
                            &copy; 2021  
                        </div>
                    </footer>
 
                  <script src='https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js' ></script>
                </body>
                </html> ");
    }
}