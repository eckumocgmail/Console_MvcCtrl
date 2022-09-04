using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace transport
{
    public class HttpFiles
    {
        public static void PutFiles(string url, params string[] files)
        {

            var http = new HttpClient();
            try
            {

                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                foreach (var file in files)
                {
                    int last = ((int)Math.Max(file.LastIndexOf("/"), file.LastIndexOf("\\")));
                    
                    string filename = file.Substring(last + 1);
                    var data = System.IO.File.ReadAllBytes(file);
                    multiContent.Add(new ByteArrayContent(data), "file", filename);
                }


                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url)
                {
                    Content = multiContent
                };


                HttpResponseMessage response = http.SendAsync(request).Result;
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        Console.WriteLine("OK");
                        break;
                    default: throw new Exception("" + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Исключение Http", ex);
            }
        }
    }
}
