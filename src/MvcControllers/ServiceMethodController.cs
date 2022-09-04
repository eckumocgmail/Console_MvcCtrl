using eckumoc_common_api.MvcModule.MvcInterfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MVC.Controllers
{

    

    /// <summary>
    /// Контроллер методов сервисного объекта
    /// </summary>
    /// <typeparam name="IBehaviour">тип сервиса</typeparam>
    //[Route("/api/[controller]")]
    //[Route("/api/[controller]/[action]")]
    //[Route("/api/[controller]/[action]/{id}")]
    [Route("/[controller]/[action]")]

    public class ServiceMethodController<IBehaviour> : BaseHttpMethodController
    {               
        private object EventsHandler;
        public object GetService(Type ServiceType) => this.ControllerContext.HttpContext.RequestServices.GetService(ServiceType);

        public ServiceMethodController( )
        {                   
        }


        [HttpPost()]
        public async Task<IActionResult> OnPost(string method)
        {
            this.EventsHandler = (IBehaviour)this.GetService(typeof(IBehaviour));
            this.EventsHandler = this.EventsHandler == null ? this : this.EventsHandler;
            string MethodName = method;
            var Method = this.EventsHandler.GetType().GetMethod(MethodName);
            if (Method == null)
            {
                await Http404();
            }
            var TextParameters = GetFormParameters();
            object[] InvokeArguments = GetArguments(Method, TextParameters);
            object result = Method.Invoke(this.EventsHandler, InvokeArguments);
       
            string content = ToControlPanelHtml(result);
            await SendTextHtml(content);
            return Ok();
        }

        private async Task Http404()
            => await WriteHtml(404, "Ничего не найдено");

        public async Task WriteHtml(int status, string content)
        {
            this.HttpContext.Response.StatusCode = status;
            this.HttpContext.Response.ContentType = "text/html;charset=UTF-8";
            await this.HttpContext.Response.WriteAsync(content);
        }


        
        private async Task SendTextHtml(string content)
        {
            HttpContext.Response.ContentType = "text/html;charset=URF-8";
            await HttpContext.Response.WriteAsync(content);            
        }
 
        private string ToControlPanelHtml(object result)
        {
            string html = "";
            html += @"<head><link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bootstrap@4.1.3/dist/css/bootstrap.min.css"" integrity=""sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO"" crossorigin=""anonymous""></head>";
            html += "<div>";

            html += "<div>";
            result.GetType().GetMethods().Select(m=>m.Name).ToList().ForEach(methodName=> {
                html += $"<button class='btn btn-primary'>{methodName}</button>";
            });
            html += "</div>";

            html += "<div>";
            result.GetType().GetOwnPropertyNames().ToList().ForEach(methodName => {
                html += $"<div>{result.GetType()}</div>";
            });
            html += "</div>";

            html += "</div>";
            return html;
        }

        [HttpGet()]
        public async Task<IActionResult> OnGet( string method )
        {
            await Task.CompletedTask;

            var ctrl = ControllerContext;
            var desc = ControllerContext.ActionDescriptor;
            string[] AvailableMethods = typeof(IBehaviour).GetMethods().Where(method=> method.Name != "ToString" && method.Name != "GetType" && method.Name != "GetHashCode" && method.Name != "Equals" && method.Name.StartsWith("get_") == false && method.Name.StartsWith("set_") == false ).Select(m => m.Name).ToArray();
            if(string.IsNullOrEmpty(method))
                return new RequireMethodNameResult(AvailableMethods);
     
            this.EventsHandler = (IBehaviour)this.GetService(typeof(IBehaviour));
            this.EventsHandler = this.EventsHandler == null ? this : this.EventsHandler;
            lock (this.EventsHandler)
            {
                
                string MethodName = method;//GetMethodName();
                if (MethodName == null)
                    return new RequireMethodNameResult(AvailableMethods);
                var Method = this.EventsHandler.GetType().GetMethod(MethodName);
                if (Method == null)
                    return NotFound();
               
                var TextParameters = GetParameters();
                string[] RequiredParameters = this.ValidateParametersDeclaration(Method, TextParameters.Keys.ToArray());
                if (RequiredParameters.Length > 0)
                    return new RequireParametersResult(MethodName, RequiredParameters);
                object[] InvokeArguments = GetArguments(Method, TextParameters);
                object result = Method.Invoke(this.EventsHandler, InvokeArguments);
                return Ok(result);
            }

        }

        private Dictionary<string, string> GetFormParameters()
        {
            var parameters = new Dictionary<string, string>();
            ControllerContext.HttpContext.Request.Form.ToList().ForEach(kv => parameters[kv.Key] = kv.Value);
            return parameters;
        }
        private Dictionary<string, string> GetParameters()
        {
            var parameters = new Dictionary<string, string>();
            ControllerContext.HttpContext.Request.Query.ToList().ForEach(kv => parameters[kv.Key] = kv.Value);
            return parameters;
        }


        /// <summary>
        /// Вовзращает массив требуемых параметров
        /// </summary>
        private string[] ValidateParametersDeclaration(MethodInfo Action, string[] Args)
        {
            List<string> RequiredParameters = Action.GetParameters().ToList().Where(p => p.IsOptional == false).Select(p => p.Name).ToList();
            return RequiredParameters.Except(Args).ToArray();
        }
        private object[] GetArguments(MethodInfo Action, Dictionary<string, string> Parameters)
        {
            var Result = new List<object>();
            var DeclaredParametersList = Action.GetParameters().ToList();
            DeclaredParametersList.Sort((p1, p2) => p2.Position - p1.Position);
            DeclaredParametersList.ForEach(p => Result.Add(ConvertTextParameter(p, Parameters[p.Name])));
            return Result.ToArray();
        }
        private object ConvertTextParameter(ParameterInfo Parameter, string Text)
        {
            object ArgumentRef = null;
            if (IsPrimitive(Parameter.ParameterType))
            {
                switch (Parameter.ParameterType.Name)
                {
                    case nameof(Int32): return ToInt(Text);
                    case "String": return Text;
                    
                    case nameof(Int64): return ToLong(Text);
                    case nameof(DateTime): return ToDateTime(Text);
                    case nameof(System.Decimal): return ToFloat(Text);
                    default: throw new Exception("Преобразование текста в тип " + Parameter.ParameterType.Name + " не поддерживается");
                }
            }
            else
            {
                try
                {
                    ArgumentRef = Create(Parameter.ParameterType);
                    Dictionary<string, object> ValuesMap = ToDictionary(Text);

                }
                catch(Exception ex)
                {
                    EcKuMoC.WriteLine(ex);
                    //throw new Exception("Не удалось конвертировать текстовый параметр типа "+ Parameter.ParameterType.FullName +" "+ex.Message);
                    return Text;
                }
            }
            throw new NotImplementedException();
        }

        private Dictionary<string, object> ToDictionary(string text) => JsonConvert.DeserializeObject<Dictionary<string, object>>(text);

        private object ToFloat(string text) => float.Parse(text.Trim());

        private object ToDateTime(string text) => DateTime.Parse(text.Trim());

        private object ToLong(string text) => long.Parse(text.Trim());

        private object ToInt(string text) => int.Parse(text.Trim());

        private static string[] PrimitiveTypeNames = new string[] 
        {
            nameof(System.String),
            nameof(System.Int32),
            nameof(System.Int64),
            nameof(System.DateTime),
            nameof(System.Decimal),
            nameof(System.Double)
        };

        private bool IsPrimitive(Type type) => PrimitiveTypeNames.Contains(type.Name);

        private void Resolve(object ArgumentRef, Dictionary<string, object> ValuesMap)
        {
            foreach (var kv in ValuesMap)
            {
                Set(ArgumentRef, kv.Key, kv.Value);
            }
        }

        private void Set(object argumentRef, string key, object value)
        {
            argumentRef.GetType().GetProperty(key).SetValue(argumentRef, value);
        }

        private object Create(Type parameterType)
        {
            var DefaultConstructor = parameterType.GetConstructors().Where(c => c.GetParameters().Count() == 0).First();
            return DefaultConstructor.Invoke(new object[0]);
        }

        public override async Task<IActionResult> OnGet() => await this.OnGet(null);

        public override async Task<IActionResult> OnPost() => await OnPost(null);

        public override async Task<IActionResult> OnPut() => await OnPost(null);

        public override async Task<IActionResult> OnPatch() => await OnPost(null);

        public override async Task<IActionResult> OnOptions() => await OnPost(null);
    }


    /// <summary>
    /// 
    /// </summary>
    public class ServiceEndpointController : Controller
    {
        public IActionResult NavigateToParent() => new LocalRedirectResult("..");
    }



    /// <summary>
    /// 
    /// </summary>
    public class RequireMethodNameResult : BadRequestResult
    {
        private readonly string[] AvailableMethods;
        public RequireMethodNameResult(string[] AvailableMethods)
        {
            this.AvailableMethods = AvailableMethods;
        }
        public override void ExecuteResult(ActionContext context)
        {
            context.HttpContext.Response.ContentType = "text/html;charset=UTF-8;";
            context.HttpContext.Response.WriteAsync($"<form onsubmit=\"event.preventDefault();\" action=\"{GetType().Name}\">").Wait();
            AvailableMethods.ToList().ForEach(method => {
                context.HttpContext.Response.WriteAsync($"<button style=\"width: 100%;\" class=\"btn btn-primary\" onclick=\"location.href=('/Home?method={method}')\">{method}</button>").Wait();
            });
            context.HttpContext.Response.WriteAsync($"</form>").Wait();

        }
    }



    public class RequireParametersResult : BadRequestResult
    {

        private readonly string Method;
        private readonly string[] RequiredParameters;
        public RequireParametersResult(string Method, string[] RequiredParameters)
        {
            this.Method = Method;
            this.RequiredParameters = RequiredParameters;
        }
        public override void ExecuteResult(ActionContext context)
        {

            //RequiredParameters.ToList().ForEach(p => context.ModelState.AddModelError("Require", p));
            context.HttpContext.Response.WriteAsync($"<html>").Wait();
            context.HttpContext.Response.WriteAsync($"<head>").Wait();
            foreach (string cssFile in new string[] { "lib/bootstrap/dist/css/bootstrap.min.css" })
            {
                string linkText = $"<link rel=\"stylesheet\" href=\"{cssFile}\" />";
                context.HttpContext.Response.WriteAsync(linkText).Wait();
            }
            context.HttpContext.Response.WriteAsync($"</head>").Wait();
            context.HttpContext.Response.WriteAsync($"<body>").Wait();
            context.HttpContext.Response.WriteAsync($"<form method=\"post\" style=\"padding: 20px;\" action=\"Home\">").Wait();
            context.HttpContext.Response.WriteAsync($"<h2></h2>").Wait();
            context.HttpContext.Response.WriteAsync($"<input name=\"method\" type=\"hidden\" value=\"{Method}\" />").Wait();

            RequiredParameters.ToList().ToList().ForEach(method => {
                context.HttpContext.Response.WriteAsync($"<div style=\"width: 100%;\" class=\"form-group\"><label>{method}</label><input class=\"form-control\" type=\"text\" name=\"{method}\" id=\"{method}\"" + "/></div>").Wait();
            });
            context.HttpContext.Response.WriteAsync($"<div align=\"right\"><button class=\"btn btn-primary\">ok</button></div>").Wait();

            context.HttpContext.Response.WriteAsync($"</form>").Wait();
            foreach (string cssFile in new string[] { "lib/jquery/dist/jquery.min.js" })
            {
                string linkText = $"<script src=\"{cssFile}\"></script>";
                context.HttpContext.Response.WriteAsync(linkText).Wait();
            }
            context.HttpContext.Response.WriteAsync($"</body>").Wait();
            context.HttpContext.Response.WriteAsync($"</html>").Wait();




        }
    }
}
