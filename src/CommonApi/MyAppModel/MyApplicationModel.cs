
using DataODBC;

using System.Collections.Generic;
using System.Reflection;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ApplicationCore.Converter.Models
{
    public interface IMyApplicationBuilder{

        public void Get();
        public void AddApi(string url);
        public void AddAssembly(Assembly assembly);
        public void AddCallingAssembly();
        public void AddExecutingAssembly();

    }


    /// <summary>
    /// Коллекция сетевых сервисов
    /// </summary>
    public class MyApplicationModel: Dictionary<string, MyControllerModel>, IMyApplicationBuilder
    {
        public List<OdbcDataSource> datasources = new List<OdbcDataSource>();

        public void AddApi(string url)
        {
            throw new System.NotImplementedException();
        }

        public void AddAssembly(Assembly assembly)
        {
            throw new System.NotImplementedException();
        }

        public void AddCallingAssembly()
        {
            throw new System.NotImplementedException();
        }

        public void AddExecutingAssembly()
        {
            throw new System.NotImplementedException();
        }

        public void Get()
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// Выполняется проверка машрутов
        /// </summary>
        public async Task Ping(string baseUrl)
        {

            
            
            foreach( var ctrl in Values)
            {
                string url = baseUrl + ctrl.Path;
                Console.WriteLine("GET "+ url + " => " + await HttpService.TryRequest("GET", url));                
                Console.WriteLine("HEAD " + url + " => " + await HttpService.TryRequest("HEAD", url));
                Console.WriteLine("POST " + url + " => " + await HttpService.TryRequest("POST", url));
                Console.WriteLine("PATCH " + url + " => "+ await HttpService.TryRequest("PATCH", url));
                Console.WriteLine("PUT " + url + " => "+ await HttpService.TryRequest("PUT", url));
                Console.WriteLine("DELETE " + url + " => "+ await HttpService.TryRequest("DELETE", url));
                Console.WriteLine("OPTIONS " + url + " => "+ await HttpService.TryRequest("OPTIONS", url));


                foreach(var action in ctrl.Actions.Values)
                {
                    url = baseUrl + action.Path;
                    Console.WriteLine("\t GET " + url + " => " + await HttpService.TryRequest("GET", url));
                    Console.WriteLine("\t HEAD " + url + " => " + await HttpService.TryRequest("HEAD", url));
                    Console.WriteLine("\t POST " + url + " => " + await HttpService.TryRequest("POST", url));
                    Console.WriteLine("\t PATCH " + url + " => " + await HttpService.TryRequest("PATCH", url));
                    Console.WriteLine("\t PUT " + url + " => " + await HttpService.TryRequest("PUT", url));
                    Console.WriteLine("\t DELETE " + url + " => " + await HttpService.TryRequest("DELETE", url));
                    Console.WriteLine("\t OPTIONS " + url + " => " + await HttpService.TryRequest("OPTIONS", url));
                }
           
            }
        }

        public string url;
       
        public void Run(string[] args)
        {
         
          
        }


        /// <summary>
        /// Должен содержать сериализацию MvcApplicationModel
        /// </summary>
        /// <param name="json"></param>
        public void Configure(string json)
        {
            Console.WriteLine(json);
        }

        public void Import(params string[] args)
        {
            foreach(var arg in args)
            {
                string json = System.IO.File.ReadAllText(arg);
                Import(json);

            }
        }

        private void Import(string json)
        {
            var cmd = JsonConvert.DeserializeObject<MyApplicationModel>(json);

            this["WebApi"] = new MyControllerModel();
            foreach (var actionMap in cmd.Values.ToList().Select(ctrl => ctrl.Actions).ToList())
            {
                foreach (var action in actionMap)
                {
                    this["WebApi"].Actions[action.Key] = action.Value;

                    var sw = new StringWriter();
                    sw.Write($"{url}{action.Value.Path}");
                    if (action.Value.Parameters.Count > 0) sw.Write("?");
                    foreach (var p in action.Value.Parameters)
                    {
                        sw.Write($"{p.Key}={p.Value.ParameterType }");

                    }
                    string query = $"{"http://localhost:5000"}{sw.ToString()}";
                    Console.WriteLine(query);
                    try
                    {
                        Console.WriteLine(HttpService.Request("get", $"{query}"));
                    }
                    catch(Exception ex)
                    {
                        OnError(ex);
                    }
                    
                }
            }
        }

        private void OnError(Exception ex)
        {
            Console.WriteLine(ex);
        }

        public void LoadFile(string v)
        {
            string json = System.IO.File.ReadAllText(v);
            Import(json);
        }

        private void TraceApi()        
        {
             
            foreach (var kv1 in this)
            {

                Console.WriteLine($"{kv1.Value.Path}");
                foreach (var kv2 in kv1.Value.Actions)
                {

                    Console.WriteLine($"{kv2.Key}");
                    Console.Write($"{kv2.Value.Path}?");
                    foreach (var kv3 in kv2.Value.Parameters)
                    {
                        Console.Write($"{kv3.Key}={kv3.Value.ParameterType} ");
                    }
                    Console.WriteLine($"");
                }
            }
        }        




            public class HttpService
    {
        public static List<string> HTTP_METHODS = new List<string>() { "get", "post", "put", "patch", "delete", "head", "options", "delete" };




        public static string Rus(string eng) =>
            Request("get", @"https://translate.google.com/#view=home&op=translate&sl=en&tl=ru&text=" + eng);


        public static string Eng(string rus) =>
            Request("get", @"https://translate.google.com/#view=home&op=translate&sl=ru&tl=en&text=" + rus);


        public static async Task<string> RequestAsync(string method, string url)
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }



        public static string Request(string method, string url)
        {
            if (url == null)
                throw new Exception("не указан обязательный параметр url (параметры чувствительны к регистру)");
            if (method == null)
                throw new Exception("не указан обязательный параметр method (параметры чувствительны к регистру)");
            if (!HTTP_METHODS.Contains(method.ToLower())) throw new Exception("не верно задан параметр method (параметры чувствительны к регистру) укажите один из: get, post, put, head, options or delete");
            Task<string> response = RequestAsync(method, url);
            response.Wait();
            string res = response.Result;
            Debug.WriteLine(res);
            return res;
        }

        public static async Task<int> TryRequest(string method, string url)
        {
            try
            {
                int code = await GetHttpStatus(method, url);
                return code;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"[{method}]{url}");
                Console.WriteLine($"[{ex.Message}]");
                return 0;
            }
        }

        private static async Task<int> GetHttpStatus(string method, string url)
        {
            if (url == null)
                throw new Exception("не указан обязательный параметр url (параметры чувствительны к регистру)");
            if (method == null)
                throw new Exception("не указан обязательный параметр method (параметры чувствительны к регистру)");
            if (!HTTP_METHODS.Contains(method.ToLower()))
                throw new Exception("не верно задан параметр method (параметры чувствительны к регистру) укажите один из: get, post, put, head, options or delete");

            System.Net.Http.HttpClient _httpClient = new System.Net.Http.HttpClient();
            _httpClient.BaseAddress = new Uri(url);
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Clear();


            var services = new StringContent(GetXrdsDocument(),
                        Encoding.UTF8, "application/json-patch+json");

            switch (method.ToLower())
            {
                case "get":
                    var got = await _httpClient.GetAsync(url);
                    return (int)got.StatusCode;
                case "post":
                    var posted = await _httpClient.PostAsync(url, services);
                    return (int)posted.StatusCode;
                case "patch":
                    var patched = await _httpClient.PatchAsync(url, services);
                    return (int)patched.StatusCode;
                case "put":
                    var puted = await _httpClient.PatchAsync(url, services);
                    return (int)puted.StatusCode;
                case "options":
                    var configured = await _httpClient.PatchAsync(url, services);
                    return (int)configured.StatusCode;
                case "delete":
                    var deleted = await _httpClient.PatchAsync(url, services);
                    return (int)deleted.StatusCode;
                default:
                    throw new Exception("HTTP-метод указан некорректно, проверьте пож-та: method=" + method);
            }

        }

        private static string GetXrdsDocument()
        {
            return "{}";
        }


        /// <summary>
        /// Выполнение запроса
        /// </summary>  
        public static string Request(string json)
        {
            try
            {
                JObject options = JsonConvert.DeserializeObject<JObject>(json);
                if (!options.ContainsKey("url")) throw new Exception("не указан обязательный параметр url (параметры чувствительны к регистру)");
                if (!options.ContainsKey("method")) throw new Exception("не указан обязательный параметр method (параметры чувствительны к регистру)");
                string url = options["url"].Value<string>();
                string method = options["method"].Value<string>();
                if (!HTTP_METHODS.Contains(method)) throw new Exception("не верно задан параметр method (параметры чувствительны к регистру) укажите один из: get, post, put, head, options or delete");
                Task<string> response = RequestAsync(method, url);
                response.Wait();
                return response.Result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }

        }
    }

    }
}
